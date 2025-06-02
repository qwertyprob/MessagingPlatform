$(function () {
        const urlParams = new URLSearchParams(window.location.search);
        const isEdit = urlParams.get('IsEdit') || urlParams.get('isedit');

        // Disable form if isEdit = false
        if (isEdit && isEdit.toLowerCase() === 'false') {
            $('form input, form select, form textarea, form button').attr('disabled', true);
            $('#btn-success').prop('disabled', true);
            $('#btn-primary').prop('disabled', true);
        }

        // Select2 for contacts
        $('#multi-select').select2({
            placeholder: "",
            tags: true,
            tokenSeparators: [',', ' ']
        });

        // DateTime picker for SMS scheduling
        $('#smsScheduledDateTime').dxDateBox({
            type: "datetime",
            displayFormat: "dd-MM-yyyy HH:mm",
            placeholder: "",
            showClearButton: true,
            width: "100%"
        });

        // Initial toggle
        toggleScheduledSms();

        // Bind toggle handler
        $('#scheduleToggle').on('change', toggleScheduledSms);

        // Chat preview
        $('#inputText').on('input', function () {
            var userInput = $(this).val();
            $('#div1').text(userInput);

            if (userInput === "") {
                $('#chat-container').hide();
            } else {
                $('#chat-container').show();
            }
        });
    });

    // Toggle scheduled SMS block
    function toggleScheduledSms() {
        const isChecked = $('#scheduleToggle').is(':checked');
        if (isChecked) {
            $('#smsScheduledWrapper').hide();
        } else {
            $('#smsScheduledWrapper').show();
        }
}

