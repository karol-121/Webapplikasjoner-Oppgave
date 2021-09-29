//global attributes
let TourType; //variable thats hold value of tour type, one way or two way
let Routes; //denne skal holde array med departures.
let DeparturesLeave; //holder utreiser for en vei eller tur
let DeparturesReturn; //holder utreiser for tilbake tur

//todo: it would be nice if all html dom objects would be defined here as variables, 
//but this has to happend somewhere later in code or something, othervise they are just empty useless objects

new DateUtilities();// oppretter objekt fra classen slik at den er defined

//summary: autostart funksjon som kaller på nødvendige funksjoner
$(function () {
    fetchRoutes();
    updateTourType();
});

//summary: funksjon som henter og populerer dropdown meny for strekninger
function fetchRoutes() {

    const url = "API/GetRoutes";

    $.get(url, function (routes) {

        console.log("route fetch ok")

        let string = new String();

        Routes = routes;

        for (let route of routes) {
            string += "<option value='"+route.id+"'>"+route.origin+" - "+route.destination+"</option>"
        }

        $("#route").html(string);

    }).fail(function () {

        console.log("route fetch failed!");

    })
}

//summary: funksjon som oppdaterer verdier til tur type og styrer avhenge input elemeter
function updateTourType() {
    TourType = $("#tourType").val(); //update the global variable with current state

    if (TourType == 0) {
        $("#dateReturn").prop('disabled', true);

    } else {
        $("#dateReturn").prop('disabled', false);

    }
    
}


//function that shows all departures 
function show() {
    //this basicly needs refactoring

    const route = $("#route").val();
    const routeReverse = route //here the reverse route id should be get from Routes 
    const dateLeave = $("#dateLeave").val();
    const dateReturn = $("#dateReturn").val();
    const passengers = $("#passengers").val();

    //input validation goes here i guess, just do not verify the date return and route reverse if one way is choosen
    //question is if the date should be validated, as currently it will default to todays date anyway.
    //could be also nice if route reverse if wrong, do not allow for two way orders

    const dateIntervalLeave = new DateInterval(DateUtilities.inputToDateObject(dateLeave));
    const dateIntervalReturn = new DateInterval(DateUtilities.inputToDateObject(dateReturn));

    DeparturesLeave = fetchDepartures(route, dateIntervalLeave, passengers);


    if (TourType == 1) {
        //if both tours are choosed, fetch also the returns
        DeparturesReturn = fetchDepartures(routeReverse, dateIntervalReturn, passengers);
    }

}

//summary: lager spørring og henter data fra serveren
//parameters: route - id til route, from - Date objekt fra dato, to - Date objekt til dato, passengers - antall personer
function fetchDepartures(route, dateInterval, passengers) {

    const from = dateInterval.getStartInterval();
    const to = dateInterval.getEndInterval();

    //url for api call
    const url = "API/GetDepartures?Route=" + route + "&From=" + DateUtilities.toApiDateString(from) +
        "&To=" + DateUtilities.toApiDateString(to) + "&Passengers=" + passengers;

    $.get(url, function (data) {
        console.log("departure fetch ok");

        return data;

    }).fail(function () {
        console.log("departure fetch failed!");
        return null;
        //her skal det vises noe alert tingy.

    });

}

function displayData(Departures, dateInterval) {
    //i do like it to populate both tables where it is possible

    const header = "Utreise (" + DateUtilities.toLocalDateString(dateInterval.getStartInterval()) + " - " + DateUtilities.toLocalDateString(dateInterval.getEndInterval()) + ")";
    $("#timetable-header").html(header);

    let tableContent = "<tr><th>dato:</th><th>pris</th><th></th></tr>";
    for (let t of Departures) {
        tableContent += "<tr><td>" + DateUtilities.isoToLocalDateString(t.date) + "</td><td>" + t.cruise.passeger_Price + "</td><td><a href='index.html'> velg</a></td</tr>";
    }
    $("#timetable").html(tableContent);

}