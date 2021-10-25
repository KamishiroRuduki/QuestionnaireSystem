using QuestionnaireSystem.ORM.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestionnaireSystem.DBSouce
{
    public class OptionManger
    {
        public static void UpdateStaticSum(Guid questionnaireid, Guid questionid, string option)
        {
            try
            {
                using (ContextModel context = new ContextModel())
                {
                    var query =
                        (from item in context.Statics
                         where item.QuestionID == questionid && item.QuestionOption == option
                         select item);

                    var list = query.FirstOrDefault();
                    if (list != null)
                    {
                        list.Sum += 1;
                    }
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);

            }
        }

        public static List<Static> GetStaticByQuestionID(Guid questionnid)
        {
            try
            {
                using (ContextModel context = new ContextModel())
                {
                    var query =
                        (from item in context.Statics
                         where item.QuestionID == questionnid
                         orderby item.ID
                         select item);

                    var list = query.ToList();
                    return list;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
                return null;

            }
        }

    }
}
