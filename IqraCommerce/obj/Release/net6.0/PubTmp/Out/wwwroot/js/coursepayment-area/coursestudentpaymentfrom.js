export const print = (model) => {
    console.log('model=>', model);

    Global.CallServer('/Course/GetCoursePayment?studentId=' + model.StudentId, function (response) {
        if (!response.IsError) {

            console.log(['', response.Data]);

            const windowToPrint = window.open("", "PRINT", "height=800,width=1200");

            windowToPrint.document.write(`<!DOCTYPE html><html lang="en">`);

            renderHTML(windowToPrint, model, response.Data);

            windowToPrint.document.write(`</body></html>`);
            windowToPrint.document.close();
            windowToPrint.focus();


        } else {

        }
    }, function (response) {
        alert('Network error had occured.');

    }, {}, 'POST');
};

const renderHTML = (windowToPrint, report, list) => {

    // date format
    var datetime = new Date().toLocaleDateString('en-US', {
        day: "2-digit",
        month: "short",
        year: "numeric"
    });

    // date format
    var date = new Date();

    var monthNames = ["January", "February", "March", "April", "May", "June",
        "July", "August", "September", "October", "November", "December"
    ];

    var currentMonth = monthNames[date.getMonth()];

    // Number to words convert
    var charge = report.Paid.toFixed(0);

    var oneToTwenty = ['', 'one ', 'two ', 'three ', 'four ', 'five ', 'six ', 'seven ', 'eight ', 'nine ', 'ten ',
        'eleven ', 'twelve ', 'thirteen ', 'fourteen ', 'fifteen ', 'sixteen ', 'seventeen ', 'eighteen ', 'nineteen '];
    var tenth = ['', '', 'twenty', 'thirty', 'forty', 'fifty', 'sixty', 'seventy', 'eighty', 'ninety'];

    if (charge.toString().length > 7) return 'overlimit';

    //let num = ('0000000000'+ numberInput).slice(-10).match(/^(\d{1})(\d{2})(\d{2})(\d{2})(\d{1})(\d{2})$/);
    let num = ('0000000' + charge).slice(-7).match(/^(\d{1})(\d{1})(\d{2})(\d{1})(\d{2})$/);

    if (!num) return;

    let outputText = num[1] != 0 ? (oneToTwenty[Number(num[1])] || `${tenth[num[1][0]]} ${oneToTwenty[num[1][1]]}`) + ' million ' : '';

    outputText += num[2] != 0 ? (oneToTwenty[Number(num[2])] || `${tenth[num[2][0]]} ${oneToTwenty[num[2][1]]}`) + 'hundred ' : '';
    outputText += num[3] != 0 ? (oneToTwenty[Number(num[3])] || `${tenth[num[3][0]]} ${oneToTwenty[num[3][1]]}`) + ' thousand ' : '';
    outputText += num[4] != 0 ? (oneToTwenty[Number(num[4])] || `${tenth[num[4][0]]} ${oneToTwenty[num[4][1]]}`) + 'hundred ' : '';
    outputText += num[5] != 0 ? (oneToTwenty[Number(num[5])] || `${tenth[num[5][0]]} ${oneToTwenty[num[5][1]]} `) : '';



    // <head></head>
    renderPageHeader(windowToPrint);

    // <body>
    renderOpeningBodyTab(windowToPrint);

    // <header></header>
    renderReportHeader(windowToPrint, report);

    renderReportDateAndSerial(windowToPrint, report, datetime);

    renderPersonalInfo(windowToPrint, report, outputText, list, currentMonth);

    renderSignatories(windowToPrint, report);

    renderClosingTags(windowToPrint);
};

