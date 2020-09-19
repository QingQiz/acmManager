// Switch to Register
$('#SignUpLink').click(function () {
    $('#SignUpForm').css('bottom', 0)

    $("#LoginForm").animate({
        bottom: '-200px',
        opacity: 0,
    }, 500, function () {
        $('#LoginForm').hide();
        $('.alert').hide();
        $('#SignUpForm').show().animate({
            opacity: 1,
        },1000);
    });
});

// Switch to Login
$('#BackToLogin').click(function () {
    $('#LoginForm').css('bottom', 0)

    $('#SignUpForm').animate({
        bottom: '-200px',
        opacity: 0,
    }, 500, function () {
        $('#SignUpForm').hide();
        $('.alert').hide();
        $('#LoginForm').show().animate({
            opacity: 1,
        },1000);
    })
});

let putErrorMessage = function(xhr) {
    let errorMessage = JSON.parse(xhr.responseText)['error'];
    errorMessage = errorMessage['details'] == null ? errorMessage['message'] : errorMessage['details'];
    errorMessage = errorMessage.trim().split('\n').join('<br>');

    $('.ErrorMessage').html(errorMessage);
    $('.alert-danger').css('opacity', 0).show().animate({opacity: 1}, 500);
}

let LoginEvent = function (url) {
    $('#Login').click(function () {
        let form = $('form').first();
        $.ajax({
            type: 'post',
            data: form.serialize(),
            success: function (data, status) {
                let targetUrl = window.location.origin + data['targetUrl'];
                window.location.replace(targetUrl);
            },
            error: function (xhr, status, error) {
                putErrorMessage(xhr)
            },
            url: url
        });
    });
}

let RegisterEvent = function (url) {
    $('#Register').click(function() {
        let form = $('form').last();
        $.ajax({
            type: 'post',
            data: form.serialize(),
            success: function (data, status) {
                $('.alert').hide();
                $('#RegisterSuccess').css('opacity', 0).show().animate({opacity: 1}, 500);
            },
            error: function (xhr, status, error) {
                console.log(xhr.responseText);
                putErrorMessage(xhr);
            },
            url: url
        });
    });
}
