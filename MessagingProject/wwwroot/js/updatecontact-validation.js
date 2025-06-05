function decodeHtmlEntities(text) {
    const txt = document.createElement("textarea");
    txt.innerHTML = text;
    return txt.value;
}
function validateUpdateContactForm() {
    let isValid = true;

    $('.error-name-update').text(' ');
    $('.error-email-update').text(' ');
    $('.error-phone-update').text(' ');

    const name = $('#name-update').val().trim();
    if (!name) {
        $('#error-name-update').text(decodeHtmlEntities(validationName));
        isValid = false;
    }

    //email
    const email = $('#email-update').val().trim();
    if (!email) {
        $('#error-email-update').text(decodeHtmlEntities(validationEmail));
        isValid = false;
    } else if (!validateEmail(email)) {
        $('#error-email-update').text(decodeHtmlEntities(validationEmailValid));
        isValid = false;
    }
    const phone = $('#phone-update').val().trim();
    if (!phone) {
        $('#error-phone-update').text(decodeHtmlEntities(validationPhone));
        isValid = false;
    } else if (!validatePhone(phone)) {
        $('#error-phone-update').text(decodeHtmlEntities(validationPhoneValid));
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
function attachLiveContactValidation() {
    $('#name-update').on('input', function () {
        const value = $(this).val().trim();
        if (value) {
            $('#error-name-update').text('');
        }
    });

    $('#email-update').on('input', function () {
        const value = $(this).val().trim();
        if (value && validateEmail(value)) {
            $('#error-email-update').text('');
        }
    });

    $('#phone-update').on('input', function () {
        const value = $(this).val().trim();
        if (value && validatePhone(value)) {
            $('#error-phone-update').text('');
        }
    });
}

//Async validation
$(document).ready(function () {
    attachLiveContactValidation();
});