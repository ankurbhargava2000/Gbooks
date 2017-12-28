
function ChnageCompany(id) {
    $.ajax({
        url: '/Shared/ChnageCompany',
        type: 'POST',
        data: { 'CompanyId': id },
        dataType:'json',
        success: function (response) {
            if (response != null && response.success) {
                //alert(response.responseText);
                location.reload();
            }
        },
        error:function(xhr) {
            console.log(xhr.innerText);
        }
    });
}
