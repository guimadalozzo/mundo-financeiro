using System;
using System.IO;
using Newtonsoft.Json;

namespace MundoFinanceiro.Database.Utils
{
    internal static class Settings
    {
        static Settings()
        {
            // Read configuration file
            var json = File.ReadAllText($"{Environment.CurrentDirectory}/settings.json");
            var obj = JsonConvert.DeserializeObject<EnvironmentSettings>(json);

            ConnectionString = obj.ConnectionString;
        }
        
        internal static string ConnectionString { get; }
        
        #region Internal
        // ReSharper disable once ClassNeverInstantiated.Local
        private class EnvironmentSettings
        {
            // ReSharper disable once MemberHidesStaticFromOuterClass
            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            public string ConnectionString { get; set; }
        }
        #endregion

    }
}