﻿//summary: Objekt som genererer fra og til dato basert på inn dato.
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

    //konstruktor slik som det er forventet at class skal ha.
    constructor() {

    }

    //summary: metode som tar dag, måned og år verdi ut av et date objekt
    //parameters: Date dateObject - dato objekt hvorfra verdiene skal tas ut
    //returs: array med dag, måned og år verdi 
    static #disassembleDateObject(dateObject) {

        let day = dateObject.getDate();
        let month = dateObject.getMonth() + 1; //konvertering til 1-indeks månder (1 = januar)
        const year = dateObject.getFullYear()

        //konvertering til 2-digit dag verdi, nødvendig for formatering
        if (day < 10) {
            day = "0" + day;
        }

        //konvertering til 2-digit måned verdi, nødvendig for formatering
        if (month < 10) {
            month = "0" + month;
        }

        return [day, month, year];
    }

    //summary: metode som konverterer dato objekt til string i format "yyyy-mm-dd" som api ønsker.
    //returns: string med fomatert dato
    static toApiDateString(dateObject) {

        const date = this.#disassembleDateObject(dateObject);

        return date[2] + "-" + date[1] + "-" + date[0]; //return attributer i ønskende format
    }

    //summary: metode som konverterer dato objekt til string i format "dd.mm.yyyy" som er vanlig lokal tid notasjon.
    //returns: string med fomatert dato
    static toLocalDateString(dateObject) {

        const date = this.#disassembleDateObject(dateObject);

        return date[0] + "." + date[1] + "." + date[2] //return attributer i ønskende format
    }


    //summary: metode som konverterer dato streng i formatt "yyyy-mm-dd" til dato objekt.
    //parameters: String dateString - dato i "yyyy-mm-dd" streng 
    //returns: Date objekt.
    static inputToDateObject(dateString) {

        const year = Number(dateString.substring(0, 4));
        const month = Number(dateString.substring(5, 7)) - 1; //konvertering til 0-indeks månder (0 = januar)
        const day = Number(dateString.substring(8, 10));

        return new Date(year, month, day);
    }

    //summary: metode som formaterer json dato streng til lokal tid notasjon
    //parameters: String dateString - dato i json "yyyy-mm-ddThh:MM:ss" streng format
    //returns: String med dato i format "dd.mm.yyyy HH:MM"
    static isoToLocalDateString(dateString) {

        const day = dateString.substring(8, 10);
        const month = dateString.substring(5, 7);
        const year = dateString.substring(0, 4);
        const hour = dateString.substring(11, 13);
        const minute = dateString.substring(14, 16);

        return day + "." + month + "." + year + " " + hour + ":" + minute;
    }

}
