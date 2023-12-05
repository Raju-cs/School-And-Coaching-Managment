 (function () {
     console.log("TeacherFee");
     var url = '/StudentModule/DashBoard/';
     var incomeHTML;
     var logInUserHTML;

     fetch(url, {
         method: 'GET',
         headers: {
             'Content-Type': 'application/json'
         },
     }).then(res => {
         if (res.status === 200) {
             return res.json();
         }

         throw Error(res.statusText);
     }).then(data => {
         // Handle the data from the response
         console.log(data.Data); // Log the data to the console or perform further actions

         domManupulationAll(data)
         studentAdmissionChart(data.Data[7]);
        
         
     }).catch(err => alert(err));


     function formatDateString(dateString) {
         const inputDate = new Date(dateString);

         return inputDate.toLocaleString('en-US', {
             year: '2-digit',
             month: '2-digit',
             day: '2-digit',
             hour: 'numeric',
             minute: 'numeric',
             hour12: true,
         });
     }

     function domManupulationAll(data) {

         document.querySelector('.js-students-count').innerHTML = data.Data[0][0].TotalStudents;
         document.querySelector('.js-Teachers-count').innerHTML = data.Data[1][0].TotalTeacher;
         document.querySelector('.js-courses-count').innerHTML = data.Data[2][0].TotalCourse;
         document.querySelector('.js-module-count').innerHTML = data.Data[3][0].TotalModule;
         document.querySelector('.js-expense-count').innerHTML = data.Data[6][0].TotalExpense;

         incomeHTML = `
           <h1>Teacher Income</h1>
         <p>${data.Data[4][0].TotalTeacherIncome || 0}</p>
         <h1>Coaching Income</h1>
         <p>${data.Data[4][0].CoachingIncome || 0}</p>
         <h1>Toatal Income</h1>
         <p>${data.Data[4][0].GrandTotalIncome || 0}</p>
      `;
         document.querySelector('.js-income-total').innerHTML = incomeHTML;

         const formattedDateString1 = formatDateString(data.Data[5][0].LastAccessAt);
         const formattedDateString2 = formatDateString(data.Data[5][1].LastAccessAt);
         const formattedDateString3 = formatDateString(data.Data[5][2].LastAccessAt);
         const formattedDateString4 = formatDateString(data.Data[5][3].LastAccessAt);
         const formattedDateString5 = formatDateString(data.Data[5][4].LastAccessAt);

         logInUserHTML = `
                    <ul>
                    <li>
                        <b>User Name</b>
                        <b>Last Access Time</b>
                    </li>
                    <li>
                        <span>${data.Data[5][0].UserName}</span>
                        <span>${formattedDateString1}</span>
                    </li>
                    <li>
                        <span>${data.Data[5][1].UserName}</span>
                        <span>${formattedDateString2}</span>
                    </li>
                    <li>
                        <span>${data.Data[5][2].UserName}</span>
                        <span>${formattedDateString3}</span>
                    </li>
                    <li>
                        <span>${data.Data[5][3].UserName}</span>
                        <span>${formattedDateString4}</span>
                    </li>
                    <li>
                        <span>${data.Data[5][4].UserName}</span>
                        <span>${formattedDateString5}</span>
                    </li>
                </ul>

       `;

         document.querySelector('.js-user-lastActivity').innerHTML = logInUserHTML;
     }

     function studentAdmissionChart(admissionData) {
         var ctx = document.getElementById('admissionsChart').getContext('2d');
         var innerArray = admissionData;

         // Now innerArray contains your desired data
         var years = innerArray.map(item => item.Year.toString());
         var totalStudent = innerArray.map(item => item.TotalStudentsCreated);
         console.log('years', years);
         console.log('totalStudentsCreated', totalStudent);
         var myChart = new Chart(ctx, {
             type: 'bar',
             data: {
                 labels: years,
                 datasets: [{
                     label: 'Number of Admissions',
                     data: totalStudent,
                     backgroundColor: 'rgba(0, 116, 217, 0.7)',
                     borderWidth: 1,
                 }]
             },
             options: {
                 scales: {
                     y: {
                         beginAtZero: true,
                         title: {
                             display: true,
                             text: 'Number of Students',
                         }
                     },
                 },
                 plugins: {
                     title: {
                         display: true,
                         text: 'Student Admissions Over the Years',
                     },
                 },
             }
         });
     }
    

})();