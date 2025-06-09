// ajax.js

function GetClaimsInfo() {
    $.ajax({
        url: "/GetUserByClaims",
        type: "GET",
        success: function (response) {

            $('#response-container').html(response.result);
        }
    });
}




