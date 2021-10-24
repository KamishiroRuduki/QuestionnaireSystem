<%@ Page Title="" Language="C#" MasterPageFile="~/master/user.Master" AutoEventWireup="true" CodeBehind="confirm.aspx.cs" Inherits="QuestionnaireSystem.confirm" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .auto-style1 {
            width: 661px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="entry-content" style="padding-left: 300px;" >
        <div class="auto-style1">
            <asp:Literal ID="litTitle" runat="server"></asp:Literal><br />
            <asp:Literal ID="litCaption" runat="server"></asp:Literal><br />
            <br />
            <br />
            姓名&nbsp&nbsp<asp:Label ID="lblName" runat="server" ></asp:Label><br />
            <br />
            手機&nbsp&nbsp<asp:Label ID="lblPhone" runat="server" ></asp:Label><br />
            <br />
            Email&nbsp<asp:Label ID="lblEmail" runat="server" ></asp:Label><br />
            <br />
            年齡&nbsp&nbsp<asp:Label ID="lblAge" runat="server" ></asp:Label><br />
            <br />
        </div>
        <div>
            <asp:Literal ID="litAnswer" runat="server"></asp:Literal>
        </div>
        <asp:Button ID="btnCancel" runat="server" Text="取消" OnClick="btnCancel_Click" />
        <asp:Button ID="btnSubmit" runat="server" Text="送出" OnClick="btnSubmit_Click" />
        
    </div>
       
</asp:Content>
