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


                }
                else
                {
                    Response.Redirect("/list.aspx");
                    return;
                }

            }
            if (HttpContext.Current.Session["Name"] != null)
            {
                this.txtName.Text = HttpContext.Current.Session["Name"] as string;
            }
            if (HttpContext.Current.Session["Phone"] != null)
            {
                this.txtPhone.Text = HttpContext.Current.Session["Phone"] as string;
            }
            if (HttpContext.Current.Session["Email"] != null)
            {
                this.txtEmail.Text = HttpContext.Current.Session["Email"] as string;
            }
            if (HttpContext.Current.Session["Age"] != null)
            {
                this.txtAge.Text = HttpContext.Current.Session["Age"] as string;
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {

            
            string idtext = this.Request.QueryString["ID"];
            var list2 = QuestionManger.GetQuestionsListByQuestionnaireID(idtext.ToGuid());
            string name = this.txtName.Text;
            string email = this.txtEmail.Text;
            string phone = this.txtPhone.Text;
            string age = this.txtAge.Text;
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

            if(!string.IsNullOrWhiteSpace(name))
                HttpContext.Current.Session["Name"] = name;
            if (!string.IsNullOrWhiteSpace(email))
                HttpContext.Current.Session["Email"] = email;
            if (!string.IsNullOrWhiteSpace(phone))
                HttpContext.Current.Session["Phone"] = phone;
            if (!string.IsNullOrWhiteSpace(age))
                HttpContext.Current.Session["Age"] = age;
            HttpContext.Current.Session["Answer"] = anwser;
            if (string.IsNullOrWhiteSpace(this.txtName.Text) || string.IsNullOrWhiteSpace(this.txtEmail.Text) || string.IsNullOrWhiteSpace(this.txtPhone.Text) || string.IsNullOrWhiteSpace(this.txtAge.Text))
            {
                this.ltMsg.Text = "姓名或Email或電話或年齡有漏填";
                return;
            }
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