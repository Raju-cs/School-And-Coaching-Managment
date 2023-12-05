var Controller = new function () {

    var _options;

    this.Show = function (options) {
        _options = options;
        console.log("options=>", _options);

        Global.Add({
            title: 'UnApproveStudent Information',
            selected: 0,
            Tabs: [
                {
                    title: 'UnApproveStudent Information',

                    columns: [

                        { field: 'NickName', title: 'Nick Name', filter: true, position: 3, add: { sibling: 4 } },
                        { field: 'Name', title: 'Full Name(English)', filter: true, position: 4, add: { sibling: 4 } },
                        { field: 'StudentNameBangla', title: 'Full Name(Bangla)', filter: true, position: 5, add: { sibling: 4 } },
                        { field: 'PhoneNumber', title: 'Phone Number', filter: true, position: 6, add: { sibling: 4 } },
                        { field: 'DateOfBirth', title: 'Date Of Birth', filter: true, position: 7, add: { sibling: 4 }, dateFormat: 'dd/MM/yyyy', },
                        { field: 'ChooseSubject', title: 'Learn Subjects', filter: true, position: 11, add: { sibling: 4 } },
                        { field: 'Nationality', title: 'Nationality', filter: true, position: 11, add: { sibling: 4 } },
                        { field: 'StudentSchoolName', title: 'School Name', filter: true, position: 12, add: { sibling: 4 }, required: false },
                        { field: 'StudentCollegeName', title: 'College Name', filter: true, position: 13, add: { sibling: 4 }, required: false },
                        { field: 'Class', title: 'Class', filter: true, position: 14, add: { sibling: 4 } },
                        { field: 'Section', title: 'Section', filter: true, position: 17, add: { sibling: 4 }, required: false },
                        { field: 'FathersName', title: 'Fathers Name', filter: true, position: 18, add: { sibling: 4 }, required: false },
                        { field: 'FathersOccupation', title: 'Fathers Occupation', filter: true, position: 19, add: { sibling: 4 }, required: false },
                        { field: 'FathersPhoneNumber', title: 'Fathers Phone Number', filter: true, position: 19, add: { sibling: 4 }, required: false },
                        { field: 'FathersEmail', title: 'Fathers Email Address', filter: true, position: 20, add: { sibling: 4 }, required: false },
                        { field: 'MothersName', title: 'Mothers Name', filter: true, position: 21, add: { sibling: 4 }, required: false },
                        { field: 'MothersOccupation', title: 'Mothers Occupation', filter: true, position: 22, add: { sibling: 4 }, required: false },
                        { field: 'MothersPhoneNumber', title: 'Mothers Phone Number', filter: true, position: 23, add: { sibling: 4 }, required: false },
                        { field: 'MothersEmail', title: 'Mothers Email Address', filter: true, position: 24, add: { sibling: 4 }, required: false },
                        { field: 'GuardiansName', title: 'Guardians Name', filter: true, position: 25, add: { sibling: 4 } },
                        { field: 'GuardiansOccupation', title: 'Guardians Occupation', filter: true, position: 26, add: { sibling: 4 } },
                        { field: 'GuardiansPhoneNumber', title: 'Guardians Phone Number', filter: true, position: 27, add: { sibling: 4 } },
                        { field: 'GuardiansEmail', title: 'Guardians Email Address', filter: true, position: 28, add: { sibling: 4 } },
                        { field: 'PresentAddress', title: 'Present Address', filter: true, position: 29, add: { sibling: 2 } },
                        { field: 'PermanantAddress', title: 'Permanant Address', filter: true, position: 30, add: { sibling: 2 } },
                        { field: 'District', title: 'District', filter: true, position: 31, add: false, },
                        { field: 'Remarks', title: 'Remarks', filter: true, add: { sibling: 1, type: 'textarea' }, required: false },

                    ],
                    DetailsUrl: function () {
                        return '/UnApproveStudent/BasicInfo?Id=' + _options.Id;
                    },
                    onLoaded: function (tab, data) {

                    }
                }
            ],

            name: 'UnApproveStudent Information',
            url: '/lib/IqraService/Js/OnDetailsWithTab.js?v=OrderDetails' + Math.random(),

        });
    }
};