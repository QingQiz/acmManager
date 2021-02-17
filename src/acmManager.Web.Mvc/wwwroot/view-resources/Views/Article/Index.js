$(function () {
    $('#search-article-btn').click(function () {
        let keyword = $('#search-article-content').val();
        let userId = $('#search-article-user').val();
        
        if (userId === '0') {
            location.href = location.pathname + "?keyword=" + keyword;
        } else {
            location.href = location.pathname + "?keyword=" + keyword + "&user=" + userId;
        }
    });
    
    $('.text-preview').each(function () {
        let id = $(this).attr('id');
        previewMd(id, $(this).find('textarea').text());
    })
})