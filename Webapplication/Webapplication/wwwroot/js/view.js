function show() {

    //this function shall fetch data from server and show it.
    const route = $("#route").val();
    const day = $("#date").val();
    const passengers = $("#passengers").val();

    const b = new DateInterval(DateInterval.parseDate(day));

    console.log(route);
    console.log(b.getStartInterval());
    console.log(b.getEndInterval());
    console.log(passengers);

    const url = "API/GetDepartures?Route=" + route + "&From=" + DateInterval.toApiDateString(b.getStartInterval()) + "&To=" + DateInterval.toApiDateString(b.getEndInterval()) + "&Passengers=" + passengers;
    $.get(url, function (data) {
        console.log("boi");
    }).fail(function () {
        alert("error!")
    });
}
