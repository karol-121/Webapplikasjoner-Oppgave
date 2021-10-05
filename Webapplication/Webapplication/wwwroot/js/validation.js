//summary: objekt som holder alle validerings funksjoner relatert til registrering av ordre.
class registerValidation {

    constructor() {

    }

    //summary: funksjon med regex sjekk for navn
    //parameters: string - streng som skal valideres
    //returns: true - hvis streng har korrekt syntaks, false - hvis streng har feil syntaks
    checkName(string) {

        
        const regexp = /^[a-zA-ZÆØÅæøå]{2,25}( [a-zA-ZÆØÅæøå]{1,25}){0,1}$/;
        return regexp.test(string);

    }

    //summary: funksjon med regex sjekk for etternavn
    //parameters: string - streng som skal valideres
    //returns: true - hvis streng har korrekt syntaks, false - hvis streng har feil syntaks
    checkSurname(string) {

        const regexp = /^[a-zA-ZÆØÅæøå]{2,25}( [a-zA-ZÆØÅæøå]{1,25}){0,1}$/;
        return regexp.test(string);

    }

    //summary: funksjon med regex sjekk for alder
    //parameters: string - streng som skal valideres
    //returns: true - hvis streng har korrekt syntaks, false - hvis streng har feil syntaks
    checkAge(string) {

        const regexp = /^[0-9]{1,3}$/;
        return regexp.test(string);

    }

    //summary: funksjon med regex sjekk for adress
    //parameters: string - streng som skal valideres
    //returns: true - hvis streng har korrekt syntaks, false - hvis streng har feil syntaks
    checkAddress(string) {

        const regexp = /^([a-zA-ZÆØÅæøå]{2,20}){1}( [a-zA-ZÆØÅæøå]{2,20}){0,4}( [0-9]{0,3}){0,1}[a-zA-Z]{0,1}$/;
        return regexp.test(string);

    }

    //summary: funksjon med regex sjekk for postnummer
    //parameters: string - streng som skal valideres
    //returns: true - hvis streng har korrekt syntaks, false - hvis streng har feil syntaks
    checkZip(string) {

        const regexp = /^[0-9]{4}$/;
        return regexp.test(string);

    }

    //summary: funksjon med regex sjekk for by
    //parameters: string - streng som skal valideres
    //returns: true - hvis streng har korrekt syntaks, false - hvis streng har feil syntaks
    checkCity(string) {

        const regexp = /^[a-zA-ZÆØÅæøå]{2,25}( [a-zA-ZÆØÅæøå]{1,25}){0,1}$/;
        return regexp.test(string);

    }

    //summary: funksjon med regex sjekk for telefonnummer
    //parameters: string - streng som skal valideres
    //returns: true - hvis streng har korrekt syntaks, false - hvis streng har feil syntaks
    checkPhone(string) {

        const regexp = /^(\+\([0-9]{1,3}\)|\+[0-9]{1,3})?( ?[0-9]{1,4}){2,4}$/;
        return regexp.test(string);

    }

    //summay: funksjon med regex sjekk for epost
    //parameters: string - streng som skal valideres
    //returns: true - hvis streng har korrekt syntaks, false - hvis streng har feil syntaks
    checkEmail(string) {

        const regexp = /^[a-zA-Z0-9._\-]{2,20}@[a-zA-Z0-9._\-]{2,20}$/;
        return regexp.test(string);

    }

    //summary: funksjon med regex sjekk for antall voksen passasjerer
    //parameters: string - streng som skal valideres
    //returns: true - hvis streng har korrekt syntaks, false - hvis streng har feil syntaks
    checkAdults(string) {

        const regexp = /^[0-9]{1,2}$/;
        return regexp.test(string);

    }

    //summary: funksjon med regex sjekk for antall mindreårig passasjerer
    //parameters: string - streng som skal valideres
    //returns: true - hvis streng har korrekt syntaks, false - hvis streng har feil syntaks
    checkUnderage(string) {

        const regexp = /^[0-9]{1,2}$/;
        return regexp.test(string);

    }

    //summary: funksjon med regex sjekk for antall husdyr
    //parameters: string - streng som skal valideres
    //returns: true - hvis streng har korrekt syntaks, false - hvis streng har feil syntaks
    checkPets(string) {

        const regexp = /^[0-9]{1,2}$/;
        return regexp.test(string);

    }

    //summary: funksjon med regex sjekk for motorvogn
    //parameters: string - streng som skal valideres
    //returns: true - hvis streng har korrekt syntaks, false - hvis streng har feil syntaks
    checkVehicles(string) {

        const regexp = /^[0-9]{1,2}$/;
        return regexp.test(string);

    }
}