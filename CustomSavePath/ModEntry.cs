using System.Reflection;
using System.Reflection.Emit;
using GenericModConfigMenu;
using HarmonyLib;
using StardewHack;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;
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
                string dirPath;
                dirPath = attr.HasFlag(FileAttributes.Directory)
                    ? Path.GetFullPath(config.Path)
                    : Path.GetDirectoryName(config.Path)!;
                Monitor.Log("Custom Save Path: " + dirPath, LogLevel.Info);
                PatchEnumerator(typeof(SaveGame), "getSaveEnumerator", SaveGame_getSaveEnumerator);
                PatchEnumerator(typeof(SaveGame), "getLoadEnumerator", SaveGame_getLoadEnumerator);
                PatchCConstructor(typeof(Constants), Constants_cctor);
                Patch(typeof(SaveGame), "IsNewGameSaveNameCollision", SaveGame_IsNewGameSaveNameCollision);
                Patch(typeof(SaveGame), "ensureFolderStructureExists", SaveGame_ensureFolderStructureExists);
                Patch(typeof(StartupPreferences), "ensureFolderStructureExists", StartupPreferences_ensureFolderStructureExists);
                Patch(typeof(StartupPreferences), "_savePreferences", StartupPreferences__savePreferences);
                Patch(typeof(StartupPreferences), "_loadPreferences", StartupPreferences__loadPreferences);
                Patch(typeof(LoadGameMenu), "FindSaveGames", LoadGameMenu_FindSaveGames);
                Patch(typeof(LoadGameMenu), "deleteFile", LoadGameMenu_deleteFile);
                // Patch(typeof(Environment), "GetFolderPath", Environment_GetFolderPath, typeof(Environment.SpecialFolder), typeof(Environment.SpecialFolderOption));
                
            }
            catch (Exception e) when (e is FileNotFoundException or DirectoryNotFoundException)
            {
                Monitor.Log($"Wrong path '{config.Path}'", LogLevel.Error);
            }
        }

        private void LoadGameMenu_deleteFile()
        {
            var code = FindCode(
                Ldc_I4_S(26), // code to replace,
                Call(typeof(Environment), "GetFolderPath", typeof(Environment.SpecialFolder)) // code to replace
            );

            code.Splice(0, 2, Ldstr(config.Path));
        }

        private void LoadGameMenu_FindSaveGames()
        {
            var code = FindCode(
                Ldc_I4_S(26), // code to replace,
                Call(typeof(Environment), "GetFolderPath", typeof(Environment.SpecialFolder)) // code to replace
            );

            code.Splice(0, 2, Ldstr(config.Path));
        }

        private void StartupPreferences__savePreferences()
        {
            var code = FindCode(
                Ldc_I4_S(26), // code to replace,
                Call(typeof(Environment), "GetFolderPath", typeof(Environment.SpecialFolder)) // code to replace
            );

            code.Splice(0, 2, Ldstr(config.Path));
        }      
        private void StartupPreferences__loadPreferences()
        {
            var code = FindCode(
                Ldc_I4_S(26), // code to replace,
                Call(typeof(Environment), "GetFolderPath", typeof(Environment.SpecialFolder)) // code to replace
            );

            code.Splice(0, 2, Ldstr(config.Path));
        }

        private void StartupPreferences_ensureFolderStructureExists()
        {
            var code = FindCode(
                Ldc_I4_S(26), // code to replace,
                Call(typeof(Environment), "GetFolderPath", typeof(Environment.SpecialFolder)) // code to replace
            );

            code.Splice(0, 2, Ldstr(config.Path));
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

        private void Environment_GetFolderPath()  // tried to do this but no
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
                new CodeInstruction(OpCodes.Ceq),
                Stloc_0(),
                Ldloc_0(),
                Brfalse(label),
                Ldstr(config.Path),
                Br(label2)
            );
        }

        private void PatchCConstructor(Type type, Action patch)
        {
            var method = AccessTools.DeclaredConstructor(type, null, true);
            if (method == null)
                throw new Exception("Failed to find cctor in " + type.FullName + ".");
            ChainPatch(method, patch.Method);
        }

        private void Constants_cctor()
        {
            var code = FindCode(
                Ldc_I4_S(26), // code to replace,
                Call(typeof(Environment), "GetFolderPath", typeof(Environment.SpecialFolder)) // code to replace
            );

            code.Splice(0, 2, Ldstr(config.Path));
        }

        private void SaveGame_getLoadEnumerator()
        {
            var code = FindCode(
                Ldc_I4_S(26), // code to replace,
                Call(typeof(Environment), "GetFolderPath", typeof(Environment.SpecialFolder)) // code to replace
            );

            code.Splice(0, 2, Ldstr(config.Path));
        }

        private void SaveGame_ensureFolderStructureExists()
        {
            var code = FindCode(
                Ldc_I4_S(26), // code to replace,
                Call(typeof(Environment), "GetFolderPath", typeof(Environment.SpecialFolder)) // code to replace
            );

            code.Splice(0, 2, Ldstr(config.Path));

            var code2 = FindCode(
                Ldc_I4_S(26), // code to replace,
                Call(typeof(Environment), "GetFolderPath", typeof(Environment.SpecialFolder)) // code to replace
            );

            code2.Splice(0, 2, Ldstr(config.Path));
        }

        private void SaveGame_IsNewGameSaveNameCollision()
        {
            var code = FindCode(
                Ldc_I4_S(26), // code to replace,
                Call(typeof(Environment), "GetFolderPath", typeof(Environment.SpecialFolder)) // code to replace
            );

            code.Splice(0, 2, Ldstr(config.Path));
        }

        private void SaveGame_getSaveEnumerator()
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

        private void PatchEnumerator(Type type, string methodName, Action patch)
        {
            var declaredMethod = AccessTools.DeclaredMethod(type, methodName);
            var method = AccessTools.EnumeratorMoveNext(declaredMethod);
            if (method == null)
                throw new Exception("Failed to find method \"" + methodName + "\" in " + type.FullName + ".");
            ChainPatch(method, patch.Method);
        }
    }
}