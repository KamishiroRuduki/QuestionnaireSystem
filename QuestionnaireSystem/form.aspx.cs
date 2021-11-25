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
                    if (list.State == 0)
                    {
                        Response.Redirect("/list.aspx");
                        return;
                    }
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
                        this.lblTitle.Text = list.Title;
                        if(list.Caption != null)
                        {
                            this.litCaption.Text = list.Caption;
                        }
                        
                    }
                    //檢查姓名、mail、電話、年齡有沒有session資料，有就做回填的動作
                    if (HttpContext.Current.Session["Name"] != null)
                    {
                        this.txtName.Text = HttpContext.Current.Session["Name"] as string;
                      //  HttpContext.Current.Session["Name"] = null;
                    }
                    if (HttpContext.Current.Session["Phone"] != null)
                    {
                        this.txtPhone.Text = HttpContext.Current.Session["Phone"] as string;
                      //  HttpContext.Current.Session["Phone"] = null;
                    }
                    if (HttpContext.Current.Session["Email"] != null)
                    {
                        this.txtEmail.Text = HttpContext.Current.Session["Email"] as string;
                      //  HttpContext.Current.Session["Email"] = null;
                    }
                    if (HttpContext.Current.Session["Age"] != null)
                    {
                        this.txtAge.Text = HttpContext.Current.Session["Age"] as string;
                      //  HttpContext.Current.Session["Age"] = null;
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
            string name = this.txtName.Text;
            string email = this.txtEmail.Text;
            string phone = this.txtPhone.Text;
            string age = this.txtAge.Text;
            string[] arr = new string[100];//測試用陣列不用理它
            string anwser = string.Empty;//放回答用
            for( int i = 0; i<list2.Count; i++)
            {
                if(this.Request.Form[list2[i].ID.ToString()] == null)
                {
                    if(list2[i].IsMust == true)
                    {
                        Response.Write($"<Script language='JavaScript'>alert('有必填的問題沒填'); </Script>");                        
                        return;
                    }
                    arr[i] = " ";//測試用不用理它
                    anwser += " ";//假如該題未填，此題答案給它一格空格
                    anwser += ";";//答案分割用
                }
                else {
                    arr[i] = this.Request.Form[list2[i].ID.ToString()];//測試用不用理它
                    //  HttpContext.Current.Session[list2[i].ID.ToString()] = this.Request.Form[list2[i].ID.ToString()];
                    anwser += this.Request.Form[list2[i].ID.ToString()];//抓前端控制項輸入的值
                    if (i < list2.Count-1)//最後一題不放分號
                    anwser += ";";
                }
            }
            //按下送出，把資料暫存至session
            if(!string.IsNullOrWhiteSpace(name))
                HttpContext.Current.Session["Name"] = name;
            if (!string.IsNullOrWhiteSpace(email))
                HttpContext.Current.Session["Email"] = email;
            if (!string.IsNullOrWhiteSpace(phone))
                HttpContext.Current.Session["Phone"] = phone;
            if (!string.IsNullOrWhiteSpace(age))
                HttpContext.Current.Session["Age"] = age;
            HttpContext.Current.Session["Answer"] = anwser;

            //檢查是否有漏填
            if (string.IsNullOrWhiteSpace(this.txtName.Text) || string.IsNullOrWhiteSpace(this.txtEmail.Text) || string.IsNullOrWhiteSpace(this.txtPhone.Text) || string.IsNullOrWhiteSpace(this.txtAge.Text))
            {
                Response.Write($"<Script language='JavaScript'>alert('姓名或Email或電話或年齡有漏填'); </Script>");
                //this.ltMsg.Text = "姓名或Email或電話或年齡有漏填<br/>";
                return;
            }
            if (!PersonManger.IsMailCreated(email, idtext.ToGuid()))
            {
                Response.Write($"<Script language='JavaScript'>alert('此信箱已經被使用過了'); </Script>");
                //this.ltMsg.Text = "此信箱已經被使用過了<br/>";
                return;
            }
            if (!PersonManger.IsPhoneCreated(phone, idtext.ToGuid()))
            {
                Response.Write($"<Script language='JavaScript'>alert('此手機已經被使用過了'); </Script>");
                //this.ltMsg.Text = "此手機已經被使用過了<br/>";
                return;
            }
            Response.Redirect("/confirm.aspx?ID=" + idtext); //跳至確認頁
        }

        protected void btnCancel_Click(object sender, EventArgs e)//按取消跳回list頁前，先把session資料清除
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