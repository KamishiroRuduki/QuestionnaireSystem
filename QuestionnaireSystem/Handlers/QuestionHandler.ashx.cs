using QuestionnaireSystem.ORM.DBModels;
using QuestionnaireSystem.DBSouce;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QuestionnaireSystem.Extensions;

namespace QuestionnaireSystem.Handlers
{
    /// <summary>
    /// QuestionHandler の概要の説明です
    /// </summary>
    public class QuestionHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string ID = context.Request.QueryString["QuestionnaireID"];
            if (string.IsNullOrEmpty(ID))
            {
                context.Response.StatusCode = 400;
                context.Response.ContentType = "text/plain";
                context.Response.Write("required");
                context.Response.End();
            }
            Guid questionnaireid = ID.ToGuid();
            List<Question> list = QuestionManger.GetQuestionsListByQuestionnaireID(questionnaireid);
            string jsonText = Newtonsoft.Json.JsonConvert.SerializeObject(list);
            context.Response.ContentType = "application/json";
            context.Response.Write(jsonText);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}