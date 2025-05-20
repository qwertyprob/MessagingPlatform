function decodeHtmlEntities(text) {
    const txt = document.createElement("textarea");
    txt.innerHTML = text;
    return txt.value;
}
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

function validateEmail(email) {
    const re = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return re.test(email);
}
