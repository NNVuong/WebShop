$(document).ready(function () {
    $(function () {
        $(".add-to-wishlist").click(function () {
            var productid = $('#ProductId').val();
            $.ajax({
                url: '/api/wishlist/add',
                type: "POST",
                dataType: "JSON",
                data: {
                    productID: productid,
                },
                success: function (response) {
                    if (response.result == 'Redirect') {
                        window.location = response.url;
                    }
                    else {
                        loadHeaderCart(); //Add Product success
                        location.reload();
                    }
                    console.log(response); // log to the console to see whether it worked
                },
                error: function (error) {
                    alert("There was an error posting the data to the server: " + error.responseText);
                }
            });
        });
        $(".removewishlist").click(function () {
            var productid = $(this).attr("data-mahh");
            $.ajax({
                url: "/api/wishlist/remove",
                type: "POST",
                dataType: "JSON",
                data: { productID: productid },
                success: function (result) {
                    if (result.success) {
                        location.reload();
                    }
                },
                error: function (rs) {
                    alert("Remove Cart Error !")
                }
            });
        });
    });
});
