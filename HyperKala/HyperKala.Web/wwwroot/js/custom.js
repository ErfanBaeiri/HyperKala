function ShowMessage(title, text, theme) {
    window.createNotification({
        closeOnClick: true,
        displayCloseButton: false,
        positionClass: 'toast-top-right',
        showDuration: 4000,
        theme: theme !== '' ? theme : 'success'
    })({
        title: title !== '' ? title : 'اعلان',
        message: decodeURI(text)
    });
}

function FillPageId(pageId) {
    document.getElementById("PageId").value = pageId;
    document.getElementById("filter-Form").submit();
}

//function FillPageId(pageId) {
//    $("#PageId").val(pageId)
//    $("#filter-Form").submit()
//}