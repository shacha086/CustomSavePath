using HarmonyLib;
using StardewModdingAPI;
using StardewValley;

// ReSharper disable InconsistentNaming UnusedType.Global UnusedMember.Global
namespace CustomSavePath;

[HarmonyPatch(typeof(Constants), "DataPath", MethodType.Getter)]
[HarmonyPriority(Priority.First)]
internal class ConstantsDataPath
{
    private static string Result { get; } =
        Path.Combine(Program.GetAppDataFolder(), "StardewValley");
        
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