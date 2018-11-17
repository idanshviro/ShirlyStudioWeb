
                                var infowindow = new google.maps.InfoWindow({
                                    content: contentString
                                });
                                function initMap() {
                                    //map..
                                    var map = new google.maps.Map(document.getElementById('dvMap'), {
                                        center: {
                                            lat: 31.994938,
                                            lng: 34.767188
                                        },
                                        zoom: 16
                                    });
                                    //marker..
                                    var marker = new google.maps.Marker({
                                        position: {
                                            lat: 31.994938,
                                            lng: 34.767188
                                        },
                                        clickable: true,
                                        map: map,
                                        draggable: false
                                    });
                                    marker.addListener('click', function () {
                                        infowindow.open(map, marker);
                                    });

                                    //function to show  a note when clicking on the location
                                    var infowindow = new google.maps.InfoWindow({
                                        content: "שירלי מרכז אומנויות"
                                    });

                                    google.maps.event.addListener(marker, 'click', function () {
                                        infowindow.open(map, marker);
                                    });
                                }
    function (d, s, id) {
            var js, fjs = d.getElementsByTagName(s)[0];
            if (d.getElementById(id)) return;
            js = d.createElement(s); js.id = id;
            js.src = 'https://connect.facebook.net/he_IL/sdk.js#xfbml=1&version=v3.1';
            fjs.parentNode.insertBefore(js, fjs);
        }(document, 'script', 'facebook-jssdk');