let contactList = [];
let aliasList = [];
let campaignId = getCampaignId();
let campaign = getCampaignSms(campaignId);

function getCampaignId() {
    let params = new URLSearchParams(window.location.search);
    let id = params.get('ListId');
    console.log(id);
    return id;
}

//Main
$(async function () {
    let campaignId = getCampaignId();
    if (campaignId != null && campaignId != undefined) {
        console.log('updatemode');
        await getCampaignSms(campaignId);
    } else {
        console.log('createMode');
        getAlias();
        getContact();
    }
});


// CAMPAIGN BY ID
async function getCampaignSms(id) {
    if (!id) {
        return;
    }
        const response = await $.ajax({
            url: '/Sms/GetSmsCampaign/' + id,
            type: 'GET'
        });

        campaign = response;


        $('#sendName').val(response.Name);
        $('#description').val(response.Description);
        $('#smsMessage').val(response.Message);

        $('#smsMessage').trigger('input');
        $('#aliasName').val(response.ShortName);

        //Костыль
        $('#sms-contact').text(response.ContactListID.split(',')
            .filter(p => p.trim() !== '').length);

        await getAlias(campaign.ShortName);

        let selectedContacts = response.ContactListID ? response.ContactListID.split(',') : [];
        getContact(selectedContacts);

        const phones = response.PhoneList.split(',').filter(e => e.trim() !== '');
        const $counter = $('#sms-count');

        $counter.text(phones.length);

        getContact(selectedContacts);

    
    
}



//GET ALL CONTACTS
async function getContact(selectedContactsRaw = []) {
    const response = await $.ajax({
        url: '/Contacts/GetContactLists',
        type: 'GET'
    });

    $('#multi-select').empty();

    //Hashmap name,id
    const nameToIdMap = {};
    const knownIds = new Set();

    response.forEach(function (contact) {
        $('#multi-select').append(
            `<option value="${contact.Id}">${contact.Name}</option>`
        );
        nameToIdMap[contact.Name.trim()] = contact.Id;
        knownIds.add(contact.Id.toString());
    });

    const selectedContactIds = [];

    selectedContactsRaw.forEach(function (rawValue) {
        const trimmed = rawValue.trim();

        // if Name, search Id
        const mappedId = nameToIdMap[trimmed];

        if (mappedId) {
            selectedContactIds.push(mappedId);
        } else {
            //add phonenumber
            if (!$(`#multi-select option[value="${trimmed}"]`).length) {
                $('#multi-select').append(
                    `<option value="${trimmed}">${trimmed}</option>`
                );
            }
            selectedContactIds.push(trimmed);
        }
    });

    $('#multi-select').val(selectedContactIds).trigger('change');
}





function getAlias(shortName) {
    $.ajax({
        url: '/Sms/ESMAlias',
        type: 'GET',
        success: function (response) {
            $('#aliasName').empty();

            response.forEach(function (alias) {
                $('#aliasName').append(`<option value="${alias}">${alias}</option>`);
            });

            if (shortName && response.includes(shortName)) {
                $('#aliasName').val(shortName).trigger('change');
            }
        },
        error: function (xhr, status, error) {
            console.error('Error fetching alias list:', status, error);
        }
    });
}




