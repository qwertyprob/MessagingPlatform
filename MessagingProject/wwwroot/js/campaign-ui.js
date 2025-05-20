// UI Email Reply
function toggleEmailInput() {
    $('#emailInputWrapper').toggle($('#enableReplies').is(':checked'));
}

// UI Scheduled Email Toggle
function toggleScheduledEmail() {
    $('#emailscheduledWrapper').toggle(!$('#scheduleToggle').is(':checked'));
}



// Initialization
$(function () {
    const urlParams = new URLSearchParams(window.location.search);
    const isEdit = urlParams.get('IsEdit') || urlParams.get('isedit');

    // UI Disable forms
    if (isEdit && isEdit.toLowerCase() === 'false') {
        $('form input, form select, form textarea, form button').attr('disabled', true);
        $('#btn-success').prop('disabled', true);
        $('#btn-primary').prop('disabled', true);
    }


    // Select2 for contact list
    $('#multi-select').select2({
        placeholder: "",
        tags: true,
        tokenSeparators: [',', ' ']
    });

    // DateTime picker
    $('#scheduledDateTime').dxDateBox({
        type: "datetime",
        displayFormat: "dd-MM-yyyy HH:mm",
        placeholder: "",
        showClearButton: true,
        width: "100%"
    });

    // Initial toggle state
    toggleEmailInput();
    toggleScheduledEmail();

    // Checkbox bindings
    $('#enableReplies').on('change', toggleEmailInput);
    $('#scheduleToggle').on('change', toggleScheduledEmail);

    //Receive checkbox
    $('#enableReplies').on('change', function () {
        if ($(this).is(':checked')) {
            $('#receive').hide();
        } else {
            $('#receive').show();
        }
    });
});


