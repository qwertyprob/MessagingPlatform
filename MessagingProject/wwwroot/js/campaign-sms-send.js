//ISO DATE
function toLocalIsoStringWithOffset(date) {
    const pad = (n) => n.toString().padStart(2, '0');
    const tzOffset = -date.getTimezoneOffset();
    const sign = tzOffset >= 0 ? '+' : '-';
    const offsetHours = pad(Math.floor(Math.abs(tzOffset) / 60));
    const offsetMinutes = pad(Math.abs(tzOffset) % 60);

    return `${date.getFullYear()}-${pad(date.getMonth() + 1)}-${pad(date.getDate())}T${pad(date.getHours())}:${pad(date.getMinutes())}:00${sign}${offsetHours}:${offsetMinutes}`;
}

async function sendSmsCampaign() {

    let token = await getToken();
    //Iso Custom date
    let now = new Date();
    let localCreated = toLocalIsoStringWithOffset(now);

    //ID contacts
    let selectedIds = $('#multi-select option:selected').map(function () {
        return $(this).attr('id');
    }).get();

    //Scheduled date
    let scheduledDate = $('#smsScheduledDateTime').dxDateBox('instance').option('value');
    let localScheduled = scheduledDate ? toLocalIsoStringWithOffset(new Date(scheduledDate)) : localCreated;

    //true if on send now
    let isSendNow = $('#scheduleToggle').is(':checked');

    let request = {
        token: token,
        id: campaignId || null,
        name: $('#sendName').val(),
        description: $("#description").val(),
        message: $("#smsMessage").val(),
        shortName: $("#aliasName").val(),
        phoneList: selectedIds.join(',') || campaign.ContactListId,
        contactListId: ($('#multi-select').val() || []).join(','),
        contactListNames: ($('#multi-select option:selected').map(function () {
            return $(this).text();
        }).get() || []).join(','),
        createDate: campaign.createDate || localCreated,
        scheduledDate: isSendNow ? localCreated : localScheduled,
        status: campaign.Status || 2,


    }
    if (request.id == null) {
        delete request.id;
    }

    if (!isSendNow) {
        request.status = 1; // Set status to 1 if not sending now
    }

    console.log("Request being sent:");
    console.log(JSON.stringify(request));

    $.ajax({
        url: '/Sms/UpsertCampgaign',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(request),
        success: function () {
            console.log('Campaign saved:');
            window.location.href = "/SMS/SmsNewsLetters";
        },
        error: function (xhr, status, error) {
            console.error("Ошибка при сохранении кампании:", error);
        }
    });


}



async function getToken() {
    try {
        const response = await $.ajax({
            url: '/getuserByClaims',
            type: 'GET'
        });
        return response.Token;
    } catch (error) {
        console.error("Ошибка при получении токена:", error);
        return null;
    }
}