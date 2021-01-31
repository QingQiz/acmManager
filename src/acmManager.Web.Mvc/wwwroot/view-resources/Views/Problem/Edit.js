let putErrorMessage = function(xhr) {
    let errorMessage = JSON.parse(xhr.responseText)['error'];
    errorMessage = errorMessage['details'] == null ? errorMessage['message'] : errorMessage['details'];
    errorMessage = errorMessage.trim().split('\n').join('');

    alert(errorMessage);
}

let selectProblemTypeEvent = function () {
    setTimeout(function () {
        let elem = $('select[name=TypeIds]');
        let selected = elem.selectpicker('val');

        elem.find('option').each(function () {
            let t = $(this);
            t.attr('selected', selected.indexOf(t.val()) >= 0);
        });
    }, 100);
}

let formSubmitEvent = function () {
    let form = $('#edit-problem-solution-form');

    $.ajax({
        url: form.attr('action'),
        method: 'post',
        data: form.serialize(),
        success: function (data) {
            console.log(data);
        },
        error: function (xhr, a, b) {
            putErrorMessage(xhr);
        }
    })
}

$(function () {
    createEditor("problem-description-content", false);
    createEditor("problem-solution-content", false);

    $('.card-header').click(function (event) {
        let target = $(this).find('button').attr('data-target');
        $(target).collapse('toggle');
    });

    $('select[name=TypeIds]').selectpicker();
    
    // selectpicker won't create dropdown-item on init
    // so we add event listener on click
    $('.multi-selector').click(function () {
        $('.dropdown-item').click(selectProblemTypeEvent);
    })
    
    $('#edit-problem-solution-submit-btn').click(formSubmitEvent);
});
