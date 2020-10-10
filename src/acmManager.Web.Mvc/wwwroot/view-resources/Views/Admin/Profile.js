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

// delete certificate
$('.delete-certificate-submit').click(function () {
    let form = $(this).closest("form");

    $.ajax({
        url: form.attr('action'),
        method: 'post',
        data: form.serialize(),
        success: function () {
            form.find('.delete-certificate-dismiss').click();
            location.reload();
        }
    });
});

// 当切换tab时，网 url 里放置锚
$('.list-group-item').click(function () {
    let href = window.location.href.split('#')[0];
    window.location.replace(href + $(this).attr('href'));
});

// 用url里的锚定位tab
$(function () {
    let href = '#' + window.location.href.split('#')[1];
    $(`a[href="${href}"`).click();
});
