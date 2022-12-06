var _roleName = $('#txt-roleName').text();
$('body').off('click', '.remove').on('click', '.remove', RemoveRole);

function RemoveRole() {
    var model = new Object();

    model.UserName = $(this).attr('data-user');
    model.RoleName = _roleName

    $.ajax({
        type: 'post',
        url: '/role/RemoveRole',
        dataType: 'json',
        data: JSON.stringify(model),
        contentType: "application/json; charset=utf-8",
        success: function (response) {

            if (response.statusCode == 200) {
                location.reload();

            } else {
                bootbox.alert("Đã xảy ra lỗi")
            }
        }
    })

}

$('body').off('click', '.add').on('click', '.add', AddRole);

function AddRole() {
    var model = new Object();

    model.UserName = $(this).attr('data-user');
    model.RoleName = _roleName

    $.ajax({
        type: 'post',
        url: '/role/AssignRole',
        dataType: 'json',
        data: JSON.stringify(model),
        contentType: "application/json; charset=utf-8",
        success: function (response) {

            if (response.statusCode == 200) {
                location.reload();

            } else {
                bootbox.alert("Đã xảy ra lỗi")
            }
        }
    })

}