using System.Reflection;
using GenericModConfigMenu;
using HarmonyLib;
using StardewModdingAPI;
using static StardewHack.Instructions;

namespace CustomSavePath;

public class ModConfig
{
    public string Path { get; set; } = string.Empty;
}

// ReSharper disable once UnusedType.Global
public class ModEntry : HackWithConfig<ModEntry, ModConfig>
{
    // ReSharper disable once ParameterHidesMember
    public override void HackEntry(IModHelper helper)
    {
        if (Config.Path == "")
        {
            Monitor.Log("Custom Save Path has not been set, ignoring...", LogLevel.Info);
            return;
        }

        try
        {
            var attr = File.GetAttributes(Config.Path);
            var dirPath = attr.HasFlag(FileAttributes.Directory)
                ? Path.GetFullPath(Config.Path)
                : Path.GetDirectoryName(Config.Path)!;
            Monitor.Log("Custom Save Path: " + dirPath, LogLevel.Info);
            Patch(typeof(Environment), "GetFolderPath", Environment_GetFolderPath, typeof(Environment.SpecialFolder), typeof(Environment.SpecialFolderOption));
        }
        catch (Exception e) when (e is FileNotFoundException or DirectoryNotFoundException)
        {
            Monitor.Log($"Wrong path '{Config.Path}'", LogLevel.Error);
        }
    }

    private void Patch(Type type, string methodName, Action patch, params Type[] parameters)
    {
        Type[]? param = null;
        if (parameters.Length > 0) param = parameters;
        var method = AccessTools.DeclaredMethod(type, methodName, param);
        if (method == null)
            throw new Exception("Failed to find method \"" + methodName + "\" in " + type.FullName + ".");
        ChainPatch(method, patch.Method);
    }

    private void Environment_GetFolderPath()
    {
        var a = AllCode();
        var start = a.Start[0];
        var end = a.End[-1];
        var label = generator.DefineLabel();
        var label2 = generator.DefineLabel();
        start.labels.Add(label);
        end.labels.Add(label2);
        a.Insert(0,
            Ldarg_0(),
            Ldc_I4_S(26),
            Bne_Un(label),
            Ldstr(Config.Path),
            Br(label2)
        );
    }

    protected override void OnPatched()
    {
        harmony.PatchAll(Assembly.GetExecutingAssembly());
    }

    protected override void InitializeApi(IGenericModConfigMenuApi api)
    {
        api.AddTextOption(
            mod: ModManifest,
            name: () => "Custom Save Path",
            tooltip: () => "Restart needed.",
            getValue: () => Config.Path,
            setValue: path => Config.Path = path
        );
    }
}