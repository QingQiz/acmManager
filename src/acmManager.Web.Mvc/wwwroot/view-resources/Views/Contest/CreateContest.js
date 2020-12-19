let editor = createEditor("contest-description", false)

$('#create-contest-submit-btn').click(function () {
    let form = $('#create-contest-form');

    let setVal = function (name, sel, func) {
        let val;
        if (func === undefined) {
            val = $(sel).val();
        } else {
            val = func($(sel));
        }
        if (val === "" || val === undefined) {
            alert(`${name} can NOT be empty`);
            return 1;
        }
        form.find(`input[name=${name}]`).val(val);
        return 0;
    }
    
    if (setVal('Name', '#contest-name')) return;
    if (setVal('DescriptionTitle', '#contest-name')) return;
    if (setVal('DescriptionContent', '#contest-description textarea', a => a.text())) return;
    if (setVal('SignUpStartTime', '#contest-sign-up-start-time')) return;
    if (setVal('SignUpEndTime', '#contest-sign-up-end-time')) return;
    
    $.ajax({
        url: form.attr('action'),
        method: 'post',
        data: form.serialize(),
        success: () => location.href = '/Contest',
        error: () => alert('error')
    })
});