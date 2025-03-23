var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblUsers').DataTable({
        "ajax": {
            "url": "/Subject/GetAllStudents",
            "type": "GET",
            "datatype": "json",
            "dataSrc": function (json) {
                console.log(json); // Log the API response to the console for debugging
                return json.data;
            }
        },
        "columns": [
            { "data": "firstName", "width": "20%" },
            { "data": "lastName", "width": "15%" },
            { "data": "email", "width": "20%" },
            { "data": "adress", "width": "15%" },
            {
                "data": "id",
                "render": function (data, type, row) {
                    return `
                        <form method="post" action="/Subject/SetProfessors">
                            <input type="hidden" name="Id" value="${data}" />
                            <button type="submit" class="btn btn-sm btn-primary">Set as Professor</button>
                        </form>
                    `;
                },
                "width": "15%"
            }
        ],
        "responsive": true,
        "autoWidth": false,
        "pagingType": "full_numbers",
        "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]],
        "language": {
            "emptyTable": "No data available"
        }
    });
}
