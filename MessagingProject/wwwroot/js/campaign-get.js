let templatesList = [];
let contactList = [];
let campaignId = getCampaignId();
let templateId = getTemplateId();

$(function () {
    let campaignId = getCampaignId();
    let templateId = getTemplateId();

    if (campaignId != null && campaignId != undefined) {
        console.log('updatemode');
        getCampaign(campaignId);

    }
    else if (templateId != null && templateId != undefined) {
        console.log('templatemode');
        getTemplates();
        getContact();
    }
    else {
        console.log('createmode');
        getTemplates();
        getContact();
    }
});
//ListIdParam
function getCampaignId() {
    let params = new URLSearchParams(window.location.search);
    let id = params.get('ListId');

    return id;
}

function getTemplateId() {
    let params = new URLSearchParams(window.location.search);
    let id = params.get('Id');

    return id;

}

// GET ONE TEPLATE
function getTemplate(id) {
    $.ajax({
        url: '/Template/GetTemplate/' + id,
        type: 'GET',
        success: function (response) {
            if (response.ImageTemplate) {
                $('#imageField').attr('src', response.ImageTemplate);
                $('#templateName').empty();
                $('#templateName').append(`<option value="${response.Id}" selected>${response.Name}</option>`);
                $('#templateSubject').val(response.Name);

                console.log('Template data retrieved successfully:', response);
            } else {
                console.log('No image URL available in the response.');
            }
        },
        error: function (xhr, status, error) {
            console.error('Error fetching template:', status, error);
        }
    });
}

// GET TEMPLATE LIST
function getTemplates(selectedTemplateId = null) {
    $.ajax({
        url: '/Template/TemplateList',
        type: 'GET',
        success: function (response) {
            if (response) {
                templatesList = response;
                $('#templateName').empty();

                templatesList.forEach(function (template) {
                    $('#templateName').append(
                        `<option value="${template.Id}">${template.Name}</option>`
                    );
                });

                let idToSelect = selectedTemplateId || getTemplateId() || templatesList[0].Id;
                $('#templateName').val(idToSelect).trigger('change');

                console.log('Template data retrieved successfully:', response);
            } else {
                console.log('No templates in the response.');
            }
        },
        error: function (xhr, status, error) {
            console.error('Error fetching templates:', status, error);
        }
    });
}


// SELECT HANDLER
$('#templateName').on('change', function () {
    let selectedId = $(this).val();
    console.log('Selected template ID:', selectedId);

    let selectedTemplate = templatesList.find(t => t.Id.toString() === selectedId.toString());

    if (selectedTemplate) {
        $('#templateSubject').val(selectedTemplate.Subject);
        $('#imageField').attr('src', selectedTemplate.ImageTemplate);
    } else {
        console.log('Template not found for ID:', selectedId);
    }
});

// CAMPAIGN BY ID
function getCampaign(id) {
    $.ajax({
        url: '/Email/GetCampaign/' + id,
        type: 'GET',
        success: function (response) {
            console.log('Campaign data retrieved successfully:', response);

            let templateId = response.Template;
            getTemplates(templateId);


            let selectedContacts = response.ContactListID.split(',');
            getContact(selectedContacts);
        },
        error: function (xhr, status, error) {
            console.error('Error fetching campaign:', status, error);
        }
    });
}
//GET ALL CONTACTS
function getContact(selectedContacts = []) {
    $.ajax({
        url: '/Contacts/GetContactLists',
        type: 'GET',
        success: function (response) {
            console.log('Contact list retrieved:', response);
            $('#multi-select').empty();

            response.forEach(function (contact) {
                let isSelected = selectedContacts.includes(contact.Name);
                $('#multi-select').append(
                    `<option value="${contact.Name}" ${isSelected ? 'selected' : ''}>${contact.Name}</option>`
                );
            });
        },
        error: function (xhr, status, error) {
            console.error('Error fetching contact lists:', status, error);
        }
    });
}