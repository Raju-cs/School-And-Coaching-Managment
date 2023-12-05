(function () {
    const controller = 'CoursePayment';

    function paymentDate(td) {
        td.html(new Date(this.PaymentDate).toLocaleDateString('en-US', {
            day: "2-digit",
            month: "short",
            year: "numeric"
        }));
    }

    function studentInfo(row) {
        console.log("row=>", row);
        Global.Add({
            Id: row.StudentId,
            url: '/js/student-area/student-details-modal.js',
        });
    }

    const coursePaymentHistoryTab = {
        Id: 'BBC23DC6-A099-494D-BEB4-E8B98993A27D',
        Name: 'COURSE_PAYMENT_HISTORY',
        Title: 'Course Payment History',
        filter: [],
        onDataBinding: () => { },
        columns: [
            { field: 'DreamersId', title: 'DreamersId', filter: true, position: 2 },
            { field: 'StudentName', title: 'StudentName', filter: true, position: 3, Click: studentInfo },
            { field: 'CourseName', title: 'CourseName', filter: true, position: 3, Click: studentInfo },
            { field: 'Paid', title: 'Paid', filter: true, position: 6 },
            { field: 'PaymentDate', title: 'PaymentDate', filter: true, add: { sibling: 2, }, position: 6, dateFormat: 'dd/MM/yyyy', bound: paymentDate },
            { field: 'CreatedAt', dateFormat: 'dd/MM/yyyy hh:mm', title: 'Creation Date' },
        ],
        Printable: { container: $('void') },
        //remove: { save: `/${controller}/Remove` },
        Url: 'Get',
    }

    //Tabs config
    const tabs = {
        container: $('#page_container'),
        Base: {
            Url: `/${controller}/`,
        },
        items: [coursePaymentHistoryTab],
        periodic: {
            container: '.filter_container',
            type: 'ThisMonth',
        },
    };

    //Initialize Tabs
    Global.Tabs(tabs);
    tabs.items[0].set(tabs.items[0]);

})();