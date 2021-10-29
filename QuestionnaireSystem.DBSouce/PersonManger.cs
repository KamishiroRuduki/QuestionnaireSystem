using QuestionnaireSystem.ORM.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestionnaireSystem.DBSouce
{
    public class PersonManger
    {
        /// <summary>
        /// 建立回答人
        /// </summary>
        /// <param name="person"></param>
        public static void CreatePerson(Person person)
        {
            try
            {
                using (ContextModel context = new ContextModel())
                {
                    context.People.Add(person);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);

            }
        }
        ///// <summary>
        ///// 用ID找到該回答人
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public static Person GetPerson(Guid id)
        //{
        //    try
        //    {
        //        using (ContextModel context = new ContextModel())
        //        {
        //            var query =
        //                (from item in context.People
        //                 where item.ID == id                         
        //                 select item);

        //            var list = query.FirstOrDefault();
        //            return list;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.WriteLog(ex);
        //        return null;
        //    }
        //}
        /// <summary>
        /// 用ID找到該回答人
        /// </summary>
        /// <param name="personid"></param>
        /// <returns></returns>
        public static Person GetPersonbyID(Guid personid)
        {
            try
            {
                using (ContextModel context = new ContextModel())
                {
                    var query =
                        (from item in context.People
                         where item.ID == personid
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

        public static List<Person> GetPersonList(Guid questionnaireid)
        {
            try
            {
                using (ContextModel context = new ContextModel())
                {
                    var query =
                        (from item in context.People
                         where item.QuestionnaireID == questionnaireid
                         orderby item.CreateDate descending
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

        public static List<PersonAnswerModel> GetPersonAnswerList(Guid questionnaireid)
        {
            try
            {
                using (ContextModel context = new ContextModel())
                {
                    var query = (from item in context.Questionnaires
                                 where item.QuestionnaireID == questionnaireid
                                 join item2 in context.People on item.QuestionnaireID equals item2.QuestionnaireID
                                 join item4 in context.Answers on item2.ID equals item4.PersonID
                                 join item3 in context.Questions on item4.QuestionID equals item3.ID where item3.IsDel == false
                                 select new
                                 {
                                     item2.Name,
                                     item2.Email,
                                     item2.Phone,
                                     item2.Age,
                                     quesName = item3.Name,
                                     item4.AnswerOption
                                 });

                    List<PersonAnswerModel> list = query.Select(obj => new PersonAnswerModel()
                    {
                        Name = obj.Name,
                        Email = obj.Email,
                        Phone =obj.Phone,
                        Age = obj.Age,
                        QuestionName = obj.quesName,
                        Answer = obj.AnswerOption
                    }).ToList();
                    return list;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
                return null;
            }
        }
        public static bool IsMailCreated(string email, Guid questionnaireid)
        {

            try
            {
                using (ContextModel context = new ContextModel())
                {
                    var query =
                        (from item in context.People
                         where item.Email == email && item.QuestionnaireID == questionnaireid
                         select item);

                    var list = query.FirstOrDefault();
                    if (list is null)
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
                return false;
            }
        }
        /// <summary>
        /// 判斷此手機是否已經被使用
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static bool IsPhoneCreated(string phone, Guid questionnaireid)
        {

            try
            {
                using (ContextModel context = new ContextModel())
                {
                    var query =
                        (from item in context.People
                         where item.Phone == phone && item.QuestionnaireID == questionnaireid
                         select item);

                    var list = query.FirstOrDefault();
                    if (list is null)
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex);
                return false;
            }
        }
    }
}
