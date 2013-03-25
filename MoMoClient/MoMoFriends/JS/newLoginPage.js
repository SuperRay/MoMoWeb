$(function () {   
    ///表单验证规则
    $("#loginform").validate({
        onsubmit: true,//表单提交时验证

        rules: {
            //规则定义
            username: {
                required: true,
                minlength: 2,
                maxlength: 50
            },
            pwd: {
                required: true,
                maxlength: 20
            }
        },
        //当输入信息不符合规则时的信息提示定义
        messages: {
            username: {
                required: "请输入姓名",
                minlength: jQuery.format("用户名无效"),
                maxlength: jQuery.format("用户名无效")
            },
            pwd: {
                required: "请输入密码"
            }
        },

        submitHandler: function (form) {
            $.ajax({
                url: "../Ajax/LoginHandler.ashx",
                type: "POST",
                data: "username=" + escape($('#username').val()) + "&password=" + escape($('#pwd').val()),
                beforeSend: function () {
                    //$("#loading").css("display", "block"); //点击登录后显示loading，隐藏输入框
                    $("#err_m").val("正在登陆...");

                },
                success: function (msg) {
                    //$("#loading").hide(); //隐藏loading
                    if (msg == "success") {
                        $("#err_m").val("成功！");
                        //parent.tb_remove();                     
                        parent.document.location.href = "../HTML/MainPage.html"; //如果登录成功则跳到管理界面
                        //parent.tb_remove();
                    }
                    if (msg == "fail") {
                        $("#err_m").css("display", "block");
                        $("#err_m").val("用户名或密码错误！");
                    }
                },
                complete: function (data) {
                    //$("#loading").css("display", "none"); //点击登录后显示loading，隐藏输入框
                    $("#login").css("display", "block");
                },
                error: function (XMLHttpRequest, textStatus, thrownError) {
                }
            });
        },

        invalidHandler: function (form, validator) {
            return false;
        }
    });
})