<%@ Page Title="" Language="C#" MasterPageFile="~/master/manger.Master" AutoEventWireup="true" CodeBehind="Detail.aspx.cs" Inherits="QuestionnaireSystem.SystemAdmin.Detail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        function getArgs(strParame) { //抓Request.QueryString用
            var args = new Object();
            var query = location.search.substring(1); // Get query string
            var pairs = query.split("&"); // Break at ampersand
            for (var i = 0; i < pairs.length; i++) {
                var pos = pairs[i].indexOf('='); // Look for "name=value"
                if (pos == -1) continue; // If not found, skip
                var argname = pairs[i].substring(0, pos); // Extract the name
                var value = pairs[i].substring(pos + 1, pairs[i].length); // Extract the value
                value = decodeURIComponent(value); // Decode it, if needed
                args[argname] = value; // Store as a property
            }
            return args[strParame]; // Return the object
        }
        $(function () {
            $("#tabs").tabs();//頁籤
            
            $("#btnDL").click(function () { //用AJAX取得CSV檔
                var ID = getArgs("ID");
              //  var strURL = "http://localhost:2305/Handlers/CsvHandler.ashx?QuestionnaireID=" + ID;
                var strURL = "/Handlers/CsvHandler.ashx?QuestionnaireID=" + ID;
                $.ajax({
                    url: strURL,
                    type: "POST",
                    data: {},
                    success: function (result) {

                    }
                });
            });

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
            <asp:Button ID="btnCanceltab1" runat="server" Text="取消" OnClick="btnCanceltab1_Click" /><asp:Button ID="btnSubmittab1" runat="server" Text="送出" OnClick="btnSubmittab1_Click" />
        </div>
        <div id="tabs-2">
            <div>
                種類&nbsp<asp:DropDownList ID="CommonDDList" runat="server" AutoPostBack="false"></asp:DropDownList>
                <asp:Button ID="btnCommonQusetion" runat="server" Text="套用" OnClick="btnCommonQusetion_Click" Visible="false" />
                <asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click">套用</asp:LinkButton><br />

                問題&nbsp<asp:TextBox ID="txtQusetion" runat="server"></asp:TextBox>&nbsp
            <asp:DropDownList ID="TypeDDList" runat="server" OnSelectedIndexChanged="TypeDDList_SelectedIndexChanged">
                <asp:ListItem Value="0">單選方塊</asp:ListItem>
                <asp:ListItem Value="1">複選方塊</asp:ListItem>
                <asp:ListItem Value="2">文字方塊</asp:ListItem>
                <asp:ListItem Value="3">文字方塊(數字)</asp:ListItem>
                <asp:ListItem Value="4">文字方塊(Email)</asp:ListItem>
                <asp:ListItem Value="5">文字方塊(日期)</asp:ListItem>
            </asp:DropDownList>
                <asp:CheckBox ID="CheckBox2" runat="server" />必填
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
                        <asp:CheckBoxField DataField="IsMust" HeaderText="必填" />
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
        <div id="tabs-3">
            <button type="button" id="btnDL" >匯出</button><br/>
            <asp:GridView ID="PersonView" runat="server" AutoGenerateColumns="False" AllowPaging="True" OnPageIndexChanging="PersonView_PageIndexChanging" CellPadding="10">
                <Columns>
                    <asp:BoundField DataField="Name" HeaderText="姓名" />
                    <asp:BoundField DataField="CreateDate" HeaderText="填寫時間" />
                    <asp:HyperLinkField DataNavigateUrlFields="QuestionnaireID,ID" DataNavigateUrlFormatString="\SystemAdmin\Detail.aspx?ID={0}&PersonID={1}#tabs-3" HeaderText="觀看細節" Text="前往" />
                </Columns>

            </asp:GridView>
            <asp:Label ID="lblNametab3" runat="server" Text="姓名" Visible="false"></asp:Label>&nbsp&nbsp<asp:TextBox ID="txtNametab3" runat="server" Enabled="false" Visible="false"></asp:TextBox>&nbsp&nbsp&nbsp&nbsp
            <asp:Label ID="lblPhonetab3" runat="server" Text="手機" Visible="false"></asp:Label>&nbsp&nbsp<asp:TextBox ID="txtEmailtab3" runat="server" Enabled="false" Visible="false"></asp:TextBox><br />
            <br />
            <asp:Label ID="lblEmailtab3" runat="server" Text="Email" Visible="false"></asp:Label>&nbsp&nbsp<asp:TextBox ID="txtPhonetab3" runat="server" Enabled="false" Visible="false"></asp:TextBox>&nbsp&nbsp&nbsp&nbsp
            <asp:Label ID="lblAgetab3" runat="server" Text="年齡" Visible="false"></asp:Label>&nbsp&nbsp<asp:TextBox ID="txtAgetab3" runat="server" Enabled="false" Visible="false"></asp:TextBox><br />
            <asp:Label ID="lblDatetab3" runat="server" Text="填寫時間" Visible="false"></asp:Label>&nbsp<asp:Label ID="lblCreateTime" runat="server" Visible="false"></asp:Label><br />
            <br />
            <asp:PlaceHolder ID="PHtab3" runat="server"></asp:PlaceHolder>
            <br />
            <asp:Button ID="btnReturntab3" runat="server" Text="返回" OnClick="btnReturntab3_Click1" />
        </div>
        <div id="tabs-4">
            <p>統計</p>
            <asp:PlaceHolder ID="PHtab4" runat="server"></asp:PlaceHolder>
        </div>
    </div>
</asp:Content>
