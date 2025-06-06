function decodeHtmlEntities(text) {
    const txt = document.createElement("textarea");
    txt.innerHTML = text;
    return txt.value;
}

function validateEmail(email) {
    const re = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return re.test(email);
}

// Сделай validateForm глобальной:
function validateForm() {
    let isValid = true;

    $('.error-message').text('');

    const sendName = $('#sendName').val().trim();
    if (!sendName) {
        $('#error-sendName').text(decodeHtmlEntities(validationTitle));
        isValid = false;
    }

    const scheduledVal = $('#scheduledDateTime').text().trim();
    const isScheduledVisible = $('#emailscheduledWrapper').is(':visible');
    if (isScheduledVisible && !scheduledVal) {
        $('#error-scheduledDateTime').text(decodeHtmlEntities(validationScheduled));
        isValid = false;
    }

    const templateVal = $('#templateName').val();
    if (!templateVal) {
        $('#error-templateName').text(decodeHtmlEntities(validationTemplate));
        isValid = false;
    }

    const templateSubject = $('#templateSubject').val();
    if (!templateSubject) {
        $('#error-templateSubject').text(decodeHtmlEntities(validationSubject));
        isValid = false;
    }

    const selectedContacts = $('#multi-select').val();
    if (!selectedContacts || selectedContacts.length === 0) {
        $('#error-multi-select').text(decodeHtmlEntities(validationContacts));
        isValid = false;
    }

    if ($('#enableReplies').is(':checked')) {
        const replyEmail = $('#replyEmail').val().trim();
        if (!replyEmail) {
            $('#error-replyEmail').text(decodeHtmlEntities(validationEmail));
            isValid = false;
        } else if (!validateEmail(replyEmail)) {
            $('#error-replyEmail').text(decodeHtmlEntities(validationEmailValid));
            isValid = false;
        }
    }

    return isValid;
}

$(document).ready(function () {
    $('#sendName').on('input', function () {
        if ($(this).val().trim()) $('#error-sendName').text('');
    });

    $('#templateName').on('change input', function () {
        if ($(this).val()) $('#error-templateName').text('');
    });

    $('#templateSubject').on('input', function () {
        if ($(this).val()) $('#error-templateSubject').text('');
    });

    $('#multi-select').on('change', function () {
        const selected = $(this).val();
        if (selected && selected.length > 0) $('#error-multi-select').text('');
    });

    $('#replyEmail').on('input', function () {
        const email = $(this).val().trim();
        if (email && validateEmail(email)) $('#error-replyEmail').text('');
    });

    $('#scheduledDateTime').on('input change', function () {
        if ($('#emailscheduledWrapper').is(':visible') && $(this).text().trim()) {
            $('#error-scheduledDateTime').text('');
        }
    });
});
