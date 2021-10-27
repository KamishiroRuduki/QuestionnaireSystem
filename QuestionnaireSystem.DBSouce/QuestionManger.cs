using QuestionnaireSystem.ORM.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestionnaireSystem.DBSouce
{
    public class QuestionManger
    {
        public static List<Question> GetQuestionsListByQuestionnaireID(Guid questionnaireid)
        {
            try
            {
                using (ContextModel context = new ContextModel())
                {
                    var query =
                        (from item in context.Questions
                         where item.QuestionnaireID == questionnaireid
                         orderby item.Number 
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
        public static Question GetQuestionByQuestionID(Guid questionid)
        {
            try
            {
                using (ContextModel context = new ContextModel())
                {
                    var query =
                        (from item in context.Questions
                         where item.ID == questionid
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

        public static void CreateQuestion(Question question)
        {
            try
            {
                using (ContextModel context = new ContextModel())
                {
                    context.Questions.Add(question);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);

            }
        }

        public static void UpdateQuestion(Guid questionid, Question question)
        {
            try
            {
                using (ContextModel context = new ContextModel())
                {
                    var query =
                        (from item in context.Questions
                         where item.ID == questionid
                         select item);

                    var list = query.FirstOrDefault();
                    if (list != null)
                    {
                        list.Name = question.Name;
                        list.Type = question.Type;
                        list.QusetionOption = question.QusetionOption;
                        list.IsMust = question.IsMust;
                    }
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);

            }
        }
    }
}
