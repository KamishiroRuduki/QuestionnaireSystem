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
                    if (list2 != null)
                    {
                        
                    }

                }
            }
            }
    }
}