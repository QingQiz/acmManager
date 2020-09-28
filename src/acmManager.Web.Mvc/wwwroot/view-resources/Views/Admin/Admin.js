// Pagination
$(function () {
    let pageSize = parseInt($('#get-all-user-filter-MaxResultCount').val());
    let skipCount = parseInt($('#get-all-user-filter-skip-count').val());
    let pageIndex = Math.floor(skipCount / pageSize) + 1;

    let indexArray;

    if (pageIndex < 4) {
        indexArray = [1, 2, 3, 4, 5, 6, 7];
    } else {
        indexArray = [pageIndex - 3, pageIndex - 2, pageIndex - 1, pageIndex, pageIndex + 1, pageIndex + 2, pageIndex + 3]
    }

    let res = indexArray.map(function (x) {
        return `<li class="page-item"><a class="page-link" href="#">${x}</a></li>`;
    });

    if (pageIndex < 4) {
        res[pageIndex - 1] = `<li class="page-item active"><a class="page-link" href="#">${pageIndex}</a></li>`;
    } else {
        res[3] = `<li class="page-item active"><a class="page-link" href="#">${pageIndex}</a></li>`;
    }
    
    $('#get-all-user-page-footer').html(
        `<ul class="pagination justify-content-end">
            <li class="page-item"><a class="page-link" href="#" id="get-all-user-page-first">First</a></li>
            <li class="page-item ${pageIndex === 1 ? 'disabled' : ''}"><a class="page-link" href="#" id="get-all-user-page-previous">Previous</a></li>`
        + res.join('\n')
        + `<li class="page-item"><a class="page-link" href="#" id="get-all-user-page-next">Next</a></li></ul>`).show();
    
    // Pagination Event
    let gotoPage = function (n) {
        if (n === pageIndex) return;
        $('#get-all-user-filter-skip-count').val((n - 1) * pageSize);
        $('#get-all-user-filter-form').submit();
    }

    $('#get-all-user-page-footer .page-link').click(function (){
        let nextPage = parseInt($(this).text());
        if (isNaN(nextPage)) return;
        gotoPage(nextPage);
    });
    
    $('#get-all-user-page-next').click(function () {
        gotoPage(pageIndex + 1);
    });

    $('#get-all-user-page-previous').click(function () {
        gotoPage(pageIndex - 1);
    });
    
    $('#get-all-user-page-first').click(function () {
        gotoPage(1);
    })
});

