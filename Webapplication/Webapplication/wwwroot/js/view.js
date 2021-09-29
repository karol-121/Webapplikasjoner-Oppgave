let tourType; //variable thats hold value of tour type, one way or two way


//function that runs by default
$(function () {
    
    fetchRoutes();
    updateType();
});

//function that gets and populates the dropdown with routes 
function fetchRoutes() {
    const url = "API/GetRoutes";
    $.get(url, function (data) {

        let s = new String();

        for (let r of data) {
            s += "<option value='"+r.id+"'>"+r.origin+" - "+r.destination+"</option>"
        }

        $("#route").html(s);

    }).fail(function () {
        alert("error!");
    })
}

//funtcion that updates the disabled, enabled return date based on tour type
function updateType() {
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

    new DateUtilities();// oppretter objekt fra classen slik at den er defined

    const b = new DateInterval(DateUtilities.parseDate(day));
    
    //url som henter utreiser.
    const url = "API/GetDepartures?Route=" + route + "&From=" + DateUtilities.toApiDateString(b.getStartInterval()) +
        "&To=" + DateUtilities.toApiDateString(b.getEndInterval()) + "&Passengers=" + passengers;

    $.get(url, function (data) {

        const header = "Utreise " + DateUtilities.toApiDateString(b.getStartInterval()) + " - " + DateUtilities.toApiDateString(b.getEndInterval());
        $("#timetable-header").html(header);

        let tableContent = "<tr><th>dato:</th><th>pris</th><th></th></tr>";
        for (let t of data) {
            tableContent += "<tr><td>" + t.date + "</td><td>" + t.cruise.passeger_Price + "</td><td><a href='index.html'> velg</a></td</tr>";
        }
        $("#timetable").html(tableContent);

    }).fail(function () {
        alert("error!");
    });
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