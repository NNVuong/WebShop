$(document).ready(function () {
    $(function () {
        $(".add-to-cart").click(function () {
            var productid = $('#ProductId').val();
            var soLuong = $('#txtsoLuong').val();
            $.ajax({
                url: '/api/cart/add',
                type: "POST",
                dataType: "JSON",
                data: {
                    productID: productid,
                    amount: soLuong
                },
                success: function (response) {
                    if (response.result == 'Redirect') {
                        window.location = response.url;
                    }
                    else {
                        loadHeaderCart();
                        location.reload();
                    }
                    console.log(response); // log to the console to see whether it worked
                },
                error: function (error) {
                    alert("There was an error posting the data to the server: " + error.responseText);
                }
            });
        });

        $(".removecart").click(function () {
            var productid = $(this).attr("data-mahh");
            $.ajax({
                url: "/api/cart/remove",
                type: "POST",
                dataType: "JSON",
                data: { productID: productid },
                success: function (result) {
                    if (result.success) {
                        location.reload();
                        loadHeaderCart();
                    }
                },
                error: function (rs) {
                    //alert("Error !")
                    alert("Error !");
                }
            });
        });

        $(".cartItem").click(function () {
            var productid = $(this).attr("data-mahh");
            var soluong = parseInt($(this).val());
            $.ajax({
                url: "/api/cart/update",
                type: "POST",
                dataType: "JSON",
                data: {
                    productID: productid,
                    amount: soluong
                },
                success: function (result) {
                    if (result.success) {
                        loadHeaderCart();
                        location.reload();
                    }
                },
                error: function (rs) {
                    //alert("Cập nhật Cart Error !")
                    alert("Error !");
                }
            });
        });
    });

    function loadHeaderCart() {

        $('#miniCart').load("/AjaxContent/HeaderCart");
        $('#numberCart').load("/AjaxContent/NumberCart");

    }
});