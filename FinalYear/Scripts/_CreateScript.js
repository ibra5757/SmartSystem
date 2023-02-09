
$(function () {
    var createBtn = $(".acreate");
    if (createBtn) {

    createBtn.click(function () {
        var $buttonClicked = $(this);
        var TeamDetailPostBackURL = $buttonClicked.attr('data-url');
        $.ajax({
            type: "GET",
            url: TeamDetailPostBackURL,
            //contentType: "application/json; charset=utf-8",
            //datatype: "json",
            success: function (data) {
                $('#myModalContent').html(data);
                $('#myModal').modal('show');
            },
            error: function 
                    (jqXHR, exception) {
                var msg = '';
                if (jqXHR.status === 0) {
                    msg = 'Not connect.\n Verify Network.';
                } else if (jqXHR.status == 404) {
                    msg = 'Requested page not found. [404]';
                } else if (jqXHR.status == 500) {
                    msg = 'Internal Server Error [500].';
                } else if (exception === 'parsererror') {
                    msg = 'Requested JSON parse failed.';
                } else if (exception === 'timeout') {
                    msg = 'Time out error.';
                } else if (exception === 'abort') {
                    msg = 'Ajax request aborted.';
                } else {
                    msg = 'Uncaught Error.\n' + jqXHR.responseText;
                }
                alert(msg);
            }
        });
    });


    }

    $("#closbtn").click(function () {
        $('#myModal').modal('hide');
    });
});