const renderPageHeader = (windowToPrint) => {
    windowToPrint.document.write(`
    <head>
        <meta charset="UTF-8" />
        <meta http-equiv="X-UA-Compatible" content="IE=edge" />
        <meta name="viewport" content="width=device-width, initial-scale=1.0" />
         <link href="https://fonts.googleapis.com/css2?family=Poppins:wght@300;400;500;600;700;800;900&display=swap" rel="stylesheet" />
        <title>${'Student Form'}</title>
        <style>
            body {
    margin: 0;
    padding: 0;
    font-size: 16px;
    -webkit-box-sizing: border-box;
    box-sizing: border-box;
    font-family: "Poppins", sans-serif;
}
h1,
h2,
h3,
h4,
h5,
h6,
p {
    margin: 0;
    padding: 0;
}

.container {
    max-width: 620px;
    width: 100%;
    margin: 0 auto;
    border: 1px solid #ddd;
    margin-top: 10px;
    margin-bottom: 10px;
    position: relative;
    z-index: 1;
    box-sizing: border-box;
    overflow: hidden;
}

/*Start money receipt css*/

.header-left > h1 {
    font-size: 13px;
    font-weight: 400;
    color: #000;
    font-style: italic;
    margin-left: 5px;
}

.header-area {
    display: flex;
    justify-content: space-between;
    align-items: center;
    border-bottom: 1px solid #eedada;
    padding-bottom: 7px;
    background: lightgreen;
}


.header-left img {
    width: 150px;
    height: auto;
}

.inner-right > p {
    font-size: 12px;
    font-weight: 400;
    color: #000;
}

.inner-right > span {
    padding-left: 0;
    font-weight: 600;
    color: #000;
    font-size: 14px;
}


.phone.d-flex {
    flex: 0 0 41%;
}
.phone.d-flex span {
    border: 1px solid #9a8484;
    flex: 0 0 62px;
    margin-left: 4px;
    /*height: 21px;*/
    line-height: 21px;
    border-radius: 3px;
    margin-top: 1px;
}

.cmn-text2 span {
    border: 1px solid #000;

    border-radius: 3px;
}

.cmn-text2 {
    font-size: 13px;
    font-weight: 500;
    line-height: 2;
    border: 1px solid #000;
}
.personal-information-main {
    display: flex;
    justify-content: space-between;
    padding: 10px;
}

.mony-recept > h3 {
    font-weight: 600;
    letter-spacing: 1px;
    display: inline-block;
    font-size: 16px;
    background: forestgreen;
    color: white;
    width: 207px;
}

.date-serial {
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding-left: 68%;
}

.sl-nomber > span {
    font-weight: 400;
    font-size: 14px;
}


/*End money receipt css*/

.header-right {
    flex: 0 0 39%;
}

.header-left {
    flex: 0 0 49%;
}

span {
    font-weight: 500;
    padding-left: 13px;
    font-size: 11px;
}

.personal-info-inner {
    padding: 10px 0px;
    flex: 0 0 60%;
}
.subject-item {
    flex: 0 0 40%;
}

.total-item > h4 {
    position: relative;
    z-index: 1;
}


.parents-information-main .cmn-heading {
    padding-top: 0;
}

.cmn-text {
    font-size: 12px;
    font-weight: 500;
    line-height: 2;
}

.repeat-item h4 {
    position: relative;
    z-index: 1;
}

.repeat-item h4::after {
    position: absolute;
    content: "";
    left: 109px;
    bottom: 7px;
    width: 76%;
    border: 1px dashed #000;
    z-index: -1;
    overflow: hidden;
}

.total-word-item h4::after {
    position: absolute;
    content: "";
    left: 112px;
    bottom: 109px;
    width: 42%;
    border: 1px dashed #000;
    z-index: -1;
    overflow: hidden;
}
.d-flex {
    display: flex;
    /* align-items: center; */
    padding-left: 45px;
}

.total-item h4::after {
    position: absolute;
    content: "";
    left: 100px;
    bottom: 0px;
    width: 79%;
    border: 1px dashed #000;
    z-index: -1;
    overflow: hidden;
}

.total-item.total-item-amount h4::after {
    left: 38px;
    bottom: 0px;
    width: 96%;
}


.repeat-item h4 span {
    position: relative;
    bottom: 5px;
}

.inner-repeat-item {
    display: grid;
    grid-template-columns: repeat(2, 1fr);
    gap: 35px;
}

.cmn-text > b {
    font-weight: 500;
    display: inline-block;
    background: #fff;
}

.inner-repeat-item .repeat-item h4::after {
    position: absolute;
    content: "";
    left: 45px;
    bottom: 8px;
    width: 87%;
    border: 1px dashed #000;
    z-index: -1;
}

.parents-info-common {
    margin-bottom: 10px;
}

.ftr-info-flex {
    display: flex;
    justify-content: space-between;
    align-items: center;
}

.ftr-info-flex .repeat-item.parents-info {
    flex: 0 0 51%;
}

.phone.d-flex {
    flex: 0 0 41%;
}

.signature-area {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin: 0px 4px 0px 4px;
}

.sig-left > h5 {
    border-top: 1px dashed #000;
    display: ;
}

.sig-left span {
    text-align: center;
    display: block;
}

.sig-left {
    margin-top: 39px;
}

.footer-color{
     height: 12px;
    background: #269826;
}




        </style>
    </head>`);
};

