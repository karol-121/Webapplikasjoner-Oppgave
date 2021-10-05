//summary: objekt som holder alle validerings funksjoner relatert til registrering av ordre.
class registerValidation {
    #isValid;

    constructor() {
        this.#isValid = true;
    }

    //summary: registrerer validity for hver element og markeres relevant dom element som feil
    //argument: valid - boolean som forteller om element er valid eller ikke, jquerySelector - referanse til dom element som sjekkes 
    #registerValidity(valid, jquerySelector) {
        if (!valid) {
            this.#isValid = false;
            jquerySelector.addClass('is-invalid');
        }
    }

    //summary: funksjon med regex sjekk for navn
    //parameters: jquerySelector - dom element som verdien til skal valideres
    checkName(jquerySelector) {
 
        const regexp = /^[a-zA-ZÆØÅæøå]{2,25}( [a-zA-ZÆØÅæøå]{1,25}){0,1}$/;
        const result = regexp.test(jquerySelector.val());
        this.#registerValidity(result, jquerySelector);


    }

    //summary: funksjon med regex sjekk for etternavn
    //parameters: jquerySelector - dom element som verdien til skal valideres
    checkSurname(jquerySelector) {

        const regexp = /^[a-zA-ZÆØÅæøå]{2,25}( [a-zA-ZÆØÅæøå]{1,25}){0,1}$/;
        const result = regexp.test(jquerySelector.val());
        this.#registerValidity(result, jquerySelector)

    }

    //summary: funksjon med regex sjekk for alder
    //parameters: jquerySelector - dom element som verdien til skal valideres
    checkAge(jquerySelector) {

        const regexp = /^[0-9]{1,3}$/;
        const result = regexp.test(jquerySelector.val());
        this.#registerValidity(result, jquerySelector)

    }

    //summary: funksjon med regex sjekk for adress
    //parameters: jquerySelector - dom element som verdien til skal valideres
    checkAddress(jquerySelector) {

        const regexp = /^([a-zA-ZÆØÅæøå]{2,20}){1}( [a-zA-ZÆØÅæøå]{2,20}){0,4}( [0-9]{0,3}){0,1}[a-zA-Z]{0,1}$/;
        const result = regexp.test(jquerySelector.val());
        this.#registerValidity(result, jquerySelector)

    }

    //summary: funksjon med regex sjekk for postnummer
    //parameters: jquerySelector - dom element som verdien til skal valideres
    checkZip(jquerySelector) {

        const regexp = /^[0-9]{4}$/;
        const result = regexp.test(jquerySelector.val());
        this.#registerValidity(result, jquerySelector)

    }

    //summary: funksjon med regex sjekk for by
    //parameters: jquerySelector - dom element som verdien til skal valideres
    checkCity(jquerySelector) {

        const regexp = /^[a-zA-ZÆØÅæøå]{2,25}( [a-zA-ZÆØÅæøå]{1,25}){0,1}$/;
        const result = regexp.test(jquerySelector.val());
        this.#registerValidity(result, jquerySelector)

    }

    //summary: funksjon med regex sjekk for telefonnummer
    //parameters: jquerySelector - dom element som verdien til skal valideres
    checkPhone(jquerySelector) {

        const regexp = /^(\+\([0-9]{1,3}\)|\+[0-9]{1,3})?( ?[0-9]{1,4}){2,4}$/;
        const result = regexp.test(jquerySelector.val());
        this.#registerValidity(result, jquerySelector)

    }

    //summay: funksjon med regex sjekk for epost
    //parameters: jquerySelector - dom element som verdien til skal valideres
    checkEmail(jquerySelector) {

        const regexp = /^[a-zA-Z0-9._\-]{2,20}@[a-zA-Z0-9._\-]{2,20}$/;
        const result = regexp.test(jquerySelector.val());
        this.#registerValidity(result, jquerySelector)

    }

    //summary: hjelpe metode som sjekker enkel tall, 0-99
    //parameters: jquerySelector - dom element som verdien til skal valideres
    #checkSimpleNumber(jquerySelector) {

        const regexp = /^[0-9]{1,2}$/;
        const result = regexp.test(jquerySelector.val());
        this.#registerValidity(result, jquerySelector)
    }

    //summary: funksjon med regex sjekk for antall voksen passasjerer
    //parameters: jquerySelector - dom element som verdien til skal valideres
    checkAdults(jquerySelector) {

        this.#checkSimpleNumber(jquerySelector);

    }

    //summary: funksjon med regex sjekk for antall mindreårig passasjerer
    //parameters: jquerySelector - dom element som verdien til skal valideres
    checkUnderage(jquerySelector) {

        this.#checkSimpleNumber(jquerySelector);

    }

    //summary: funksjon med regex sjekk for antall husdyr
    //parameters: jquerySelector - dom element som verdien til skal valideres
    checkPets(jquerySelector) {

        this.#checkSimpleNumber(jquerySelector);

    }

    //summary: funksjon med regex sjekk for motorvogn
    //parameters: jquerySelector - dom element som verdien til skal valideres
    checkVehicles(jquerySelector) {

        this.#checkSimpleNumber(jquerySelector);

    }

    //summary: returnerer validity flag som forteller om en eller flere sjekk har feilet
    //returns: true - hvis alle gjennomførte sjekk er valide, false - hvis en eller flere sjekk har feilet.
    isValid() {
        return this.#isValid;
    }
}