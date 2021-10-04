﻿//global attributes
let Routes; //denne skal holde array med departures.
let TourType; //variable thats hold value of tour type, one way or two 
let DeparturesLeave; //holder utreiser for en vei eller tur
let DeparturesReturn; //holder utreiser for tilbake tur

const cart = new Cart(updateProceed); //cart objekt som holder rede på departures som velges

new DateUtilities();// oppretter objekt fra classen slik at den er defined

//summary: autostart funksjon som kaller på nødvendige funksjoner
$(function () {
    fetchRoutes();
    updateTourType();
    updateDOM_inputDate();
});

//summary: funksjon som oppdaterer attributer til dom input date slik at de viser dagens dato.
function updateDOM_inputDate() {
    const a = new Date();
    $('#date-leave').val(DateUtilities.toApiDateString(a));

    $('#date-return').val(DateUtilities.toApiDateString(a));

}

//summary: funksjon som henter og populerer dropdown meny for strekninger
function fetchRoutes() {

    const url = "API/GetRoutes";

    $.get(url, function (routes) {

        Routes = routes; //populate lokal variable som vil holde fetched routes, noe som kan akseseres senere

        let string = new String();

        for (var i = 0; i < routes.length; i++) {
            string += "<option value='" + i + "'>" + routes[i].origin + " - " + routes[i].destination + "</option>"
        }

        $('#route').html(string);

    }).fail(function () {

        Alert($('#alert-container'), "danger", "Henting av ruter gikk galt. Prøv igjen senere."); //print alert til brukeren

    })
}

//summary: funksjon som oppdaterer verdier til tur type og styrer avhenge input elemeter
function updateTourType() {
    
    if ($("#tour-type").val() == 1) {
        $('#date-return').prop('disabled', true);

    } else {
        $('#date-return').prop('disabled', false);
    }

}

//summary: funksjon som oppdaterer knappen, den er subscriberen til cart objekt, så den blir kjøret hver gang cart items tilstand endres
function updateProceed() {

    if (cart.getItemCount() == TourType) { //sammenlike antall items og tur type, det forventes 1 item for tur type 1 og 2 for 2.
        $('#button-proceed').show();

        $('html').scrollTop($('#button-proceed').offset().top); //scroller til knappen når den viser seg slik at brukeren vet at den befinner seg der
        //Det er ikke git at brukeren merker at knappen ble vist dersom det er content som skal scrolles.

    } else {
        $('#button-proceed').hide();
    }

}

//summary: funksjonen som samler data og bestemmer hvilke utreiser skal fetches fra serveren
function dispatchDepartureFetching() {

    $('.form-control').removeClass('is-invalid'); //fjerner tidligere feilmeldinger 

    let isValid = true; //flag som bestemmer om data til sammen er valide eller ikke 
    let isDateNull = false;

    const routes_index = $('#route').val(); 

    const routeId = Routes[routes_index].id; 
    const routeIdReverse = Routes[routes_index].return_id;

    const passengers = $('#passengers').val(); //henter personer verdi fra input 

    if (passengers < 1) { //antall personer er invalid hvis tallet er mindre enn 1
        isValid = false;
        $('#passengers').addClass('is-invalid');
    }

    const type = $("#tour-type").val(); //henter tour type verdi fra input

    const dateLeave = DateUtilities.inputToDateObject($('#date-leave').val()); //henter utreise dato fra input og konverterer til dato objekt
    const dateReturn = DateUtilities.inputToDateObject($('#date-return').val()); //henter retur dato fra input og konverterer til dato objekt

    if (dateLeave === null) {
        isValid = false;
        isDateNull = true;
        $('#date-leave').addClass('is-invalid');
    }

    if (type == 2) {
        if (dateReturn === null) {
            isValid = false;
            isDateNull = true;
            $('#date-return').addClass('is-invalid');
        }

        if (!isDateNull && dateReturn.getTime() < dateLeave.getTime()) {
            isValid = false;
            $('#date-leave').addClass('is-invalid');
            $('#date-return').addClass('is-invalid');
        }
    }



    if (isValid) { //hvis data er valide, skal man fetche data.

        const dateIntervalLeave = new DateInterval(dateLeave);
        const dateIntervalReturn = new DateInterval(dateReturn);

        fetchDepartures(routeId, dateIntervalLeave, passengers, processLeaveDepartures); //fetch tur utreise 

        if (type == 2) {
            //dersom det skal vises retur utreiser
            fetchDepartures(routeIdReverse, dateIntervalReturn, passengers, processReturnDepartures); //fetch disse utreiser

        } else {

            //dersom det skal ikke vises retur utreiser
            cleanDepartures($('#timetable-return'));    //fjen tidligere infromasjon siden denne runden forventes det ingen data. 
            //Ja den vil også fjerne selv om ingenting finnes men man må leve med det.

        }

        cart.emptyCart(); //nullstiller carten etter at nye utreiser skal vises som betyr at de gamle er irrelevante.

    }

}

