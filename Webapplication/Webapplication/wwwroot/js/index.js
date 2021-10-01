//global attributes
let TourType; //variable thats hold value of tour type, one way or two way
let Routes; //denne skal holde array med departures.
let DeparturesLeave; //holder utreiser for en vei eller tur
let DeparturesReturn; //holder utreiser for tilbake tur

const cart = new Cart(updateProceed);

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
    $("#dateLeave").val(DateUtilities.toApiDateString(a));
    //$("#dateLeave").attr("min",DateUtilities.toApiDateString(a));

    $("#dateReturn").val(DateUtilities.toApiDateString(a));
    //$("#dateReturn").attr("min",DateUtilities.toApiDateString(a));

}

//summary: funksjon som henter og populerer dropdown meny for strekninger
function fetchRoutes() {

    const url = "API/GetRoutes";

    $.get(url, function (routes) {

        console.log("route fetch ok")

        let string = new String();

        Routes = routes; //populate lokal variable som vil holde fetched routes, noe som kan akseseres senere

        for (var i = 0; i < routes.length; i++) {
            string += "<option value='" + i + "'>" + routes[i].origin + " - " + routes[i].destination + "</option>"
        }

        $("#route").html(string);

    }).fail(function () {

        console.log("route fetch failed!");

    })
}

//summary: funksjon som oppdaterer verdier til tur type og styrer avhenge input elemeter
function updateTourType() {
    TourType = $("#tourType").val(); //update the global variable with current state

    //her skal man også legge til loggikken angående den proceed knappen
    //hvis det er kun utreise, så det er nok at utreise er valgt,
    //hvis det er utreise og tilbake, så må begge være valgt for å enable knappen

    if (TourType == 0) {
        $("#dateReturn").prop('disabled', true);
        $("#timetable-return").hide();

    } else {
        $("#dateReturn").prop('disabled', false);
        $("#timetable-return").show();
    }

    updateProceed();
}

//todo: det blir kanskje bedre at update tour type og update proceed funksjoner blir slått sammen siden de gjør det samme ish.

//summary: funksjon som oppdaterer knappen
//this should be subscribed to array and then the check will update itself whenever array state changes.
function updateProceed() {
    if ((TourType == 1 && cart.getItemCount() === 2) || (TourType == 0 && cart.getItemCount() >= 1)) {
        $('#button-proceed').show();
    } else {
        $('#button-proceed').hide();
    }
}

//summary: funksjonen som samler data og bestemmer hvilke utreiser skal fetches fra serveren
function dispatchDepartureFetching() {

    //todo: maybe make it so it does not fetch new values if nothing changed
    const routes_index = $("#route").val();

    const routeId = Routes[routes_index].id;
    const routeIdReverse = Routes[routes_index].return_id;

    const dateLeave = $("#dateLeave").val();
    const dateReturn = $("#dateReturn").val();
    const passengers = $("#passengers").val();


    //input validation goes here i guess, just do not verify the date return and route reverse if one way is choosen
    //question is if the date should be validated, as currently it will default to todays date anyway.
    //could be also nice if route reverse if wrong, do not allow for two way orders

    //also check if the return date is greater than leave, otherwise it is a error as you can not return before you go.

    const dateIntervalLeave = new DateInterval(DateUtilities.inputToDateObject(dateLeave));
    const dateIntervalReturn = new DateInterval(DateUtilities.inputToDateObject(dateReturn));

    fetchDepartures(routeId, dateIntervalLeave, passengers, processLeaveDepartures);


    if (TourType == 1) {
        //dersom det skal vises retur utreiser
        fetchDepartures(routeIdReverse, dateIntervalReturn, passengers, processReturnDepartures); //fetch disse utreiser
        
    } else {

        //dersom det skal ikke vises retur utreiser
        cleanDepartures($("#timetable-return"));    //fjen tidligere infromasjon siden denne runden forventes det ingen data. 
                                                    //Ja den vil også fjerne selv om ingenting finnes men man må leve med det.

    }

    cart.emptyCart();
    //clean the cart here as dispaching new routes can make current choosen irrelenat.

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
        console.log("departure fetch ok");
        dataProcessingFunction(routeId, dateInterval, data);

    }).fail(function () {
        console.log("departure fetch failed!");
        //her skal det vises noe alert tingy.

    });
}

