using GenericModConfigMenu;
using StardewHack;
using StardewModdingAPI;
using StardewValley;
using static StardewHack.Instructions;

namespace CustomSavePath
{
    public class ModConfig
    {
        public string Path { get; set; } = string.Empty;
    }

    public class ModEntry : HackWithConfig<ModEntry, ModConfig>
    {
        // ReSharper disable once ParameterHidesMember
        public override void HackEntry(IModHelper helper)
        {
            if (config.Path == "")
            {
                Monitor.Log("Custom Save Path has not been set, ignoring...", LogLevel.Info);
                return;
            }

            try
            {
                var attr = File.GetAttributes(config.Path);
                var dirPath = attr.HasFlag(FileAttributes.Directory)
                    ? Path.GetFullPath(config.Path)
                    : Path.GetDirectoryName(config.Path)!;
                Monitor.Log("Custom Save Path: " + dirPath, LogLevel.Info);
                Patch(typeof(Program), "GetAppDataFolder", Program_GetAppDataFolder);
            }
            catch (Exception e) when (e is FileNotFoundException or DirectoryNotFoundException)
            {
                Monitor.Log($"Wrong path '{config.Path}'", LogLevel.Error);
            }
        }

        private void Program_GetAppDataFolder()
        {
            var code = FindCode(
                Ldc_I4_S(26), // code to replace,
                Call(typeof(Environment), "GetFolderPath", typeof(Environment.SpecialFolder)) // code to replace
            );

            code.Splice(0, 2, Ldstr(config.Path));
        }

        protected override void InitializeApi(IGenericModConfigMenuApi api)
        {
            api.AddTextOption(
                mod: ModManifest,
                name: () => "Custom Save Path",
                tooltip: () => "Restart needed.",
                getValue: () => config.Path,
                setValue: path => config.Path = path
            );
        }

    }
}