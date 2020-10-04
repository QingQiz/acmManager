// GetAllUser Pagination
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

// GetAllUserFilter Submission
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

// UserPromoteFilter Pagination
let userPromotePaginationEvent = function () {
    let pageSize = parseInt($('#user-promote .user-promote-filter-MaxResultCount').val());
    let skipCount = parseInt($('#user-promote .user-promote-filter-skip-count').val());
    let pageIndex = Math.floor(skipCount / pageSize) + 1;

    let gotoPage = function (n) {
        $('#user-promote .user-promote-filter-skip-count').val((n - 1) * pageSize);
        $('#user-promote .user-promote-filter-submit-btn').click();
    }

    $('#user-promote-table .page-index').click(function () {
        let page = parseInt($(this).text());
        gotoPage(page);
    });

    $('#user-promote-table .page-next').click(function () {
        gotoPage(pageIndex + 1);
    });

    $('#user-promote-table .page-previous').click(function () {
        gotoPage(pageIndex - 1);
    });

    $('#user-promote-table .page-first').click(function () {
        gotoPage(1);
    });
}

// UserPromoteFilter Submission
$('#user-promote .user-promote-filter-submit-btn').click(function () {
    let form = $('#user-promote-form');
    
    $.ajax({
        url: form.attr('action'),
        method: 'post',
        data: form.serialize(),
        success: function (res) {
            $('#user-promote-filter').collapse('hide');
            $('#user-promote-table').html(res);
            userPromotePaginationEvent();
        }
    })
})

$(function () {
    $('#user-management .get-all-user-filter-submit-btn').click();
    $('#user-promote .user-promote-filter-submit-btn').click();
});