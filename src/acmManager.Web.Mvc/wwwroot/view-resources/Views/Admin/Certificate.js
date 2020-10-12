$('input[name=TimeStart]').change(function () {
    let timeStart = $(this).val();
    $('input[name=TimeEnd]').attr('min', timeStart);
});

$('input[name=TimeEnd]').change(function () {
    let timeEnd = $(this).val();
    $('input[name=TimeStart]').attr('max', timeEnd);
});

$('#certificate-management .get-all-certificate-filter-submit-btn').click(function () {
    let form = $('#get-all-certificate-filter-form');
    
    $.ajax({
        url: form.attr('action'),
        method: 'post',
        data: form.serialize(),
        success: function (data) {
            $('#certificate-table').html(data);
        },
        error: function (xhr, a, b) {
            alert('error: ' + a + ' ' + b);
        }
    })
});