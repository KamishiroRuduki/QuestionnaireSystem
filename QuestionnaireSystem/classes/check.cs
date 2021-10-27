using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuestionnaireSystem.classes
{
    public class check
    {
        public static bool IsDate(string strDate)
        {
            try
            {
                DateTime.Parse(strDate);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}