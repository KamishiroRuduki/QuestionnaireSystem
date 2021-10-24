using QuestionnaireSystem.DBSouce;
using QuestionnaireSystem.Extensions;
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

        protected void btnSubmit_Click(object sender, EventArgs e)
        {

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            string idtext = this.Request.QueryString["ID"];
            Response.Redirect("/Form.aspx?ID=" + idtext);

        }
    }
}