// ajax.js

// Функция для проверки токена
function checkToken() {
    $.ajax({
        url: "/GetUserByClaims",
        type: "GET",
        success: function (response) {
            if (!response.isValid) {
                window.location.href = "/Login";
            }
        },
        error: function () {
            window.location.href = "/Login";
        }
    });
}

function GetClaimsInfo() {
    $.ajax({
        url: "/GetUserByClaims",
        type: "GET",
        success: function (response) {

            $('#response-container').html(response.result);
        }
    });
}


