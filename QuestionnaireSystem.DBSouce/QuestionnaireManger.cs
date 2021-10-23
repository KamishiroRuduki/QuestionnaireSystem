using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuestionnaireSystem.ORM.DBModels;

namespace QuestionnaireSystem.DBSouce
{
    public class QuestionnaireManger
    {
        public static List<Questionnaire> GetQuestionnaireList()
        {
            try
            {
                using (ContextModel context = new ContextModel())
                {
                    var query =
                        (from item in context.Questionnaires
                         orderby item.ID descending
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

        public static Questionnaire GETQuestionnaire(Guid questionnaireid)
        {
            try
            {
                using (ContextModel context = new ContextModel())
                {
                    var query =
                        (from item in context.Questionnaires
                         where item.QuestionnaireID == questionnaireid                        
                         select item);

                    var list = query.FirstOrDefault();
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
