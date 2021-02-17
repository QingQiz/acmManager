$(function () {
    $('.text-preview').each(function () {
        let id = $(this).attr('id');
        previewMd(id, $(this).find('textarea').text());
    })
})
