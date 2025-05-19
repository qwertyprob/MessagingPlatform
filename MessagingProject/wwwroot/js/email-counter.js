//Counter
$('#multi-select').on('change', function () {
    updateEmailCount();
});

//Counter method ajax request
function updateEmailCount() {
    let selectedValues = $('#multi-select').val() || [];
    let clientInput = selectedValues.join(',');

    let selectedOptionIds = $('#multi-select option:selected').map(function () {
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