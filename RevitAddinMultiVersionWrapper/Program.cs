using System.Diagnostics;

namespace RevitAddinMultiVersionWrapper;

using System.Reflection;
using Autodesk.Revit.UI;

internal class App : IExternalApplication
{
    private const string RevitAppClass = "RevitAddinMultiVersion.App";
    private const string DefaultVersion = "R25";
    private const string DllName = "RevitAddinMultiVersion";
    
    private IExternalApplication? _dllInstance;

    private string? _tempFilePath;
    private Assembly? _zstdAssembly;
    
    void ExtractResource( string resource, string path )
    {
        var assembly = Assembly.GetExecutingAssembly();
        Stream? stream = assembly.GetManifestResourceStream( resource );
        byte[] bytes = new byte[(int)stream.Length];
        stream.Read( bytes, 0, bytes.Length );
        File.WriteAllBytes( path, bytes);
    }

    public Result OnStartup(UIControlledApplication application)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var version = DllVersion(application);
        
        LoadZstdSharp();

        _tempFilePath = Path.Combine(Path.GetTempPath(), $"{DllName}{version}.dll");
        try
        {
            DecompressDll(assembly, version, _tempFilePath);
        }
        catch (Exception ex)
        {
            TaskDialog.Show("Decompression Error", ex.ToString());
            return Result.Failed;
        }

        if (!File.Exists(_tempFilePath))
        {
            TaskDialog.Show("Error", $"DLL not found at {_tempFilePath}");
            return Result.Failed;
        }
        
        _dllInstance = Assembly.LoadFrom(_tempFilePath)
            .CreateInstance(RevitAppClass) as IExternalApplication;

        if (_dllInstance == null)
        {
            TaskDialog.Show("Error", "Failed to create instance of the add-in class.");
            return Result.Failed;
        }

        try
        {
            _dllInstance.OnStartup(application);
        }
        catch (Exception ex)
        {
            TaskDialog.Show("OnStartup Error", ex.ToString());
            return Result.Failed;
        }

        return Result.Succeeded;
    }
    
    public Result OnShutdown(UIControlledApplication application)
    {
        _dllInstance?.OnShutdown(application);
        
        string exePath = Path.Combine(Path.GetTempPath(), "GPSdllMover.exe");  
        ExtractResource( "GPSrvtTab.Resources.GPSrvtTabDLLMover.exe", exePath );
            
        Process process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                    
                Arguments = "-",
                    
                FileName = exePath,
                UseShellExecute = false,
                CreateNoWindow = true // Set to false if you want to see the console window
            }
        };
            
        try
        {
            process.Start();
            process.WaitForExit();
        }
        finally
        {
            if (File.Exists(exePath))
            {
                File.Delete(exePath);
            }
        }

        return Result.Succeeded;
    }

    private string DllVersion(UIControlledApplication application)
    {
        return application.ControlledApplication.VersionNumber switch
        {
            "2014" => "R14",
            "2015" => "R15",
            "2016" => "R16",
            "2017" => "R17",
            "2018" => "R18",
            "2019" => "R19",
            "2020" => "R20",
            "2021" => "R21",
            "2022" => "R22",
            "2023" => "R23",
            "2024" => "R24",
            "2025" => "R25",
            _ => DefaultVersion
        };
    }
    
    
    
    
    private void LoadZstdSharp()
    {
        
        var zstdDllFilePath = Path.Combine(Path.GetTempPath(), "WrapperZstdSharp.dll");
        
        using var stream = Assembly.GetExecutingAssembly()
            .GetManifestResourceStream($"{typeof(App).Namespace}.Resources.ZstdSharp.dll");
        if (stream == null)
        {
            throw new InvalidOperationException("ZstdSharp.dll not found in resources.");
        }
            
        using var fileStream = new FileStream(zstdDllFilePath, FileMode.Create, FileAccess.Write);
            
        stream.CopyTo(fileStream);
        _zstdAssembly = Assembly.LoadFrom(zstdDllFilePath);
    }
    
    private void DecompressDll(Assembly assembly, string version, string tempFilePath)
    {
        
        var dllName = $"{DllName}{version}.zstd";

        if (_zstdAssembly == null)
        {
            TaskDialog.Show("Error", "ZstdSharp assembly not loaded.");
            throw new InvalidOperationException("ZstdSharp assembly not loaded.");
        }
        
        var decompressionStreamType = _zstdAssembly.GetType("ZstdSharp.DecompressionStream");
        if (decompressionStreamType == null)
        {
            throw new InvalidOperationException("ZstdSharp.DecompressionStream type not found.");
        }
        
        // Create instance of ZstdSharp.DecompressionStream
        var decompressionStreamConstructor = decompressionStreamType.GetConstructor([typeof(Stream)]);
        if (decompressionStreamConstructor == null)
        {
            throw new InvalidOperationException("No suitable constructor for ZstdSharp.DecompressionStream.");
        }
        
        using var stream = assembly.GetManifestResourceStream($"{typeof(App).Namespace}.Resources.{dllName}"); 
        var decompressorStream = decompressionStreamConstructor.Invoke([stream]);
        using var fileStream = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write);
        
        var copyToMethod = decompressionStreamType.GetMethod("CopyTo", [typeof(Stream)]);
        if (copyToMethod == null)
        {
            throw new InvalidOperationException("CopyTo method not found on ZstdSharp.DecompressionStream.");
        }
        
        copyToMethod.Invoke(decompressorStream, [fileStream]);
        
        // Dispose of the decompression stream properly (if IDisposable)
        if (decompressorStream is IDisposable disposable) 
        {
            disposable.Dispose();
        }
    }
    
}