const renderOpeningBodyTab = (windowToPrint) => {
    windowToPrint.document.write(`<body><div class="container">`);
}

const renderReportHeader = (windowToPrint, report) => {
    windowToPrint.document.write(`
     <header class="header-area">
                <div class="header-left">
                    <img src="../../images/logo.png" alt="logo" />
                    <h1>'Every greart dream begins with a dreamer'</h1>
                </div>
                <div class="header-right">
                    <div class="inner-right">
                        <p>House-25, Ishakha Avenue, Sector-06,Uttara, Dhaka-1230. </p>
                       <span>Cell: 01401-430059</span>
                    </div>
                    <div class="mony-recept">
                        <h3>MONEY RECEIPT</h3>
                    </div>
                </div>
            </header>
    `);
}

const renderReportDateAndSerial = (windowToPrint, report, datetime) => {


    windowToPrint.document.write(`
      <div class="date-serial">
                <div class="sl-nomber" style=" padding-right: 20px">
                   <span>Date : <small>${datetime}</small> </span>
                </div>
            </div>
    `);
}

const renderPersonalInfo = (windowToPrint, report, outputText, list, currentMonth) => {
    var paid = report.Paid;
    console.log(currentMonth);

    windowToPrint.document.write(`
       <div class="personal-information-main">
                <div class="personal-info-inner">
                    
                    <div class="repeat-item">
                        <h4 class="cmn-text">Student's Name : <span>${report.StudentName}</span></h4 >
                    </div >

                    <div class="inner-repeat-item">
                        <div class="repeat-item">
                            <h4 class="cmn-text"><b>Class</b> : <span>${report.Class}</span></h4>
                        </div>
                        <div class="repeat-item">
                            <h4 class="cmn-text"><b>Month</b> : <span>${currentMonth}</span></h4>
                        </div>
                    </div>

                    <div class="inner-repeat-item">
                        <div class="repeat-item">
                            <h4 class="cmn-text"><b>Paid</b> : <span>${paid.toFixed(0)}</span></h4>
                        </div>
                        <div class="repeat-item">
                            <h4 class="cmn-text"><b>Due</b> : <span>${report.Due.toFixed(0)}</span></h4>
                        </div>
                    </div>

                   <div class="total-item">
                        <h4 class="cmn-text">Total In Words : <span>${outputText}</span></h4>
                    </div>

                      <div class="total-item total-item-amount">
                        <h4 class="cmn-text">Total : <span>${paid.toFixed(0)}</span></h4>
                      </div>
                   
                </div >

    <div class="subject-item">
        `+ getModules(list) + `
    </div>

            </div >
    `);
};
function getModules(list) {
    var txt = '';
    list.forEach((item) => {
        txt += `<div class="phone d-flex">
            <span>`+ item.CourseName + `</span>
            <span>`+ item.Paid + `</span>
        </div>`;
    });
    return txt;
};

const renderSignatories = (windowToPrint, report) => {
    windowToPrint.document.write(`
        <div class="signature-area">
                <div class="sig-left">
                    <span></span>
                    <h5 class="cmn-text">Authorized Signature</h5>
                </div>
                <div class="sig-left">
                    <span></span>
                    <h5 class="cmn-text">Receipient Signature</h5>
                </div>
            </div>

            <div class="footer-color">
             
            </div>
    `);
}

const renderClosingTags = (windowToPrint) => {
    windowToPrint.document.write(`
            </div>
        </body>
    </html>
    `);
}

const formattedDate = (date) => {
    const options = { year: 'numeric', month: 'long', day: 'numeric' };
    return new Date(date).toLocaleDateString('us-US', options);
}