
                        $(document).ready(function () {


                            var city = 'rishon lezion';
                            var key = 'f5a9dcc0f6833e86ac420e23ee102d20';
                            $.ajax({
                                url: 'https://api.openweathermap.org/data/2.5/weather',
                                dataType: 'json',
                                type: 'GET',
                                data: { q: city, appid: key, units: 'metric' },

                                success: function (data) {
                                    var wf = '';
                                    $.each(data.weather, function (index, val) {
                                        wf += '<p><b>' + data.name + "</b><img src= https://openweathermap.org/img/w/" + val.icon + ".png></p>" +
                                            data.main.temp + '&deg;C ' + ' | ' + val.main + ',' + val.description
                                        if (val.description == "moderate rain" || val.description == "shower rain" || val.description == "rain" || val.description == "thunderstorm" || val.description == "snow" || val.description == "mist" || val.description == "thunderstorm with rain") {
                                            wf += '  - Due to an extreame weather no class for today :( ';

                                        }
                                        else { wf += '  - A wonderful weather for art ! :)' }
                                    });
                                    $("#showWeatherForcast").html(wf);
                                }


                            })


                        }
                        )