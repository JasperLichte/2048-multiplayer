using System;
using System.IO;
using Newtonsoft.Json;

namespace server.model
{
    class Config
    {
        public int maxUsers {get;set;}
        public int boardSize{get;set;}
        public long roundDuration{get;set;}
        public String databaseType{get;set;}
        public String connectionString {get;set;}
        public long updateIntervall {get;set;}


private Config(){
}
        public static Config loadConfig()
        {
            Config config = new Config();
            using (StreamReader file = File.OpenText(@"config\config.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                config = (Config)serializer.Deserialize(file, typeof(Config));
            }
            return config;
        }
        //Gets called by Newtonsoft Json Converter
        //shoul the connectionString be serialized(written to json String) 
        public bool ShouldSerializeconnectionString()
        {
            return false;
        }
        public bool ShouldSerializedatabaseType()
        {
            return false;
        }
    }
}