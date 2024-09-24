namespace RevitAddinMultiVersionWrapper;

using System.Reflection;
using Autodesk.Revit.UI;
using ZstdSharp;

internal class App : IExternalApplication
{

    private const string RevitAppClass = "RevitAddinMultiVersion.App";
    private const string DefaultVersion = "R25";
    
    private IExternalApplication? _dllInstance;

    private string? _tempFilePath;
    
    public Result OnStartup(UIControlledApplication application)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var version = DllVersion(application);

        _tempFilePath = Path.Combine(Path.GetTempPath(), $"RevitAddinMultiVersion{version}.dll");
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
        
        /*
       if (File.Exists(TempFilePath))
       {
           File.Delete(TempFilePath);
       }*/

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
    
    private static void DecompressDll(Assembly assembly, string version, string tempFilePath)
    {
        var dllName = $"RevitAddinMultiVersion{version}.zstd";

        using var stream = assembly.GetManifestResourceStream($"RevitAddinMultiVersionWrapper.Resources.{dllName}");
        using var decompressorStream = new DecompressionStream(stream);
        using (var fileStream = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write))
        {
            decompressorStream.CopyTo(fileStream);
        }
    }
    
}