// modalHandler.js
window.showErrorModal = function () {
    var errorModal = new bootstrap.Modal(document.getElementById('errorModal'));
    errorModal.show();

    var seconds = 5;
    var timerSpan = document.getElementById("timer");

    var countdown = setInterval(function () {
        seconds--;
        timerSpan.textContent = seconds;

        if (seconds <= 0) {
            clearInterval(countdown);
            errorModal.hide();
        }
    }, 1000);
};
