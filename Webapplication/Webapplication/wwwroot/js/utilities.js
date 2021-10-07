//summary: Objekt som genererer fra og til dato basert på inn dato.
//Interval skal være 7 dager lang med start dato 3 dager før inn dato og slutt dato 3 dager etter inn dato.
//dersom inn dato er tidligere enn 3 dager fra dagens dato, skal intervalen starte fra dagens dato og slutte 7 dager etter.
class DateInterval {
    #nowDate = new Date();
    #requestDate;
    #startInterval;
    #endInterval; 
    #maxInterval = 604800000; //7 dager representert i millisekunder
    #intervalOffset = 259200000; //3 dager representer i millisekunder

    constructor(requestDate) {

        this.#nowDate.setHours(0, 0, 0, 0); //nullstiller tid for nå-dato for å ekskludere det som en variabel ved videre kalkulasjoner.

        this.#requestDate = requestDate; //lagrer requested date

        //hvis forskjellen mellom nå-dato og inn-dato er større enn 3 dager.
        if (this.#requestDate.getTime() - this.#nowDate.getTime() > this.#intervalOffset) {

            this.#startInterval = new Date(this.#requestDate.getTime() - this.#intervalOffset); //start-intervall = 3 dager før inn-dato
            this.#endInterval = new Date(this.#requestDate.getTime() + this.#intervalOffset); //slutt-intervall = 3 dager etter inn-dato

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

    //summary: metode som returnerer primær dato
    //returns: Date objekt
    getRequestedDate() {
        return this.#requestDate;
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

    //summary: hjelpe metode som tar dag, måned og år verdi ut av et date objekt
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

        return new Date(year, month, day); //returner et date objekt
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

//summary: Objekt som representerer et abstrakt versjon av shopping cart. 
//Hovedsaklig er det vanlig array men i tilegg er det mulig å koble på funksjon som vil kjøres etter at det er endring i kurven
class Cart {
    #cart = [];
    #subscriber;

    constructor(subscriber) {
        this.#subscriber = subscriber; //lagre referanse til subscriber funksjon
    }

    //summary: legger til et objekt på spesifikt plass
    //parameters: index - plass hvor objektet skal legges på, obj - objektet som skal legges inn.
    addToCart(index, obj) {
            
        this.#cart[index] = obj;
        this.#subscriber();
    }

    //summary: fjerner objekt på spesifik plass
    //parameters: index - plass på hvilken et objekt skal fjernes
    removeFromCart(index) {

        this.#cart.splice(index, 1);
        this.#subscriber();
    }

    //summary: nullstiller carten
    emptyCart() {

        this.#cart = [];
        this.#subscriber();
    }

    //summary: returnerer objekt på spesifik plass
    //prameters: index - plass til objektet som skal returneres
    //reutrns: objekt 
    getItem(index) {

        return this.#cart[index];
    }

    //summary: returnerer antall elementer som finnes i carten.
    //reutrns: integer med antall elementer 
    getItemCount() {

        let count = 0;
        for (let item of this.#cart) {
            if (item != null) {
                count++
            }
        }

        return count;
    }

    //summary: returnerer elementer som finnes i carten.
    //reutrns: array med objekter
    getItems() {
        return this.#cart;
    }

}

//summary: Funksjon som printer et bootstrap alert av valgt type og meldig til et bestemt plass. 
//Denne funskjonen appender til destinasjon som betyr at alertene vil stacke seg
//parameters: destination - jquery dom selector som definerer hvor meldingen skal printes, type - type av feilmelding (warning, danger osv.), melding - hva alerten skal si.
function BootstrapAlert(destination, type, message) {

    let s = '<div class="alert alert-' + type + ' alert-dismissible fade show" role="alert">'
        + message + '<button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button></div>';

    destination.append(s);
}

// Autofylling av city etter aa ha skrevet inn zipcode
function AutofillCity(jquerySelectorZip, jquerySelectorCity) {
    let x = jquerySelectorZip.val();

    x = x.slice(0, 2);


    // to forste tallene utgir hvilken city
    if (x >= '00' && x <= '12') {

        jquerySelectorCity.val('Oslo');
    }
    else if (x >= '13' && x <= '15') {

        jquerySelectorCity.val('Akershus');
    }
    else if (x >= '16' && x <= '18') {

        jquerySelectorCity.val('Østfold');
    }
    else if (x >= '19' && x <= '21') {

        jquerySelectorCity.val('Akershus');
    }
    else if (x >= '22' && x <= '26') {

        jquerySelectorCity.val('Hedmark');
    }
    else if (x >= '27' && x <= '29') {

        jquerySelectorCity.val('Oppland');
    }
    else if (x >= '30' && x <= '30') {

        jquerySelectorCity.val('Buskerud');
    }
    else if (x >= '31' && x <= '32') {

        jquerySelectorCity.val('Vestfold');
    }
    else if (x == '35' && x <= '35') {

        jquerySelectorCity.val('Oppland');
    }
    else if (x >= '33' && x <= '36') {

        jquerySelectorCity.val('Buskerud');
    }
    else if (x >= '36' && x <= '39') {

        jquerySelectorCity.val('Telemark');
    }
    else if (x >= '40' && x <= '44') {

        jquerySelectorCity.val('Rogaland');
    }
    else if (x >= '45' && x <= '47') {

        jquerySelectorCity.val('Vest-Agder');
    }
    else if (x >= '48' && x <= '49') {

        jquerySelectorCity.val('Aust-Agder');
    }
    else if (x >= '50' && x <= '59') {

        jquerySelectorCity.val('Hordaland');
    }
    else if (x == '55') {

        jquerySelectorCity.val('Rogaland');
    }
    else if (x == '57' && x == '59') {

        jquerySelectorCity.val('Sogn og Fjordane');
    }
    else if (x >= '60' && x <= '66') {

        jquerySelectorCity.val('Møre og Romsdal');
    }
    else if (x >= '67' && x <= '69') {

        jquerySelectorCity.val('Sogn og Fjordane');
    }
    else if (x >= '70' && x <= '75') {

        jquerySelectorCity.val('Sør-Trøndelag');
    }
    else if (x == '76') {

        jquerySelectorCity.val('Nord-Trøndelag');
    }
    else if (x == '77') {

        jquerySelectorCity.val('Sør-Trøndelag');
    }
    else if (x >= '78' && x <= '79') {

        jquerySelectorCity.val('Nord-Trøndelag');
    }
    else if (x >= '80' && x <= '89') {

        jquerySelectorCity.val('Nordaland');
    }
    else if (x == '84' && x >= '90' && x <= '94') {

        jquerySelectorCity.val('Troms');
    }

    else if (x == '91' && x >= '95' && x <= '99') {

        jquerySelectorCity.val('Finmark');
    }

    else {
        jquerySelectorCity.val('');
    }
}