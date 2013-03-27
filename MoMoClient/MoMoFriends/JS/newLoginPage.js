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
                required: "用户名不能为空",
                minlength: jQuery.format("用户名无效"),
                maxlength: jQuery.format("用户名无效")
            },
            pwd: {
                required: "密码不能为空"
            }
        },

        errorContainer: "#err_m",

        errorLabelContainer: $("#err_m"),

        wrapper: "string",

        errorPlacement: function (error, element) {
            $("#err_m").css("display", "none");
            if (element.attr("name") == "username")
            { error.appendTo("#err_m"); }
            else if (element.attr("name") == "pwd") {
                error.appendTo("#err_m");
            }
        } ,

        submitHandler: function (form) {
            $.ajax({
                url: "../Ajax/LoginHandler.ashx",
                type: "POST",
                data: "username=" + escape($('#username').val()) + "&password=" + escape($('#pwd').val()),
                beforeSend: function () {                     
                    $("#err_m").css("display", "none"); //点击登录后隐藏提示信息
                    $("#context").css("cursor", "wait");
                },
                success: function (msg) {

                    if (msg == "success") {                    
                        parent.document.location.href = "../HTML/MainPage.html"; //如果登录成功则跳到管理界面
                    }
                    if (msg == "fail") {
                        $("#err_m").css("display", "block");
                        $("#err_m").html('<label class="error">用户名或密码错误</label>');
                        $("#context").css("cursor", "default");
                    }
                },
                complete: function (data) {

                   
                },
                error: function (XMLHttpRequest, textStatus, thrownError) {
                }
            });
        },

        invalidHandler: function (form, validator) {
            $("#context").css("cursor", "default");
            return false;
        }
    });
})