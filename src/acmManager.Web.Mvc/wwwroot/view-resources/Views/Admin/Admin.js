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
    
    $('#user-promote-table tr').click(function () {
        $(this).find('input[type=checkbox]').click();
    });
 
    checkbox.click(function (e) {
        e.stopPropagation();
        let userId = $(this).attr('id').split('-').slice(-1);
        if ($(this).is(':checked')) {
            // add an input to form
            form.append(`<input type="hidden" name="Users" value="${userId}" id="user-promote-form-user-${userId}">`);
            selected++;
        } else {
            // remove the input from form
            let inputId = `#user-promote-form-user-${userId}`;
            $(inputId).remove();
            selected--;
        }
        
        if (selected === checkboxSize) {
            // click selectAll when all checkbox is checked
            if (!selectAll.is(':checked')) selectAll.prop('checked', true);
        } else {
            // click selectAll when none checkbox is checked
            if (selectAll.is(':checked')) selectAll.prop('checked', false);
        }

        // show submit btn if select any
        if (selected === 0) {
            $('#user-promote-form-submit-collapse').collapse('hide');
        } else {
            $('#user-promote-form-submit-collapse').collapse('show');
        }
    });

    // select all and unselect all
    selectAll.click(function (e) {
        e.stopPropagation();
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

let userPromoteSubmitEvent = function () {
    $('#user-promote-form-submit-btn').click(function () {
        let form = $('#user-promote-form');
        let formData = Array.from(form.find('input[name="Users"]').map(function () {
            return $(this).attr('id').split('-').slice(-1);
        }));
        
        $.ajax({
            url: form.attr('action'),
            type: 'POST',
            data: {
                ids: formData,
                __RequestVerificationToken: form.find('input[name="__RequestVerificationToken"]').attr('value')
            },
            success: function () {
                $('#user-promote-filter .user-promote-filter-submit-btn').click();
                $('.get-all-user-filter-submit-btn').click();
            },
            dataType: 'json',
            traditional: true
        })
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
            userPromoteSubmitEvent();
        }
    })
})

$('a[href="#user-management"]').on('shown.bs.tab', function (e) {
    $('#user-management .get-all-user-filter-submit-btn').click();
});

$('a[href="#user-promote"]').on('shown.bs.tab', function (e) {
    $('#user-promote .user-promote-filter-submit-btn').click();
});

$(function () {
    $('a[href="#user-management"]').trigger('shown.bs.tab')
});


// Create User
$('#create-user-submit-btn').click(function () {
    let form = $('#create-user-form');

    $('#create-user-confirm-username').html(form.find('input[name=StudentNumber]').val())
    $('#create-user-confirm-password').html(form.find('input[name=Password]').val())
});

$('#create-user-confirm-btn').click(function () {
    let form = $('#create-user-form');
    
    $.ajax({
        url: form.attr('action'),
        method: 'post',
        data: form.serialize(),
        success: function () {
            $('.modal').modal('hide');
            location.reload();
        },
        error: function (xhr, a, b) {
            $('.modal').modal('hide');
            putErrorMsg(xhr, '#create-user-form .alert')
        }
    })
});

let putErrorMsg = function (xhr, selector='.alert-danger') {
    let errorMessage = JSON.parse(xhr.responseText)['error'];
    errorMessage = errorMessage['details'] == null ? errorMessage['message'] : errorMessage['details'];
    errorMessage = errorMessage.trim().split('\n').join('<br>');

    $(selector).html(errorMessage).css('opacity', 0).show().animate({opacity: 1}, 500);
}
