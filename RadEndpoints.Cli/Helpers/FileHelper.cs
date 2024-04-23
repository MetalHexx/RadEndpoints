using Newtonsoft.Json;

namespace RadEndpoints.Cli.Helpers
{
    /// <summary>
    /// A quick and dirty file helper utility to save / fetch object data as json.  
    /// </summary>
    public static class FileHelper
    {
        /// <summary>
        /// Saves object data to a file in json format.
        /// </summary>
        /// <param name="data">data to save</param>
        /// <param name="path">path to save the data to</param>
        public static void SaveDataToFile(object data, string path)
        {
            File.WriteAllText
            (
                path,
                JsonConvert.SerializeObject
                (
                    data,
                    Formatting.Indented,
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Auto,
                        Formatting = Formatting.Indented
                    }
                )
            );
        }

        /// <summary>
        /// Gets object data given a type and path
        /// </summary>
        /// <typeparam name="T">type of data to deserialize</typeparam>
        /// <param name="filePath">path to the json file</param>
        /// <returns></returns>
        public static T? GetDataFromFile<T>(string filePath)
        {
            using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
            using (var reader = new StreamReader(stream))
            {
                var content = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<T>(content);
            }
        }

        /// <summary>
        /// Gets string content from a given file
        /// </summary>
        /// <typeparam name="T">type of data to deserialize</typeparam>
        /// <param name="filePath">path to the json file</param>
        /// <returns></returns>
        public static string GetFileAsString(string filePath)
        {
            using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
            using (var reader = new StreamReader(stream))
                return reader.ReadToEnd();
        }
    }
}