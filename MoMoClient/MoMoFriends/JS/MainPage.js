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
    var oTable = $('#example').dataTable({
        "bProcessing": true,
        "sAjaxSource": '../Scripts/arrays.txt',
        "sPaginationType": 'full_numbers'
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
		
		if ( sGrade == "A" )
			sTitle =  sBrowser+' will provide a first class (A) level of CSS support.';
		else if ( sGrade == "C" )
			sTitle = sBrowser+' will provide a core (C) level of CSS support.';
		else if ( sGrade == "X" )
			sTitle = sBrowser+' does not provide CSS support or has a broken implementation. Block CSS.';
		else
			sTitle = sBrowser+' will provide an undefined level of CSS support.';
		
		alert( sTitle )
	} );
    //给表格上的数据添加单击事件---结束
    //=================================================================================================
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