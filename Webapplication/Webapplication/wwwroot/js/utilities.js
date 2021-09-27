//this class contains logic to determine the start and end interval based on one date.
class DateInterval {
    #x = new Date();
    #y;

    constructor(y) {
        //here: do all the fancy math that will define the from and to date
        this.#y = y;
    }

    getStartInterval() {
        //here only return start date 
        return this.#y + 1;
    }

    getEndInterval() {
        //here only return end date 
        return this.#y - 1;
    }
}
