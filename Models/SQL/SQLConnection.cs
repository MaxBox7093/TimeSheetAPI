using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Data;
using System.IO;

namespace TimeSheetAPI.Models.SQL
{
    public class SQLConnection
    {
        // Строка подключения
        private string? connectionString;
        protected SqlConnection connection;

        public SQLConnection()
        {
            LoadConnectionString();
        }

        private void LoadConnectionString()
        {
            // Путь к файлу Secret.json
            string secretFilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Microsoft\\UserSecrets\\3e9284de-b6f4-4d81-878e-768c2e46f294\\secrets.json";

            // Проверяем, существует ли файл
            if (File.Exists(secretFilePath))
            {
                // Чтение содержимого файла
                string json = File.ReadAllText(secretFilePath);

                // Десериализация JSON в объект
                dynamic? secretObject = JsonConvert.DeserializeObject(json);

                // Извлечение строки подключения
                connectionString = secretObject?.DefaultConnection;
            }
            else
            {
                // Если файл не найден, выводим сообщение об ошибке или предпринимаем соответствующие действия
                Console.WriteLine("Файл Secret.json не найден.");
            }
        }

        public SqlConnection ConnectionDB()
        {
            // Проверяем, была ли успешно загружена строка подключения
            if (!string.IsNullOrEmpty(connectionString))
            {
                // Создаем подключение к базе данных
                connection = new SqlConnection(connectionString);
                connection.Open();
            }
            else
            {
                // В случае, если строка подключения не была загружена, выводим сообщение об ошибке или предпринимаем соответствующие действия
                Console.WriteLine("Не удалось загрузить строку подключения из файла Secret.json.");
            }

        return connection;
        }
    }
}
