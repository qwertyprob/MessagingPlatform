//Counter
$('#multi-select').on('change', function () {
    updateSmsCount();
});

function updateSmsCount() {
    let selectedIds = $('#multi-select option:selected').map(function () {
        return $(this).val(); // тут вернется contact.Id, потому что value = contact.Id
    }).get();

    let selectedNames = $('#multi-select option:selected').map(function () {
        return $(this).text(); // Name
    }).get();

    let clientID = selectedIds.join(',');    
    let phoneList = selectedNames.join(','); 


    $.ajax({
        url: '/Sms/NumbersCount',
        type: 'GET',
        data: { phoneList: phoneList, clientID: clientID },
        success: function (count) {
            $('#sms-contact').text(count);
        },
        error: function () {
            $('#sms-contact').text("Error!");
        }
    });
}
