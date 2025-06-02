function getCampaignId() {
    let params = new URLSearchParams(window.location.search);
    let id = params.get('ListId');
    console.log(id);
    return id;
}

