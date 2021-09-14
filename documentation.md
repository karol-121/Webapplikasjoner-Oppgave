# Dokumentasjon

## Api:
*Controller rotue: "API/[action]" f.eks: localhost:123456/API/GetRoutes*  

- List<Route> **GetRoutes**() - *henter liste med alle ruter som finnes i databasen*

- List<Cruise> **FindCruises**(int RouteId, int PassengerAmount, int Year, int Month, int Day) - *henter liste med cruiser som går på spesifisert rute, på spesifisert dato og har plass til spesifisert antall personer*

- Void **RegisterOrder**(OrderInformation OrderInformation) - *Registrerer ordre med data sendt inn i OrderInformation objektet*

