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

    // Toggle scheduled SMS block
    function toggleScheduledSms() {
        const isChecked = $('#scheduleToggle').is(':checked');
        if (isChecked) {
            $('#smsScheduledWrapper').hide();
        } else {
            $('#smsScheduledWrapper').show();
        }
    }

    // Initial toggle
    toggleScheduledSms();
    $('#scheduleToggle').on('change', toggleScheduledSms);

    // Chat preview
    function updateChatPreview() {
        const userInput = $('#smsMessage').val();
        $('#div1').text(userInput);

        if (userInput === "") {
            $('#chat-container').hide();
        } else {
            $('#chat-container').show();
        }
    }

    // Character counter logic
    function countCharacters(text) {
        return text.length;
    }

    function isGsm7Bit(text) {
        const gsm7bitChars =
            '@£$¥èéùìòÇ\nØø\rÅåΔ_ΦΓΛΩΠΨΣΘΞ\x1BÆæßÉ ' +
            '!\"#¤%&\'()*+,-./0123456789:;<=>?¡' +
            'ABCDEFGHIJKLMNOPQRSTUVWXYZÄÖÑÜ§¿' +
            'abcdefghijklmnopqrstuvwxyzäöñüà';

        for (let i = 0; i < text.length; i++) {
            if (!gsm7bitChars.includes(text[i])) {
                return false;
            }
        }
        return true;
    }

    function updateCharCount() {
        const text = $('#smsMessage').val();
        const length = countCharacters(text);
        let maxSingle, maxPerSegment;

        if (!isGsm7Bit(text)) {
            maxSingle = 67;
            maxPerSegment = 67;
        } else {
            maxSingle = 160;
            maxPerSegment = 153;
        }

        const max = length > maxSingle ? maxPerSegment : maxSingle;
        $('#sms-max').text(max);
        $('#sms-symbols-counter').text(length + '/' + max);

        let smsCount = 1;
        if (length > maxSingle) {
            smsCount = Math.ceil(length / maxPerSegment);
            $('#sms-count').addClass('over-limit');
            $('#smsCountSpan').text(`Превышен лимит одного сообщения — будет отправлено ${smsCount} SMS`);
            $('#smsCountSpan').show();
        } else {
            $('#sms-count').removeClass('over-limit');
            $('#smsCountSpan').hide();
        }

        if (isEdit && isEdit.toLowerCase() === 'false') {
            $('#smsCountSpan').hide();
        }

        $('#sms-count').text(smsCount);
    }

    $('#smsMessage').on('input', function () {
        updateCharCount();
        updateChatPreview();
    });

    // init 
    updateCharCount();
    updateChatPreview();
});
