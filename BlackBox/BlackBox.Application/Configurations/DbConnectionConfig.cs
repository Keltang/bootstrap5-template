namespace BlackBox.Application.Configurations
{
    public class DbConnectionConfig
    {
        public string ConnectionString { get; }

        public DbConnectionConfig(string value)
        {
            ConnectionString = value; 
        }
    }
}
