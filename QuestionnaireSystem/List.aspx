﻿<%@ Page Title="" Language="C#" MasterPageFile="~/master/user.Master" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="QuestionnaireSystem.List" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="container" style="border: 3px #000000 solid;">
        <img src="/img/magnifier.png" width="50" height="50" />
        <table>
            <tr>
                <asp:TextBox runat="server" ID="tbSearch" class="form-control" placeholder="請輸入搜尋"></asp:TextBox>
            </tr>
            <tr>
                <td>
                    <img src="/img/calendar.png" width="50" height="50" /></td>
                <td>
                    <table></table>
                    <label>選擇起始日期：</label>
                    <div class='input-group date' id='datetimepickerStart'>
                        <asp:TextBox type="text" TextMode="Date" CssClass="form-control" ID="txtDatetimeStart" runat="server"></asp:TextBox>
                        <span class="input-group-addon">
                            <span class="glyphicon glyphicon-calendar"></span>
                        </span>
                    </div>
                    <label>選擇結束日期：</label>
                    <div class='input-group date' id='datetimepickerEnd'>
                        <asp:TextBox type='text' TextMode="Date" CssClass="form-control" ID="txtDatetimeEnd" runat="server"></asp:TextBox>
                        <span class="input-group-addon">
                            <span class="glyphicon glyphicon-calendar"></span>
                        </span>
                    </div>
                    <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-outline-success" Text="搜尋" OnClick="btnSearch_Click" />
                </td>
            </tr>
        </table>
    </div>
    <br />
    
    <div>
        <asp:GridView ID="QuestionnaireView" runat="server" AutoGenerateColumns="False" CellPadding="10" AllowPaging="True" OnPageIndexChanging="QuestionnaireView_PageIndexChanging" OnRowDataBound="QuestionnaireView_RowDataBound">
            <Columns>
                <asp:BoundField DataField="ID" HeaderText="#" />
               <asp:TemplateField HeaderText="問卷">
                    <ItemTemplate>
                        <asp:Label ID="lblFormTitle" runat="server" Text="Label"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
              <%--  <asp:HyperLinkField DataNavigateUrlFields="QuestionnaireID" DataNavigateUrlFormatString="\Form.aspx?ID={0}" DataTextField="Title" HeaderText="問卷" />--%>
                <asp:TemplateField HeaderText="狀態">
                    <ItemTemplate>
                       <%--  <%# ((int)Eval("State") == 0) ? "開放" : "已關閉" %> --%>
                        <asp:Label ID="lblFormState" runat="server" Text="Label"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                
                <asp:BoundField DataField="StartTime" HeaderText="開始時間" DataFormatString="{0:yyyy/MM/dd}" />
                <asp:TemplateField HeaderText="結束時間">
                    <ItemTemplate>
                       <%--  <%# ((int)Eval("State") == 0) ? "開放" : "已關閉" %> --%>
                        <asp:Label ID="lblEndTime" runat="server" Text="Label"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:HyperLinkField DataNavigateUrlFields="QuestionnaireID" DataNavigateUrlFormatString="\static.aspx?ID={0}" HeaderText="觀看統計" Text="前往" />
            </Columns>

        </asp:GridView>
    </div>
</asp:Content>
