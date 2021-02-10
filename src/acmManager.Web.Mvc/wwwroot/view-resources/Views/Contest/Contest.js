previewMd('contest-description-content', $('#contest-content > textarea').text())

if ($.find('#contest-result')) {
    previewMd('contest-result-content', $('#contest-result > textarea').text());
}

$('.card-header').click(function (event) {
    let target = $(this).find('button').attr('data-target');
    $(target).collapse('toggle');
});

$('[data-toggle="tooltip"]').tooltip();