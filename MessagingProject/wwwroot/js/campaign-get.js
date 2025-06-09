let templatesList = [];
let contactList = [];
let campaignId = getCampaignId();
let templateId = getTemplateId();
let campaign = {};
let template = {};
let contact = {};
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

// GET ONE TEMPLATE
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
                $('#templateBody').val(response.Body);

                /*console.log('Template data retrieved successfully:', response);*/
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
    return new Promise((resolve, reject) => {
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

                    resolve(); 
                } else {
                    console.log('No templates in the response.');
                    resolve();
                }
            },
            error: function (xhr, status, error) {
                console.error('Error fetching templates:', status, error);
                reject(error);
            }
        });
    });
}



// SELECT HANDLER
$('#templateName').on('change', function () {
    let selectedId = $(this).val();
    templateId = selectedId;
    /*console.log('Selected template ID:', selectedId);*/

    let selectedTemplate = templatesList.find(t => t.Id.toString() === selectedId.toString());

    if (selectedTemplate) {
        $('#templateSubject').val(selectedTemplate.Subject);
        $('#imageField').attr('src', selectedTemplate.ImageTemplate);
        $('#templateBody').val(selectedTemplate.Body);
        console.log(selectedTemplate.Body);
        
    } else {

        console.log('Template not found for ID:', selectedId);

    }
});

// CAMPAIGN BY ID
async function getCampaign(id) {
    try {
        const response = await $.ajax({
            url: '/Email/GetCampaign/' + id,
            type: 'GET'
        });

        campaign = response;

        let templateId = response.Template;
        await getTemplates(templateId); 

        $('#sendName').val(response.Name);
        $('#templateSubject').val(response.Subject);
        $('#templateBody').val(response.Body); 

 
        const emails = response.ContactList.split(',').filter(e => e.trim() !== '');
        const $counter = $('#email-count');

        $counter.text(emails.length);
        

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


//GET CONTACT BY ID
function getContactById(id) {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: '/Contacts/SingleListQuery/' + id,
            type: 'GET',
            success: function (response) {
                if (response && response.data) {
                    resolve(response.data); 
                } else {
                    console.warn('Empty data for id:', id);
                    resolve(null); 
                }
            },
            error: function (xhr, status, error) {
                console.error('Ошибка при получении ID ' + id + ':', error);
                resolve(null); 
            }
        });
    });
}


async function getContactsByIds(ids) {
    const contacts = [];

    for (const id of ids) {
        const contact = await getContactById(id);
        if (contact !== null) {
            contacts.push(contact);
        }
    }

    return contacts;
}


