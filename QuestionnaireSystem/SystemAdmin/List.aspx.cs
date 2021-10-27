using QuestionnaireSystem.DBSouce;
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
    }
}