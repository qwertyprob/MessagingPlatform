function decodeHtmlEntities(text) {
    const txt = document.createElement("textarea");
    txt.innerHTML = text;
    return txt.value;
}
function validateContactForm() {
    let isValid = true;

    $('.error-name').text(' ');
    $('.error-email').text(' ');
    $('.error-phone').text(' ');

    const name = $('#name').val().trim();
    if (!name) {
        $('#error-name').text(decodeHtmlEntities(contactvalidationName));
        isValid = false;
    }

    //email
    const email = $('#email').val().trim();
    if (!email) {
        $('#error-email').text(decodeHtmlEntities(contactvalidationEmail));
        isValid = false;
    } else if (!validateEmail(email)) {
        $('#error-email').text(decodeHtmlEntities(contactvalidationEmailValid));
        isValid = false;
    }
    const phone = $('#phone').val().trim();
    if (!phone) {
        $('#error-phone').text(decodeHtmlEntities(contactvalidationPhone));
        isValid = false;
    } else if (!validatePhone(phone)) {
        $('#error-phone').text(decodeHtmlEntities(contactvalidationPhoneValid));
        isValid = false;
    }

    

    return isValid;
}

function validateEmail(email) {
    const re = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return re.test(email);
}
function validatePhone(phone) {
    const pattern = /^(?:0\d{8}|\+373\d{8})$/;
    return pattern.test(phone);
}

//Live validation
function attachLiveValidation() {
    $('#name').on('input', function () {
        const value = $(this).val().trim();
        if (value) {
            $('#error-name').text('');
        }
    });

    $('#email').on('input', function () {
        const value = $(this).val().trim();
        if (value && validateEmail(value)) {
            $('#error-email').text('');
        }
    });

    $('#phone').on('input', function () {
        const value = $(this).val().trim();
        if (value && validatePhone(value)) {
            $('#error-phone').text('');
        }
    });
}

//Async validation
$(document).ready(function () {
    attachLiveValidation();
});