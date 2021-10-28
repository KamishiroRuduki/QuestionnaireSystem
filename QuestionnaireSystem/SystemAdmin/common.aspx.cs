using QuestionnaireSystem.DBSouce;
using QuestionnaireSystem.ORM.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QuestionnaireSystem.SystemAdmin
{
    public partial class common : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var list = CommonQuestionManager.GetCommonQuestionsList();
                if (list != null)
                {
                    //假如session有問題List的資料就用該資料做資料繫結
                    if (HttpContext.Current.Session["CommonQusetionList"] == null)
                    {
                        this.QusetionView.DataSource = list;
                        this.QusetionView.DataBind();
                    }
                    else
                    {
                        var list3 = HttpContext.Current.Session["CommonQusetionList"];
                        this.QusetionView.DataSource = list3;
                        this.QusetionView.DataBind();
                    }
                }
                if (HttpContext.Current.Session["CommonQuestionID"] != null)
                {
                    if (HttpContext.Current.Session["CommonQusetionList"] == null)
                    {
                        string quesStr = HttpContext.Current.Session["CommonQuestionID"].ToString();
                        var ques = CommonQuestionManager.GetCommonQuestionByID(Convert.ToInt32(quesStr));
                        if (ques != null)
                        {
                            txtQusetion.Text = ques.Name;
                            txtAnswer.Text = ques.QusetionOption;
                            TypeDDList.SelectedValue = ques.Type.ToString();
                        }
                    }
                    else
                    {
                        string questionIDstr = HttpContext.Current.Session["CommonQuestionID"].ToString();
                        int sessionQuesid = Convert.ToInt32(questionIDstr);
                        var list3 = (List<CommonQuestion>)HttpContext.Current.Session["CommonQusetionList"];
                        for (int i = 0; i < list3.Count; i++)
                        {
                            if (sessionQuesid == list3[i].Number)
                            {
                                txtQusetion.Text = list3[i].Name;
                                txtAnswer.Text = list3[i].QusetionOption;
                                TypeDDList.SelectedValue = list3[i].Type.ToString();
                                break;
                            }
                        }
                    }
                }

            }

        }

        protected void TypeDDList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(txtQusetion.Text))
            {
                Response.Write($"<Script language='JavaScript'>alert('問題名稱未填寫'); location.href='/SystemAdmin/common.aspx'; </Script>");
                return;
            }
            int type = Convert.ToInt32(this.TypeDDList.SelectedValue);
            if (string.IsNullOrWhiteSpace(txtAnswer.Text) && (type == 0 || type == 1))
            {
                Response.Write($"<Script language='JavaScript'>alert('單選、複選方塊答案欄不能為空'); location.href='/SystemAdmin/common.aspx'; </Script>");
                return;
            }
            List<CommonQuestion> list3 = new List<CommonQuestion>(); //新的問題List存放session用
            if (HttpContext.Current.Session["CommonQusetionList"] == null)
            {
                list3 = CommonQuestionManager.GetCommonQuestionsList();
            }
            else
            {
                list3 = (List<CommonQuestion>)HttpContext.Current.Session["CommonQusetionList"];
            }

            //檢查是新增還是編輯
            if (HttpContext.Current.Session["CommonQuestionID"] == null)
            {
                //   int number = list3[list3.Count - 1].Number;
                int number;
                if (list3.Count == 0)
                    number = 0;
                else
                    number = list3[list3.Count - 1].Number;
                CommonQuestion newQuestion = new CommonQuestion()
                {

                    Type = type,
                    Name = txtQusetion.Text,
                    Number = number + 1,
                    IsDel = false
                };

                if (type == 0 || type == 1)// 單選、複選才將回答加入
                {
                    newQuestion.QusetionOption = txtAnswer.Text;
                }
                list3.Add(newQuestion);
            }
            else
            {
                string questionIDstr = HttpContext.Current.Session["CommonQuestionID"].ToString();
                int sessionQuesid = Convert.ToInt32(questionIDstr);
                for (int i = 0; i < list3.Count; i++)
                {
                    if (sessionQuesid == list3[i].ID) //找到符合的那一筆做更新
                    {
                        list3[i].Name = txtQusetion.Text;
                        list3[i].Type = type;
                        list3[i].IsChange = 1;
                        if (type == 0 || type == 1)
                        {
                            list3[i].QusetionOption = txtAnswer.Text;
                        }
                        break;
                    }
                }
                HttpContext.Current.Session["CommonQuestionID"] = null;
            }
            HttpContext.Current.Session["CommonQusetionList"] = list3;
            Response.Redirect(Request.Url.ToString());
        }

        protected void btnDeltab2_Click(object sender, EventArgs e)
        {
            List<CommonQuestion> list3 = (List<CommonQuestion>)HttpContext.Current.Session["CommonQusetionList"];
            List<int> removeList = new List<int>();
            for (int i = 0; i < QusetionView.Rows.Count; i++)
            {
                CheckBox cb = (CheckBox)QusetionView.Rows[i].FindControl("cbDeltab2");
                if (cb.Checked)
                {
                    //找到FormNumber
                    string formNumber = QusetionView.Rows[i].Cells[1].Text;
                    //利用FormNumber做刪除
                    int intformNumber;
                    if (int.TryParse(formNumber, out intformNumber))
                    {
                        removeList.Add(intformNumber);
                    }
                }

            }
            for (int i = 0; i < removeList.Count; i++)
            {
                CommonQuestionManager.DelCommonQuestion(removeList[i]); //DB刪除
                if (HttpContext.Current.Session["CommonQusetionList"] != null)
                {
                    var itemToRemove = list3.SingleOrDefault(r => r.Number == removeList[i]);
                    if (itemToRemove != null)
                        list3.Remove(itemToRemove);//session資料同時也要做處理
                }
                else
                    list3 = CommonQuestionManager.GetCommonQuestionsList();
            }
            HttpContext.Current.Session["CommonQusetionList"] = list3;
            //重整頁面
            Response.Redirect(Request.Url.ToString());
        }

        protected void btnCanceltab2_Click(object sender, EventArgs e)
        {
            HttpContext.Current.Session["CommonQuestionID"] = null;
            HttpContext.Current.Session["CommonQusetionList"] = null;
            Response.Redirect($"/SystemAdmin/list.aspx");
        }

        protected void btnSubmittab2_Click(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["CommonQusetionList"] != null)
            {
                string idtext = this.Request.QueryString["ID"];
                var list2 = CommonQuestionManager.GetCommonQuestionsList();
                List<CommonQuestion> list3 = (List<CommonQuestion>)HttpContext.Current.Session["CommonQusetionList"];
                for (int i = 0; i < list2.Count; i++)
                {
                    if (list3[i].IsChange != 0)
                    {
                        list3[i].IsChange = 0;
                        CommonQuestionManager.UpdateCommonQuestion(list3[i].ID, list3[i]);
                    }
                }
                for (int i = list2.Count; i < list3.Count; i++)
                {
                    CommonQuestionManager.CreateCommonQuestion(list3[i]);
                }

                HttpContext.Current.Session["CommonQusetionList"] = null;
                Response.Write($"<Script language='JavaScript'>alert('問題新增/編輯已完成'); location.href='/SystemAdmin/list.aspx'; </Script>");
            }
            else
                Response.Redirect("/SystemAdmin/list.aspx");
        }

        protected void QusetionView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void QusetionView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            var row = e.Row;
            if (row.RowType == DataControlRowType.DataRow)
            {
                Label lbl = row.FindControl("lbltype") as Label;
                var dr = row.DataItem as CommonQuestion;
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

        protected void QusetionView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Upate")
            {

                var questionID = e.CommandArgument.ToString();
                // var thisOrder = UserInfoManager.GETUserInfoAccount(custAccount);
                HttpContext.Current.Session["CommonQuestionID"] = questionID;

                Response.Redirect(Request.Url.ToString());


            }
        }

        protected void QusetionView_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {

        }
    }
}