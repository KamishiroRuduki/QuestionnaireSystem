using CsvHelper;
using QuestionnaireSystem.DBSouce;
using QuestionnaireSystem.Extensions;
using QuestionnaireSystem.ORM.DBModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;

namespace QuestionnaireSystem.Handlers
{
    /// <summary>
    /// CsvHandler の概要の説明です
    /// </summary>
    public class CsvHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            string ID = context.Request.QueryString["QuestionnaireID"];
            if (string.IsNullOrEmpty(ID))
            {
                context.Response.StatusCode = 400;
                context.Response.ContentType = "text/plain";
                context.Response.Write("required");
                context.Response.End();
            }
            var fileName = string.Format("D:\\PersonAnswerData_{0}.csv", DateTime.Now.ToString("yyyyMMddHHmmssfff"));
            Guid questionnaireid = ID.ToGuid();
            List<PersonAnswerModel> list = PersonManger.GetPersonAnswerList(questionnaireid);
            using (var writer = new StreamWriter(File.Open(fileName, FileMode.Create),  Encoding.GetEncoding(65001)))
            {
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(list);
                }
            }
            context.Response.Clear();
            context.Response.ClearContent();
            context.Response.ClearHeaders();
            context.Response.ContentType = "text/csv";
            context.Response.AddHeader("Content-Disposition", "attachment;filename=" + fileName);
            //context.Response.AddHeader("Content-Length", fileInfo.Length.ToString());
            context.Response.AddHeader("Content-Transfer-Encoding", "binary");
            //context.Response.AddHeader("application/octet-stream", fileName, True);
            //context.Response.ContentType = "text/csv";
            //context.Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
            Thread.Sleep(10);
            //context.Response.WriteFile(fileName);
            context.Response.TransmitFile(fileName);
            context.Response.Flush();
            context.Response.End();
            
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