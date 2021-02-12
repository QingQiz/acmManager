$(function () {
    $('#search-solution-btn').click(function () {
        let keyword = $('#search-solution-content').val();
        let userId = $('#search-solution-user').val();
        
        if (userId === '0') {
            location.href = location.pathname + "?keyword=" + keyword;
        } else {
            location.href = location.pathname + "?keyword=" + keyword + "&user=" + userId;
        }
    });
    $('[data-toggle="tooltip"]').tooltip();
})