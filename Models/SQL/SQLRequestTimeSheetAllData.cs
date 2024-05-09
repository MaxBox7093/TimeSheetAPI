using Microsoft.Data.SqlClient;
using System.Threading.Tasks;

namespace TimeSheetAPI.Models.SQL
{
    public class SQLRequestTimeSheetAllData : SQLConnection
    {
        private SqlConnection connection;

        public SQLRequestTimeSheetAllData()
        {
            connection = ConnectionDB();
        }

        //Выборка всех проводок по id пользователя с ограничениями по дате
        public List<TimeSheetAllData> SelectTimeSheetByDate(int Id_user, string dateStart, string dateEnd)
        {
            List<TimeSheetAllData> dataList = new List<TimeSheetAllData>();
            string request = "SELECT ts.ts_date, ts.ts_time, ts.description, t.task_name, p.project_name FROM Time_sheet ts " +
                "INNER JOIN Task t ON ts.ts_ref = t.Id_task " +
                "INNER JOIN Project_control pc ON t.task_ref = pc.id_project " +
                "INNER JOIN Project p ON pc.id_project = p.Id_project " +
                "WHERE pc.Id_user = @Id_user AND ts.ts_date >= @dateStart AND ts.ts_date <= @dateEnd";
            var command = new SqlCommand(request, connection);
            command.Parameters.AddWithValue("@Id_user", Id_user);
            command.Parameters.AddWithValue("@dateStart", Convert.ToDateTime(dateStart));
            command.Parameters.AddWithValue("@dateEnd", Convert.ToDateTime(dateEnd));

            using (var reader = command.ExecuteReader())
            {
                // Перебираем результаты запроса
                while (reader.Read())
                {
                    TimeSheetAllData data = new TimeSheetAllData();

                    // Заполняем данные из результата запроса в объект TimeSheetAllData
                    DateTime tsDate = reader.GetDateTime(reader.GetOrdinal("ts_date"));
                    data.date_ts = tsDate.ToShortDateString();
                    data.time_ts = reader.GetInt32(reader.GetOrdinal("ts_time"));
                    data.description_ts = reader.GetString(reader.GetOrdinal("description"));
                    data.task_name = reader.GetString(reader.GetOrdinal("task_name"));
                    data.project_name = reader.GetString(reader.GetOrdinal("project_name"));

                    dataList.Add(data);
                }
            }
            return dataList;
        }

        //Выборка всех проводок по id пользователя
        public List<TimeSheetAllData> SelectAllTimeSheet(int Id_user)
        {
            List<TimeSheetAllData> dataList = new List<TimeSheetAllData>();
            string request = "SELECT ts.ts_date, ts.ts_time, ts.description, t.task_name, p.project_name FROM Time_sheet ts " +
                "INNER JOIN Task t ON ts.ts_ref = t.Id_task " +
                "INNER JOIN Project_control pc ON t.task_ref = pc.id_project " +
                "INNER JOIN Project p ON pc.id_project = p.Id_project " +
                "WHERE pc.Id_user = @Id_user";
            var command = new SqlCommand(request, connection);
            command.Parameters.AddWithValue("@Id_user", Id_user);

            using (var reader = command.ExecuteReader())
            {
                // Перебираем результаты запроса
                while (reader.Read())
                {
                    TimeSheetAllData data = new TimeSheetAllData();

                    // Заполняем данные из результата запроса в объект TimeSheetAllData
                    DateTime tsDate = reader.GetDateTime(reader.GetOrdinal("ts_date"));
                    data.date_ts = tsDate.ToShortDateString();
                    data.time_ts = reader.GetInt32(reader.GetOrdinal("ts_time"));
                    data.description_ts = reader.GetString(reader.GetOrdinal("description"));
                    data.task_name = reader.GetString(reader.GetOrdinal("task_name"));
                    data.project_name = reader.GetString(reader.GetOrdinal("project_name"));

                    dataList.Add(data);
                }
            }
            return dataList;
        }
    }
}
