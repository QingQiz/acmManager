let editor1 = createEditor("problem-description-content", false)
let editor2 = createEditor("problem-solution-content", false)

$('.card-header').click(function (event) {
    let target = $(this).find('button').attr('data-target');
    $(target).collapse('toggle');
});

$('[data-toggle="tooltip"]').tooltip();

// $('.selectpicker').selectpicker('val');