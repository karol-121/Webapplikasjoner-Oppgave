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

//summary: funksjon som henter valgte departures som skal ligge i session storage. Dersom de finnes ikke, returneres til index.html
function getChoosenDepartures() {

    if (window.sessionStorage.getItem("choosed-departures")) { //sjekkes om session storage fungerer og at item med departures eksisterer

        Departures = JSON.parse(window.sessionStorage.getItem("choosed-departures")); //valgt departures parses til objekt for videre bruk

    } else {

        window.location.replace("index.html"); //dersom valgte departures finnes ikke, gå til index.html
    }
}

//summary: funksjon som oppdaterer total pris dersom endringer påføres inn i form
function updateTotalPrice() {
    
    let total = 0;

    for (d of Departures) { //ta ut priser, gang med antall valgt og summer for hver departure
        total = total + d.cruise.cruiseDetails.passeger_Price * formFields.adults.val();
        total = total + d.cruise.cruiseDetails.passegner_Underage_Price * formFields.underage.val();
        total = total + d.cruise.cruiseDetails.pet_Price * formFields.pets.val();
        total = total + d.cruise.cruiseDetails.vehicle_Price * formFields.vehicles.val();
    }

    $('#price-total').html(total + " kr"); //print sub total
    
}

//summary: funksjon som validerer input fra form
//returs: true - ved valid form, false - ved invalid form
function validateInput() {
    $('.form-control').removeClass('is-invalid');

    const validator = new registerValidation();

    validator.checkName(formFields.name);
    validator.checkSurname(formFields.surname);
    validator.checkAddress(formFields.address);
    validator.checkZip(formFields.zip);
    validator.checkCity(formFields.city);
    validator.checkAge(formFields.age);
    validator.checkPhone(formFields.phone);
    validator.checkEmail(formFields.email);
    validator.checkAdults(formFields.adults);
    validator.checkUnderage(formFields.underage);
    validator.checkPets(formFields.pets);
    validator.checkVehicles(formFields.vehicles);

    return validator.isValid();
}

//summary: funksjon som registrerer det som skal, det vil si det som har blitt valgt tidligere
function registerOrder() {

    const valid = validateInput(); //input data valideres

    if (valid) {
        //hvis input data er valide

        const items = new Array(); //lage liste med alle elementer som skal registreres

        for (let d of Departures) { //for hver valgt departure, lag et bilett

            const ticket = {
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
            };

            items.push(ticket);

        }


        //lage order objekt som skal inneholde liste med alle biletter som skal registreres
        const order = {
            Items: items
        }


        //registrering av ordre
        const url = "API/RegisterOrder"
        $.post(url, order, function () {

            //dersom registrering er vellykket
            success();

        }).fail(function () {

            //dersom det oppstår feil 
            fail();

        });

    }


}


//summary: funksjon som behandler vellykket registrering
function success() {

    window.sessionStorage.removeItem("choosed-departures"); //fjerne departures session item, siden de skal ikke brukes lenger
    window.sessionStorage.setItem("register-successfull", ""); //sette et session verdi om vellykket registering som index.html kan bruke
    window.location.href = "index.html"; //gå til index.html

}

//summary: funksjon som behandler feil ved registrering
function fail() {

    //vise feilmelding
    BootstrapAlert($('#alert-container'), "danger", "Det gikk noe galt med registrering");
}

//summary: funksjon som fyller inn byen basert på zip input
function updateZip() {
    AutofillCity(formFields.zip, formFields.city);
}


     

















