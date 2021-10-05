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
        pets: $('#order-pets'),
        vehicles: $('#order-vehicles')

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

//summary: funksjon som oppdaterer total pris dersom endringer påføres inn i form
function updateTotalPrice() {
    
    let total = 0;

    for (d of Departures) {
        total = total + d.cruise.cruiseDetails.passeger_Price * formFields.adults.val();
        total = total + d.cruise.cruiseDetails.passegner_Underage_Price * formFields.underage.val();
        total = total + d.cruise.cruiseDetails.pet_Price * formFields.pets.val();
        total = total + d.cruise.cruiseDetails.vehicle_Price * formFields.vehicles.val();
    }

    $('#price-total').html(total + " kr");
    
}

//summary: funksjon som validerer input fra form
//returs: true - ved valid form, false - ved invalid form
function validateInput() {
    let isValid = true;



    return isValid;
}

//summary: funksjon som registrerer ordre for hver departure i Departures kun hvis form er valid.
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










