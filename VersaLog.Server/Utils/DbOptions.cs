namespace VersaLog.Utils
{
    public class DbOptions
    {
        public string DatabaseName { get; set; } = string.Empty;
        public string Server { get; set; } = string.Empty;
        public string Port { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public string CreateConnectionString()
        {
            return $"Host={Server};Port={Port};Username={Username};Password={Password};Database={DatabaseName}";
        }
    }

}
