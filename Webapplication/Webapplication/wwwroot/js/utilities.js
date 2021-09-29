//summary: Objekt som genererer fra og til dato basert på inn dato.
//Interval skal være 7 dager lang med start dato 3 dager før inn dato og slutt dato 3 dager etter inn dato.
//dersom inn dato er tidligere enn 3 dager fra dagens dato, skal intervalen starte fra dagens dato og slutte 7 dager etter.
class DateInterval {
    #nowDate = new Date();
    #startInterval;
    #endInterval; 
    #maxInterval = 604800000; //7 dager representert i millisekunder
    #intervalOffset = 259200000; //3 dager representer i millisekunder

    constructor(requestDate) {

        this.#nowDate.setHours(0, 0, 0, 0); //nullstiller tid for nå-dato for å ekskludere det som en variabel ved videre kalkulasjoner.

        //hvis forskjellen mellom nå-dato og inn-dato er større enn 3 dager.
        if (requestDate.getTime() - this.#nowDate.getTime() > this.#intervalOffset) {

            this.#startInterval = new Date(requestDate.getTime() - this.#intervalOffset); //start-intervall = 3 dager før inn-dato
            this.#endInterval = new Date(requestDate.getTime() + this.#intervalOffset); //slutt-intervall = 3 dager etter inn-dato

        } else {

            //hvis forskjellen mellom nå-dato og inn-dato er mindre enn 3 dager.
            this.#startInterval = new Date(this.#nowDate.getTime()); //start-intervall = nå-dato
            this.#endInterval = new Date(this.#nowDate.getTime() + this.#maxInterval); //slutt-intervall = 7 dager etter nå-dato

        }
    }

    //summary: metode som returnerer beregnet start intervall dato
    //returns: Date objekt
    getStartInterval() {
        return this.#startInterval;
    }

    //summary: metode som returnerer beregnet slutt intervall dato
    //returns: Date objekt
    getEndInterval() {
        return this.#endInterval;
    }
}

//summary: Objekt som har statiske hjelpe metoder som konverterer dato objekter og strenger til ulike formater
class DateUtilities {
    static #day;
    static #month;
    static #year;

    static #hour;
    static #minute;

    constructor() {

    }

    //summary: Hjelpe metode som tar imot date objekt og tar ut dens attributer og lagrer dem som eksterne attributer slik at andre metoder kan bruke dem videre.
    static #disassembleDate(dateObject) {

        this.#day = dateObject.getDate();
        this.#month = dateObject.getMonth() + 1; //konvertering til 1-indeks månder (1 = januar)
        this.#year = dateObject.getFullYear()

        this.#hour = dateObject.getHours();
        this.#minute = dateObject.getMinutes();

        //konvertering til 2-digit dag verdi, nødvendig for formatering
        if (this.#day < 10) {
            this.#day = "0" + this.#day;
        }

        //konvertering til 2-digit måned verdi, nødvendig for formatering
        if (this.#month < 10) {
            this.#month = "0" + this.#month;
        }

    }

    //summary: metode som konverterer dato objekt til string i format "yyyy-mm-dd" som api ønsker.
    //returns: string med fomatert dato
    static toApiDateString(dateObject) {

        this.#disassembleDate(dateObject); //ta ut attributter

        return this.#year + "-" + this.#month + "-" + this.#day; //return attributer i ønskende format
    }


    //summary: metode som konverterer dato objekt til string i format "dd.mm.yyyy hh:mm" som er vanlig lokalt.
    //returns: string med formatert dato
    static toLocalDateString(dateObject) { //not needed for now at least

        this.#disassembleDate(dateObject);

        return this.#day + "." + this.#month + "." + this.#year + " " + this.#hour + ":" + this.#minute
    }

    //summary: metode som konverterer dato streng i formatt "yyyy-mm-dd" til dato objekt.
    //parameters: String dateString - dato i "yyyy-mm-dd" streng 
    //returns: Date objekt.
    static parseDate(dateString) {

        const year = Number(dateString.substring(0, 4));
        const month = Number(dateString.substring(5, 7)) - 1; //konvertering til 0-indeks månder (0 = januar)
        const day = Number(dateString.substring(8, 10));

        return new Date(year, month, day);
    }

    //summary: metode som formaterer json dato streng til lokal tid notasjon
    //parameters: String dateString - dato i json "yyyy-mm-ddThh:MM:ss" streng format
    //returns: String med dato i format "dd.mm.yyyy HH:MM"
    static convertJsonToLocalDateString(dateString) {
        //not implemented
    }

}
