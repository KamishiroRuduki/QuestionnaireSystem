using QuestionnaireSystem.DBSouce;
using QuestionnaireSystem.Extensions;
using QuestionnaireSystem.ORM.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QuestionnaireSystem
{
    public partial class confirm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Request.QueryString["ID"] != null)
            {
                string idtext = this.Request.QueryString["ID"];
                var list = QuestionnaireManger.GETQuestionnaire(idtext.ToGuid());
                var list2 = QuestionManger.GetQuestionsListByQuestionnaireID(idtext.ToGuid());

                if (list != null)
                {

                    this.litTitle.Text = list.Title;
                    if (list.Caption != null)
                    {
                        this.litCaption.Text = list.Caption;
                    }
                }
                if (HttpContext.Current.Session["Name"] != null)
                {
                    this.lblName.Text = HttpContext.Current.Session["Name"].ToString();
                }
                if (HttpContext.Current.Session["Phone"] != null)
                {
                    this.lblPhone.Text = HttpContext.Current.Session["Phone"].ToString();
                }
                if (HttpContext.Current.Session["Email"] != null)
                {
                    this.lblEmail.Text = HttpContext.Current.Session["Email"].ToString();
                }
                if (HttpContext.Current.Session["Age"] != null)
                {
                    this.lblAge.Text = HttpContext.Current.Session["Age"].ToString();
                }
                if (list2 != null)
                {
                    if (HttpContext.Current.Session["Answer"] != null)
                    {
                        char[] delimiterChars = { ';' };
                        string[] answerArr = HttpContext.Current.Session["Answer"].ToString().Split(delimiterChars);
                        this.litAnswer.Text = "<div>";
                        for (int i = 0; i < list2.Count; i++)
                        {
                            this.litAnswer.Text += $"<p>{i + 1}.{list2[i].Name}<p>";
                            this.litAnswer.Text += $"<p>{answerArr[i]}<p><br/><br/>";
                        }
                        this.litAnswer.Text += "</div>";
                    }
                }
            }


        }
        /// <summary>
        /// 問卷回答送出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string idtext = this.Request.QueryString["ID"];
            var list = QuestionnaireManger.GETQuestionnaire(idtext.ToGuid());
            var list2 = QuestionManger.GetQuestionsListByQuestionnaireID(idtext.ToGuid());
            Person newperson = new Person()
            {
                ID = Guid.NewGuid(),
                QuestionnaireID = idtext.ToGuid(),
                Name = HttpContext.Current.Session["Name"].ToString(),
                Email = HttpContext.Current.Session["Email"].ToString(),
                Phone = HttpContext.Current.Session["Phone"].ToString(),
                Age = HttpContext.Current.Session["Age"].ToString(),
                CreateDate = DateTime.Now


            };
            PersonManger.CreatePerson(newperson);

            Person person = PersonManger.GetPersonbyID(newperson.ID);
            if (HttpContext.Current.Session["Answer"] != null)
            {
                char[] delimiterChars = { ';' };
                string[] answerArr = HttpContext.Current.Session["Answer"].ToString().Split(delimiterChars);

                for (int i = 0; i < list2.Count; i++)
                {
                    Answer answer = new Answer()
                    {
                        QuestionID = list2[i].ID,
                        PersonID = person.ID,
                        AnswerOption = answerArr[i]
                    };
                    AnswerManger.CreateAnswer(answer); //建立回答

                    if (list2[i].Type == 0 && !string.IsNullOrWhiteSpace(answerArr[i]))
                    {
                        //Static staticSum = new Static()
                        //{
                        //    QuestionnaireID = idtext.ToGuid(),
                        //    QuestionID = list2[i].ID,
                        //    QuestionOption = answerArr[i]

                        //};
                        OptionManger.UpdateStaticSum(idtext.ToGuid(), list2[i].ID, answerArr[i]);

                    }
                    if (list2[i].Type == 1 && !string.IsNullOrWhiteSpace(answerArr[i]))
                    {
                        //Static staticSum = new Static()
                        //{
                        //    QuestionnaireID = idtext.ToGuid(),
                        //    QuestionID = list2[i].ID,
                        //    QuestionOption = answerArr[i]

                        //};
                        char[] checkboxAnsChars = { ',' };
                        string[] checkboxAns = answerArr[i].Split(checkboxAnsChars);
                        for (int j = 0; j < checkboxAns.Length; j++)
                        {
                            OptionManger.UpdateStaticSum(idtext.ToGuid(), list2[i].ID, checkboxAns[j]);

                        }
                    }
                }

            }
            HttpContext.Current.Session["Name"] = null;
            HttpContext.Current.Session["Email"] = null;
            HttpContext.Current.Session["Phone"] = null;
            HttpContext.Current.Session["Age"] = null;
            HttpContext.Current.Session["Answer"] = null;
            Response.Redirect("/list.aspx");
            return;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            string idtext = this.Request.QueryString["ID"];
            Response.Redirect("/Form.aspx?ID=" + idtext);

        }
    }
}