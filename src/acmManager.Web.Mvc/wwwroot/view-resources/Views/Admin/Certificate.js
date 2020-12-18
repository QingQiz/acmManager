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

// GetAllCertificate Pagination
let getAllCertificateEvent = function () {
    let pageSize = parseInt($('#certificate-management .certificate-filter-MaxResultCount').val());
    let skipCount = parseInt($('#certificate-management .certificate-filter-SkipCount').val());
    let pageIndex = Math.floor(skipCount / pageSize) + 1;

    let gotoPage = function (n) {
        $('#certificate-management .certificate-filter-SkipCount').val((n - 1) * pageSize);
        $('#certificate-management .get-all-certificate-filter-submit-btn').click();
    }

    $('#certificate-table .page-index').click(function () {
        let page = parseInt($(this).text());
        gotoPage(page);
    });

    $('#certificate-table .page-next').click(function () {
        gotoPage(pageIndex + 1);
    });

    $('#certificate-table .page-previous').click(function () {
        gotoPage(pageIndex - 1);
    });

    $('#certificate-table .page-first').click(function () {
        gotoPage(1);
    });
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
            getAllCertificateEvent();
        },
        error: function (xhr, a, b) {
            alert('error: ' + a + ' ' + b);
        }
    })
});
