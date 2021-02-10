$(function () {
    $('[data-toggle="tooltip"]').tooltip();
    
    previewMd("problem-description", $('#problem-description > textarea').text());
    previewMd("solution-content", $('#solution-content > textarea').text());
})