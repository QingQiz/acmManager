$(function () {
    $('[data-toggle="tooltip"]').tooltip();
    
    previewMd("problem-description", $('#problem-description > textarea').text());
    previewMd("solution-content", $('#solution-content > textarea').text());
    
    $('.comments').each(function () {
        let id = $(this).attr('id');
        previewMd(id, $(this).find('textarea').text());
    })
})