let Departures;
let formFields;


$(function () {
    getChoosenDepartures();

    formFields = {
        name: $('#customer-name'),
        surname: $('#customer-surname'),
        address: $('#customer-address'),
        zip: $('#customer-zip'),
        city: $('#customer-city'),
        age: $('#customer-age'),
        phone: $('#customer-phone'),
        email: $('#customer-email'),
        adults: $('#order-adults'),
        underage: $('#order-underage'),
        pets: $('#customer-pets'),
        vehicles: $('#customer-vehicles')

    }

});

function getChoosenDepartures() {
    console.log("hello this is your function");

    if (window.sessionStorage.getItem("choosed-departures")) { //sjekkes om session storage fungerer og at item med departures eksisterer

        Departures = JSON.parse(window.sessionStorage.getItem("choosed-departures")); //valgt departures parses til objekt for videre bruk
        console.log(Departures);

    } else {

        window.location.href = "index.html";
    }
}


function validateInput() {
    return true;
}

function registerOrder() {

    const valid = validateInput();

    if (valid) {

        for (d of Departures) {

            const OrderInformation = {
                Name: formFields.name.val(),
                Surname: formFields.surname.val(),
                Age: formFields.age.val(),
                Address: formFields.address.val(),
                Zip_Code: formFields.zip.val(),
                City: formFields.city.val(),
                Phone: formFields.phone.val(),
                Email: formFields.email.val(),
                Departure_Id: d.id,
                Passengers: formFields.adults.val(),
                Passengers_Underage: formFields.underage.val(),
                Pets: formFields.pets.val(),
                Vehicles: formFields.vehicles.val()
            }

            const url = "API/RegisterOrder";
            $.post(url, OrderInformation).done(function () {


                alert("nice one!");

            }).fail(function () {

                BootstrapAlert($('#alert-container'), "danger", "Det gikk noe galt med registrering");
            });





        }

    }

    

    


}










