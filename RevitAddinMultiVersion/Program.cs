namespace RevitAddinMultiVersion;

using Autodesk.Revit.UI;

class App : IExternalApplication
{
    public Result OnStartup(UIControlledApplication application)
    {
        TaskDialog.Show("Remember, I'm always watching >:3", "MultiVersion:" + getVersion());
        return Result.Succeeded;
    }

    public Result OnShutdown(UIControlledApplication application)
    {
        return Result.Succeeded;
    }


    public string getVersion()
    {
        #if REVIT2014
                return "R14";
        #elif REVIT2015
                return "R15";
        #elif REVIT2016
                return "R16";
        #elif REVIT2017
                return "R17";
        #elif REVIT2018
                return "R18";
        #elif REVIT2019
                return "R19";
        #elif REVIT2020
                return "R20";
        #elif REVIT2021
                return "R21";
        #elif REVIT2022
                return "R22";
        #elif REVIT2023
                return "R23";
        #elif REVIT2024
                return "R24";
        #elif REVIT2025
                return "R24";
        #else
                return "Unknown version";
        #endif
    }

}