let updateProfileEvent = function (url) {
    $('#UpdateProfile').click(function () {
        let formData = $('#ProfileForm').serialize();
        
        $.ajax({
            type: 'post',
            data: formData,
            url: url,
            success: function () {
                location.reload();
            },
            error: function (xhr, status, error) {
                let errorMessage = JSON.parse(xhr.responseText)['error'];
                errorMessage = errorMessage['details'] == null ? errorMessage['message'] : errorMessage['details'];
                errorMessage = errorMessage.trim().split('\n').join('<br>');
                
                $('.alert-danger').html(errorMessage).css('opacity', 0).show().animate({opacity: 1}, 500);
            }
        })
    })
}