// Total price dynamic implementation start
let pricePerDayElement = document.getElementById('pricePerDay');
let customersCountElement = document.getElementById('customersCount');
let startDateElement = document.getElementById('From');
let endDateElement = document.getElementById('To');
let totalPriceElement = document.getElementById('totalPrice');

if (pricePerDayElement && customersCountElement && startDateElement && endDateElement && totalPriceElement) {

    function calculateTotalPrice() {
        let pricePerDay = parseFloat(pricePerDayElement.value);
        let customersCount = parseFloat(customersCountElement.value);
        let startDate = new Date(startDateElement.value);
        let endDate = new Date(endDateElement.value);

        let differenceInTime = endDate.getTime() - startDate.getTime();
        let differenceInDays = differenceInTime / (1000 * 3600 * 24) + 1;

        let totalPrice = (pricePerDay * customersCount * differenceInDays).toFixed(2);

        if (isNaN(pricePerDay) || isNaN(customersCount) || isNaN(differenceInDays) || differenceInDays < 0 || Number(totalPrice) < 0) {
            totalPriceElement.innerHTML = '0.00 BGN';
        } else {
            totalPriceElement.innerHTML = totalPrice + ' BGN';
        }
    }

    // Calculate total price initially (in case the fields are pre-filled)
    calculateTotalPrice();

    // Recalculate every time the inputs change
    pricePerDayElement.addEventListener('input', calculateTotalPrice);
    customersCountElement.addEventListener('input', calculateTotalPrice);
    startDateElement.addEventListener('input', calculateTotalPrice);
    endDateElement.addEventListener('input', calculateTotalPrice);
    // Total price dynamic implementation end



    //Promocode dynamic message implementation start
    document.getElementById('promocode').addEventListener('input', function () {
        if (this.value) {
            document.getElementById('message').style.display = 'block';
        } else {
            document.getElementById('message').style.display = 'none';
        }
    });
    //Promocode dynamic message implementation end


    //Calendar end date automatically change start
    document.addEventListener('DOMContentLoaded', function () {
        document.getElementById('From').addEventListener('change', function () {
            var fromDate = this.value;
            document.getElementById('To').value = fromDate;
        });
    });
    //Calendar end date automatically change end


}
