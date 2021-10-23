using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestionnaireSystem.DBSouce
{
    public class Logger
    {
        public static void WriteLog(Exception ex)
        {
            string msg =
                $@" {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}
                    {ex.ToString()}
                ";

            System.IO.File.AppendAllText("C:\\Users\\p4786\\C sharp\\QuestionnaireSystem\\Log.log", msg);

            throw ex;
        }
    }
}
