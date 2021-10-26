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
                    lbl.Text = "文字方塊(數字)";
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
            List<Question> list3 = new List<Question>();
            if(HttpContext.Current.Session["QusetionList"] == null)
            {
                 list3 = QuestionManger.GetQuestionsListByQuestionnaireID(idtext.ToGuid());
            }
            else
            {
                 list3 = (List<Question>)HttpContext.Current.Session["QusetionList"];
            }
            
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
            
            HttpContext.Current.Session["QusetionList"] = list3;

            Response.Redirect($"/SystemAdmin/Detail.aspx?ID={idtext}#tabs-2");

        }

        protected void btnSubmittab1_Click(object sender, EventArgs e)
        {

        }
    }
}