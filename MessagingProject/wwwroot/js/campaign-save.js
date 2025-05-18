async function saveCampaign() {
    let token = await getToken();

    let template = templatesList.find(t => t.id == campaign.Id) || '';
    let selectedIds = $('#multi-select option:selected').map(function () {
        return $(this).attr('id');
    }).get();
    let scheduledDate = new Date();
    let isoDate = scheduledDate.toISOString();

    let request = {
        token: token,
        id: campaignId || null,
        name: $('#sendName').val(),
        subject: $('#templateSubject').val(),
        body: template.Body || campaign.Body || '',
        contactList: campaign.ContactList || selectedIds.join(','),
        created: isoDate,
        scheduled: campaign.Scheduled || isoDate,
        status: campaign.Status || 0,
        template: parseInt($('#templateName').val()) || campaign.Template,
        contactListID: ($('#multi-select').val() || []).join(','),
        replyTo: $('#replyEmail').val() || ''
    };

    if (request.id == null) {
        delete request.id;
    }

    if (!$('#scheduleToggle').is(':checked')) {
        let date = $('#scheduledDateTime').dxDateBox('instance').option('value');
        request.scheduled = date.toISOString();
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