namespace RevitAddinMultiVersionWrapper;

using System.Reflection;
using Autodesk.Revit.UI;
using ZstdSharp;

[Serializable]
class App : MarshalByRefObject, IExternalApplication
{

    private const string RevitAppClass = "RevitAddinMultiVersion.App";
    private const string DefaultVersion = "R25";
    
    //private AppDomain? _dllAppDomain;
    private IExternalApplication? _dllInstance;
    
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

        var currentSetup = AppDomain.CurrentDomain.SetupInformation;

        var appDomainSetup = new AppDomainSetup
        {
            ApplicationBase = currentSetup.ApplicationBase,
            PrivateBinPath = currentSetup.PrivateBinPath,
            ConfigurationFile = currentSetup.ConfigurationFile
        };
        
        //_dllAppDomain = AppDomain.CreateDomain("RevitAddinMultiVersion", null, appDomainSetup);
        //_dllAppDomain.AssemblyResolve += DllAppDomain_AssemblyResolve;

        /*
        var assemblyName = AssemblyName.GetAssemblyName(TempFilePath).FullName;

        try
        {
            _dllInstance = _dllAppDomain.CreateInstanceAndUnwrap(
                assemblyName,
                RevitAppClass
            ) as IExternalApplication;
        }
        catch (Exception ex)
        {
            TaskDialog.Show("CreateInstance Error", ex.ToString());
            return Result.Failed;
        }*/
        
        _dllInstance = Assembly.LoadFrom(TempFilePath)
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
       if (_dllAppDomain != null)
       {
           AppDomain.Unload(_dllAppDomain);
       }*/
        
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
    
    private Assembly DllAppDomain_AssemblyResolve(object sender, ResolveEventArgs args)
    {
        // First, check if the assembly is already loaded in the default AppDomain
        var loadedAssembly = AppDomain.CurrentDomain.GetAssemblies()
            .FirstOrDefault(a => a.FullName == args.Name);

        if (loadedAssembly != null)
        {
            // Load the assembly bytes into the new AppDomain
            var assemblyPath = loadedAssembly.Location;
            return Assembly.LoadFrom(assemblyPath);
        }

        // Attempt to load the assembly from Revit's installation directory
        var assemblyName = new AssemblyName(args.Name).Name + ".dll";

        // Attempt to load the assembly from the add-in's directory
        var addinAssemblyPath = Path.Combine(
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
            assemblyName);

        if (File.Exists(addinAssemblyPath))
        {
            return Assembly.LoadFrom(addinAssemblyPath);
        }

        // Attempt to load the assembly from the temporary directory where the decompressed DLL is
        var tempAssemblyPath = Path.Combine(
            Path.GetDirectoryName(TempFilePath),
            assemblyName);

        if (File.Exists(tempAssemblyPath))
        {
            return Assembly.LoadFrom(tempAssemblyPath);
        }

        // If the assembly cannot be found, return null
        return null;
    }
    
}