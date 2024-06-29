using HarmonyLib;
using StardewModdingAPI;

// ReSharper disable InconsistentNaming UnusedType.Global UnusedMember.Global
namespace CustomSavePath;

[HarmonyPatch(typeof(Constants), "DataPath", MethodType.Getter)]
internal class ConstantsDataPath
{
    private static string Result { get; } =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "StardewValley");
        
    [HarmonyPrefix]
    public static bool Prefix(ref string __result)
    {
        __result = Result;
        return false;
    }
}

[HarmonyPatch(typeof(Constants), "LogDir", MethodType.Getter)]
internal class ConstantsLogDir
{
    private static string Result { get; } =
        Path.Combine(Constants.DataPath, "ErrorLogs");
    
    [HarmonyPrefix]
    public static bool Prefix(ref string __result)
    {
        __result = Result;
        return false;
    }
}

[HarmonyPatch(typeof(Constants), "SavesPath", MethodType.Getter)]
internal class ConstantsSavesPath
{
    private static string Result { get; } =
        Path.Combine(Constants.DataPath, "Saves");
    
    [HarmonyPrefix]
    public static bool Prefix(ref string __result)
    {
        __result = Result;
        return false;
    }
}