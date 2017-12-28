$(function () {

    var start = moment();
    var end = moment();

    function cb(start, end) {
        
        var startDateFromLocalStorage = localStorage.getItem("StartDate");
        var endDateFromLocalStorage = localStorage.getItem("EndDate");
        
        if (startDateFromLocalStorage !== null &&
            endDateFromLocalStorage !== null &&
            startDateFromLocalStorage !== 'undefined' &&
            endDateFromLocalStorage !== 'undefined') {
            start = moment(startDateFromLocalStorage);
            end = moment(endDateFromLocalStorage);
        }
         $('#daterange span').html(start.format('MMMM D, YYYY') + ' - ' + end.format('MMMM D, YYYY'));
           
       
    }
    
    $('#daterange').daterangepicker({
        startDate: start,
        endDate: end,
        ranges: {
            'Today': [moment(), moment()],
            'Yesterday': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
            'Last 7 Days': [moment().subtract(6, 'days'), moment()],
            'Last 30 Days': [moment().subtract(29, 'days'), moment()],
            'This Month': [moment().startOf('month'), moment().endOf('month')],
            'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
        }
    }, cb);

    cb(start, end);

    $('#daterange').on('apply.daterangepicker', function (ev, picker) {
        
        var startDate = picker.startDate.format('YYYY-MM-DD');
        var endDate = picker.endDate.format('YYYY-MM-DD');
        
        
        localStorage.setItem("StartDate", startDate);        
        localStorage.setItem("EndDate", endDate);

        window.location = '/Dashboard/Index' + '?strtDate=' + startDate + '&edDate='+endDate;
       
    });
});