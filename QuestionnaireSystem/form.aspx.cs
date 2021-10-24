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
    public partial class form : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)

        {
            if (!IsPostBack)
            {
                if (this.Request.QueryString["ID"] != null)
                {
                    string idtext = this.Request.QueryString["ID"];
                    var list = QuestionnaireManger.GETQuestionnaire(idtext.ToGuid());
                    var list2 = QuestionManger.GetQuestionsListByQuestionnaireID(idtext.ToGuid());
                    this.HFID.Value = idtext;
                   // HttpContext.Current.Session["Answer"] = idtext;
                    if (list != null)
                    {
                        if (list.StartTime != null)
                        {
                            DateTime start = (DateTime)list.StartTime;
                            this.litDate.Text += start.ToString("yyyy/MM/dd");
                        }
                        this.litDate.Text += "~";
                        if (list.EndTime != null)
                        {
                            DateTime end = (DateTime)list.EndTime;
                            this.litDate.Text += end.ToString("yyyy/MM/dd");
                        }
                        this.litTitle.Text = list.Title;
                        if(list.Caption != null)
                        {
                            this.litCaption.Text = list.Caption;
                        }
                        
                    }
                    if (HttpContext.Current.Session["Name"] != null)
                    {
                        this.txtName.Text = HttpContext.Current.Session["Name"].ToString();
                    }
                    if (HttpContext.Current.Session["Phone"] != null)
                    {
                        this.txtPhone.Text = HttpContext.Current.Session["Phone"].ToString();
                    }
                    if (HttpContext.Current.Session["Email"] != null)
                    {
                        this.txtEmail.Text = HttpContext.Current.Session["Email"].ToString();
                    }
                    if (HttpContext.Current.Session["Age"] != null)
                    {
                        this.txtAge.Text = HttpContext.Current.Session["Age"].ToString();
                    }

                }
                else
                {
                    Response.Redirect("/list.aspx");
                    return;
                }

            }
            }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string idtext = this.Request.QueryString["ID"];
            var list2 = QuestionManger.GetQuestionsListByQuestionnaireID(idtext.ToGuid());
            string[] arr = new string[100];
            string anwser = string.Empty;
            for( int i = 0; i<list2.Count; i++)
            {
                if(this.Request.Form[list2[i].ID.ToString()] == null)
                {
                    arr[i] = " ";
                    anwser += " ";
                    anwser += ";";
                }
                else {
                    arr[i] = this.Request.Form[list2[i].ID.ToString()];
                    //  HttpContext.Current.Session[list2[i].ID.ToString()] = this.Request.Form[list2[i].ID.ToString()];
                    anwser += this.Request.Form[list2[i].ID.ToString()];
                    if(i < list2.Count-1)
                    anwser += ";";
                }
            }
            HttpContext.Current.Session["Name"] = this.txtName.Text;
            HttpContext.Current.Session["Email"] = this.txtEmail.Text;
            HttpContext.Current.Session["Phone"] = this.txtPhone.Text;
            HttpContext.Current.Session["Age"] = this.txtAge.Text;
            HttpContext.Current.Session["Answer"] = anwser;
            Response.Redirect("/confirm.aspx?ID=" + idtext); 
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            HttpContext.Current.Session["Name"] = null;
            HttpContext.Current.Session["Email"] = null;
            HttpContext.Current.Session["Phone"] = null;
            HttpContext.Current.Session["Age"] = null;
            HttpContext.Current.Session["Answer"] = null;
            Response.Redirect("/list.aspx");
            return;
        }
    }
}