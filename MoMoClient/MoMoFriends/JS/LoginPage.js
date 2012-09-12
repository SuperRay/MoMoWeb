$(function () {
    var cookieName = "username";
    if ($.cookie(cookieName)) {
        $("#username").val($.cookie(cookieName));
    }

    ///是否保存用户名点击事件
    $("#remuser").click(function () {
        if (this.checked) {
            //选中则将用户名存在cookie中，保存期限为10天
            $.cookie(cookieName, $("#username").val(), { path: '/', expires: 10 });
        }
        else {
            //没选中则将cookie清空
            $.cookie(cookieName, null, { path: '/' });
        }
    });

    ///登录按钮点击事件
    $("#btnLogin").click(function(){
        if ($("#username").val() == "" || $("#password").val() == "") {
            $("#loginTips").val("用户名或密码不能为空！");
        }
        else {
            ///与后台进行交互，判断用户名与密码是否正确
            $.ajax({
                url: "../Ajax/LoginHandler.ashx",
                type: "POST",                
                data: "username=" + escape($('#username').val()) + "&password=" + escape($('#password').val()),
                beforeSend: function () {
                    //$("#loading").css("display", "block"); //点击登录后显示loading，隐藏输入框
                    $("li.Login").css("display", "none");
                },
                success: function (msg) {
                    //$("#loading").hide(); //隐藏loading
                    if (msg == "success") {
                        $("#loginTips").val("成功！");
                        //parent.tb_remove();                     
                        parent.document.location.href = "../HTML/MainPage.html"; //如果登录成功则跳到管理界面
                        //parent.tb_remove();
                    }
                    if (msg == "fail") {
                        $("#loginTips").val("用户名或密码错误！");
                    }
                },
                complete: function (data) {
                    //$("#loading").css("display", "none"); //点击登录后显示loading，隐藏输入框
                    $("#login").css("display", "block");
                },
                error: function (XMLHttpRequest, textStatus, thrownError) {
                }
            });


        }
    })
})