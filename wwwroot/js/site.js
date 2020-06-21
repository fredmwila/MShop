// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function update_url(url) {

    history.pushState(null, null, url);
}

function update_data(id,data) {

    $(id).html(data);
}



//function load_number_input() {
    
//}