namespace RevitAddinMultiVersion;

using Autodesk.Revit.UI;

[Serializable]
class App : IExternalApplication
{
    public Result OnStartup(UIControlledApplication application)
    {
        return Result.Succeeded;
    }

    public Result OnShutdown(UIControlledApplication application)
    {
        return Result.Succeeded;
    }
}