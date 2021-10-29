using QuestionnaireSystem.ORM.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestionnaireSystem.DBSouce
{
    public class AnswerManger
    {
        /// <summary>
        /// 將回答寫進DB
        /// </summary>
        /// <param name="answer"></param>
        public static void CreateAnswer(Answer answer)
        {
            try
            {
                using (ContextModel context = new ContextModel())
                {
                    context.Answers.Add(answer);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);

            }
        }
        /// <summary>
        /// 依問題ID，該到該回答者在該問題所做的回答
        /// </summary>
        /// <param name="quesid"></param>
        /// <param name="personid"></param>
        /// <returns></returns>
        public static Answer GetAnswer(Guid quesid, Guid personid)
        {
            try
            {
                using (ContextModel context = new ContextModel())
                {
                    var query =
                        (from item in context.Answers
                         where item.PersonID == personid && item.QuestionID == quesid
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
