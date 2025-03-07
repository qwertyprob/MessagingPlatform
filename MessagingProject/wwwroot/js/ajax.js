// ajax.js

// Функция для проверки токена
function checkToken() {
    $.ajax({
        url: "/Get",
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


