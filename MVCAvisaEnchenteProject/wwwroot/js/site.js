$(document).ready(function () {
    ChangeIconButtonFiltro();
});

$(".decimal-input").numeric({ decimal: ",", negative: false, decimalPlaces: 2 });
$(".geolocalizacao-input").numeric({ decimal: ".", negative: false, decimalPlaces: 9 });

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

salvarAjax = (form) => {

    try {
        $.ajax({
            type: "POST",
            url: form.action,
            data: new FormData(form),
            async: false,
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

                        if (form.id === "form-criar-editar-usuario") {
                            var pesquisaAvancadaUsuarios = document.getElementById('pesquisa-avancada-usuarios');
                            return realizaPesquisaAvancada(pesquisaAvancadaUsuarios);
                        }
                        if (form.id === "form-criar-editar-ponto-de-sensoriamento") {
                            var pesquisaAvancadaPontosDeSensoriamento = document.getElementById('pesquisa-avancada-pontos-de-sensoriamento');
                            return realizaPesquisaAvancada(pesquisaAvancadaPontosDeSensoriamento);
                        }
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


realizaPesquisaAvancada = form => {

    try {
        $.ajax({
            type: "POST",
            url: form.action,
            data: new FormData(form),
            async: false,
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
                    if (form.id === "pesquisa-avancada-usuarios") {
                        $('#pv-listar-usuarios').html(res.html);
                    }
                    if (form.id === "pesquisa-avancada-pontos-de-sensoriamento") {
                        $('#pv-listar-pontos-sensoriamento').html(res.html);
                    }
                    if (form.id === "pesquisa-avancada-alertas-risco") {
                        $('#pv-listar-alertas-risco').html(res.html);
                    }
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

function deletarPorIdEController(id, controllerName) {
    Swal.fire({
        title: 'Tem Certeza?',
        text: "Não é possível reverter essa exclusão!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#08357c',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Sim, Pode Excluir!'
    }).then((result) => {
        if (result.isConfirmed) {

            if (controllerName.toLowerCase() === "parametrosnotificacao") {
                location.href = "/" + controllerName + "/Deletar/" + id;
            }
            else {
                $.ajax({
                    type: "DELETE",
                    url: "/" + controllerName + "/Deletar/" + id,
                    success: function (res) {
                        if (res.erro != undefined && res.erro) {
                            Swal.fire({
                                title: res.messageErro,
                                confirmButtonColor: '#08357c',
                                customClass: {
                                    confirmButton: 'btn btn-primary btn-md',
                                },
                            })

                        }
                        else {

                            Swal.fire({
                                title: 'Excluido!',
                                text: 'Esse Registro Foi Excluido com Sucesso',
                                icon: 'success',
                                confirmButtonColor: '#08357c'
                            })

                            if (controllerName.toLowerCase() === "usuario") {
                                var pesquisaAvancadaUsuarios = document.getElementById('pesquisa-avancada-usuarios');
                                return realizaPesquisaAvancada(pesquisaAvancadaUsuarios);
                            }
                            if (controllerName.toLowerCase() === "pontodesensoriamento") {
                                var pesquisaAvancadaPontosDeSensoriamento = document.getElementById('pesquisa-avancada-pontos-de-sensoriamento');
                                return realizaPesquisaAvancada(pesquisaAvancadaPontosDeSensoriamento);
                            }
                        }

                    }
                })
            }
        }
    })
}

ativaPontoDeSensoriamento = (url, id) => {
    $.ajax({
        type: "POST",
        url: url,
        data: { id: id },
        success: function (res) {
            if (res.valido) {
                Swal.fire({
                    title: "Entidade Ativada no Helix com Sucesso!",
                    confirmButtonColor: '#08357c',
                    customClass: {
                        confirmButton: 'btn btn-primary btn-md',
                    },
                }).then((result) => {
                    if (result.isConfirmed) {
                        var pesquisaAvancadaPontosDeSensoriamento = document.getElementById('pesquisa-avancada-pontos-de-sensoriamento');
                        return realizaPesquisaAvancada(pesquisaAvancadaPontosDeSensoriamento);
                    }
                })
            }
            else {
                Swal.fire({
                    title: "Erro ao tentar ativa entidade no Helix!",
                    confirmButtonColor: '#08357c',
                    customClass: {
                        confirmButton: 'btn btn-primary btn-md',
                    },
                })
            }
        }
    });
}

function loadingTemplate(message) {

    return '<i class="fa fa-spinner fa-spin fa-fw fa-2x"></i>';
}

function rebuild() {
    $('#table').bootstrapTable('destroy')
        .bootstrapTable('showLoading')
        .bootstrapTable()
}


ChangeIconButtonFiltro = () => {
    var icon = $('#filtrar-collapse-icon');

    if (icon.val() !== undefined) {
        if (icon[0].classList.contains("showing-collapse")) {
            icon[0].classList.remove("fa-angle-down")
            icon[0].classList.remove("showing-collapse")
            icon[0].classList.add("fa-angle-up")
        } else {
            icon[0].classList.remove("fa-angle-up")
            icon[0].classList.add("fa-angle-down")
            icon[0].classList.add("showing-collapse")
        }

    }
};