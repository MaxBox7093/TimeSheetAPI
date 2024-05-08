using Microsoft.Data.SqlClient;
using System.Threading.Tasks;

namespace TimeSheetAPI.Models.SQL
{
    public class SQLRequestTimeSheet : SQLConnection
    {
        private SqlConnection connection;

        public SQLRequestTimeSheet()
        {
            connection = ConnectionDB();
        }

        //Добавление нового TimeSheet по id задачи
        public void AddNewTimeSheet(TimeSheet timeSheet)
        {
            try
            {
                DateTime date = DateTime.Parse(timeSheet.date);
                int time = timeSheet.time;
                string? description = timeSheet.description;
                int ts_ref = timeSheet.ts_ref;

                //Запрос на добавление нового проекта
                string request = "INSERT INTO Time_sheet (ts_date, ts_time, description, ts_ref) VALUES (@ts_date, @ts_time, @description, @ts_ref)";
                var command = new SqlCommand(request, connection);
                command.Parameters.AddWithValue("@ts_date", date);
                command.Parameters.AddWithValue("@ts_time", time);
                command.Parameters.AddWithValue("@description", description);
                command.Parameters.AddWithValue("@ts_ref", ts_ref);

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при добавлении проекта: " + ex.Message);
            }
        }

        //Получение проводок по id задачи
        public List<TimeSheet> SelectTimeSheet(int Id_task)
        {
            List<TimeSheet> timeSheets = new List<TimeSheet>();

            try
            {
                // Запрос на выборку всех проектов для конкретного пользователя
                string request = "SELECT * FROM Time_sheet WHERE ts_ref = @Id_task";
                var command = new SqlCommand(request, connection);
                command.Parameters.AddWithValue("@Id_task", Id_task);

                using (var reader = command.ExecuteReader())
                {
                    // Перебираем результаты запроса
                    while (reader.Read())
                    {
                        TimeSheet ts = new TimeSheet();

                        // Получаем данные о проекте
                        ts.id = reader.GetInt32(reader.GetOrdinal("Id_ts"));
                        DateTime tsDate = reader.GetDateTime(reader.GetOrdinal("ts_date"));
                        ts.date = tsDate.ToShortDateString();
                        ts.time = reader.GetInt32(reader.GetOrdinal("ts_time"));
                        ts.description = reader.GetString(reader.GetOrdinal("description"));
                        ts.ts_ref = reader.GetInt32(reader.GetOrdinal("ts_ref"));

                        timeSheets.Add(ts);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при выборе проектов: " + ex.Message);
            }

            return timeSheets;
        }

        //Удаление проводки
        public void DeleteTimeSheet(ref int id_ts)
        {
            try
            {
                string request = "DELETE Time_sheet WHERE Id_ts=@id_ts";
                var command = new SqlCommand(request, connection);
                command.Parameters.AddWithValue("@id_ts", id_ts);

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при удалении задачи: " + ex.Message);
            }
        }

        //Изменение проводки по её экземпляру
        public void UpadateTimeSheet(TimeSheet ts) 
        {
            int id_ts = ts.id;
            string new_ts_date = ts.date;
            int new_ts_time = ts.time;
            string? new_description = ts.description;

            try
            {
                //Запрос на обновление данных проекта
                string request = "UPDATE Time_sheet SET ts_date = @new_ts_date, ts_time = @new_ts_time, description = @new_description" +
                    "  WHERE Id_ts = @id_ts";
                var command = new SqlCommand(request, connection);
                command.Parameters.AddWithValue("@id_ts", id_ts);
                command.Parameters.AddWithValue("@new_ts_date", new_ts_date);
                command.Parameters.AddWithValue("@new_ts_time", new_ts_time);
                command.Parameters.AddWithValue("@new_description", new_description);

                command.ExecuteNonQuery();
            }
            catch (Exception ex) 
            {
                Console.WriteLine("Ошибка при обновлении задачи" + ex.Message);
            }
        }
    }
}
