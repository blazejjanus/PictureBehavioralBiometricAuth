using PictureBehavioralBiometricAuth.Shared;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PictureBehavioralBiometricAuth.Db {
    public class DbSettings {
        public string Url { get; set; } = string.Empty;
        public int Port { get; set; }
        public string DatabaseName { get; set; } = string.Empty;
        public string User { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        [JsonIgnore]
        public string ConnectionString => $"Server={Url};Port={Port};User Id={User};Password={Password};Database={DatabaseName};Pooling=true;SearchPath=BiometriaBehawioralna_doubtbush";

        private static string _settingsPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PictureBehavioralBiometricAuth", "config.json");

        public static DbSettings ReadDbSettings() {
            try {
                return JsonSerializer.Deserialize<DbSettings>(File.ReadAllText(_settingsPath)) ?? throw new ArgumentNullException("Deserialized config was null!");
            }catch(Exception exc) {
                throw new ConfigurationException("Failed to read configuration file.", exc);
            }
        }

        public void WriteDbSettings() {
            if (!Directory.Exists(Path.GetDirectoryName(_settingsPath)))
                Directory.CreateDirectory(Path.GetDirectoryName(_settingsPath));
            File.WriteAllText(_settingsPath, JsonSerializer.Serialize(this, new JsonSerializerOptions() { WriteIndented = true }));
        }
    }
}
