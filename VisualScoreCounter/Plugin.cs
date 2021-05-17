using IPA;
using IPA.Config.Stores;
using IPALogger = IPA.Logging.Logger;

namespace VisualScoreCounter
{
    [Plugin(RuntimeOptions.DynamicInit)]
    public class Plugin
    {
        internal static Plugin instance { get; private set; }
        internal static string Name => "VisualScoreCounter";

        [Init]
        public Plugin(IPALogger logger, IPA.Config.Config config)
        {
            Configuration.Instance = config.Generated<Configuration>();
            instance = this;
            Logger.log = logger;
            Logger.log.Debug("Logger initialized.");
        }

        [OnEnable]
        public void OnEnable() { }

        [OnDisable]
        public void OnDisable() { }
    }
}
