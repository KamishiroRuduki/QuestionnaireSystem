using QuestionnaireSystem.DBSouce;
using QuestionnaireSystem.Extensions;
using QuestionnaireSystem.ORM.DBModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;

namespace QuestionnaireSystem
{
    public partial class _static : System.Web.UI.Page
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
                    Panel panelChartHolder;
                    panelChartHolder = new Panel();
                    Controls.Add(panelChartHolder);
                    for (int i =0; i<list2.Count; i++)
                    {
                        if( list2[i].Type == 0 || list2[i].Type == 1)
                        {
                            

                            Literal literal = new Literal();
                            /*  literal.Text += "<script type='text/javascript'> var " & list2[i].Name & " = [['網管(" & Q1sum1.ToString & "票-" & ((Q1sum1 / sum) * 100).ToString("##.##") & "%)'," & Q1sum1.ToString() & _"]";*/
                            Bindchart(i, panelChartHolder);

                        }
                    }
                }
                
            }
        }

        private void Bindchart( int i, Panel panelChartHolder)
        {
            ChartArea mainArea;
            Chart mainChart;
            Series mainSeries;

            mainChart = new Chart();
            mainSeries = new Series("MainSeries");
            string idtext = this.Request.QueryString["ID"];
            var list2 = QuestionManger.GetQuestionsListByQuestionnaireID(idtext.ToGuid());
            var list3 = OptionManger.GetStaticByQuestionID(list2[i].ID);
            string[] XPointMember = new string[list3.Count];
            int[] YPointMember = new int[list3.Count];
            Series ser = new Series(list2[i].Name);
            ser.Name = list2[i].Name;
            //  ser.ChartArea = "ChartArea1";
            mainSeries.ChartType = SeriesChartType.Pie;
           // mainChart.Series.Add(ser);
            for (int count = 0; count < list3.Count; count++)
            {
                ////storing Values for X axis  
                //XPointMember[count] = list3[count].QuestionOption;
                ////storing values for Y Axis  
                //YPointMember[count] = list3[count].Sum;
                mainSeries.Points.AddXY(list3[count].QuestionOption, list3[count].Sum);

            }
            ////binding chart control  
            //chart.Series[0].Points.DataBindXY(XPointMember, YPointMember);

            ////Setting width of line  
            //chart.Series[0].BorderWidth = 10;
            ////setting Chart type   
            //chart.Series[0].ChartType = SeriesChartType.Pie;
            //mainChart.Series[$"{list2[i].Name}"].XValueMember = "QuestionOption";
            //mainChart.Series[$"{list2[i].Name}"].YValueMembers = "Sum";
            //mainChart.DataBind();
            mainChart.Series.Add(mainSeries);
            mainArea = new ChartArea("MainArea");
            mainChart.ChartAreas.Add(mainArea);

            panelChartHolder.Controls.Add(mainChart);
           // PlaceHolder1.Controls.Add(mainChart);


        }
    }
}