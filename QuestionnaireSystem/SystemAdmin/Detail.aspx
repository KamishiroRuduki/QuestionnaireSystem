<%@ Page Title="" Language="C#" MasterPageFile="~/master/manger.Master" AutoEventWireup="true" CodeBehind="Detail.aspx.cs" Inherits="QuestionnaireSystem.SystemAdmin.Detail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        $(function () {
            $("#tabs").tabs();
        });
    </script>

    <div id="tabs">
        <ul>
            <li><a href="#tabs-1">問卷</a></li>
            <li><a href="#tabs-2">問題</a></li>
            <li><a href="#tabs-3">填寫資料</a></li>
            <li><a href="#tabs-4">統計</a></li>
        </ul>
        <div id="tabs-1">
            問卷名稱&nbsp&nbsp<asp:TextBox ID="txtTitle" runat="server" Width="380px"></asp:TextBox><br />
            <br />
            描述內容&nbsp&nbsp<asp:TextBox ID="txtCaption" runat="server" Height="114px" Width="380px"></asp:TextBox><br />
            <br />
            開始時間&nbsp&nbsp<asp:TextBox ID="txtStartTime" TextMode="Date" runat="server" Width="380px"></asp:TextBox><br />
            <br />
            結束時間&nbsp&nbsp<asp:TextBox ID="txtEndTime" TextMode="Date" runat="server" Width="380px"></asp:TextBox><br />
            <br />
            <asp:CheckBox ID="CheckBox1" runat="server" OnCheckedChanged="CheckBox1_CheckedChanged" />
            已啟用
            <br />
            <asp:Button ID="btnCanceltab1" runat="server" Text="取消" /><asp:Button ID="btnSubmittab1" runat="server" Text="送出" OnClick="btnSubmittab1_Click" />
        </div>
        <div id="tabs-2">
            <div>
                種類&nbsp<asp:DropDownList ID="CommonDDList" runat="server"></asp:DropDownList><br />

                問題&nbsp<asp:TextBox ID="txtQusetion" runat="server"></asp:TextBox>&nbsp
            <asp:DropDownList ID="TypeDDList" runat="server">
                <asp:ListItem Value="0">單選方塊</asp:ListItem>
                <asp:ListItem Value="1">複選方塊</asp:ListItem>
                <asp:ListItem Value="2">文字方塊</asp:ListItem>
                <asp:ListItem Value="3">文字方塊(數字)</asp:ListItem>
                <asp:ListItem Value="4">文字方塊(Email)</asp:ListItem>
                <asp:ListItem Value="5">文字方塊(日期)</asp:ListItem>
            </asp:DropDownList>
                <asp:CheckBox ID="CheckBox2" runat="server" />必填
                <br />
                回答&nbsp<asp:TextBox ID="txtAnswer" runat="server"></asp:TextBox>&nbsp&nbsp&nbsp&nbsp&nbsp
                <asp:Button ID="btnAdd" runat="server" Text="加入" OnClick="btnAdd_Click" /><br />
            </div>
            <div>
                <asp:GridView ID="QusetionView" runat="server" OnRowDataBound="QusetionView_RowDataBound" AutoGenerateColumns="False" CellPadding="10">
                    <Columns>
                        <asp:BoundField DataField="Number" HeaderText="#" />
                        <asp:BoundField DataField="Name" HeaderText="問題" />
                        <asp:TemplateField HeaderText="種類">
                            <ItemTemplate>
                                <asp:HiddenField ID="HiddenField1" runat="server" />
                                <asp:Label ID="lbltype" runat="server" Text="Label"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:CheckBoxField DataField="IsMust" HeaderText="必填" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Button ID="btnUpdate" runat="server" Text="編輯" CommandName="Upate" CommandArgument='<%# Eval("ID") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>

                </asp:GridView>
            </div>
            <asp:Button ID="btnCanceltab2" runat="server" Text="取消" /><asp:Button ID="btnSubmittab2" runat="server" Text="送出" />
        </div>
        <div id="tabs-3">
            <asp:Button ID="Button1" runat="server" Text="匯出" />
            <asp:GridView ID="PersonView" runat="server"></asp:GridView>
        </div>
        <div id="tabs-4">
            <p>統計</p>
        </div>
    </div>
</asp:Content>
