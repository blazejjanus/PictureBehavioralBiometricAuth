using System.Text.Json.Serialization;

namespace PictureBehavioralBiometricAuth.Shared.Config
{
    public class DbSettings
    {
        public string Url { get; set; } = string.Empty;
        public int Port { get; set; }
        public string DatabaseName { get; set; } = string.Empty;
        public string User { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        [JsonIgnore]
        public string ConnectionString => $"Server={Url};Port={Port};User Id={User};Password={Password};Database={DatabaseName};Pooling=true;SearchPath=BiometriaBehawioralna_doubtbush";
    }
}
