$(function () {
    //dette er jquery funksjonen som kjøres med en gang siden loader, holder den for å ikke glemme syntaksen
});


function registerOrder() {

    const OrderInformation = {
        Name: $("#name").val(),
        Surname: $("#surname").val(),
        Age: $("#age").val(),
        Address: $("#address").val(),
        Zip_Code: $("#zip").val(),
        City: $("#city").val(),
        Phone: $("#phone").val(),
        Email: $("#email").val(),
        Departure_Id: $("#dep").val(),
        Passengers: $("#adults").val(),
        Passengers_Underage: $("#underage").val(),
        Pets: $("#pets").val(),
        Vehicles: $("#vehicles").val()
    }

    const url = "API/RegisterOrder";
    $.post(url, OrderInformation).done(function () {
        alert("nice one!")
    }).fail(function () {
        alert("error!")
    });

}








