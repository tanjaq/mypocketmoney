using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApplication2.DomainObjects;

namespace WpfApplication2.Repos
{
    public class LocalXmlStorage
    {

        private const string filename = "file.json";
        public string SerializeAndSaveFile(RepoCollection records)
        {
            var serializedString = Newtonsoft.Json.JsonConvert.SerializeObject(records);
            StreamWriter writer = new StreamWriter(filename);
            writer.Write(serializedString);
            writer.Flush();
            writer.Close();
            return serializedString;
        }

        public RepoCollection DeserializeFile()
        {
            StreamReader reader = new StreamReader(filename);
            var json = reader.ReadToEnd();
            var records = Newtonsoft.Json.JsonConvert.DeserializeObject<RepoCollection>(json);
            return records;

        }
    }
}
