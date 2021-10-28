using QuestionnaireSystem.ORM.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestionnaireSystem.DBSouce
{
    public class CommonQuestionManager
    {
        public static List<CommonQuestion> GetCommonQuestionsList()
        {
            try
            {
                using (ContextModel context = new ContextModel())
                {
                    var query =
                        (from item in context.CommonQuestions
                         where  item.IsDel == false
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

        public static CommonQuestion GetCommonQuestionByID(int questionid)
        {
            try
            {
                using (ContextModel context = new ContextModel())
                {
                    var query =
                        (from item in context.CommonQuestions
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
        public static void CreateCommonQuestion(CommonQuestion question)
        {
            try
            {
                using (ContextModel context = new ContextModel())
                {
                    context.CommonQuestions.Add(question);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);

            }
        }

        public static void UpdateCommonQuestion(int id, CommonQuestion question)
        {
            try
            {
                using (ContextModel context = new ContextModel())
                {
                    var query =
                        (from item in context.CommonQuestions
                         where item.ID == id
                         select item);

                    var list = query.FirstOrDefault();
                    if (list != null)
                    {
                        list.Name = question.Name;
                        list.Type = question.Type;
                        list.QusetionOption = question.QusetionOption;                        
                    }
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);

            }
        }
        public static void DelCommonQuestion( int id)//找到那張問卷的第幾個問題
        {
            try
            {
                using (ContextModel context = new ContextModel())
                {
                    var query =
                        (from item in context.CommonQuestions
                         where item.ID == id 
                         select item);

                    var list = query.FirstOrDefault();
                    if (list != null)
                    {
                        list.IsDel = true;
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
