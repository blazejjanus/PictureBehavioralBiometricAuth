using System.Text.Json;
using System.Diagnostics;

namespace PictureBehavioralBiometricAuth.Shared.Config {
    public class AppSettings {
        /// <summary>
        /// Database access settings
        /// </summary>
        public DbSettings DbSettings { get; set; } = new DbSettings();
        /// <summary>
        /// Whether to show debug only view
        /// </summary>
        public bool DebugMode { get; set; }
        /// <summary>
        /// % of similarity between registration behavior and login behavior required to pass login
        /// </summary>
        public int LoginPassThreshold { get; set; } = 90;

        public AppSettings() {
            DebugMode = Debugger.IsAttached;
        }

        private static string _settingsPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PictureBehavioralBiometricAuth", "config.json");

        public static AppSettings ReadSettings() {
            try {
                return JsonSerializer.Deserialize<AppSettings>(File.ReadAllText(_settingsPath)) ?? throw new ArgumentNullException("Deserialized config was null!");
            } catch (Exception exc) {
                throw new ConfigurationException("Failed to read configuration file.", exc);
            }
        }

        public void WriteSettings() {
            if (!Directory.Exists(Path.GetDirectoryName(_settingsPath)))
                Directory.CreateDirectory(Path.GetDirectoryName(_settingsPath));
            File.WriteAllText(_settingsPath, JsonSerializer.Serialize(this, new JsonSerializerOptions() { WriteIndented = true }));
        }
    }
}
