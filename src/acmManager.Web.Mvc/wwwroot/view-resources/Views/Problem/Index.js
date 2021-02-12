$(function () {
    $('#search-solution-btn').click(function () {
        let keyword = $('#search-solution-content').val();
        
        location.href = location.pathname + "?keyword=" + keyword;
    });
    $('[data-toggle="tooltip"]').tooltip();
})