//global attributes
let tourType; //variable thats hold value of tour type, one way or two way
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
    tourType = $("#type").val();

    if (tourType == 0) {
        $("#date2").prop('disabled', true);

    } else {
        $("#date2").prop('disabled', false);

    }
    
}


//function that shows all departures 
function show() {

    const route = $("#route").val();
    const day = $("#date").val();
    const passengers = $("#passengers").val();

    //input validering klient skal gaa her i guess

    const dateInterval = new DateInterval(DateUtilities.parseDate(day));

    fetchDepartures(route, dateInterval, passengers);

    
}

//summary: lager spørring og henter data fra serveren
//parameters: route - id til route, from - Date objekt fra dato, to - Date objekt til dato, passengers - antall personer
//returns: json data, null ved feil/mangel
function fetchDepartures(route, dateInterval, passengers) {

    const from = dateInterval.getStartInterval();
    const to = dateInterval.getEndInterval();

    //url for api call
    const url = "API/GetDepartures?Route=" + route + "&From=" + DateUtilities.toApiDateString(from) +
        "&To=" + DateUtilities.toApiDateString(to) + "&Passengers=" + passengers;

    $.get(url, function (data) {
        console.log("departure fetch ok");

        displayData(data, dateInterval);

    }).fail(function () {
        console.log("departure fetch failed!");
        //her skal det vises noe alert tingy.

    });

}

function displayData(Departures, dateInterval) {

    const header = "Utreise " + DateUtilities.toApiDateString(dateInterval.getStartInterval()) + " - " + DateUtilities.toApiDateString(dateInterval.getEndInterval());
    $("#timetable-header").html(header);

    let tableContent = "<tr><th>dato:</th><th>pris</th><th></th></tr>";
    for (let t of Departures) {
        tableContent += "<tr><td>" + t.date + "</td><td>" + t.cruise.passeger_Price + "</td><td><a href='index.html'> velg</a></td</tr>";
    }
    $("#timetable").html(tableContent);

}





//proto function that prints table header dates from interval, probably will not be used
function a(startdate) {
    let c = new String();
    let b = startdate;

    for (let i = 0; i < 7; i++) {
        let d = DateInterval.toApiDateString(b);
        c += "<th>" + d + "</th>";
        b = new Date (b.getTime() + 86400000);
    }

    $("#timetable-head").html(c);

}