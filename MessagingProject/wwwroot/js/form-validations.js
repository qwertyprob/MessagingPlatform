function validateForm() {
    let isValid = true;

    // Очистка предыдущих сообщений об ошибках
    $('.error-message').text('');

    const sendName = $('#sendName').val().trim();
    if (!sendName) {
        $('#error-sendName').text('Please enter the newsletter title.');
        isValid = false;
    }

    const templateVal = $('#templateName').val();
    if (!templateVal) {
        $('#error-templateName').text('Please select a template.');
        isValid = false;
    }

    const templateSubject = $('#templateSubject').val();
    if (!templateSubject) {
        $('#error-templateSubject').text('Please select a subject.');
        isValid = false;
    }

    const selectedContacts = $('#multi-select').val();
    if (!selectedContacts || selectedContacts.length === 0) {
        $('#error-multi-select').text('Please select at least one contact.');
        isValid = false;
    }

    if ($('#enableReplies').is(':checked')) {
        const replyEmail = $('#replyEmail').val().trim();
        if (!replyEmail) {
            $('#error-replyEmail').text('Please enter a reply email.');
            isValid = false;
        } else if (!validateEmail(replyEmail)) {
            $('#error-replyEmail').text('Please enter a valid email address.');
            isValid = false;
        }
    }

    return isValid;
}

function validateEmail(email) {
    const re = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return re.test(email);
}
