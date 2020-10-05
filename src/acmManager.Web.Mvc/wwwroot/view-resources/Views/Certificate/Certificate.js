$('#test-btn').click(function () {
    let form = $('#upload-certificate-form').closest("form");
    let formData = new FormData(form[0]);
    
    $.ajax(form.attr('action'), {
        method: 'post',
        data: formData,
        processData: false,
        contentType: false,
        success: function () {
            alert('success')
        },
        error: function(xhr, error, status) {
            alert('error');
            console.log(error, status);
        }
    })
});

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
})