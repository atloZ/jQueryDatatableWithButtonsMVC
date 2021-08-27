var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#DT_load').DataTable({
        "ajax": {
            "url": "/Addresses/GetAllActive/",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": 'addressLine1', "autoWidth": true },
            { "data": 'addressLine2', "autoWidth": true },
            { "data": 'city', "autoWidth": true },
            { "data": 'stateProvinceId', "autoWidth": true },
            { "data": 'postalCode', "autoWidth": true },
            { "data": 'rowguid', "autoWidth": true },
            { "data": 'modifiedDate', "autoWidth": true },
            {
                "data": "addressId",
                "render": function (data) {
                    return `<div class="text-center">
                        <a href="/Addresses/Upsert?id=${data}" class='btn btn-success text-white' style='cursor:pointer; width:70px;'>
                            Edit
                        </a>
                        &nbsp;
                        <a class='btn btn-danger text-white' style='cursor:pointer; width:70px;'
                            onclick=Delete('/Addresses/Delete?id='+${data})>
                            Delete
                        </a>
                        </div>`;
                }, "width": "40%"
            }
        ],
        "language": {
            "emptyTable": "no data found"
        },
        "width": "100%"
    });
}

function Delete(url) {
    swal({
        title: "Are you sure?",
        text: "Once deleted, you will not be able to recover",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data) {
                    console.log("debug: " + data);
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