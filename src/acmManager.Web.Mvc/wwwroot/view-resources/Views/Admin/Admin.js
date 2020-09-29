// Pagination
let getAllUserPaginationEvent = function () {
    let pageSize = parseInt($('#get-all-user-filter-MaxResultCount').val());
    let skipCount = parseInt($('#get-all-user-filter-skip-count').val());
    let pageIndex = Math.floor(skipCount / pageSize) + 1;

    let gotoPage = function (n) {
        $('#get-all-user-filter-skip-count').val((n - 1) * pageSize);
        $('#get-all-user-filter-submit-btn').click();
    }

    $('.get-all-user-page-link').click(function () {
        let page = parseInt($(this).text());
        gotoPage(page);
    });
    
    $('#get-all-user-page-next').click(function () {
        gotoPage(pageIndex + 1);
    });

    $('#get-all-user-page-previous').click(function () {
        gotoPage(pageIndex - 1);
    });

    $('#get-all-user-page-first').click(function () {
        gotoPage(1);
    });
}

$('#get-all-user-filter-submit-btn').click(function () {
    let form = $('#get-all-user-filter-form');
    
    $.ajax({
        url: form.attr('action'),
        method: 'post',
        data: form.serialize(),
        success: function (res) {
            $('#user-filter').collapse('hide');
            $('#get-all-user-table').html(res);
            getAllUserPaginationEvent();
        }
    })
});

$(function () {
    $('#get-all-user-filter-submit-btn').click();
});