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
    public partial class List : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session.Abandon();//只要到list頁就先清空session
            //HttpContext.Current.Session["QuestionID"] = null;
            //HttpContext.Current.Session["QusetionList"] = null;
            if (!IsPostBack)
            {
                  

                var list = QuestionnaireManger.GetQuestionnaireList();
                if (list.Count > 0)
                {
                    this.QuestionnaireView.DataSource = list;
                    this.QuestionnaireView.DataBind();
                }
            }
        }

        protected void QuestionnaireView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            QuestionnaireView.PageIndex = e.NewPageIndex;
            this.QuestionnaireView.DataBind();
        }

        protected void QuestionnaireView_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void btnCreate1_Click(object sender, EventArgs e)
        {
            Response.Redirect($"/SystemAdmin/Detail.aspx?#tabs-1");
        }

        protected void QuestionnaireView_RowDataBound1(object sender, GridViewRowEventArgs e)
        {
            var row = e.Row;
            if (row.RowType == DataControlRowType.DataRow)
            {
                Label lblFormState = row.FindControl("lblFormState") as Label;
                Label lblEndTime = row.FindControl("lblEndTime") as Label;
                var rowData = row.DataItem as Questionnaire;
                //var drFormState = (DataRowView)row.DataItem ;
                //int FormState = drFormState.Row.Field<int>("State");
                string datetime = "2999-12-31";
                DateTime maxDate = Convert.ToDateTime(datetime);
                if (rowData.EndTime > maxDate)
                {
                    lblEndTime.Text = "-";
                }
                else
                {
                    lblEndTime.Text = rowData.EndTime.ToString("yyyy-MM-dd");

                }
                if (rowData.EndTime < DateTime.Now || rowData.StartTime > DateTime.Now)////當現在時間已經超過結束時間或者還沒到開始時間時自動轉為關閉
                {
                    rowData.State = 0;
                }
                switch (rowData.State)
                {
                    case 0:
                        lblFormState.Text = "已關閉";
                        break;
                    case 1:
                        lblFormState.Text = "開放中";
                        break;
                }


            }
        }

        protected void btnDel_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < QuestionnaireView.Rows.Count; i++)
            {
                CheckBox cb = (CheckBox)QuestionnaireView.Rows[i].FindControl("CheckBox1");
                if (cb.Checked)
                {
                    //找到FormNumber
                    string formNumber = QuestionnaireView.Rows[i].Cells[1].Text;
                    //利用FormNumber做刪除
                    int intformNumber;
                    if (int.TryParse(formNumber, out intformNumber))
                        QuestionnaireManger.DelQuestionnaire(intformNumber);
                }

            }
            //重整頁面
            Response.Redirect(Request.Url.ToString());
        }
        /// <summary>
        /// 搜尋
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            var searchText = this.tbSearch.Text;
            if (searchText == "" && this.txtDatetimeStart.Text == "" && this.txtDatetimeEnd.Text == "")
                return;
            DateTime searchTimeStart = DateTime.MinValue;
            DateTime searchTimeEnd = DateTime.MaxValue;
            if (this.txtDatetimeStart.Text != "")
                searchTimeStart = Convert.ToDateTime(this.txtDatetimeStart.Text);
            if (this.txtDatetimeEnd.Text != "")
                searchTimeEnd = Convert.ToDateTime(this.txtDatetimeEnd.Text);
            var searchList = QuestionnaireManger.GetQuestionnaireListBySearch(searchText, searchTimeStart, searchTimeEnd);
            if (searchList.Count == 0) 
            { 
                Response.Write($"<Script language='JavaScript'>alert('查無資料'); </Script>");
                return;
            }
            this.QuestionnaireView.DataSource = searchList;
            this.QuestionnaireView.DataBind();

        }
    }
}