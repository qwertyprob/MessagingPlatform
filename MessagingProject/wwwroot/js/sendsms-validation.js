function decodeHtmlEntities(text) {
    const txt = document.createElement("textarea");
    txt.innerHTML = text;
    return txt.value;
}

function validateEmail(email) {
    const re = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return re.test(email);
}

function sendSmsValidate() {
    let isValid = true;

    $('.error-message').text('');

    const sendName = $('#sendName').val().trim();
    if (!sendName) {
        $('#error-sendName').text(decodeHtmlEntities(validationTitle));
        isValid = false;
    }
    const message = $('#smsMessage').val().trim();
    if (!message) {
        $('#error-messageSms').text(decodeHtmlEntities(validationMessage));
        isValid = false;
    }

    const description = $('#description').val().trim();
    if (!description) {
        $('#error-description').text(decodeHtmlEntities(validationDescription));
        isValid = false;
    }

    const selectedAlias = $('#aliasName').val();
    if (!selectedAlias) {
        $('#error-aliasName').text(decodeHtmlEntities(validationAlias));
        isValid = false;
    }

    const selectedContacts = $('#multi-select').val();
    if (!selectedContacts || selectedContacts.length === 0) {
        $('#error-multi-select').text(decodeHtmlEntities(validationContacts));
        isValid = false;
    }

    const scheduledVal = $('#smsScheduledDateTime').text().trim();
    const isScheduledVisible = $('#smsScheduledWrapper').is(':visible');
    if (isScheduledVisible && !scheduledVal) {
        $('#error-smsScheduledDateTime').text(decodeHtmlEntities(validationScheduled));
        isValid = false;
    }

    return isValid;
}

$(document).ready(function () {
    $('#sendName').on('input', function () {
        if ($(this).val().trim()) $('#error-sendName').text('');
    });
    $('#smsMessage').on('input', function () {
        if ($(this).val().trim()) $('#error-messageSms').text('');
    });

    $('#description').on('input', function () {
        if ($(this).val().trim()) $('#error-description').text('');
    });

    $('#aliasName').on('change', function () {
        if ($(this).val()) $('#error-aliasName').text('');
    });

    $('#multi-select').on('change', function () {
        const selected = $(this).val();
        if (selected && selected.length > 0) $('#error-multi-select').text('');
    });

    $('#smsScheduledDateTime').on('input change', function () {
        if ($('#smsScheduledWrapper').is(':visible') && $(this).text().trim()) {
            $('#error-smsScheduledDateTime').text('');
        }
    });
});