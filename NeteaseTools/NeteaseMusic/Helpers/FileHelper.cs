using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NeteaseMusic.Helpers
{
    public static class FileHelper
    {
        public static string parentDirectory = Environment.CurrentDirectory;
        public static void SaveJsonFile(string data, string path)
        {
            File.WriteAllText(PathHander(path,true), data);
        }
        public static void SaveJsonFile(object obj, string path)
        {
            var data = JsonConvert.SerializeObject(obj);
            SaveJsonFile(data,path);
        }
        public static string ReadJsonFile(string path)
        {
            return File.ReadAllText(PathHander(path));
        }
        public static JObject ReadJsonFile2JObj(string path)
        {
            return JObject.Parse(ReadJsonFile(path));
        }
        public static List<JToken> ReadJsonFile2JTokens(string path)
        {
            return JsonConvert.DeserializeObject<List<JToken>>(ReadJsonFile(path));
        }
        public static string PathHander(string path,bool isSave = false)
        {
            if (!path.Contains(":"))
            {
                path = parentDirectory + "/" + path;
               
            }
            //path = path.Replace("/", "\\");
            if (isSave)
            {
                string directory = Path.GetDirectoryName(path);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
            }
            return path;
        }
        public static string PathJointForSave(string filename,string type,string defaultName)
        {
            if (string.IsNullOrEmpty(filename))
            {
                filename =  Path.Combine(type, defaultName + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".json");
            }
            else if(!filename.Contains(":"))
            {
                filename =  Path.Combine(type,filename);
            }
            if (string.IsNullOrEmpty(Path.GetExtension(filename)))
            {
                filename = filename + ".json";
            }
            return filename;
        }
    }
}
