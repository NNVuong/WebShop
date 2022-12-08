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
        url: '/Account/Delete',
        dataType: 'json',
        data: { userId: transferId },
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