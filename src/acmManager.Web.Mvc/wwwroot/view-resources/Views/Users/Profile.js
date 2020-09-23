let clickUserPhotoEvent = function () {
    $('.author-card-avatar button').click(function () {
        $(this).addClass('selected');
    })
}

let putErrorMsg = function (xhr) {
    let errorMessage = JSON.parse(xhr.responseText)['error'];
    errorMessage = errorMessage['details'] == null ? errorMessage['message'] : errorMessage['details'];
    errorMessage = errorMessage.trim().split('\n').join('<br>');

    $('.alert-danger').html(errorMessage).css('opacity', 0).show().animate({opacity: 1}, 500);
}

let updateProfileEvent = function () {
    $('#UpdateProfile').click(function () {
        let form = $('#ProfileForm');
        
        $.ajax({
            type: 'post',
            data: form.serialize(),
            url: form.attr('action'),
            success: location.reload ,
            error: function (xhr, status, error) {
                putErrorMsg(xhr);
            }
        })
    })
}

let selectFileEvent = function () {
    $('.custom-file-input').change(function () {
        let fileName = $(this).val().split('\\').pop();
        $('.custom-file-label').addClass('selected').html(fileName);
    });
}

let InitCropper = function () {
    let isInitialized = false;
    let cropper = '';
    let file = '';
    $("#CropperFile").change(function () {
        file = this.files[0];

        if (file) {
            let oFReader = new FileReader();
            oFReader.readAsDataURL(file);
            oFReader.onload = function () {
                $("#cropper-img").attr('src', this.result);
                $('#cropper-img').addClass('ready');
                if (isInitialized === true) {
                    cropper.destroy();
                }
                initCropper();
            }
        }
    });

    function initCropper() {
        let vEl = document.getElementById('cropper-img');
        cropper = new Cropper(vEl, {
            viewMode: 1,
            dragMode: 'move',
            aspectRatio: 1,
            checkOrientation: false,
            cropBoxMovable: false,
            cropBoxResizable: false,
            zoomOnTouch: false,
            zoomOnWheel: true,
            guides: false,
            highlight: true,
        });
        isInitialized = true;
    }

    $('#UpdatePhotoBtn').click(function () {
        cropper.getCroppedCanvas().toBlob(function (blob) {
            let form = $('#UpdatePhotoForm').closest("form");
            let fn = $("#CropperFile").val().split('\\').pop() + ".png";
            let formData = new FormData(form[0]);
            formData.append('Photo', blob, fn);

            $.ajax(form.attr('action'), {
                method: "POST",
                data: formData,
                processData: false,
                contentType: false,
                success: function () {
                    $('.close').click();
                    location.reload();
                },
                error: function (xhr, status, error) {
                    $('.close').click();
                    location.reload();
                    putErrorMsg(xhr);
                }
            });
        });
    })
}
