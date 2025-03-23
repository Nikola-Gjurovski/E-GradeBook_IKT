var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblProfessors').DataTable({
        "ajax": {
            "url": "/Subject/GetAllProfessors",
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
                        <form method="post" action="/Subject/Professors">
                            <input type="hidden" name="Id" value="${data}" />
                            <button type="submit" class="btn btn-sm btn-primary">Remove Professor</button>
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
