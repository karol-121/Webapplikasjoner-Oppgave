//this class contains logic to determine the start and end interval based on one date.
class DateInterval {
    #x = new Date();
    #y; //start date
    #z; //end date
    #a = 604800000; //7 days represented in miliseconds
    #b = 259200000; //3 days represented in miliseconds

    constructor(y) {

        this.#x.setHours(0, 0, 0, 0); //restet time
        if (y.getTime() - this.#x.getTime() > this.#b) {

            //if difference between today and given date is more than 3 days
            this.#y = new Date(y.getTime() - this.#b); //start interval 3 days before given date
            this.#z = new Date(y.getTime() + this.#b); //end interval 3 day later given date

            console.log("difference is more than 3 days");
            

        } else {

            //if difference between today and given date is less than 3 days
            this.#y = new Date(this.#x.getTime()); //start interval on current date
            this.#z = new Date(this.#x.getTime() + this.#a); //end interval 7 days later.

            console.log("difference is less than 3 days");

        }
    }

    getStartInterval() {
        //return start date
        return this.#y;
    }

    getEndInterval() {
        //return end date
        return this.#z;
    }

    static toApiDateString(f) {
        //TODO: here is some kind of error that exist.

        //convert and return date object to date string that api likes
        let h = f.getDay();
        let g = f.getMonth() + 1;

        if (h < 10) { // if day is lover than 10, append 0 to convert it to 2 digit
            h = "0" + h;
        }

        if (g < 10) { // if month is lower than 10, append 0 to convert it to 2 digit
            g = "0" + g;
        }

        return f.getFullYear() + "-" + g + "-" + h;
    }

    //function that should convert date in string format yyyy-mm-dd and return an date object
    static parseDate(a) {
        //maybe regex check the a parameter, it should be a string.

        const year = Number(a.substring(0, 4));
        const month = Number(a.substring(5, 7)) -1;
        const day = Number(a.substring(8, 10));

        return new Date(year, month, day);
    }
}
