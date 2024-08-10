using HangFire3.IService;
using System.Data.SqlClient;
using System.Data;

namespace HangFire3.Repository
{
    public class Reprocess : IReprocess
    {
        public string ConnectionString { get; set; }
        public IConfiguration Configuration { get; }
        public Reprocess(IConfiguration configuration)
        {
            Configuration = configuration;
            ConnectionString = configuration.GetConnectionString("DataBaseContext");
        }

        public void CallStoreProcedureFirst()
        {
            // DateTime currentDate = new DateTime(2023, 8, 1);
            DateTime currentDate = DateTime.Now;
            if (currentDate.Day == 1)
            {
                DateTime yesterday = currentDate.AddDays(-1);
                DateTime firstDayOfMonth = new DateTime(yesterday.Year, yesterday.Month, 1);
                DateTime currentDateIterator = firstDayOfMonth;
                StoreProcedureGreneric(currentDateIterator, yesterday);
                string content = "SP_INSERTHRMISDATA : This SP Has Been  Run  Successfully";
                TraceService(content);
            }
            else
            {
                DateTime firstDayOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
                DateTime yesterday = currentDate.AddDays(-1);
                DateTime currentDateIterator = firstDayOfMonth;
                StoreProcedureGreneric(currentDateIterator, yesterday);
                string content = "SP_INSERTHRMISDATA : This SP Has Been  Run  Successfully";
                TraceService(content);
            }
        }

        public void StoreProcedureGreneric(DateTime currentDateIterator, DateTime yesterday)
        {
            while (currentDateIterator <= yesterday)
            {
                string DATE1 = currentDateIterator.ToString("yyyyMMdd");
                string DATE2 = currentDateIterator.ToString("dd-MMM-yyyy");
                try
                {
                    SqlConnection connection = new SqlConnection(ConnectionString);
                    
                        SqlCommand command = new SqlCommand("SP_INSERTHRMISDATA", connection);
                        command.Parameters.Add(new SqlParameter("@DATE1", SqlDbType.NVarChar) { Value = DATE1 });
                        command.Parameters.Add(new SqlParameter("@DATE2", SqlDbType.NVarChar) { Value = DATE2 });
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = 1000;
                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                    
                }
                catch (Exception ex)
                {
                    string error = ex.Message;
                    TraceService(error);
                }
                currentDateIterator = currentDateIterator.AddDays(1);
            }

        }

        void IReprocess.CallSecondStoreProcedure()
        {

            // DateTime currentDate = new DateTime(2023, 8, 1);
            DateTime currentDate = DateTime.Now;
            if (currentDate.Day == 1)
            {
                DateTime yesterday = currentDate.AddDays(-1);
                DateTime firstDayOfMonth = new DateTime(yesterday.Year, yesterday.Month, 1);
                string FROMDATE = firstDayOfMonth.ToString("yyyyMMdd");
                string TODATE = yesterday.ToString("yyyyMMdd");
                StoreprocedureSecondGeneric(FROMDATE, TODATE);
                string content1 = "UPDATE_ATTENDANCE_DATA_By_Code : This SP Has Been Run  Successfully";
                TraceService(content1);
            }
            else
            {
                DateTime firstDayOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
                DateTime yesterday = currentDate.AddDays(-1);
                string FROMDATE = firstDayOfMonth.ToString("yyyyMMdd");
                string TODATE = yesterday.ToString("yyyyMMdd");
                StoreprocedureSecondGeneric(FROMDATE, TODATE);
                string content1 = "UPDATE_ATTENDANCE_DATA_By_Code : This SP Has Been Run  Successfully";
                TraceService(content1);
            }
        }
        public void StoreprocedureSecondGeneric(string FROMDATE, string TODATE)
        {
            try
            {
                SqlConnection connection = new SqlConnection(ConnectionString);
              
                    SqlCommand command = new SqlCommand("UPDATE_ATTENDANCE_DATA_By_Code", connection);
                    command.Parameters.Add(new SqlParameter("@FROMDATE", SqlDbType.NVarChar) { Value = FROMDATE });
                    command.Parameters.Add(new SqlParameter("@TODATE", SqlDbType.NVarChar) { Value = TODATE });
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 1000;
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
               
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                TraceService(error);
            }
        }

        void IReprocess.CallThirdStoreProcedure()
        {
            try
            {

                SqlConnection connection = new SqlConnection(ConnectionString);
            
                    SqlCommand command = new SqlCommand("HRMIS_ESS_Roster_Assign", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 1000;
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                string content2 = "HRMIS_ESS_Roster_Assign : This SP Has Been Run  Successfully";
                TraceService(content2);

            }
            catch (Exception ex)
            {
                string error = ex.Message;
                TraceService(error);
            }
        }

        void IReprocess.CallFourthStoreProcedure()
        {
            try
            {
                SqlConnection connection = new SqlConnection(ConnectionString);
                
                    SqlCommand command = new SqlCommand("HRMIS_ESS_ATT_Update_Deviation_Approved", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 1000;
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                string content3 = "HRMIS_ESS_ATT_Update_Deviation_Approved : This SP Has Been Run Successfully";
                TraceService(content3);

            }
            catch (Exception ex)
            {
                string error = ex.Message;
                TraceService(error);
            }
        }

        public void TraceService(string content)
        {
            string finalcontent = DateTime.Now.ToString() + " => " + content;
            string LogPath = Configuration["LogPath"];
            if (!Directory.Exists(LogPath + @"\LOGS\"))
            {
                DirectoryInfo di = Directory.CreateDirectory(LogPath + @"\LOGS\");
            }
            FileStream fs = new FileStream(LogPath + @"\LOGS\" + "log" + DateTime.Now.Day + DateTime.Now.Month + DateTime.Now.Year + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            sw.BaseStream.Seek(0, SeekOrigin.End);
            sw.WriteLine(finalcontent);
            sw.Flush();
            sw.Close();
        }
    }
}
