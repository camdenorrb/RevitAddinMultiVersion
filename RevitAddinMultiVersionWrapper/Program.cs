namespace RevitAddinMultiVersionWrapper;

using System.Reflection;
using Autodesk.Revit.UI;
using ZstdSharp;

class App : IExternalApplication
{
    
    private const string DefaultVersion = "R25";
    private static readonly string TempFilePath = Path.GetTempPath();
    
    public Result OnStartup(UIControlledApplication application)
    {

        var assembly = Assembly.GetExecutingAssembly();
        
        switch (application.ControlledApplication.VersionNumber)
        {
            case "2014":
                LoadDll(assembly, "R14", TempFilePath);
                break;
            case "2015":
                LoadDll(assembly, "R15", TempFilePath);
                break;
            case "2016":
                LoadDll(assembly, "R16", TempFilePath);
                break;
            case "2017":
                LoadDll(assembly, "R17", TempFilePath);
                break;
            case "2018":
                LoadDll(assembly, "R18", TempFilePath);
                break;
            case "2019":
                LoadDll(assembly, "R19", TempFilePath);
                break;
            case "2020":
                LoadDll(assembly, "R20", TempFilePath);
                break;
            case "2021":
                LoadDll(assembly, "R21", TempFilePath);
                break;
            case "2022":
                LoadDll(assembly, "R22", TempFilePath);
                break;
            case "2023":
                LoadDll(assembly, "R23", TempFilePath);
                break;
            case "2024":
                LoadDll(assembly, "R24", TempFilePath);
                break;
            case "2025":
                LoadDll(assembly, "R25", TempFilePath);
                break;
            default:
                LoadDll(assembly, DefaultVersion, TempFilePath);
                break;
        }
    
        return Result.Succeeded;
    }

    public Result OnShutdown(UIControlledApplication application)
    {
        if (File.Exists(TempFilePath))
        {
            File.Delete(TempFilePath);
        }

        return Result.Succeeded;
    }

    private void LoadDll(Assembly assembly, string version, string tempFilePath)
    {
        
        var dllName = $"RevitAddinMultiVersion{version}.zstd";

        using var stream = assembly.GetManifestResourceStream("RevitAddinMultiVersionWrapper.Resources." + dllName);
        using var decompressorStream = new DecompressionStream(stream);
        using (var fileStream = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write))
        {
            decompressorStream.CopyTo(fileStream);
        }

        Assembly.LoadFrom(tempFilePath);
    }
}