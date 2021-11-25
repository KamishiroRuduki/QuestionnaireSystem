<%@ Page Title="" Language="C#" MasterPageFile="~/master/manger.Master" AutoEventWireup="true" CodeBehind="common.aspx.cs" Inherits="QuestionnaireSystem.SystemAdmin.common" %>
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
    <li><a href="#tabs-1">常用問題</a></li>
  </ul>
  <div id="tabs-1">
                  <div>

                問題&nbsp<asp:TextBox ID="txtQusetion" runat="server"></asp:TextBox>&nbsp
            <asp:DropDownList ID="TypeDDList" runat="server" OnSelectedIndexChanged="TypeDDList_SelectedIndexChanged">
                <asp:ListItem Value="0">單選方塊</asp:ListItem>
                <asp:ListItem Value="1">複選方塊</asp:ListItem>
                <asp:ListItem Value="2">文字方塊</asp:ListItem>
                <asp:ListItem Value="3">文字方塊(數字)</asp:ListItem>
                <asp:ListItem Value="4">文字方塊(Email)</asp:ListItem>
                <asp:ListItem Value="5">文字方塊(日期)</asp:ListItem>
            </asp:DropDownList>
                <br />
                回答&nbsp<asp:TextBox ID="txtAnswer" runat="server"></asp:TextBox>(多個答案以;分隔)&nbsp&nbsp&nbsp&nbsp&nbsp
                <asp:Button ID="btnAdd" runat="server" Text="加入" OnClick="btnAdd_Click" /><br />
            </div>
            <div>
                <asp:Button ID="btnDeltab2" runat="server" Text="刪除" OnClick="btnDeltab2_Click" /><br/>
                <asp:GridView ID="QusetionView" runat="server" OnRowDataBound="QusetionView_RowDataBound" AutoGenerateColumns="False" CellPadding="10" OnRowCancelingEdit="QusetionView_RowCancelingEdit" OnRowCommand="QusetionView_RowCommand" OnRowDeleting="QusetionView_RowDeleting">
                    <Columns>
                        <asp:TemplateField HeaderText="刪除">
                            <EditItemTemplate>
                                <asp:CheckBox ID="cbDeltab2" runat="server" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="cbDeltab2" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Number" HeaderText="#" />
                        <asp:BoundField DataField="Name" HeaderText="問題" />
                        <asp:TemplateField HeaderText="種類">
                            <ItemTemplate>
                                <asp:HiddenField ID="HiddenField1" runat="server" />
                                <asp:Label ID="lbltype" runat="server" Text="Label"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Button ID="btnUpdate" runat="server" Text="編輯" CommandName="Upate" CommandArgument='<%# Eval("ID") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>

                </asp:GridView>
            </div>
            <asp:Button ID="btnCanceltab2" runat="server" Text="取消" OnClick="btnCanceltab2_Click" /><asp:Button ID="btnSubmittab2" runat="server" Text="送出" OnClick="btnSubmittab2_Click" />
  </div>

</div>
</asp:Content>
