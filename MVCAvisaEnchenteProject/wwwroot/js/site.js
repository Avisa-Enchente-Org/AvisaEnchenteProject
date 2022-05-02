
$(function () {

    if ($("div.notification").length) {
        setTimeout(() => {
            $("div.notification").fadeOut();
        }, 4000);
    }

});

abrirFormModal = (url, titulo) => {
    $.ajax({
        type: "GET",
        url: url,
        success: function (res) {
            $('#form-modal .modal-body').html(res);
            $('#form-modal .modal-title').html(titulo);
            $('#form-modal').modal('show');

        }
    });
}

salvarUsuarioAjax = form => {

    try {
        $.ajax({
            type: "POST",
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                if (res.erro) {
                    Swal.fire({
                        title: res.messageErro,
                        confirmButtonColor: '#08357c',
                        customClass: {
                            confirmButton: 'btn btn-primary btn-md',
                        },
                    })
                }
                else {
                    if (res.valido) {

                        $('#form-modal .modal-body').html('');
                        $('#form-modal .modal-title').html('');
                        $('#form-modal').modal('hide');

                        var pesquisaAvancadaUsuarios = document.getElementById('pesquisa-avancada-usuarios');
                        return realizaPesquisaAvancadaUsuarios(pesquisaAvancadaUsuarios);
    
                    }
                    else {
                        $('#form-modal .modal-body').html(res.html);
                    }
                }

              
            },
            error: function (err) {
                console.log(err);
            }
        })
        return false;
    } catch (e) {
        console.log(e);
    }
    return false;
}


realizaPesquisaAvancadaUsuarios = form => {

    try {
        $.ajax({
            type: "POST",
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {

                if (res.erro) {
                    Swal.fire({
                        title: res.messageErro,
                        confirmButtonColor: '#08357c',
                        customClass: {
                            confirmButton: 'btn btn-primary btn-md',
                        },
                    })
                }
                else {
                    $('#pv-listar-usuarios').html(res.html);
                    rebuild();
                }

            },
            error: function (err) {
                console.log(err);
            }
        })

        return false;
    } catch (e) {
        console.log(e);
    }

    return false;
}

function loadingTemplate(message) {

    return '<i class="fa fa-spinner fa-spin fa-fw fa-2x"></i>';
}

function rebuild() {
    $('#table').bootstrapTable('destroy')
        .bootstrapTable('showLoading')
        .bootstrapTable()
}