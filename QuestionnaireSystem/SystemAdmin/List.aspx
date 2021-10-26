﻿<%@ Page Title="" Language="C#" MasterPageFile="~/master/manger.Master" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="QuestionnaireSystem.SystemAdmin.List" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <div>
       問卷標題:&nbsp&nbsp<asp:TextBox ID="txtTitle" runat="server" Width="337px"></asp:TextBox><br/>
       開始/結束&nbsp<asp:TextBox ID="txtStartTime" runat="server"></asp:TextBox>&nbsp
                  <asp:TextBox ID="txtEndTime" runat="server"></asp:TextBox>&nbsp&nbsp<asp:Button ID="btnSearch" runat="server" Text="搜尋" />
   </div><br/>
    
    <div>
        <asp:GridView ID="QuestionnaireView" runat="server" AutoGenerateColumns="False" CellPadding="10" AllowPaging="True" OnPageIndexChanging="QuestionnaireView_PageIndexChanging">
            <Columns>
                <asp:BoundField DataField="ID" HeaderText="#" />
                <asp:HyperLinkField DataNavigateUrlFields="QuestionnaireID" DataNavigateUrlFormatString="\SystemAdmin\Detail.aspx?ID={0}" DataTextField="Title" HeaderText="問卷" />
                <asp:TemplateField HeaderText="狀態">
                    <ItemTemplate>
                        <%# ((int)Eval("State") == 0) ? "已關閉" : "開放" %>
                    </ItemTemplate>
                </asp:TemplateField>
                
                <asp:BoundField DataField="StartTime" HeaderText="開始時間" DataFormatString="{0:yyyy/MM/dd}" />
                <asp:BoundField DataField="EndTime" HeaderText="結束時間" DataFormatString="{0:yyyy/MM/dd}" />
                <asp:HyperLinkField DataNavigateUrlFields="ID" DataNavigateUrlFormatString="\SystemAdmin\Detail.aspx?ID={0}#tabs-4" HeaderText="觀看統計" Text="前往" />
            </Columns>

        </asp:GridView>
    </div>
</asp:Content>
