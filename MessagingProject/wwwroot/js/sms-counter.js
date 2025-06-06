//Counter
$('#multi-select-sms').on('change', function () {
    updateEmailCount();
});

//Counter method ajax request
function updateEmailCount() {
    let selectedValues = $('#multi-select-sms').val() || [];
    let clientInput = selectedValues.join(',');

    let selectedOptionIds = $('#multi-select-sms option:selected').map(function () {
        return this.id;
    }).get();
    let clientID = selectedOptionIds.join(',');

    $.ajax({
        url: '/Email/GetEmailsCount',
        type: 'GET',
        data: { clientInput: clientInput, clientID: clientID },
        success: function (count) {
            $('#email-count').text(count);
        },
        error: function () {
            $('#email-count').text("Error!");
        }
    });
}