$(function () {
    //dette er jquery funksjonen som kjøres med en gang siden loader, holder den for å ikke glemme syntaksen
    fetchRoutes();
});

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


function show() {

    //this function shall fetch data from server and show it.
    const route = $("#route").val();
    const day = $("#date").val();
    const passengers = $("#passengers").val();

    const b = new DateInterval(DateInterval.parseDate(day));
    
    //url som henter utreiser.
    const url = "API/GetDepartures?Route=" + route + "&From=" + DateInterval.toApiDateString(b.getStartInterval()) +
        "&To=" + DateInterval.toApiDateString(b.getEndInterval()) + "&Passengers=" + passengers;

    $.get(url, function (data) {

        const header = DateInterval.toApiDateString(b.getStartInterval()) + " - " + DateInterval.toApiDateString(b.getEndInterval());
        $("#timetable-header").html(header);

        let tableContent = "<tr><th>date:</th><th>price:</th><th></th></tr>";
        for (let t of data) {
            tableContent += "<tr><td>" + t.date + "</td><td>" + t.cruise.passeger_Price + "</td><td><a href='index.html'>velg</a></td</tr>";
        }
        $("#timetable").html(tableContent);

    }).fail(function () {
        alert("error!");
    });
}


//proto function that prints table header dates from interval.
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