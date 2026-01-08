using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace TVPGestion_IPO.Services
{
    public class DataPersistenceService<T> where T : class
    {
        private readonly string filePath;
        private readonly string dataFolder;

        public DataPersistenceService(string fileName)
        {
            // Crear carpeta Data en la raíz de la aplicación
            dataFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
            if (!Directory.Exists(dataFolder))
            {
                Directory.CreateDirectory(dataFolder);
            }

            filePath = Path.Combine(dataFolder, fileName);
        }

        public List<T> LoadData()
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    return new List<T>();
                }

                string json = File.ReadAllText(filePath);
                if (string.IsNullOrWhiteSpace(json))
                {
                    return new List<T>();
                }

                return JsonConvert.DeserializeObject<List<T>>(json) ?? new List<T>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al cargar datos desde {filePath}: {ex.Message}", ex);
            }
        }

        public void SaveData(List<T> data)
        {
            try
            {
                string json = JsonConvert.SerializeObject(data, Formatting.Indented);
                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al guardar datos en {filePath}: {ex.Message}", ex);
            }
        }

        public void AddItem(T item)
        {
            var data = LoadData();
            data.Add(item);
            SaveData(data);
        }

        public void UpdateItem(Predicate<T> match, T updatedItem)
        {
            var data = LoadData();
            int index = data.FindIndex(match);
            if (index >= 0)
            {
                data[index] = updatedItem;
                SaveData(data);
            }
        }

        public void DeleteItem(Predicate<T> match)
        {
            var data = LoadData();
            data.RemoveAll(match);
            SaveData(data);
        }
    }
}