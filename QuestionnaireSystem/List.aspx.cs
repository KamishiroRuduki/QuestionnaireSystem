using QuestionnaireSystem.DBSouce;
using QuestionnaireSystem.ORM.DBModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QuestionnaireSystem
{
    public partial class List : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
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
            var row = e.Row;
            if (row.RowType == DataControlRowType.DataRow)
            {
                Label lblFormState = row.FindControl("lblFormState") as Label;
                Label lblFormTitle = row.FindControl("lblFormTitle") as Label;
                var rowData = row.DataItem as Questionnaire;
                //var drFormState = (DataRowView)row.DataItem ;
                //int FormState = drFormState.Row.Field<int>("State");
                switch (rowData.State)
                {
                    case 0:
                        lblFormTitle.Text = rowData.Title;
                        lblFormState.Text = "已關閉";
                        break;
                    case 1:
                        lblFormTitle.Text = $"<a href='Form.aspx?ID={rowData.QuestionnaireID}' target='_blank'>" + rowData.Title + "</a>";
                        lblFormState.Text = "開放中";
                        break;
                }

               
            }
        }
    }
}