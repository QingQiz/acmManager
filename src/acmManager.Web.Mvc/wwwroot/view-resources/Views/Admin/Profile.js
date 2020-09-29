$('#update-user-btn').click(function () {
    let form = $('#update-user-form');

    $.ajax({
        url: form.attr('action'),
        method: 'post',
        data: form.serialize(),
        success: function () {
            location.reload();
        }
    });
});

$('#delete-user-submit').click(function () {
    let form = $('#delete-user-form');
    
    $.ajax({
        url: form.attr('action'),
        method: 'post',
        data: form.serialize(),
        success: function () {
            $('#delete-user-cancel').click();
            window.history.back();
        }
    })
})

let putErrorMsg = function (xhr, selector='.alert-danger') {
    let errorMessage = JSON.parse(xhr.responseText)['error'];
    errorMessage = errorMessage['details'] == null ? errorMessage['message'] : errorMessage['details'];
    errorMessage = errorMessage.trim().split('\n').join('<br>');

    $(selector).html(errorMessage).css('opacity', 0).show().animate({opacity: 1}, 500);
}

$('#reset-password-btn').click(function () {
    let form = $('#reset-password-form');
    
    $.ajax({
        url: form.attr('action'),
        method: 'post',
        data: form.serialize(),
        success: function () {
            $('#reset-password-error').animate({opacity: 0}, 500).hide();
            $('#reset-password-success').css('opacity', 0).show().animate({opacity: 1}, 500);
        },
        error: function (xhr, status, error) {
            $('#change-password-success').animate({opacity: 0}, 500).hide();
            putErrorMsg(xhr, '#reset-password-error');
        }
    })
})
