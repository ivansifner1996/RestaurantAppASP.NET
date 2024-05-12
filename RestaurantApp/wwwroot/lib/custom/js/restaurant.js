/* DELETE RESTAURANT METHOD - */
function confirmDeleteRestaurant() {
    var delRestaurant = $("#toDeleteUr").attr("restId");;
    var token = $('[name=__RequestVerificationToken]').val();
    console.log(delRestaurant);
    // handle deletion here
    if (delRestaurant !== "") {
        $.ajax({
            type: "POST",
            url: "/Restaurants/Delete/" + delRestaurant,
            contentType: "application/json; charset=utf-8",
            headers: {
                'X-XSRF-TOKEN': token
            },
            async: true,
            success: function (result) {
                $("#DeleteUserModal").modal('hide');

                Swal.fire(
                    'Sucess!',
                    'Restaurant with an ID : ' + delRestaurant + ' deleted.',
                    'success'
                ).then(
                    setTimeout(function () {
                        window.location.href = "/Restaurants";
                    },1000))
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
}

function deleteRestaurant(value) {
    var delRestId = $(value).data('delrestid');
    var delRestName = $(value).data('delrestname');
    $("#DeleteUserModal").modal('show');
    $('#DeleteUserModal input[name="ID"]').val(delRestId);
    $("#DeleteUserModal .modal-title").html("Delete Confirmation");
    $("#DeleteUserModal .modal-body").html("Do You Want To Delete Restaurant with Name : " + "<strong class='text-danger'><span restId='" + delRestId+"' id='toDeleteUr'>" + delRestName + "</span></strong>");
}

/* EXTENSION METHOD TO GET BROWSER COOKIE */
function getCookie(cname) {
    var name = cname + "=";
    var decodedCookie = decodeURIComponent(document.cookie);
    var ca = decodedCookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) === ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) === 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}