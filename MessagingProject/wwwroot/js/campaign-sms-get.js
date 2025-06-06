let contactList = [];
let aliasList = [];

function getCampaignId() {
    let params = new URLSearchParams(window.location.search);
    let id = params.get('ListId');
    console.log(id);
    return id;
}

//Main
$(function () {

    let campaignId = getCampaignId();
    if (campaignId != null && campaignId != undefined) {
        console.log('updatemode');
        getCampaignSms(campaignId);


    }
    else {
        console.log('createMode');

    }

});

// CAMPAIGN BY ID
async function getCampaignSms(id) {
    try {
        const response = await $.ajax({
            url: '/Sms/GetSmsCampaign/' + id,
            type: 'GET'
        });

        campaign = response;

        $('#sendName').val(response.Name);
        $('#description').val(response.Description);
        $('#smsMessage').val(response.Message);

        $('#smsMessage').trigger('input');


        let selectedContacts = response.ContactListID ? response.ContactListID.split(',') : [];

        const phones = response.PhoneList.split(',').filter(e => e.trim() !== '');
        const $counter = $('#sms-count');

        $counter.text(phones.length);

        getContact(selectedContacts);

    } catch (error) {
        console.error('Error fetching campaign:', error);
    }
}

//GET ALL CONTACTS
function getContact(selectedContacts = []) {
    $.ajax({
        url: '/Contacts/GetContactLists',
        type: 'GET',
        success: function (response) {
            $('#multi-select').empty();

            response.forEach(function (contact) {
                $('#multi-select').append(
                    `<option id="${contact.Id}" value="${contact.Name}">${contact.Name}</option>`
                );
            });
            //Parse Emails input..
            selectedContacts.forEach(function (val) {
                if ($(`#multi-select option[value="${val.trim()}"]`).length === 0) {
                    $('#multi-select').append(`<option value="${val.trim()}">${val.trim()}</option>`);
                }
            });
            $('#multi-select').val(selectedContacts).trigger('change');
        },
        error: function (xhr, status, error) {
            console.error('Error fetching contact lists:', status, error);
        }
    });
}


