

const s = '2021-09-13 00:04:00';
const t = '2021-09-12 00:08:00';

const d = new Date(s); //cruise date
const e = new Date(t); //order date

const OrderInformation = {
    Name: "Jenkins",
    Surname: "Leroy",
    Age: 42,
    Address: "Chesse 324",
    Zip_Code: "1224",
    City: "Bergen",
    Phone: "12345678",
    Email: "Jenkins@mail.com",
    Order_Date: e,
    Cruise_Id: 5,
    Cruise_Date: d,
    Passengers: 1,
    Passenger_Underage: 0,
    Pets: 1,
    Vehicles: 0
}


$(function () {
    
    const url = "API/RegisterOrder";
    $.post(url, OrderInformation);
});



