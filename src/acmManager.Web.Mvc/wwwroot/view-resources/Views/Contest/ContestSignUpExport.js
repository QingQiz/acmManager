let createDownload = function (filename, text) {
    let element = document.createElement('a');
    element.setAttribute('href', 'data:text/plain;charset=utf-8,' + encodeURIComponent(text));
    element.setAttribute('download', filename);

    element.style.display = 'none';
    document.body.appendChild(element);

    element.click();

    document.body.removeChild(element);
}

$('#contest-sign-up-export').click(function () {
    let form = $('#contest-sign-up-export-form');
    $.ajax({
        url: form.attr('action'),
        method: 'get',
        data: form.serialize(),
        success: function (xhr) {
            console.log(xhr)
            createDownload("ContestSignUp.tsv", xhr.result.result);
        }
    });
})