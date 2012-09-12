$(function () {
    //皮肤换肤功能实现---开始
    var $li = $("#skin li");
    $li.click(function () {
        switchSkin(this.id);
    });
    var cookie_skin = $.cookie("MyCssSkin");
    if (cookie_skin) {
        switchSkin(cookie_skin);
    };
    //皮肤换肤功能实现---结束

    ///表单验证规则
    $("#idInfoForm").validate({
        onsubmit: true,//表单提交时验证

        rules: {
            //规则定义
            username: {
                required: true,
                minlength: 2
            },
            birthdate: {
                required: true,
                date: true
                //minlength: 6,
                //maxlength: 20
            },
            sex: {
                required: true
            },
            school: {
                required: true
            }
        },
        //当输入信息不符合规则时的信息提示定义
        messages: {
            username: {
                required: "请输入姓名",
                minlength: jQuery.format("密码不能小于{0}个字符")
            },
            birthdate: {
                required: "请输入出生日期",
                minlength: jQuery.format("日期格式为xxxx-xx-xx")
            },
            sex: {
                required: "请选择性别"
            },
            school: {
                required: "请输入毕业院校"
            }
        },

        submitHandler: function (form) {
            var strUserName = encodeURI($("#username").val());
            var strBirth = encodeURI($("#birthday").val());
            var strSex = encodeURI($('input:radio[name="sex"]:checked').val());
            var strSchool = encodeURI($("#school").val());
            var strJob = encodeURI($("#job").val());
            var strMobil = encodeURI($("#mobilphone").val());
            var strQQ = encodeURI($("#QQ").val());
            var strMsn = encodeURI($("#MSN").val());
            var strWeibo = encodeURI($("#weibo").val());
            $.ajax({
                url: "../Ajax/IDInfoHandler.ashx",
                type: "post",
                //datatype: jason,
                data:
                    {
                        username: strUserName,
                        birthday: strBirth,
                        sex: strSex,
                        school: strSchool,
                        job: strJob,
                        mobilphone: strMobil,
                        qq: strQQ,
                        msn: strMsn,
                        weibo: strWeibo
                        //$("#idInfoForm").serialize(),
                    },

                success: function (result) {
                    if (result == "success") {
                        //注册成功后跳回登录界面进行登录
                        parent.document.location.href = "../HTML/MainPage.html";
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
});

//=================================================================================================

//切换皮肤函数---开始
function switchSkin(skinName) {
    $("#" + skinName).addClass("selected")                //当前<li>元素选中
					   .siblings().removeClass("selected");  //去掉其他同辈<li>元素的选中
    $("#cssfile").attr("href", "../Styles/skin/" + skinName + ".css"); //设置不同皮肤
    $.cookie("MyCssSkin", skinName, { path: '/', expires: 10 });
}
//切换皮肤函数---结束

//=================================================================================================