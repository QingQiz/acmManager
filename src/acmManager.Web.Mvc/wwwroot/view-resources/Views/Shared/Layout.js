$('.card-header').click(function (event) {
    let target = $(this).find('button').attr('data-target');
    $(target).collapse('toggle');
});

// 百度访问统计
var _hmt = _hmt || [];
(function() {
    let hm = document.createElement("script");
    hm.src = "https://hm.baidu.com/hm.js?686023326bba34e1a6c8c701758124b6";
    let s = document.getElementsByTagName("script")[0];
    s.parentNode.insertBefore(hm, s);
})();

// scroll to top button
$(function () {
    $(window).scroll(function () {
        let w = $(this);
        
        if (w.width() < 800) return;
        
        if (w.scrollTop() > 50) {
            $('#scroll-to-top').fadeIn();
        } else {
            $('#scroll-to-top').fadeOut();
        }
    });
    
    $('#scroll-to-top').click(function () {
        $('html').animate({
            scrollTop: 0
        }, 500);
        return false;
    });
});