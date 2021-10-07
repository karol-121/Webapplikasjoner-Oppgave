

function registerOrder() {

    const OrderInformation = {
        Name: $("#fname").val(),
        Surname: $("#surname").val(),
        Age: $("#age").val(),
        Address: $("#address").val(),
        Zip_Code: $("#zipcode").val(),
        City: $("#city").val(),
        Phone: $("#phone").val(),
        Email: $("#email").val(),
        Departure_Id: $("#dep").val(),
        Passengers: $("#adults").val(),
        Passengers_Underage: $("#children").val(),
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










