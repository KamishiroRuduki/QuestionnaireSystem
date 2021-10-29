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
        /// <summary>
        /// 找到該問卷的所有問題的資料
        /// </summary>
        /// <param name="questionnaireid"></param>
        /// <returns></returns>
        public static List<Question> GetQuestionsListByQuestionnaireID(Guid questionnaireid)
        {
            try
            {
                using (ContextModel context = new ContextModel())
                {
                    var query =
                        (from item in context.Questions
                         where item.QuestionnaireID == questionnaireid && item.IsDel == false
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
        /// <summary>
        /// 依問題ID抓該筆問題的資料
        /// </summary>
        /// <param name="questionid"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 新增問題
        /// </summary>
        /// <param name="question"></param>
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
        /// <summary>
        /// 更新問題
        /// </summary>
        /// <param name="questionid"></param>
        /// <param name="question"></param>
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
        /// <summary>
        /// 將該問題設為已刪除(假刪除)
        /// </summary>
        /// <param name="questionnaireid"></param>
        /// <param name="id"></param>
        public static void DelQuestion(Guid questionnaireid, int id)//找到那張問卷的第幾個問題
        {
            try
            {
                using (ContextModel context = new ContextModel())
                {
                    var query =
                        (from item in context.Questions
                         where item.Number == id && item.QuestionnaireID == questionnaireid
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
