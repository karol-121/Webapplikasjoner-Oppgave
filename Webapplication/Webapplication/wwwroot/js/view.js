function show() {

    //this function shall fetch data from server and show it.
    const route = $("#route").val();
    const day = $("#date").val();
    const passengers = $("#passengers").val();

    const b = new DateInterval(DateInterval.parseDate(day));

    a(b.getStartInterval());
    

    const url = "API/GetDepartures?Route=" + route + "&From=" + DateInterval.toApiDateString(b.getStartInterval()) + "&To=" + DateInterval.toApiDateString(b.getEndInterval()) + "&Passengers=" + passengers;
    $.get(url, function (data) {
        console.log("boi");
    }).fail(function () {
        alert("error!")
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