//summary: funksjon som videre prosesserer utreiser, den bestemmer ting som hvor data skal printes osv.
//parameters: interval - dateinterval objekt som inneholder intervalet, departures - liste med utreiser
function processLeaveDepartures(routeId, interval, departures) {
    DeparturesLeave = departures;

    const routeObj = findRoute(routeId);
    
    displayDepartures(routeObj, interval, DeparturesLeave, $("#timetable-leave"));
    //print Departures Leave
}

//summary: funksjon som videre prosesserer tilbake utreiser, den bestemmer ting som hvor data skal printes osv.
//parameters: interval - dateinterval objekt som inneholder intervalet, departures - liste med utreiser
function processReturnDepartures(routeId, interval, departures) {
    DeparturesReturn = departures;

    const routeObj = findRoute(routeId)

    //her skal printes, endres dom elementer som er spesifike for return departure
    displayDepartures(routeObj, interval, DeparturesReturn, $("#timetable-return"));
    //print Departures Return
}

//summary: funksjon som populerer tabell objekt med inn data. Html objekt må ha spesifik oppsett.
//parameters: route - route objekt, interval - interval objekt, departures - liste med departure objekter, DOM_Source - parent node
function displayDepartures(routeObj, interval, departures, DOM_Source) {

    const title = routeObj.origin + " - " + routeObj.destination;
    DOM_Source.children("h3").html(title);

    const subtitle = DateUtilities.toLocalDateString(interval.getStartInterval()) + " - " + DateUtilities.toLocalDateString(interval.getEndInterval());
    DOM_Source.children("h4").html(subtitle);

    const header = "<tr><th>dato:</th><th>pris:</th></tr>";
    DOM_Source.children("table").children("thead").html(header);

    let tableContent = new String();

    for (var d = 0; d < departures.length; d++) {
        tableContent += "<tr data-value='" + d + "'><td>" + DateUtilities.isoToLocalDateString(departures[d].date) + "</td><td>" + departures[d].cruise.passeger_Price + " kr</td></tr>";
    }
    DOM_Source.children("table").children("tbody").html(tableContent);

    registerTableEventListeners();

}

//summary: funksjon som fjerner innhold inn i tabell objekt, den vil kun fungere på spesielt formatert html struktur
//parameters: DOM_Source - parent node til objektet som skal renses
function cleanDepartures(DOM_Source) {

    DOM_Source.children("h3").html("");

    DOM_Source.children("h4").html("");

    DOM_Source.children("table").children("thead").html("");

    DOM_Source.children("table").children("tbody").html("");

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


//summary: disse jquery funksjoner må være kjørt etter at tabellen er populert eller vil de ikke reagere på rowsa
//det er mulig å om formattere det men det er ikke bare bare her.
function registerTableEventListeners() {

    $("#table-leave tr").click(function () {

        $(this).addClass('table-active').siblings().removeClass('table-active');
        const value = $(this).data('value');

        cart.addToCart(0, DeparturesLeave[value]); //add departure til cart
    });


    $("#table-return tr").click(function () {
        $(this).addClass('table-active').siblings().removeClass('table-active');
        const value = $(this).data('value');

        cart.addToCart(1, DeparturesReturn[value]); //add departure til cart

    });

    
}

//summary funksjonen som vil takle things videre etter at kunden vil forsette. Denne skal kun kunnes kjøres etter at departures er valgt
function Proceed() {
    if (TourType == 0) { //uansett hva som skjedde tidligere, her dersom tour type er "tur", andre elementet slettes
        cart.removeFromCart(1);
        //Cart[0] = null, dersom vi forventer at dette array skal ha 2 elementer uansett
    }
    console.log(cart.getItem(0));
    console.log(cart.getItem(1));

    //gå videre, serialize eller no.
}



