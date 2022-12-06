$('body').off('click', '#btn-add').on('click', '#btn-add', Add);
function Add() {
    $('#hid-id').text('');
    $('#txt-RoleName').val('');
    $('#txt-Description').val('');
}


$('body').off('click', '.update').on('click', '.update', GetById);
function GetById() {

    var tranferId = $(this).data('id');
    $.ajax({
        type: 'post',
        url: '/Role/GetById',
        dataType: 'json',
        data: { roleId: tranferId },

        success: function (response) {


            var data = response.result;

            $('#hid-id').text(data.id);
            $('#txt-RoleName').val(data.name);
            $('#txt-Description').val(data.description);
        }
    })
}
$('body').off('click', '#btn-save').on('click', '#btn-save', Save);
function Save() {
    var model = new Object();

    model.RoleId = $('#hid-id').text();
    model.RoleName = $('#txt-RoleName').val();
    model.Description = $('#txt-Description').val();

    
    $.ajax({
        type: 'post',
        url: '/role/Save',
        dataType: 'json',
        data: JSON.stringify(model),
        contentType: "application/json; charset=utf-8",
        success: function (response) {

            if (response.statusCode == 200) {
                bootbox.alert("Thêm mới/Cập nhật thành công", function () {
                    location.reload()
                })
            } else {
                bootbox.alert("Xảy ra lỗi")

            }
        }
    })
    
    $('#modal-add').modal('hide');
}

$('body').off('click', '.delete').on('click', '.delete', ConfirmDelete);
function ConfirmDelete() {
    var tranferId = $(this).data('id');
    bootbox.confirm("Bạn chắc chắn muốn xóa", function (result) {
        if (result) {
            Delete(tranferId)
        }
    })
}
function Delete(transferId) {
    $.ajax({
        type: 'post',
        url: '/Role/Delete',
        dataType: 'json',
        data: { roleId: transferId },
        success: function (response) {

            if (response.statusCode == 200) {
                bootbox.alert("Xóa thành công", function () {
                    location.reload()
                })
            } else {
                bootbox.alert("Xảy ra lỗi")
            }
        }
    })
}