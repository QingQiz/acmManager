let putErrorMsg = function (xhr, selector='.alert-danger') {
    let errorMessage = JSON.parse(xhr.responseText)['error'];
    errorMessage = errorMessage['details'] == null ? errorMessage['message'] : errorMessage['details'];
    errorMessage = errorMessage.trim().split('\n').join('<br>');

    $(selector).html(errorMessage).css('opacity', 0).show().animate({opacity: 1}, 500);
}

$('#upload-certificate-submit-btn').click(function () {
    let form = $('#upload-certificate-form').closest("form");
    let formData = new FormData(form[0]);

    $.ajax(form.attr('action'), {
        method: 'post',
        data: formData,
        processData: false,
        contentType: false,
        success: function () {
            $('.modal').modal('hide');
            location.reload();
        },
        error: function(xhr, error, status) {
            putErrorMsg(xhr, '.modal .alert-danger')
        }
    })
});

$('input[type=file]').change(function () {
    let fileName = $(this).val().split('\\').pop();
    $('.custom-file-label').addClass('selected').html(fileName);
});
