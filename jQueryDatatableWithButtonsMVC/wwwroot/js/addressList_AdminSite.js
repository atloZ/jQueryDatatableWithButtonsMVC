var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#DT_Admin_load').DataTable({
        "ajax": {
            "url": "/Addresses/getall/",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            {
                "data": 'active',
                "name": "Active",
                "autoWidth": true,
                "render": function (data) {
                    return data == 0 ? true : false;
                }
            },
            { "data": 'addressLine1', "name": "AddressLine1", "autoWidth": true },
            { "data": 'addressLine2', "name": "AddressLine2", "autoWidth": true },
            { "data": 'city', "name": "City", "autoWidth": true },
            { "data": 'stateProvinceId', "name": "StateProvinceId", "autoWidth": true },
            { "data": 'postalCode', "name": "PostalCode", "autoWidth": true },
            { "data": 'rowguid', "name": "Rowguid", "autoWidth": true },
            { "data": 'modifiedDate', "name": "ModifiedDate", "autoWidth": true },
            {
                "data": "addressId",
                "render": function (data) {
                    return `<div class="text-center">
                        <a href="/Addresses/Upsert?id=${data}" class='btn btn-success text-white' style='cursor:pointer; width:70px;'>
                            Edit
                        </a>
                        &nbsp;
                        <a class='btn btn-danger text-white' style='cursor:pointer; width:70px;'
                            onclick=Delete('/Addresses/CascadeDelete?id='+${data})>
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