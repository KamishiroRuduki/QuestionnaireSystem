<%@ Page Title="" Language="C#" MasterPageFile="~/master/user.Master" AutoEventWireup="true" CodeBehind="form.aspx.cs" Inherits="QuestionnaireSystem.form" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        #date {
            display: flex;
            justify-content: end;
        }
        .auto-style1 {
            width: 819px;
            height: 223px;
        }
    </style>
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

            var ID = getArgs("ID"); //從Request.QueryString取得問卷ID
            var strURL = "/Handlers/QuestionHandler.ashx?QuestionnaireID=" + ID; //用AJAX取得該問卷的所有問題的資料
            $.ajax({
                url: strURL,
                type: "GET",
                data: {},
                success: function (result) {
                    for (var i = 0; i < result.length; i++) {
                        var obj = result[i];
                        var htmltext = `<div><br/><p>${i + 1}.${obj.Name}`;//第幾題跟題目名

                        if (obj.IsMust) {
                            htmltext += `(必填)</p>`;
                        }
                        else
                            htmltext += `</p>`;
                        if (obj.Type == 0) {        //單選方塊，用radio button
                            var optionArr = obj.QusetionOption.split(';'); //字串分割
                            for (var j = 0; j < optionArr.length; j++) { //把所有選項都建出來
                                htmltext += `<input type="radio" ID="${i}" name="${obj.ID}" value = ${optionArr[j]} />${optionArr[j]}`; //同一個問題裡選項的radio button的name要相同
                            }

                        }
                        else if (obj.Type == 1) {  //複選方塊，用check box
                            var optionArr = obj.QusetionOption.split(';');//字串分割
                            for (var j = 0; j < optionArr.length; j++)  //把所有選項都建出來
                                htmltext += `<input type="checkbox" ID="${optionArr[j]}" name="${obj.ID}" value =${optionArr[j]} />${optionArr[j]}`;
                            //同一個問題裡選項的check box的name要相同
                        }
                        else if (obj.Type == 2){
                          //  htmltext += `<input type="Text" ID="${i}" name="${obj.ID}" />`; //文字方塊
                            htmltext += `<input type="Text" ID="${i}" name="${obj.ID}" onkeyup="this.value=this.value.replace(/[%&',;=?$\]/g,'')" />`;
                        }
                        else if (obj.Type == 3) {
                            htmltext += `<input type="number" ID="${i}" name="${obj.ID}" onkeyup="this.value=this.value.replace(/[%&',;=?$\]/g,'')" />`; //文字方塊(數字)
                        }
                        else if (obj.Type == 4) {
                            htmltext += `<input type="email" ID="${i}" name="${obj.ID}" onkeyup="this.value=this.value.replace(/[%&',;=?$\]/g,'')" />`; //文字方塊(mail)
                        }
                        else if (obj.Type == 5) {
                            htmltext += `<input type="date" ID="${i}" name="${obj.ID}" onkeyup="this.value=this.value.replace(/[%&',;=?$\]/g,'')" />`; //文字方塊(日期)
                        }
                        htmltext += `<br /></div >`;
                        $("#main").append(htmltext);//結束這一題的區塊，並放進建好的div內
                    }

                }
            });

        });
        $(document).ready(function () { //session資料回填用，ready是等整個頁面跑完才執行此function
            var ID = getArgs("ID");
            var strURL = "/Handlers/QuestionHandler.ashx?QuestionnaireID=" + ID;
            var test = "<%=Session["Answer"] %>"; //抓出作答者存在session的回答資料
            if (test != "" && test != null) {//如果不為空或NULL才做回填的動作
                var testarr = test.split(';');//答案的字串做分割
                $.ajax({
                    url: strURL,
                    type: "GET",
                    data: {},
                    success: function (result) {
                        for (var i = 0; i < result.length; i++) {
                            var obj = result[i];
                            if (obj.Type == 0) { //單選方塊，radio button
                                if (testarr[i] != " ")
                                    $("input[name=" + obj.ID + "][value=" + testarr[i] + "]").prop("checked", true);//用問題ID找到radio button，再把value符合的選項勾選

                            }
                            else if (obj.Type == 1) { //單選方塊，check box
                                if (testarr[i] != " ") {
                                    var optionarr = testarr[i].split(','); //複數選項的關係，要把該題的答案做分割
                                    // $('input:checkbox:first').attr("checked", 'checked');
                                    //$("input[name=" + obj.ID + "]").attr("checked", true);
                                    for (var j = 0; j < optionarr.length; j++) {
                                        //  $("input:checkbox:[value=" + optionarr[j] + "]").attr("checked", 'checked');
                                        //$("input:checkbox[value=" + optionarr[j] + "]").prop("checked", true);
                                        $("input[name=" + obj.ID + "][value=" + optionarr[j] + "]").prop("checked", true);//用問題ID找到check box，再把value符合的選項勾選
                                        //$("#" + optionarr[j]).prop("checked", true); // 用ID去找到符合答案的選項並勾選(建立check box時我有把答案的值放到ID，所以可以這樣找)

                                    }
                                }
                            }
                            else {
                                var testt = testarr[i];
                                //$("#" + i).val("testt");
                                //$("#4").val("testt");
                                //var testt = testarr[i];
                                $("input[name=" + obj.ID + "]").val(testarr[i]);//用name找到文字方塊，並把值填入
                               // $("#4").attr("value", "123");
                            }
                        }

                    }
                });
            }
        });

    </script>
    <div id="date">
        <asp:Literal ID="litDate" runat="server"></asp:Literal>
    </div>

    <div class="entry-content" style="padding-left: 300px;">
        <div class="auto-style1 jumbotron text-center" >
            <asp:Label ID="lblTitle" runat="server" style="font-size: 20px;font-weight:bold" ></asp:Label><br />
            <asp:Literal ID="litCaption" runat="server"></asp:Literal><br />
            <br />
            <br />
           </div>
        <div>
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
        <asp:Button ID="btnCancel" runat="server" Text="取消" OnClick="btnCancel_Click" />
        <asp:Button ID="btnSubmit" runat="server" Text="送出" OnClick="btnSubmit_Click" /><br/>
        <asp:Literal ID="ltMsg" runat="server"></asp:Literal>
        <asp:HiddenField ID="HFID" runat="server" />
    </div>
</asp:Content>

