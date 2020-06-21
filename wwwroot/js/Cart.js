function load_cart_data(culture) {
    $.ajax({
        //get data
        url: "/" + culture + "/cart/GetCart",
        type: "Get",
        success: function (data) {
            if ($(".CartData").length > 0) {
                //add data to page
                $('.CartData').html(data);
            }
            get_cartTotal();

            load_number_input();

        },
        error: function (data) {
            $(".CartData").text("error <br/>" + data);
        }
    });
}

function get_cartTotal() {
    $.ajax({
        //get data
        url: "/cart/GetCartTotal",
        type: "Post",
        dataType: "json",
        success: function (data) {
            //if ($("#CartTotal").length > 0) {
                $('#CartTotal').html("Cart: " + data.cartTotal);

            //}
        },
    });
}