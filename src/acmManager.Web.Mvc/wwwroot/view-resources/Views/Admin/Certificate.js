$('input[name=TimeStart]').change(function () {
    let timeStart = $(this).val();
    $('input[name=TimeEnd]').attr('min', timeStart);
});

$('input[name=TimeEnd]').change(function () {
    let timeEnd = $(this).val();
    $('input[name=TimeStart]').attr('max', timeEnd);
});

let DeleteCertificateEvent = function () {
    $('.delete-certificate-btn').click(function () {
        let id = $(this).attr('id').split('-')[1];
        let inp = $('#delete-certificate-modal input[type=hidden]:nth-child(1)')

        inp.attr('value', id);
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
}

$('#certificate-management .get-all-certificate-filter-submit-btn').click(function () {
    let form = $('#get-all-certificate-filter-form');
    
    $.ajax({
        url: form.attr('action'),
        method: 'post',
        data: form.serialize(),
        success: function (data) {
            $('#certificate-table').html(data);
            DeleteCertificateEvent();
        },
        error: function (xhr, a, b) {
            alert('error: ' + a + ' ' + b);
        }
    })
});