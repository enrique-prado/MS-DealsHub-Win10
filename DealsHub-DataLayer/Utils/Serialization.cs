using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MSDealsDataLayer.Utils
{
    // Serialization utils. Uses System.Xml.Serialization.
    public static class Serialization
    {
        public async static Task<T> Deserialize<T>(string text)
        {
            T obj = default(T);

            try
            {
                byte[] bytes = Encoding.Unicode.GetBytes(text);

                using (var stream = new MemoryStream(bytes))
                {
                    obj = await Serialization.Deserialize<T>(stream);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            return obj;
        }

        public async static Task<T> DeserializeJSON<T>(string json)
        {
            T obj = default(T);

            try
            {
                byte[] jsonBytes = Encoding.Unicode.GetBytes(json);

                using (var stream = new MemoryStream(jsonBytes))
                {
                    obj = await Serialization.DeserializeJSON<T>(stream);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            return obj;
        }

        public static Task<T> Deserialize<T>(Stream stream)
        {
            return Task.Run<T>(() =>
            {
                try
                {
                    var obj = Activator.CreateInstance<T>();
                    var deserializer = new DataContractSerializer(obj.GetType());

                    obj = (T) deserializer.ReadObject(stream);
                    return obj;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
                return default(T);
                
            });
        }

        public static Task<T> DeserializeJSON<T>(Stream stream)
        {
            return Task.Run<T>(() =>
            {
                try
                {
                    var obj = Activator.CreateInstance<T>();
                    var deserializer = new DataContractJsonSerializer(obj.GetType());

                    obj = (T) deserializer.ReadObject(stream);
                    return obj;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
                return default(T);
            });
        }

        public static Task<T> DeserializeXML<T>(Stream stream)
        {
            return Task.Run<T>(() =>
            {
                try
                {
                    var obj = Activator.CreateInstance<T>();
                    var deserializer = new XmlSerializer(obj.GetType());

                    obj = (T)deserializer.Deserialize(stream);
                    return obj;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }

                return default(T);
            });
        }
    }
}
