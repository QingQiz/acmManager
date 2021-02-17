$(function () {
    previewMd("article-content", $('#solution-content > textarea').text());
    
    $('.comments').each(function () {
        let id = $(this).attr('id');
        previewMd(id, $(this).find('textarea').text());
    })
})