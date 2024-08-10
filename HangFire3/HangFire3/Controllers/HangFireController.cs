using Hangfire;
using HangFire3.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Reflection.Metadata;
using TimeZoneConverter;

namespace HangFire3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HangFireController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IReprocess _reeprocess;
        public HangFireController(IReprocess _reprocess, IConfiguration configuration)
        {
         
            _configuration = configuration;
            _reeprocess = _reprocess;
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult Recurring()
        {
              RecurringJob.AddOrUpdate("your-job-id", () => ReprocessStoreProcedureMethod(), "15 8 * * *", TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));

            //RecurringJob.AddOrUpdate(() => ReprocessStoreProcedureMethod(), Cron.Daily(3, 01));
           // RecurringJob.AddOrUpdate("Reprocess", () => ReprocessStoreProcedureMethod(), "30 3 * * *");
            return Ok($" Status : Reprocess Has Been Done Successfully");
        }

  
        [HttpPost]
        [Route("[action]")]
        public IActionResult ReprocessStoreProcedureMethod()
        {
            _reeprocess.CallStoreProcedureFirst();
            _reeprocess.CallSecondStoreProcedure();
            _reeprocess.CallThirdStoreProcedure();
            _reeprocess.CallFourthStoreProcedure();
            return Ok($" Status : Reprocess Has Been Done Successfully");
        }

        public void TraceService(string content)
        {
            string finalcontent = DateTime.Now.ToString() + " => " + content;
            string LogPath = _configuration["LogPath"];
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
