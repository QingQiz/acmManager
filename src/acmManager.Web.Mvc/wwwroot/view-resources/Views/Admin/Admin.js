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

// UserPromote Checkbox Actions
let userPromoteCheckboxEvent = function () {
    let form = $('#user-promote-form');
    let selected = 0;
    let checkbox = $('.user-promote-checkbox');
    let checkboxSize = checkbox.length;
    let selectAll = $('.user-promote-checkbox-select-all');
 
    checkbox.click(function () {
        let userId = $(this).attr('id').split('-').slice(-1);
        if ($(this).is(':checked')) {
            // add an input to form
            form.append(`<input type="hidden" name="Users[]" value="${userId}" id="user-promote-form-user-${userId}">`);
            selected++;
        } else {
            // remove the input from form
            let inputId = `#user-promote-form-user-${userId}`;
            $(inputId).remove();
            selected--;
        }
        
        // click selectAll when all checkbox is checked
        if (selected === checkboxSize) {
            if (!selectAll.is(':checked')) selectAll.click();
        }

        // click selectAll when none checkbox is checked
        if (selected === 0) {
            if (selectAll.is(':checked')) selectAll.click();
        }
        
        // show submit btn if select any
        if (selected === 0) {
            $('#user-promote-form-submit-collapse').collapse('hide');
        } else {
            $('#user-promote-form-submit-collapse').collapse('show');
        }
    });

    // select all and unselect all
    selectAll.click(function () {
        if ($(this).is(':checked')) {
            checkbox.each(function () {
                if (!$(this).is(':checked')) $(this).click();
            });
        } else {
            checkbox.each(function () {
                if ($(this).is(':checked')) $(this).click();
            });
        }
    });
}

// UserPromoteFilter Submission
$('#user-promote .user-promote-filter-submit-btn').click(function () {
    let form = $('#user-promote-filter-form');
    
    $.ajax({
        url: form.attr('action'),
        method: 'post',
        data: form.serialize(),
        success: function (res) {
            $('#user-promote-filter').collapse('hide');
            $('#user-promote-table').html(res);
            userPromotePaginationEvent();
            userPromoteCheckboxEvent();
        }
    })
})

// load one page on ready
$(function () {
    $('#user-management .get-all-user-filter-submit-btn').click();
    $('#user-promote .user-promote-filter-submit-btn').click();
});
