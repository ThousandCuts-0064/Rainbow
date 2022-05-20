using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow
{
    public static class Save
    {
        private static readonly BinaryFormatter _formatter = new BinaryFormatter();
        private static readonly string _directoryName;

        static Save()
        {
            _directoryName = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + $"\\{nameof(Save)}\\";
            if (!Directory.Exists(_directoryName)) Directory.CreateDirectory(_directoryName);
        }

        public static bool FileExists(string fileName) => File.Exists(_directoryName + fileName);
        public static bool DirectoryExists(string folderName) => Directory.Exists(_directoryName + folderName);
        public static void CreateDirectory(string name) =>
            Directory.CreateDirectory(
                Path.Combine(_directoryName, name));

        public static void Serialize(string fileName, object obj)
        {
            using (var stream = File.Open(_directoryName + fileName, FileMode.Create))
                _formatter.Serialize(stream, obj);
        }

        public static object Deserialize(string fileName)
        {
            object obj = null;
            using (var stream = File.OpenRead(_directoryName + fileName))
                obj = _formatter.Deserialize(stream);
            return obj;
        }
    }
}
