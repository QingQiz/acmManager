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