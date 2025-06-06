function toLocalIsoStringWithOffset(date) {
    const pad = (n) => n.toString().padStart(2, '0');
    const tzOffset = -date.getTimezoneOffset();
    const sign = tzOffset >= 0 ? '+' : '-';
    const offsetHours = pad(Math.floor(Math.abs(tzOffset) / 60));
    const offsetMinutes = pad(Math.abs(tzOffset) % 60);

    return `${date.getFullYear()}-${pad(date.getMonth() + 1)}-${pad(date.getDate())}T${pad(date.getHours())}:${pad(date.getMinutes())}:00${sign}${offsetHours}:${offsetMinutes}`;
}

async function saveCampaign() {
    let token = await getToken();

    let template = $('#templateBody').val();
    let selectedIds = $('#multi-select option:selected').map(function () {
        return $(this).attr('id');
    }).get();

    let now = new Date();
    let localCreated = toLocalIsoStringWithOffset(now);

    let scheduledDate = $('#scheduledDateTime').dxDateBox('instance').option('value');
    let localScheduled = toLocalIsoStringWithOffset(new Date(scheduledDate));

    let request = {
        token: token,
        id: campaignId || null,
        name: $('#sendName').val(),
        subject: $('#templateSubject').val(),
        body: template || campaign.Body || '',
        contactList: selectedIds.join(',') || campaign.ContactList,
        created: localCreated,
        scheduled: $('#scheduleToggle').is(':checked') ? localScheduled : localCreated,
        status: campaign.Status || 0,
        template: parseInt($('#templateName').val()) || campaign.Template,
        contactListID: ($('#multi-select').val() || []).join(','),
        replyTo: $('#replyEmail').val() || ''
    };

    if (request.id == null) {
        delete request.id;
    }

    console.log("Request being sent:");
    console.log(JSON.stringify(request));

    $.ajax({
        url: '/Email/UpsertCampaign',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(request),
        success: function () {
            console.log('Campaign saved:');
            window.location.href = "/Email/MailingList";
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

