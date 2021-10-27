using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QuestionnaireSystem.DBSouce;
using QuestionnaireSystem.Extensions;
using QuestionnaireSystem.ORM.DBModels;

namespace QuestionnaireSystem.SystemAdmin
{
    public partial class Detail : System.Web.UI.Page
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
                    if (list != null)
                    {
                        txtTitle.Text = list.Title;
                        txtCaption.Text = list.Caption;
                        if (list.StartTime != null)
                        {
                            DateTime start = (DateTime)list.StartTime;
                            txtStartTime.Text = start.ToString("yyyy-MM-dd");
                            //  txtStartTime.Text = start;
                        }
                        if (list.EndTime != null)
                        {
                            DateTime end = (DateTime)list.EndTime;
                            txtEndTime.Text = end.ToString("yyyy-MM-dd");
                        }
                        if (list.State == 1)
                        {
                            CheckBox1.Checked = true;
                        }
                    }
                    if (list2 != null)
                    {
                        if (HttpContext.Current.Session["QusetionList"] == null)
                        {
                            this.QusetionView.DataSource = list2;
                            this.QusetionView.DataBind();
                        }
                        else
                        {
                            var list3 = HttpContext.Current.Session["QusetionList"];
                            this.QusetionView.DataSource = list3;
                            this.QusetionView.DataBind();
                        }
                    }
                    if (HttpContext.Current.Session["QuestionID"] != null)
                    {
                        string quesStr = HttpContext.Current.Session["QuestionID"].ToString();
                        var ques = QuestionManger.GetQuestionByQuestionID(quesStr.ToGuid());
                        if (ques != null)
                        {
                            txtQusetion.Text = ques.Name;
                            txtAnswer.Text = ques.QusetionOption;
                            TypeDDList.SelectedValue = ques.Type.ToString();
                            CheckBox2.Checked = ques.IsMust;
                        }
                    }


                }
                else
                {


                }
            }
        }

        protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        protected void QusetionView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            var row = e.Row;
            if (row.RowType == DataControlRowType.DataRow)
            {
                Label lbl = row.FindControl("lbltype") as Label;
                var dr = row.DataItem as Question;
                int type = dr.Type;

                if (type == 0)
                {
                    lbl.Text = "單選方塊";
                }

                else if (type == 1)
                {
                    lbl.Text = "複選方塊";
                }

                else if (type == 2)
                {
                    lbl.Text = "文字方塊";
                }

                else if (type == 3)
                {
                    lbl.Text = "文字方塊(數字)";
                }
                else if (type == 4)
                {
                    lbl.Text = "文字方塊(Email)";
                }
                else if (type == 5)
                {
                    lbl.Text = "文字方塊(日期)";
                }
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {

            string idtext = this.Request.QueryString["ID"];
            if (string.IsNullOrWhiteSpace(txtQusetion.Text))
            {
                Response.Write($"<Script language='JavaScript'>alert('問題名稱未填寫'); location.href='/SystemAdmin/Detail.aspx?ID={idtext}#tabs-2'; </Script>");
                return;
            }
            List<Question> list3 = new List<Question>();
            if (HttpContext.Current.Session["QusetionList"] == null)
            {
                list3 = QuestionManger.GetQuestionsListByQuestionnaireID(idtext.ToGuid());
            }
            else
            {
                list3 = (List<Question>)HttpContext.Current.Session["QusetionList"];
            }

            //檢查是新增還是編輯
            if (HttpContext.Current.Session["QuestionID"] == null)
            {
                int number = list3.Count;
                int type = Convert.ToInt32(this.TypeDDList.SelectedValue);
                Question newQuestion = new Question()
                {
                    QuestionnaireID = (idtext).ToGuid(),
                    ID = Guid.NewGuid(),
                    Number = number + 1,
                    Type = type,
                    Name = txtQusetion.Text
                };
                if (this.CheckBox2.Checked)
                    newQuestion.IsMust = true;
                else
                    newQuestion.IsMust = false;
                if (type == 0 || type == 1)
                {
                    newQuestion.QusetionOption = txtAnswer.Text;
                }
                list3.Add(newQuestion);
            }
            else
            {
                int type = Convert.ToInt32(this.TypeDDList.SelectedValue);

                for ( int i =0; i<list3.Count;i++)
                {
                    string questionIDstr = HttpContext.Current.Session["QuestionID"].ToString();
                    Guid sessionQuesid = questionIDstr.ToGuid();
                    if (Guid.Equals(sessionQuesid, list3[i].ID))
                    {
                        list3[i].Name = txtQusetion.Text;
                        list3[i].Type = type;
                        if (this.CheckBox2.Checked)
                            list3[i].IsMust = true;
                        else
                            list3[i].IsMust = false;
                        if (type == 0 || type == 1)
                        {
                            list3[i].QusetionOption = txtAnswer.Text;
                        }

                    }
                }
                HttpContext.Current.Session["QuestionID"] = null;
            }
            HttpContext.Current.Session["QusetionList"] = list3;
            Response.Redirect($"/SystemAdmin/Detail.aspx?ID={idtext}#tabs-2");

        }

        protected void btnSubmittab1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                return;
            }

            Questionnaire questionnaire = new Questionnaire()
            {
                Title = txtTitle.Text,
                IsStart = 1


            };
            //起始時間跟結束時間
            if (string.IsNullOrWhiteSpace(txtStartTime.Text) || !classes.check.IsDate(txtStartTime.Text))
                questionnaire.StartTime = DateTime.Now;
            else
                questionnaire.StartTime = Convert.ToDateTime(txtStartTime.Text);

            if (string.IsNullOrWhiteSpace(txtEndTime.Text) || !classes.check.IsDate(txtEndTime.Text))
                questionnaire.EndTime = DateTime.MaxValue;
            else
                questionnaire.EndTime = Convert.ToDateTime(txtEndTime.Text);
            //內容
            if (!string.IsNullOrWhiteSpace(txtCaption.Text))
                questionnaire.Caption = txtCaption.Text;
            //啟用
            if (CheckBox1.Checked)
                questionnaire.State = 1;
            else
                questionnaire.State = 0;

            string idtext = this.Request.QueryString["ID"];
            if (string.IsNullOrWhiteSpace(idtext))
            {
                questionnaire.QuestionnaireID = Guid.NewGuid();
                QuestionnaireManger.CreateQuestionnaire(questionnaire);
                string questionnaireID = questionnaire.QuestionnaireID.ToString();
                Response.Redirect($"/SystemAdmin/Detail.aspx?ID={questionnaireID}#tabs-2");
            }
            else
            {
                QuestionnaireManger.UpdateQuestionnaire(idtext.ToGuid(), questionnaire);
                Response.Redirect($"/SystemAdmin/Detail.aspx?ID={idtext}#tabs-2");
            }

        }

        protected void btnSubmittab2_Click(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["QusetionList"] != null)
            {
                string idtext = this.Request.QueryString["ID"];
                var list2 = QuestionManger.GetQuestionsListByQuestionnaireID(idtext.ToGuid());
                List<Question> list3 = (List<Question>)HttpContext.Current.Session["QusetionList"];
                for (int i = 0; i < list2.Count; i++)
                {
                    QuestionManger.UpdateQuestion(list3[i].ID, list3[i]);
                }
                for (int i = list2.Count; i < list3.Count; i++)
                {
                    QuestionManger.CreateQuestion(list3[i]);
                }

                HttpContext.Current.Session["QusetionList"] = null;

            }
            else
                Response.Redirect($"/SystemAdmin/list.aspx");
        }

        protected void btnCanceltab2_Click(object sender, EventArgs e)
        {
            Response.Redirect($"/SystemAdmin/list.aspx");
        }

        protected void btnCanceltab1_Click(object sender, EventArgs e)
        {
            Response.Redirect($"/SystemAdmin/list.aspx");
        }

        protected void QusetionView_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {

        }

        protected void QusetionView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Upate")
            {
                string idtext = this.Request.QueryString["ID"];
                var questionID = e.CommandArgument.ToString();
                // var thisOrder = UserInfoManager.GETUserInfoAccount(custAccount);
                HttpContext.Current.Session["QuestionID"] = questionID;

                Response.Redirect($"/SystemAdmin/Detail.aspx?ID={idtext}#tabs-2");


            }
        }

        protected void QusetionView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }
    }
}