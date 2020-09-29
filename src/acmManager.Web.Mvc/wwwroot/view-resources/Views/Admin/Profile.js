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