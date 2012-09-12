$(function () {
    ///表单验证规则
    $("#regInfoForm").validate({
        onsubmit: true,//表单提交时验证

        rules: {
            //规则定义
            username: {
                required: true,
                minlength: 2
            },
            password1: {
                required: true,
                minlength: 6,
                maxlength: 20
            },
            password2: {
                required: true,
                minlength: 6,
                maxlength: 20,
                equalTo: "#pass1"
            },
            email: {
                required: true,
                email: true
            }
        },
        //当输入信息不符合规则时的信息提示定义
        messages: {
            username: {
                required: "请输入姓名",
                minlength: jQuery.format("密码不能小于{0}个字符")
            },
            password1: {
                required: "请输入密码",
                minlength: jQuery.format("密码不能小于{0}个字符")
            },
            password2: {
                required: "请输入确认密码",
                minlength: "确认密码不能小于6个字符",
                equalTo: "两次输入密码不一致"
            },
            email: {
                required: "请输入Email地址",
                email: "请输入正确的email地址"
            }
        },
        
        submitHandler: function (form) {
            var strUserName = encodeURI($("#regName").val());
            var strPassword = encodeURI($("#pass1").val());
            $.ajax({
                url: "../Ajax/RegistHandler.ashx",
                type: "post",                
                data: {
                    username: strUserName,
                    password: strPassword
                },
                success: function (result) {
                    if (result == "success") {
                       //注册成功后跳回登录界面进行登录
                        parent.document.location.href = "../HTML/IDCardPage.html";
                        sessionStorage.setItem("loginName", strUserName);
                    }
                    else {
                        alert("注册失败！");
                        //对注册失败信息进行反馈
                    }
                }
            });
        },

        invalidHandler: function (form, validator) {
            return false;
        }
    });
})