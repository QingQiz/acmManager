// Pagination
let getAllUserPaginationEvent = function () {
    let pageSize = parseInt($('#user-management .get-all-user-filter-MaxResultCount').val());
    let skipCount = parseInt($('#user-management .get-all-user-filter-skip-count').val());
    let pageIndex = Math.floor(skipCount / pageSize) + 1;

    let gotoPage = function (n) {
        $('#user-management .get-all-user-filter-skip-count').val((n - 1) * pageSize);
        $('#user-management .get-all-user-filter-submit-btn').click();
    }

    $('#get-all-user-table .page-index').click(function () {
        let page = parseInt($(this).text());
        gotoPage(page);
    });
    
    $('#get-all-user-table .page-next').click(function () {
        gotoPage(pageIndex + 1);
    });

    $('#get-all-user-table .page-previous').click(function () {
        gotoPage(pageIndex - 1);
    });

    $('#get-all-user-table .page-first').click(function () {
        gotoPage(1);
    });
}

$('#user-management .get-all-user-filter-submit-btn').click(function () {
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
    $('#user-management .get-all-user-filter-submit-btn').click();
});