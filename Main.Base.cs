using System.Linq.Expressions;

namespace SFrequencyInfo
{
    // this base handling shutdown and multiple instances of this component
    partial class Main
    {
        Main() { }
        public override void LoadPlugin()
        {
            AssertInstance(false);
            Instance = this;
            base.LoadPlugin();
            StartHandlingShutdown();
        }
        public override void UnloadPlugin(PluginState state = PluginState.Unloaded)
        {
            base.UnloadPlugin(state);
            Instance = null;
        }
        static bool InstanceExists() => Instance is not null;
        static void AssertInstance(bool exists)
        {
            if (InstanceExists() != exists)
                throw new Exception($"{nameof(SFrequencyInfo)}.{nameof(Main)} instance already exists, use {nameof(Main)}.{nameof(Instance)} to access it's instance.");
        }
        public static Main Instance { get; private set; }
        void StopHandlingShutdown()
        {
            Application.quitting -= OnShutdown;
            Provider.onCommenceShutdown -= OnShutdown;
        }
        void StartHandlingShutdown()
        {
            StopHandlingShutdown();
            Application.quitting += OnShutdown;
            Provider.onCommenceShutdown += OnShutdown;
        }
        void OnShutdown()
        {
            AssertInstance(true);
            Instance = null;
            StopHandlingShutdown();
            UnloadPlugin();
        }
        void OnApplicationQuit() => OnShutdown();
        void OnDestroy() => OnShutdown();
    }
}