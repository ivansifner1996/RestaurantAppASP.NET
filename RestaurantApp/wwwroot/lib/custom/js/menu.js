/* Manipulate Menus Method - */
function menuDialogHandler(method, url, param) {
    var token = $('[name=__RequestVerificationToken]').val();
    console.log("Request URL:", url + param);
        $.ajax({
            type: method,
            url: url + param,
            contentType: "application/json; charset=utf-8",
            headers: {
                'X-XSRF-TOKEN': token
            },
            async: true,
            success: function (result) {
                console.log(result);
            },
            error: function (error) {
                Swal.fire({
                    type: 'error',
                    title: 'Oops...',
                    text: 'Request cannot be completed. Please try again later.'
                })
            }
        });
}


function editRestaurant(method, url, param) {

    //$("#MenuDialog").toggle();
    menuDialogHandler(method, url, param)
}

$(".closeIcon").on('click', function () {
    $("#MenuDialog").toggle()
});

