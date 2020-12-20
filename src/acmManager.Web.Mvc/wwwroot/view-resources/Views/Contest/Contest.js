editormd.markdownToHTML('contest-description-content', {
    markdown: $('#contest-content > textarea').text()
});

if ($.find('#contest-result')) {
    editormd.markdownToHTML('contest-result-content', {
        markdown: $('#contest-result > textarea').text()
    });
}

$('.card-header').click(function (event) {
    let target = $(this).find('button').attr('data-target');
    $(target).collapse('toggle');
});

$('[data-toggle="tooltip"]').tooltip();