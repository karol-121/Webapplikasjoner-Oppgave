

const OrderInformation = {
    Name: "Jenkins",
    Surname: "Leroy",
    Age: 42,
    Address: "Chesse 324",
    Zip_Code: "1224",
    City: "Bergen",
    Phone: "12345678",
    Email: "Jenkins@mail.com",
    Departure_Id: 3,
    Passengers: 1,
    Passengers_Underage: 1,
    Pets: 1,
    Vehicles: 0
}


$(function () {
    const url = "API/RegisterOrder";
    $.post(url, OrderInformation);
});



