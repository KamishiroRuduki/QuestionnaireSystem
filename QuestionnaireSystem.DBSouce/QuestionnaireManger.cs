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
        /// <summary>
        /// 找到沒被設為已刪除的問卷
        /// </summary>
        /// <returns></returns>
        public static List<Questionnaire> GetQuestionnaireList()
        {
            try
            {
                using (ContextModel context = new ContextModel())
                {
                    var query =
                        (from item in context.Questionnaires
                         where item.IsStart == 1
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
        /// <summary>
        /// 用抓該筆問卷資料
        /// </summary>
        /// <param name="questionnaireid"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 新增問卷
        /// </summary>
        /// <param name="questionnaire"></param>
        public static void CreateQuestionnaire(Questionnaire questionnaire)
        {
            try
            {
                using (ContextModel context = new ContextModel())
                {
                    context.Questionnaires.Add(questionnaire);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);

            }
        }
        /// <summary>
        /// 更新問卷資料
        /// </summary>
        /// <param name="questionnaireid"></param>
        /// <param name="questionnaire"></param>
        public static void UpdateQuestionnaire(Guid questionnaireid,Questionnaire questionnaire)
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
                    if (list != null)
                    {
                        list.Title = questionnaire.Title;
                        list.StartTime = questionnaire.StartTime;
                        list.EndTime = questionnaire.EndTime;
                        list.Caption = questionnaire.Caption;
                        list.IsStart = questionnaire.IsStart;
                        list.State = questionnaire.State;
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
        /// 將該筆問卷設為已刪除(假刪除)
        /// </summary>
        /// <param name="questionnaireid"></param>
        public static void DelQuestionnaire(int id)
        {
            try
            {
                using (ContextModel context = new ContextModel())
                {
                    var query =
                        (from item in context.Questionnaires
                         where item.ID == id
                         select item);

                    var list = query.FirstOrDefault();
                    if (list != null)
                    {
                        list.IsStart = 0;
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
        /// 問卷搜尋
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public static List<Questionnaire> GetQuestionnaireListBySearch(string searchText, DateTime startTime, DateTime endTime)
        {
            try
            {
                using (ContextModel context = new ContextModel())
                {
                    var query = (from item in context.Questionnaires
                                  where item.Title.Contains(@searchText)
                                  orderby item.ID descending
                                  select item).Intersect
                                 (from item2 in context.Questionnaires
                                  where item2.IsStart == 1 && item2.StartTime > startTime && item2.EndTime < endTime
                                  select item2);

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
