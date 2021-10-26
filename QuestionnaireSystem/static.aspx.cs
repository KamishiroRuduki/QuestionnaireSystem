using QuestionnaireSystem.DBSouce;
using QuestionnaireSystem.Extensions;
using QuestionnaireSystem.ORM.DBModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
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
                    if (list != null)
                    {

                        this.litTitle.Text = list.Title;
                        if (list.Caption != null)
                        {
                            this.litCaption.Text = list.Caption;
                        }
                    }
                    
                    for (int i =0; i<list2.Count; i++)
                    {
                        if( list2[i].Type == 0 || list2[i].Type == 1)
                        {
                            Label quesname = new Label();
                            quesname.Text = (i+1).ToString() +"."+list2[i].Name;
                            PlaceHolder1.Controls.Add(quesname);
                            Panel panelChartHolder;
                            panelChartHolder = new Panel();
                            PlaceHolder1.Controls.Add(panelChartHolder);
                            Literal literal = new Literal();
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
            Legend mainlegend;
            LegendCellColumn legendCellColumn1 = new LegendCellColumn();
            LegendCellColumn legendCellColumn2 = new LegendCellColumn();
            mainChart = new Chart();
            mainSeries = new Series("MainSeries");
            mainlegend = new Legend("MainLengend");
            mainChart.Series.Add(mainSeries);
            mainChart.Legends.Add(mainlegend);
            string idtext = this.Request.QueryString["ID"];
            var list2 = QuestionManger.GetQuestionsListByQuestionnaireID(idtext.ToGuid());
            var list3 = OptionManger.GetStaticByQuestionID(list2[i].ID);
            string[] XPointMember = new string[list3.Count];
            int[] YPointMember = new int[list3.Count];
            Series ser = new Series(list2[i].Name);
            ser.Name = list2[i].Name;
            //  ser.ChartArea = "ChartArea1";
            //  mainSeries.ChartType = SeriesChartType.Pie;
            mainChart.Series["MainSeries"].ChartType = SeriesChartType.Pie;
            //  mainChart.Legends["MainLengend"].
            mainChart.Legends["MainLengend"].CellColumns.Add(legendCellColumn1);
            mainChart.Legends["MainLengend"].CellColumns.Add(legendCellColumn2);
            mainChart.Legends["MainLengend"].CellColumns[0].ColumnType = LegendCellColumnType.SeriesSymbol;
            mainChart.Legends["MainLengend"].CellColumns[1].ColumnType = LegendCellColumnType.Text;
            mainChart.Legends["MainLengend"].CellColumns[1].Text = "#VALX-#VALY";
            // mainChart.Series.Add(ser);

            for (int count = 0; count < list3.Count; count++)
            {
                ////storing Values for X axis  
                //XPointMember[count] = list3[count].QuestionOption;
                ////storing values for Y Axis  
                //YPointMember[count] = list3[count].Sum;
                mainChart.Series["MainSeries"].Points.AddXY(list3[count].QuestionOption, list3[count].Sum);

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
            
            mainArea = new ChartArea("MainArea");
            mainChart.ChartAreas.Add(mainArea);
            mainChart.Series["MainSeries"].Label = "#PERCENT{P2}";

            panelChartHolder.Controls.Add(mainChart);           
            // PlaceHolder1.Controls.Add(mainChart);


        }
    }
}