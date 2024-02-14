let datatable;
$(document).ready(function () {
    loadDataTable();
 
});

function loadDataTable() {
    datatable = $('#tblDatos').DataTable({
        "language": {
            "lengthMenu": "Mostrar _MENU_ Registros Por Pagina",
            "zeroRecords": "Ningun Registro",
            "info": "Mostrar pagina _PAGE_ de _PAGES_",
            "infoEmpty": "no hay registros",
            "infoFiltered": "(filtered from _MAX_ total registros)",
            "search": "Buscar",
            "paginate": {
                "first": "Primero",
                "last": "Último",
                "next": "Siguiente",
                "previous": "Anterior"
            }
        },
        "ajax": {
            "url":"/Admin/Marca/ObtenerTodos"
        },
        "columns": [
            { "data": "nombre", "with": "20%" },
            { "data": "descripcion", "with": "40%" },
            {
                "data": "estado",
                "render": function (data) {
                    if(data == true)
                    { 
                        return "Activo"
                    }
                    else{
                        return "Inactivo"
                    }
                },
                "with": "20%"
            },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="text-center">
                            <a href="/Admin/Marca/Upsert/${data}" class="btn btn-success text-white" style="cursor:pointer">
                                <i class="bi bi-pencil-square"></i>
                            </a>
                            <a onclick=Delete("/Admin/Marca/Delete/${data}") class="btn btn-danger text-write" style="cursor:pointer">
                                <i class="bi bi-trash3-fill"></i>
                            </a>
                        </div>
                    `;                    
                }, "with": "20%"
            }
        ]
    });
}

function Delete (url)
{
    swal({
        title: "¿Seguro de Eliminar la categoria?",
        text:"Este registro no podra recuperarse",
        icon: "warning",
        buttons: true,
        dangerMode:true
    }).then((borrar) => {
        if (borrar) {
            $.ajax({
                type:"POST",
                url: url,
                success: function (data) {
                    console.log(data);
                    if (data.success) {
                        toastr.success(data.message);
                        datatable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    })
}