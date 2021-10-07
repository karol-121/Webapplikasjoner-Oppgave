﻿let Departures;
let formFields;
let index;


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

//summary: funksjon som utløser registrering for hver element inn i array dersom input data er valide
function registerOrder() {

    const valid = validateInput(); //input data valideres

    if (valid) {
        //hvis input data er valide

        const items = new Array(); //lage liste med alle elementer som skal registreres

        for (let d of Departures) { //for hver valgt departure, register et bilett

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

    //avslutte registrering sesjon
    $.get("API/DemolishRegisterSession", function () {

    });

    window.sessionStorage.removeItem("choosed-departures"); //fjerne departures session item, siden de skal ikke brukes lenger
    window.sessionStorage.setItem("register-successfull", ""); //sette et session verdi om vellykket registering som index.html kan bruke
    window.location.href = "index.html"; //gå til index.html


}

//summary: funksjon som behandler feil ved registrering
function fail() {

    //avslutte registrering sesjon
    $.get("API/DemolishRegisterSession", function () {

    });

    BootstrapAlert($('#alert-container'), "danger", "Det gikk noe galt med registrering");
}


// Autofylling av city etter aa ha skrevet inn zipcode
function autozip() {

    // Henter inn value fra zip
    var r = document.getElementById('zip').value;
    // Omgjor om til string
    var n = r.toString();
    // forste to tallene blir hentet
    var x = n.slice(0, 2); 

        // to forste tallene utgir hvilken city
        if (x >= '00' && x <= '12') {
            document.getElementById('city').value = 'Oslo';
        }
        else if (x >= '13' && x <= '15') {
            document.getElementById('city').value = 'Akershus';

        }

        else if (x >= '16' && x <= '18') {
            document.getElementById('field2').value = 'Østfold';

        }
        else if (x >= '19' && x <= '21') {
            document.getElementById('field2').value = 'Akershus';

        }

        else if (x >= '22' && x <= '26') {
            document.getElementById('field2').value = 'Hedmark';

        }
        else if (x >= '27' && x <= '29') {
            document.getElementById('field2').value = 'Oppland';

        }
        else if (x >= '30' && x <= '30') {
            document.getElementById('field2').value = 'Buskerud';

        }
        else if (x >= '31' && x <= '32') {
            document.getElementById('field2').value = 'Vestfold';

        }
        else if (x == '35' && x <= '35') {
            document.getElementById('field2').value = 'Oppland';

        }
        else if (x >= '33' && x <= '36') {
            document.getElementById('field2').value = 'Buskerud';

        }

        else if (x >= '36' && x <= '39') {
            document.getElementById('field2').value = 'Telemark';

        }
        else if (x >= '40' && x <= '44') {
            document.getElementById('field2').value = 'Rogaland';

        }
        else if (x >= '45' && x <= '47') {
            document.getElementById('field2').value = 'Vest-Agder';

        }
        else if (x >= '48' && x <= '49') {
            document.getElementById('field2').value = 'Aust-Agder';

        }
        else if (x >= '50' && x <= '59') {
            document.getElementById('field2').value = 'Hordaland';

        }
        else if (x == '55') {
            document.getElementById('field2').value = 'Rogaland';

        }
        else if (x == '57' && x == '59') {
            document.getElementById('field2').value = 'Sogn og Fjordane';

        }
        else if (x >= '60' && x <= '66') {
            document.getElementById('field2').value = 'Møre og Romsdal';

        }
        else if (x >= '67' && x <= '69') {
            document.getElementById('field2').value = 'Sogn og Fjordane';

        }
        else if (x >= '70' && x <= '75') {
            document.getElementById('field2').value = 'Sør-Trøndelag';

        }
        else if (x == '76') {
            document.getElementById('field2').value = 'Nord-Trøndelag';

        }
        else if (x == '77') {
            document.getElementById('field2').value = 'Sør-Trøndelag';

        }
        else if (x >= '78' && x <= '79') {
            document.getElementById('field2').value = 'Nord-Trøndelag';

        }
        else if (x >= '80' && x <= '89') {
            document.getElementById('field2').value = 'Nordaland';

        }
        else if (x == '84' && x >= '90' && x <= '94') {
            document.getElementById('field2').value = 'Troms';

        }

        else if (x == '91' && x >= '95' && x <= '99') {
            document.getElementById('field2').value = 'Finmark';

        }



    }












