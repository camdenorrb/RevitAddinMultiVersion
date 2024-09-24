namespace RevitAddinMultiVersionWrapper;

using System.Reflection;
using Autodesk.Revit.UI;
using ZstdSharp;

class App : IExternalApplication
{

    private const string RevitAppClass = "RevitAddinMultiVersion.App";
    private const string DefaultVersion = "R25";
    
    private AppDomain? _dllAppDomain;
    private object? _dllInstance;
    
    private static readonly string TempFilePath = Path.Combine(Path.GetTempPath(), "RevitAddinMultiVersion.dll");
    
    public Result OnStartup(UIControlledApplication application)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var version = DllVersion(application);

        try
        {
            DecompressDll(assembly, version, TempFilePath);
        }
        catch (Exception ex)
        {
            TaskDialog.Show("Decompression Error", ex.ToString());
            return Result.Failed;
        }

        if (!File.Exists(TempFilePath))
        {
            TaskDialog.Show("Error", $"DLL not found at {TempFilePath}");
            return Result.Failed;
        }
        
        _dllAppDomain = AppDomain.CreateDomain("RevitAddinMultiVersion");
        _dllAppDomain.AssemblyResolve += DllAppDomain_AssemblyResolve;

        var assemblyName = AssemblyName.GetAssemblyName(TempFilePath).FullName;

        try
        {
            _dllInstance = _dllAppDomain.CreateInstanceAndUnwrap(
                assemblyName,
                RevitAppClass
            );
        }
        catch (Exception ex)
        {
            TaskDialog.Show("CreateInstance Error", ex.ToString());
            return Result.Failed;
        }

        if (_dllInstance == null)
        {
            TaskDialog.Show("Error", "Failed to create instance of the add-in class.");
            return Result.Failed;
        }

        try
        {
            DllCallOnStartup(_dllInstance, application);
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
        
       if (_dllInstance != null)
       {
           DllCallOnShutdown(_dllInstance, application);
       }
       
       if (_dllAppDomain != null)
       {
           AppDomain.Unload(_dllAppDomain);
       }
        
       if (File.Exists(TempFilePath))
       {
           File.Delete(TempFilePath);
       }

       return Result.Succeeded;
    }

    public string DllVersion(UIControlledApplication application)
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
    
    private void DecompressDll(Assembly assembly, string version, string tempFilePath)
    {
        var dllName = $"RevitAddinMultiVersion{version}.zstd";

        using var stream = assembly.GetManifestResourceStream($"RevitAddinMultiVersionWrapper.Resources.{dllName}");
        using var decompressorStream = new DecompressionStream(stream);
        using (var fileStream = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write))
        {
            decompressorStream.CopyTo(fileStream);
        }
    }
    
    private void DllCallOnStartup(object instance, UIControlledApplication application)
    {
        var type = instance.GetType();
        
        var onStartupMethod = type.GetMethod("OnStartup");
        if (onStartupMethod != null)
        {
            onStartupMethod.Invoke(instance, [application]);
        }
    }
    
    private void DllCallOnShutdown(object instance, UIControlledApplication application)
    {
        var type = instance.GetType();
        
        var onShutdownMethod = type.GetMethod("OnShutdown");
        if (onShutdownMethod != null)
        {
            onShutdownMethod.Invoke(instance, [application]);
        }
    }
    
    private Assembly? DllAppDomain_AssemblyResolve(object sender, ResolveEventArgs args)
    {
        // Check if the requested assembly is RevitAPI or RevitAPIUI
        if (args.Name.StartsWith("RevitAPI"))
        {
            // Load the assembly from the default AppDomain
            var assembly = AppDomain.CurrentDomain.GetAssemblies()
                .FirstOrDefault(a => a.FullName == args.Name);
            if (assembly != null)
            {
                return assembly;
            }
        }
        
        var assemblyName = new AssemblyName(args.Name).Name + ".dll";

        var tempAssemblyPath = Path.Combine(
            Path.GetDirectoryName(TempFilePath) ?? string.Empty,
            assemblyName);
        
        if (File.Exists(tempAssemblyPath))
        {
            return Assembly.LoadFrom(tempAssemblyPath);
        }
        
        

        return null;
    }
    
}