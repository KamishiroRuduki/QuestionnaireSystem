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
                    CommonDDListCreate();
                    string idtext = this.Request.QueryString["ID"];
                    LinkButton1.PostBackUrl = $"/SystemAdmin/Detail.aspx?ID={idtext}#tabs-2";
                    var list = QuestionnaireManger.GETQuestionnaire(idtext.ToGuid());
                    var list2 = QuestionManger.GetQuestionsListByQuestionnaireID(idtext.ToGuid());
                    var personList = PersonManger.GetPersonList(idtext.ToGuid());
                    //問卷
                    if (list != null)
                    {
                        QuestionnaireData(list);
                    }
                    //問題tab+統計tab 頁面建立
                    if (list2 != null)
                    {
                        //假如session有問題List的資料就用該資料做資料繫結
                        if (HttpContext.Current.Session["QusetionList"] == null)
                        {
                            QuseGridviewBind(list2);
                        }
                        else
                        {
                            var list3 = (List<Question>)HttpContext.Current.Session["QusetionList"];
                            var list4 = list3.Where(item => item.IsDel == false).ToList();
                            QuseGridviewBind(list4);
                        }
                        //統計頁，動態產生問題名跟每個回答的統計
                        for (int i = 0; i < list2.Count; i++)
                        {
                            StaticPage(i, list2);
                        }


                    }
                    //填寫資料
                    if (personList != null)
                    {
                        this.PersonView.DataSource = personList;
                        this.PersonView.DataBind();
                    }
                    //檢查session是否有問題的ID(編輯問題用)
                    if (HttpContext.Current.Session["QuestionID"] != null)
                    {
                        if (HttpContext.Current.Session["QusetionList"] == null)//檢查session是否已經有新的問題List，沒有就從DB撈資料，有就從session拿來做回填
                        {
                            string quesStr = HttpContext.Current.Session["QuestionID"].ToString();
                            var ques = QuestionManger.GetQuestionByQuestionID(quesStr.ToGuid());
                            if (ques != null)
                            {
                                QuseEditing(ques);
                            }
                        }
                        else
                        {
                            string questionIDstr = HttpContext.Current.Session["QuestionID"].ToString();
                            Guid sessionQuesid = questionIDstr.ToGuid();
                            var list3 = (List<Question>)HttpContext.Current.Session["QusetionList"];
                            for (int i = 0; i < list3.Count; i++)
                            {
                                if (Guid.Equals(sessionQuesid, list3[i].ID))
                                {
                                    QuseEditing(list3[i]);
                                    break;
                                }
                            }
                        }
                    }
                    //點選前往，顯示該回答者的回答跟資料
                    if (this.Request.QueryString["PersonID"] != null)
                    {
                        this.PersonView.Visible = false;
                        string personidtext = this.Request.QueryString["PersonID"];
                        var thisPerson = PersonManger.GetPersonbyID(personidtext.ToGuid());
                        if (thisPerson != null)
                        {
                            PersonDataView(list2, thisPerson, personidtext);
                        }



                    }




                }
                else
                {


                }
            }


        }
        //-----------------------------------------------------tab1----------------------------------------------------------------

        private void QuestionnaireData(Questionnaire list)
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

        protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 問卷送出鈕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// 取消回問題管理頁，並清空session資料
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCanceltab1_Click(object sender, EventArgs e)
        {
            HttpContext.Current.Session["QuestionID"] = null;
            HttpContext.Current.Session["QusetionList"] = null;
            Response.Redirect($"/SystemAdmin/list.aspx");
        }
        //----------------------------------------tab-2------------------------------------------------------
        private void CommonDDListCreate()
        {
            var commonlist = CommonQuestionManager.GetCommonQuestionsList(); // 
            CommonDDList.DataSource = commonlist;
            CommonDDList.DataTextField = "Name";
            CommonDDList.DataValueField = "ID";
            CommonDDList.DataBind();
        }
        private void QuseGridviewBind(List<Question> list2)
        {
            this.QusetionView.DataSource = list2;
            this.QusetionView.DataBind();
        }
        private void QuseEditing(Question ques)
        {
            txtQusetion.Text = ques.Name;
            txtAnswer.Text = ques.QusetionOption;
            TypeDDList.SelectedValue = ques.Type.ToString();
            CheckBox2.Checked = ques.IsMust;
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
        /// <summary>
        /// 問題新增或編輯
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAdd_Click(object sender, EventArgs e)
        {

            string idtext = this.Request.QueryString["ID"];
            if (string.IsNullOrWhiteSpace(txtQusetion.Text))
            {
                Response.Write($"<Script language='JavaScript'>alert('問題名稱未填寫'); location.href='/SystemAdmin/Detail.aspx?ID={idtext}#tabs-2'; </Script>");
                return;
            }
            int type = Convert.ToInt32(this.TypeDDList.SelectedValue);
            if (string.IsNullOrWhiteSpace(txtAnswer.Text) && (type == 0 || type == 1))
            {
                Response.Write($"<Script language='JavaScript'>alert('單選、複選方塊答案欄不能為空'); location.href='/SystemAdmin/Detail.aspx?ID={idtext}#tabs-2'; </Script>");
                return;
            }
            List<Question> list3 = new List<Question>(); //新的問題List存放session用
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
                int number;
                if (list3.Count == 0)//處理0筆的狀況
                    number = 0;
                else
                    number = list3[list3.Count - 1].Number;
                Question newQuestion = new Question()
                {
                    QuestionnaireID = (idtext).ToGuid(),
                    ID = Guid.NewGuid(),
                    Number = number + 1,
                    Type = type,
                    Name = txtQusetion.Text,
                    IsCommon = 0,
                    IsDel = false
                };
                if (this.CheckBox2.Checked)
                    newQuestion.IsMust = true;
                else
                    newQuestion.IsMust = false;
                if (type == 0 || type == 1)// 單選、複選才將回答加入
                {
                    newQuestion.QusetionOption = txtAnswer.Text;
                }
                list3.Add(newQuestion);
            }
            else
            {
                string questionIDstr = HttpContext.Current.Session["QuestionID"].ToString();
                Guid sessionQuesid = questionIDstr.ToGuid();
                for (int i = 0; i < list3.Count; i++)
                {
                    if (Guid.Equals(sessionQuesid, list3[i].ID)) //找到符合的那一筆做更新
                    {
                        list3[i].Name = txtQusetion.Text;
                        list3[i].Type = type;
                        list3[i].IsCommon = 1;
                        if (this.CheckBox2.Checked)
                            list3[i].IsMust = true;
                        else
                            list3[i].IsMust = false;
                        if (type == 0 || type == 1)
                        {
                            list3[i].QusetionOption = txtAnswer.Text;
                        }
                        break;
                    }
                }
                HttpContext.Current.Session["QuestionID"] = null;
            }
            HttpContext.Current.Session["QusetionList"] = list3;
            Response.Redirect($"/SystemAdmin/Detail.aspx?ID={idtext}#tabs-2");

        }

        /// <summary>
        /// 問題送出鈕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSubmittab2_Click(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["QusetionList"] != null)
            {
                string idtext = this.Request.QueryString["ID"];
                var list2 = QuestionManger.GetQuestionsListByQuestionnaireID(idtext.ToGuid());
                List<Question> list3 = (List<Question>)HttpContext.Current.Session["QusetionList"];
                for (int i = 0; i < list2.Count; i++) //依DB撈出來的LIST長度判斷要做新增還是更新
                {
                    if (list3[i].IsDel)
                    {
                        AnswerManger.DeleteAnswer(list3[i].ID);
                        OptionManger.DeleteOption(list3[i].ID, list3[i].QuestionnaireID);
                        QuestionManger.DelQuestion(idtext.ToGuid(), list3[i].ID);
                    }
                    if (list3[i].IsCommon != 0 && !list3[i].IsDel)//檢查該筆資料是否有更新
                    {
                        list3[i].IsCommon = 0;
                        QuestionManger.UpdateQuestion(list3[i].ID, list3[i]);
                        if (list3[i].Type == 0 || list3[i].Type == 1)
                        {
                            OptionManger.DeleteOption(list3[i].ID, list3[i].QuestionnaireID);//單選、複選做更新時先刪除static(統計用資料表)內的選項再重新加入
                            char[] AnsChars = { ';' };
                            string[] Ans = list3[i].QusetionOption.Split(AnsChars);

                            for (int j = 0; j < Ans.Length; j++)
                            {
                                Static option = new Static()
                                {
                                    QuestionID = list3[i].ID,
                                    QuestionnaireID = list3[i].QuestionnaireID,
                                    QuestionOption = Ans[j],
                                    Sum = 0
                                };
                                OptionManger.CreateOption(option);
                            }
                        }
                        AnswerManger.DeleteAnswer(list3[i].ID);//重置回答

                    }
                }
                for (int i = list2.Count; i < list3.Count; i++)
                {
                    if (!list3[i].IsDel)
                    {
                        QuestionManger.CreateQuestion(list3[i]);
                        if (list3[i].Type == 0 || list3[i].Type == 1)
                        {
                            char[] AnsChars = { ';' };
                            string[] Ans = list3[i].QusetionOption.Split(AnsChars);
                            for (int j = 0; j < Ans.Length; j++)
                            {
                                Static option = new Static()
                                {
                                    QuestionID = list3[i].ID,
                                    QuestionnaireID = list3[i].QuestionnaireID,
                                    QuestionOption = Ans[j],
                                    Sum = 0
                                };
                                OptionManger.CreateOption(option);
                            }
                        }
                    }
                }

                HttpContext.Current.Session["QusetionList"] = null;
                Response.Write($"<Script language='JavaScript'>alert('問題新增/編輯已完成'); location.href='/SystemAdmin/list.aspx'; </Script>");
            }
            else
                Response.Redirect("/SystemAdmin/list.aspx");
        }
        /// <summary>
        /// 取消回問題管理頁，並清空session資料
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCanceltab2_Click(object sender, EventArgs e)
        {
            HttpContext.Current.Session["QuestionID"] = null;
            HttpContext.Current.Session["QusetionList"] = null;
            Response.Redirect($"/SystemAdmin/list.aspx");
        }



        protected void QusetionView_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {

        }
        /// <summary>
        /// 找到要編輯的問題的ID，並放到session
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        protected void TypeDDList_SelectedIndexChanged(object sender, EventArgs e)
        {
            //int type = Convert.ToInt32(this.TypeDDList.SelectedValue);
            //if (type < 2) //單、複選才給輸入回答
            //{
            //    txtAnswer.Enabled = true;

            //}
            //else
            //{
            //    txtAnswer.Enabled = false;
            //    txtAnswer.Text = string.Empty;
            //}
        }
        /// <summary>
        /// 刪除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDeltab2_Click(object sender, EventArgs e)
        {
            string idtext = this.Request.QueryString["ID"];
            List<Question> list3;
            if (HttpContext.Current.Session["QusetionList"] != null)
            {
                list3 = (List<Question>)HttpContext.Current.Session["QusetionList"];
            }
            else
            {
                list3 = QuestionManger.GetQuestionsListByQuestionnaireID(idtext.ToGuid());
            }
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
                //  QuestionManger.DelQuestion(idtext.ToGuid(), removeList[i]); //DB刪除
                //if (HttpContext.Current.Session["QusetionList"] != null)
                //{
                var itemToRemove = list3.SingleOrDefault(r => r.Number == removeList[i]);
                var index = list3.IndexOf(itemToRemove);
                list3[index].IsDel = true;
                //    if (itemToRemove != null)
                //        list3.Remove(itemToRemove);//session資料同時也要做處理
                //}
                //else
                //    list3 = QuestionManger.GetQuestionsListByQuestionnaireID(idtext.ToGuid());

            }
            HttpContext.Current.Session["QusetionList"] = list3;
            //重整頁面
            Response.Redirect($"/SystemAdmin/Detail.aspx?ID={idtext}#tabs-2");
        }

        /// <summary>
        /// 常用問題套用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            int commonid;
            if (int.TryParse(this.CommonDDList.SelectedValue, out commonid))
            {
                var common = CommonQuestionManager.GetCommonQuestionByID(commonid);
                txtQusetion.Text = common.Name;
                TypeDDList.SelectedValue = common.Type.ToString();
                if (common.Type == 0 || common.Type == 1)
                    txtAnswer.Text = common.QusetionOption;

            }

        }
        //--------------------------------------tab-3----------------------------------------------------        
        /// <summary>
        /// 分頁
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void PersonView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            PersonView.PageIndex = e.NewPageIndex;
            this.PersonView.DataBind();
        }

        /// <summary>
        /// 從個人資料返回名單
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnReturntab3_Click1(object sender, EventArgs e)
        {
            string idtext = this.Request.QueryString["ID"];
            Response.Redirect($"/SystemAdmin/Detail.aspx?ID={idtext}#tabs-3");
        }

        protected void btnCommonQusetion_Click(object sender, EventArgs e)
        {

        }

        private void PersonDataView(List<Question> list2, Person thisPerson, string personidtext)
        {
            lblNametab3.Visible = true;
            lblPhonetab3.Visible = true;
            lblEmailtab3.Visible = true;
            lblAgetab3.Visible = true;
            lblDatetab3.Visible = true;
            lblCreateTime.Visible = true;
            txtNametab3.Text = thisPerson.Name;
            txtNametab3.Visible = true;
            txtEmailtab3.Text = thisPerson.Email;
            txtEmailtab3.Visible = true;
            txtPhonetab3.Text = thisPerson.Phone;
            txtPhonetab3.Visible = true;
            txtAgetab3.Text = thisPerson.Age;
            txtAgetab3.Visible = true;
            lblCreateTime.Text = thisPerson.CreateDate.ToString();
            for (int i = 0; i < list2.Count; i++)
            {
                Literal quesname = new Literal();
                quesname.Text = (i + 1).ToString() + "." + list2[i].Name;
                quesname.Text += "<br/>";
                PHtab3.Controls.Add(quesname);
                Literal personAnswer = new Literal();
                var answer = AnswerManger.GetAnswer(list2[i].ID, personidtext.ToGuid());
                if (answer != null)
                    personAnswer.Text = answer.AnswerOption;
                personAnswer.Text += "</br></br>";
                PHtab3.Controls.Add(personAnswer);
            }
        }

        private void StaticPage(int i, List<Question> list2)
        {
            Literal quesname = new Literal();
            quesname.Text = (i + 1).ToString() + "." + list2[i].Name;
            quesname.Text += "<br/>";
            PHtab4.Controls.Add(quesname);
            Literal litAnswer = new Literal();
            if (list2[i].Type == 0 || list2[i].Type == 1)
            {
                var option = OptionManger.GetStaticByQuestionID(list2[i].ID);
                var sum = OptionManger.GetOptionTotal(list2[i].ID);
                for (int j = 0; j < option.Count; j++)
                {
                    double percent = 0;
                    string percentStr = "0";
                    if (sum != 0)
                    {
                        percent = (((double)option[j].Sum / (double)sum) * 100);
                        percentStr = percent.ToString("0.00");
                    }
                    litAnswer.Text += "&nbsp&nbsp";
                    litAnswer.Text += $"{option[j].QuestionOption} {percentStr}% ({option[j].Sum})";
                    litAnswer.Text += "<br/>";

                }

            }
            else
            {
                litAnswer.Text = "  -";
            }
            litAnswer.Text += "</br></br>";
            PHtab4.Controls.Add(litAnswer);
        }
    }

}