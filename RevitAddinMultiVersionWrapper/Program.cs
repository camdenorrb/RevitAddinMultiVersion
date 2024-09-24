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
        
        DecompressDll(assembly, version, TempFilePath);
        
        _dllAppDomain = AppDomain.CreateDomain("RevitAddinMultiVersion");
        var assemblyName = AssemblyName.GetAssemblyName(TempFilePath).FullName;
        
        _dllInstance = _dllAppDomain.CreateInstanceAndUnwrap(assemblyName, RevitAppClass);
        if (_dllInstance == null)
        {
            return Result.Failed;
        }
        
        DllCallOnStartup(_dllInstance, application);
        
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

        using (var stream = assembly.GetManifestResourceStream($"RevitAddinMultiVersionWrapper.Resources.{dllName}"))
        using (var decompressorStream = new DecompressionStream(stream))
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
    
}