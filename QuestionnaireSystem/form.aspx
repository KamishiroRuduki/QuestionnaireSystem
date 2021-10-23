<%@ Page Title="" Language="C#" MasterPageFile="~/master/user.Master" AutoEventWireup="true" CodeBehind="form.aspx.cs" Inherits="QuestionnaireSystem.form" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        #date {
            display: flex;
            justify-content: end;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script>
        function getArgs(strParame) {
            var args = new Object();
            var query = location.search.substring(1); // Get query string
            var pairs = query.split("&"); // Break at ampersand
            for (var i = 0; i < pairs.length; i++) {
                var pos = pairs[i].indexOf('='); // Look for "name=value"
                if (pos == -1) continue; // If not found, skip
                var argname = pairs[i].substring(0, pos); // Extract the name
                var value = pairs[i].substring(pos+1, pairs[i].length); // Extract the value
                value = decodeURIComponent(value); // Decode it, if needed
                args[argname] = value; // Store as a property
            }
            return args[strParame]; // Return the object
        }
        $(function () {
            
            var ID = getArgs("ID");
            var strURL = "http://localhost:2305/Handlers/QuestionHandler.ashx?QuestionnaireID=" + ID;
            $.ajax({
                url: strURL,
                type: "GET",
                data: {},
                success: function (result) {
                    var table = '<table class="table table-striped">';                    
                    for (var i = 0; i < result.length; i++) {                       
                        var obj = result[i];
                        var htmltext = `<div"><br/><p>${i + 1}.${obj.Name}`;
                        if (obj.IsMust) {
                            htmltext += `(必填)</p>`;
                        }
                        else
                            htmltext += `</p>`;
                        if (obj.Type == 0) {
                            var optionArr = obj.QusetionOption.split(';');
                            for (var j = 0; j < optionArr.length; j++) {
                                htmltext += `<input type="radio" ID="quesRadio" name="quesRadio"+${i} value = ${j}/>${optionArr[j]}`;
                            }
                               
                        }
                        else if (obj.Type == 1) {
                            var optionArr = obj.QusetionOption.split(';');
                            for (var j = 0; j < optionArr.length; j++)
                                htmltext += `<input type="checkbox" ID="quesCheckbox" name="quesCheckbox"+${i} value = ${j} />${optionArr[j]}`;
                        }
                        else{
                            htmltext += `<input type="Text" ID="quesTextbox" name="quesTextbox"+${i} />`;
                        }

                        htmltext += `<br /></div >`;
                        $("#main").append(htmltext);
                    }
                    
                }
            });
            


        });

    </script>
    <div id="date">
        <asp:Literal ID="litDate" runat="server"></asp:Literal>
    </div>

    <div>
        <div>
            <asp:Literal ID="litTitle" runat="server"></asp:Literal><br />
            <asp:Literal ID="litCaption" runat="server"></asp:Literal><br />
            <br />
            <br />
            姓名&nbsp&nbsp<asp:TextBox ID="txtName" runat="server"></asp:TextBox><br />
            <br />
            手機&nbsp&nbsp<asp:TextBox ID="txtPhone" runat="server"></asp:TextBox><br />
            <br />
            Email&nbsp<asp:TextBox ID="txtEmail" runat="server"></asp:TextBox><br />
            <br />
            年齡&nbsp&nbsp<asp:TextBox ID="txtAge" runat="server"></asp:TextBox><br />
            <br />
        </div>
        <div id="main">
        </div>
        <asp:HiddenField ID="HFID" runat="server" />
    </div>
</asp:Content>
