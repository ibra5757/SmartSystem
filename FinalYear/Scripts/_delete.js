
$(function () {
    $("#Delete").click(function () {

        var id = $(this).attr('data-id');
        var url = $(this).attr('data-url');

        Swal.fire({
            icon: 'warning',
            title: 'Are you sure you want to delete this record?',
            showDenyButton: false,
            showCancelButton: true,
            confirmButtonText: 'Yes'
        }).then((result) => {
            /* Read more about isConfirmed, isDenied below */
            if (result.isConfirmed) {


                // Ajax config
                $.ajax({
                    type: "POST", //we are using GET method to get data from server side
                    url: url + id, // get the route value
                    contentType: "application/json; charset=utf-8",
                    dataType: "json", //set data
                    beforeSend: function () {//We add this before send to disable the button once we submit it so that we prevent the multiple click

                    },
                    success: function (response) {//once the request successfully process to the server side it will return result here
                        // Reload lists of employees
                        if (response.success) {

                            Swal.fire('Success.', response, 'success')
                            location.reload(true);
                        } else {

                            Swal.fire('Warning', response, 'Warning')
                        }
                    }
                });


            } else if (result.isDenied) {
                Swal.fire('Changes are not saved', '', 'info')
            }
        });

    });
});