//summary: lager spørring og henter data fra serveren
//parameters: route - id til route, from - Date objekt fra dato, to - Date objekt til dato, passengers - antall personer, 
//dataProcessingFunction - funksjonen hvor resultat data vil bli sendt til for videre prosessering.
function fetchDepartures(routeId, dateInterval, passengers, dataProcessingFunction) {

    const from = dateInterval.getStartInterval();
    const to = dateInterval.getEndInterval();

    //url for api call
    const url = "API/GetDepartures?Route=" + routeId + "&From=" + DateUtilities.toApiDateString(from) +
        "&To=" + DateUtilities.toApiDateString(to) + "&Passengers=" + passengers;

    $.get(url, function (data) {

        dataProcessingFunction(routeId, dateInterval, data);

    }).fail(function () {
        
        Alert($('#alert-container'), "danger", "Henting av data gikk galt. Prøv igjen senere."); //print alert til brukeren

    });
}

//summary: funksjon som videre prosesserer utreiser, den bestemmer ting som hvor data skal printes osv.
//parameters: interval - dateinterval objekt som inneholder intervalet, departures - liste med utreiser
function processLeaveDepartures(routeId, interval, departures) {
    DeparturesLeave = departures;


    TourType = $("#tour-type").val(); //oppdatere global verdi med nå søkt verdi.
 
    const details = $("#tour-type option:selected").text() + " for " + $('#passengers').val() + " personer:"; //den skal printe person og personer avhengig av antall
    $('#order-details').html(details); //vises titell for søket 

    //grunnet til at tour type og details er oppdatert/printet her er at disse skal utføres etter vellykket fetch. Samtidig leave departures skal skje altid ved søket.
    //dersom disse utføre før vellykket fetch, vil disse dataene oppdate gui selv om nye data kommer ikke.


    const routeObj = findRoute(routeId); //finne route for disse departures ut av departure id siden den returneres ikke av server
    
    displayDepartures(routeObj, interval, DeparturesLeave, $('#timetable-leave')); // kjøre print metode
}

//summary: funksjon som videre prosesserer tilbake utreiser, den bestemmer ting som hvor data skal printes osv.
//parameters: interval - dateinterval objekt som inneholder intervalet, departures - liste med utreiser
function processReturnDepartures(routeId, interval, departures) {
    DeparturesReturn = departures; //lagre departures som global objekt som kan akseseres senere

    const routeObj = findRoute(routeId); // finne route for disse departures ut av departure id siden den returneres ikke av server

    displayDepartures(routeObj, interval, DeparturesReturn, $('#timetable-return')); // kjøre print metode

}

//summary: funksjon som populerer tabell objekt med inn data. Html objekt må ha spesifik oppsett.
//parameters: route - route objekt, interval - interval objekt, departures - liste med departure objekter, DOM_Source - parent node
function displayDepartures(routeObj, interval, departures, DOM_Source) {

    const title = routeObj.origin + " - " + routeObj.destination;
    DOM_Source.children('#timetable-route').html(title);

    const subtitle = DateUtilities.toLocalDateString(interval.getStartInterval()) + " - " + DateUtilities.toLocalDateString(interval.getEndInterval());
    DOM_Source.children('#timetable-interval').html(subtitle);

    const header = "<tr><th>dato:</th><th>pris:</th></tr>";
    DOM_Source.children('table').children('thead').html(header);

    let tableContent = new String();

    for (var d = 0; d < departures.length; d++) {
        tableContent += "<tr data-value='" + d + "'><td>" + DateUtilities.isoToLocalDateString(departures[d].date) + "</td><td>" + departures[d].cruise.passeger_Price + " kr</td></tr>";
    }
    DOM_Source.children('table').children('tbody').html(tableContent);

    registerTableEventListeners(); // kjøre funksjon som vil registrere event listeners til tabell rekorder som har blitt lager her.

}

//summary: disse jquery funksjoner må være kjørt etter at tabellen er populert eller vil de ikke reagere på rowsa
//det er mulig å om formattere det men det er ikke bare bare her.
function registerTableEventListeners() {

    $('#table-leave tr').click(function () {

        $(this).addClass('table-active').siblings().removeClass('table-active');
        const value = $(this).data('value');

        cart.addToCart(0, DeparturesLeave[value]); //add departure til cart
        
    });


    $('#table-return tr').click(function () {

        $(this).addClass('table-active').siblings().removeClass('table-active');
        const value = $(this).data('value');

        cart.addToCart(1, DeparturesReturn[value]); //add departure til cart

    });


}

//summary: funksjon som fjerner innhold inn i tabell objekt, den vil kun fungere på spesielt formatert html struktur
//parameters: DOM_Source - parent node til objektet som skal renses
function cleanDepartures(DOM_Source) {

    DOM_Source.children('h5').html("");

    DOM_Source.children('table').children('thead').html("");

    DOM_Source.children('table').children('tbody').html("");

}

//summary: funksjon som finner route etter dens routeId (ikke Routes array index) i global Routes array
//parameters: routeId id til route objekt
//returns: route objekt, null hvis den ble ikke funnet.
function findRoute(routeId) {
    for (let r of Routes) {
        if (r.id == routeId) {
            return r;
        } 
    }

    return null;
}

//summary funksjonen som vil takle things videre etter at kunden vil forsette. Denne skal kun kunnes kjøres etter at departures er valgt
function Proceed() {

    //sjekk for session storage, hvis det ikke finnes, vis feil melding

    //ellers serialise begge elementer, hvis de er ikke null og så flytt til et annen location.

    console.log(cart.getItem(0));
    console.log(cart.getItem(1));

}



