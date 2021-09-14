

const OrderInformation = {
    Name: "Jenkins",
    Surname: "Leroy",
    Age: 42,
    Address: "Chesse 324",
    Zip_Code: "1224",
    City: "Bergen",
    Phone: "12345678",
    Email: "Jenkins@mail.com",
    Cruise_Id: 5,
    Cruise_Date: "2021-09-13",
    Passengers: 1,
    Passenger_Underage: 0,
    Pets: 1,
    Vehicles: 0
}


$(function () {
    const url = "API/RegisterOrder";
    $.post(url, OrderInformation);
});



