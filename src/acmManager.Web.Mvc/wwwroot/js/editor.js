let createEditor = function(id, readonly=false) {
    window.onbeforeunload = e => 'Sure?'

    return editormd(id, {
        autoHeight: true,
        readOnly: readonly,
        path: "/editor.md/lib/",
        placeholder: "",
        tex: true,
        tocm: true,
        //flowChart: true,
        taskList: true,
        sequenceDiagram: true,
        katexURL: {
            js: 'https://cdnjs.cloudflare.com/ajax/libs/KaTeX/0.12.0/katex.min.js',
            css: 'https://cdnjs.cloudflare.com/ajax/libs/KaTeX/0.12.0/katex.min.css'
        },
        watch: false,
        toolbarIcons: function () {
            return [
                "undo", "redo", "|",
                "bold", "del", "italic", "quote", "|",
                "h1", "h2", "h3", "h4", "|",
                "list-ul", "list-ol", "hr", "|",
                "link", "uploadImage", "uploadFile", "|",
                "code", "preformatted-text", "code-block", "table", "|",
                "watch", "fullscreen", "|",
                "help"
            ]
        },
        toolbarIconsClass: {
            uploadImage: "fa-image",
            uploadFile: "fa-file"
        },
        toolbarIconTexts: {
            uploadImage: "添加图片",
            uploadFile: "上传文件"
        },
        lang: {
            toolbar: {
                uploadFile: "上传文件",
                uploadImage: "添加图片",
            }
        },
        toolbarHandlers: {
            uploadImage: function (cm, icon, cursor, selection) {
                let elem = '#' + id;
                $(elem).append(`
                    <div class="modal fade" id="add-image">
                        <div class="modal-dialog">
                            <div class="modal-content">
                                <div class="modal-header"> 添加图片 </div>
                                <div class="modal-body">
                                    <div class="input-group mb-3">
                                        <input type="text" id="image-address" class="form-control" placeholder="或上传图片" value="">
                                        <div class="input-group-append">
                                            <span class="input-group-text">图片地址</span>
                                        </div>
                                    </div>
                                    <div class="input-group mb-3">
                                        <input type="text" id="image-description" class="form-control">
                                        <div class="input-group-append">
                                            <span class="input-group-text">图片描述</span>
                                        </div>
                                    </div>
                                    <div class="input-group mb-3">
                                        <input type="text" id="image-href" placeholder="http://" class="form-control">
                                        <div class="input-group-append">
                                            <span class="input-group-text">图片链接</span>
                                        </div>
                                    </div>
                                    <div class="input-group mb-3">
                                        <form action="/Image/Upload" method="post" class="row m-3">
                                            <input type="file" name="input" accept="image/*" class="custom-file-input">
                                            <div class="input-group-append">
                                                <span class="input-group-text custom-file-label">上传图片</span>
                                            </div>
                                        </form>
                                    </div>
                                    <div class="text-right col pr-0 mt-3">
                                        <input type="button" class="btn btn-primary" id="add-image-submit-btn" value="确定">
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                `);

                $('#add-image').modal('show').on('hidden.bs.modal', function () { this.remove(); });

                $('#add-image-submit-btn').click(function () {
                    let addr = $('#image-address').val();
                    let description = $('#image-description').val();
                    let href = $('#image-href').val();
                    
                    if (addr === "") {
                        alert("图片地址不能为空");
                        return;
                    }

                    if (href === "") {
                        cm.replaceSelection(`![${description}](${addr})`);
                    } else {
                        cm.replaceSelection(`[![${description}](${addr} "${description}")](${href} "${description}")`);
                    }
                    $('#add-image').modal('hide');
                });

                $('#add-image input[type=file]').change(function () {
                    let form = $('#add-image form').closest('form');
                    let formData = new FormData(form[0]);
                    
                    $.ajax(form.attr('action'), {
                        method: 'post',
                        data: formData,
                        processData: false,
                        contentType: false,
                        success: function (xhr, error, status) {
                            if (xhr.result.success === 0) {
                                alert(xhr.result.message);
                            } else {
                                let address = location.origin + xhr.result.url;
                                $('#image-address').val(address)
                            }
                        },
                        error: function () {
                            alert('error');
                        }
                    })
                });
            },
            uploadFile: function (cm, icon, cursor, selection) {
                let elem = '#' + id;
                $(elem).append(`
                    <div class="modal fade" id="add-file">
                        <div class="modal-dialog">
                            <div class="modal-content">
                                <div class="modal-header"> 上传文件 </div>
                                <div class="modal-body">
                                    <div class="input-group mb-3">
                                        <form action="/File/Upload" method="post" class="row m-3">
                                            <input type="file" name="input" class="custom-file-input">
                                            <div class="input-group-append">
                                                <span class="input-group-text custom-file-label">上传文件</span>
                                            </div>
                                        </form>
                                    </div>
                                    <div class="text-right col pr-0 mt-3">
                                        <input type="button" class="btn btn-primary" id="add-file-submit-btn" value="确定">
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                `);

                $('#add-file').modal('show').on('hidden.bs.modal', function () {
                    this.remove();
                });

                $('#add-file-submit-btn').click(function () {
                    let form = $('#add-file form').closest('form');
                    let formData = new FormData(form[0]);

                    $.ajax(form.attr('action'), {
                        method: 'post',
                        data: formData,
                        processData: false,
                        contentType: false,
                        success: function (xhr, error, status) {
                            let address  = location.origin + xhr.result.url;
                            let filename = xhr.result.filename

                            cm.replaceSelection(`[${filename}](${address})`);
                            
                            $('#add-file').modal('hide');
                        },
                        error: function () {
                            alert('error');
                        }
                    });
                });

                $('#add-file input[type=file]').change(function () {
                    let fileName = $(this).val().split('\\').pop();
                    $('.custom-file-label').addClass('selected').html(fileName);
                });
            }
        },
        onload: function () { }
    });
}


let previewMd = function (id, md) {
    editormd.markdownToHTML(id, {
        markdown: md,
        tex: true,
        tocm: true,
//        flowChart: true,
        taskList: true,
        sequenceDiagram: true,
    });
}