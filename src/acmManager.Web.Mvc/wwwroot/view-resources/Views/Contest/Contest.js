previewMd('contest-description-content', $('#contest-content > textarea').text())

if ($.find('#contest-result')) {
    previewMd('contest-result-content', $('#contest-result > textarea').text());
}

$('[data-toggle="tooltip"]').tooltip();