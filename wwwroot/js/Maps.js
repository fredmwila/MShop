function initMap() {
    var myLatLng = { lat: - 41.177434, lng: 174.340387 };

    var map = new google.maps.Map(document.getElementById('map'), {
        zoom: 10,
        center: myLatLng
    });

    var marker = new google.maps.Marker({
        position: myLatLng,
        map: map,
        title: 'AMO'
    });
}