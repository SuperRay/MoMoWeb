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
    //=================================================================================================

    //加载表格数据---开始
    //var oTable = $('#example').dataTable({
    //    "bProcessing": true,
    //    //"sAjaxSource": '../Scripts/arrays.txt',
    //    "bServerSide": true,
    //    'bPaginate': true,
    //    "sPaginationType": 'full_numbers',
    //    "sAjaxSource": "../Common/GetUsersInfo/AjaxHandler",
    //    'bLengthChange': true,
    //    "aoColumns": [
    //                            { "sTitle": "姓名", "sClass": "center" },
    //                            { "sTitle": "生日", "sClass": "center" },
    //                            { "sTitle": "性别", "sClass": "center" },
    //                            { "sTitle": "毕业学校", "sClass": "center" },
    //                            { "sTitle": "职业", "sClass": "center" },
    //                            { "sTitle": "手机号码", "sClass": "center" },
    //                            { "sTitle": "QQ", "sClass": "center" },
    //                            { "sTitle": "Msn", "sClass": "center" },
    //                            { "sTitle": "微博", "sClass": "center" },
    //                            { "sTitle": "英文名", "sClass": "center" },
    //                            {
    //                                "sTitle": "操作",
    //                                "sClass": "center",
    //                                "fnRender": function (obj) {
    //                                    return '<a href=\"Details/' + obj.aData[0] + '\">查看详情</a>  <input tag=\"' + obj.aData[0] + '\" type=\"checkbox\" name=\"name\" />';
    //                                }
    //                            }
    //    ]
    var oTable = $('#example').dataTable({
            "bServerSide": true,
            "sAjaxSource": "../Common/GetUsersInfo.AjaxHandler",      //mvc后台ajax调用接口。
            'bPaginate': true,                      //是否分页。
            "bProcessing": true,                    //当datatable获取数据时候是否显示正在处理提示信息。
            'bFilter': false,                       //是否使用内置的过滤功能。
            'bLengthChange': true,                  //是否允许用户自定义每页显示条数。
            'sPaginationType': 'full_numbers',      //分页样式
            "aoColumns": [
                    { "sName": "ID",
                        "bSearchable": false,
                        "bSortable": false,
                        "fnRender": function (oObj) {
                            return '<a href=\"Details/' + oObj.aData[0] + '\">View</a>';
                        }                           //自定义列的样式
                    },
                    { "sName": "COMPANY_NAME" },
                    { "sName": "ADDRESS" },
                    { "sName": "TOWN" }
            ]

        //"fnServerData": function (fnCallback, oSettings) {
        //    oSettings.jqXHR = $.ajax({
        //        "url": "../Ajax/GetInfoHandler.ashx",
        //        //"data": aoData,
        //        "success": fnCallback,
        //        "dataType": "jsonp",
        //        "cache": false
        //    });
        //}
    });
    //加载表格数据---结束
    //=================================================================================================

    //广告自动滑动显示---开始
    var len = $(".num > li").length;
    var index = 0;
    var adTimer;
    $(".num li").mouseover(function () {
        index = $(".num li").index(this);
        showImg(index);
    }).eq(0).mouseover();
    //广告自动滑动显示---结束
    //=================================================================================================

    //滑入 停止动画，滑出开始动画.
    $('.ad').hover(function () {
        clearInterval(adTimer);
    }, function () {
        adTimer = setInterval(function () {
            showImg(index)
            index++;
            if (index == len) { index = 0; }
        }, 3000);
    }).trigger("mouseleave");
    //=================================================================================================

    //给表格上的数据添加单击事件---开始
    $('#example tbody tr', oTable.fnGetNodes()).live('dblclick', function () {
        var sTitle;
        var nTds = $('td', this);
        var sBrowser = $(nTds[1]).text();
        var sGrade = $(nTds[4]).text();

        if (sGrade == "A")
            sTitle = sBrowser + ' will provide a first class (A) level of CSS support.';
        else if (sGrade == "C")
            sTitle = sBrowser + ' will provide a core (C) level of CSS support.';
        else if (sGrade == "X")
            sTitle = sBrowser + ' does not provide CSS support or has a broken implementation. Block CSS.';
        else
            sTitle = sBrowser + ' will provide an undefined level of CSS support.';

        alert(sTitle)
    });

    //给表格上的数据添加单击事件---结束
    //=================================================================================================
    //====================================================================================================

    //单击用户名显示用户详细信息
    $("#name").click(function () {
        alert("test");
    });
    loadUserInfo();
});

//=================================================================================================
//=================================================================================================
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

//显示图片广告函数---开始
function showImg(index) {
    var adHeight = $(".content_right .ad").height();
    $(".slider").stop(true, false).animate({ top: -adHeight * index }, 1000);
    $(".num li").removeClass("on")
			.eq(index).addClass("on");
}
//显示图片广告函数---结束

//===================================================================================================

//加载用户信息函数
function loadUserInfo() {

    $.ajax({
        type: "post",
        url: "../Ajax/GetInfoHandler.ashx",
        success: function (result) {
            var loginName = result;
            $("#name").title = loginName;
            document.getElementById("name").innerText = loginName;
        }
    })
}



