
$(document).ready(function () {
    $('#example').DataTable({
        "stripeClasses": ['strip1', 'strip2'],
        "oLanguage": {
            "oPaginate": {
                "sNext": '>',
                "sPrevious": '<'
            }
        },
        "language": {
            "search": "Search:",
            "searchPlaceholder": "Search data..."
        },
        "dom": '<"right"f>rt<"bottom"ip><"clear">'

    });
});
