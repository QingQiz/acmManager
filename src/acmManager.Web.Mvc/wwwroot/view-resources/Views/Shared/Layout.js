$('.card-header').click(function (event) {
    let target = $(this).find('button').attr('data-target');
    $(target).collapse('toggle');
});

