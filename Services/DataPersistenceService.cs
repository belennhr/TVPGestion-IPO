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

                var result = JsonConvert.DeserializeObject<List<T>>(json);
                return result ?? new List<T>();
            }
            catch (JsonException ex)
            {
                // Si hay error de deserialización, registrar y devolver lista vacía
                System.Diagnostics.Debug.WriteLine($"Error al deserializar {filePath}: {ex.Message}");
                return new List<T>();
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
                if (data == null)
                {
                    throw new ArgumentNullException(nameof(data), "Los datos no pueden ser nulos");
                }

                string json = JsonConvert.SerializeObject(data, Formatting.Indented, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                
                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al guardar datos en {filePath}: {ex.Message}", ex);
            }
        }

        public void AddItem(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item), "El elemento no puede ser nulo");
            }

            var data = LoadData();
            data.Add(item);
            SaveData(data);
        }

        public void UpdateItem(Predicate<T> match, T updatedItem)
        {
            if (match == null)
            {
                throw new ArgumentNullException(nameof(match));
            }
            if (updatedItem == null)
            {
                throw new ArgumentNullException(nameof(updatedItem));
            }

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
            if (match == null)
            {
                throw new ArgumentNullException(nameof(match));
            }

            var data = LoadData();
            int removedCount = data.RemoveAll(match);
            if (removedCount > 0)
            {
                SaveData(data);
            }
        }
    }
}