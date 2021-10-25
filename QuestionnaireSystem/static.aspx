<%@ Page Title="" Language="C#" MasterPageFile="~/master/user.Master" AutoEventWireup="true" CodeBehind="static.aspx.cs" Inherits="QuestionnaireSystem._static" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="entry-content" style="padding-left: 300px;" >
        <div class="auto-style1">
            <asp:Literal ID="litTitle" runat="server"></asp:Literal><br />
            <asp:Literal ID="litCaption" runat="server"></asp:Literal><br />
        </div>
        <div id ="staticTotal">
            <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
        </div>
         <asp:Chart runat="server" ID="ctl00" Height="163px" Width="382px">
             <series>
                 <asp:Series Name="Series1" ChartType="Pie"></asp:Series>
             </series>
             <chartareas>
                 <asp:ChartArea Name="ChartArea1"></asp:ChartArea>
             </chartareas>
         </asp:Chart>
        
    </div>
</asp:Content>
