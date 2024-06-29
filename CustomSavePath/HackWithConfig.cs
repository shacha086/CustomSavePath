using GenericModConfigMenu;
using StardewHack;
using StardewModdingAPI;
using StardewModdingAPI.Events;

namespace CustomSavePath
{
    public abstract class HackWithConfig<T, TC> : HackImpl<T>
        where T : HackWithConfig<T, TC>
        where TC : class, new()
    {
        internal TC Config { get; private set; } = null!;

        // ReSharper disable once ParameterHidesMember
        public sealed override void Entry(IModHelper helper)
        {
            Config = helper.ReadConfig<TC>();
            base.Entry(helper);

            if (!helper.Reflection.GetField<bool>(this, "broken").GetValue())
            {
                OnPatched();
            }
            
            Helper.Events.GameLoop.GameLaunched += OnLaunched;
        }

        protected abstract void OnPatched();

        private void OnLaunched(object? sender, GameLaunchedEventArgs e)
        {
            var api = Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
            if (api == null)
                return;
            api.Register(ModManifest, () => Config = new TC(), (Action)(() => Helper.WriteConfig(Config)));
            InitializeApi(api);
        }

        protected abstract void InitializeApi(IGenericModConfigMenuApi api);
    }
}