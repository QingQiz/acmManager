let putErrorMessage = function(xhr) {
    let errorMessage = JSON.parse(xhr.responseText)['error'];
    errorMessage = errorMessage['details'] == null ? errorMessage['message'] : errorMessage['details'];
    errorMessage = errorMessage.trim().split('\n').join('');

    alert(errorMessage);
}

let formSubmitEvent = function () {
    let form = $('#edit-article-form');

    $.ajax({
        url: form.attr('action'),
        method: 'post',
        data: form.serialize(),
        success: function (json) {
            window.onbeforeunload = undefined;
            location.replace(json.result['redirectUrl']);
        },
        error: function (xhr, a, b) {
            putErrorMessage(xhr);
        }
    })
}

$(function () {
    createEditor("article-content", false);

    $('#edit-article-submit-btn').click(formSubmitEvent);
});
