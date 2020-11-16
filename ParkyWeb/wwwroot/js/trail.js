var dataTable;

$(document).ready(function () {   
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/trails/GetAlltrails",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "name", "width": "25%" },
            { "data": "distance", "width": "25%" },
            { "data": "difficulty", "width": "15%" },
            { "data": "nationalPark.name", "width": "15%" },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="text-center">
                                <a href="/trails/Upsert/${data}" class='btn btn-success text-white'
                                    style='cursor:pointer;'> <i class='far fa-edit'></i></a>
                                    &nbsp;
                                <a onclick=Delete("/trails/Delete/${data}") class='btn btn-danger text-white'
                                    style='cursor:pointer;'> <i class='far fa-trash-alt'></i></a>
                                </div>
                            `;
                }, "width": "20%"
            }
        ],
        "initComplete": function (settings, json) {
            // Change enum value to text
            $("#tblData tbody tr td:nth-child(3)").each(function () {
                var difficultyValue = $(this).text();
                if (difficultyValue == 0) {
                    $(this).text("Easy");
                } else if (difficultyValue == 1) {
                    $(this).text("Moderate");
                } else if (difficultyValue == 2) {
                    $(this).text("Difficult");
                } else if (difficultyValue == 3) {
                    $(this).text("Expert");
                }
            });
        }
    });   
}

function Delete(url) {
    swal({
        title: "Are you sure you want to Delete?",
        text: "You will not be able to restore the data!",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: 'DELETE',
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}