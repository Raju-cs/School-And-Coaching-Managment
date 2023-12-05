
import { Gender, ACTIVE_STATUS, Religion, BLOOD_GROUP, GROUP, SHIFT, SUBJECT, CLASS } from "../dictionaries.js";
import { filter, liveRecord, OPERATION_TYPE, trashRecord } from '../filters.js';
const StudentRegistration = {};
(function () {
    var inputs, formModel = {}, form = $('#student_registration_form');

    console.log("formModel=>", formModel.DateOfBirth);


    var drp = [
        {
            title: 'Shift',
            Id: 'Shift',
            dataSource: [
                { text: 'Morning', value: SHIFT.MORNING },
                { text: 'Evening', value: SHIFT.EVENING },
            ],
            position: 15,
            add: { sibling: 4 },
            required: false,
        },
        {
            title: 'Version',
            Id: 'Version',
            dataSource: [
                { text: 'Bangla', value: SUBJECT.BANGLA },
                { text: 'English', value: SUBJECT.ENGLISH },

            ],
            position: 16,
            add: { sibling: 4 },
            required: false,
        }, {
            title: 'Class',
            Id: 'Class',
            dataSource: [
                { text: 'Six', value: CLASS.SIX },
                { text: 'Seven', value: CLASS.SEVEN },
                { text: 'Eight', value: CLASS.EIGHT },
                { text: 'Nine', value: CLASS.NINE },
                { text: 'Ten', value: CLASS.NEW_TEN },
                { text: 'Ten(Ssc Examiner)', value: CLASS.OLD_TEN },
                { text: 'Eleven', value: CLASS.ELEVEN },
                { text: 'Twelve', value: CLASS.NEW_TWELVE },
                { text: 'Twelve(Hsc Examiner)', value: CLASS.OLD_TWELVE },
            ],
            position: 14,
            add: { sibling: 4 },
        },
        {
            title: 'Group',
            Id: 'Group',
            dataSource: [
                { text: 'Science', value: GROUP.SCIENCE },
                { text: 'Commerce', value: GROUP.COMMERCE },


            ],
            position: 15,
            add: { sibling: 4 },
            required: false,


        }, {
            title: 'Blood Group',
            Id: 'BloodGroup',
            dataSource: [
                { text: 'A+', value: BLOOD_GROUP.A_POSITIVE },
                { text: 'A-', value: BLOOD_GROUP.A_NEGATIVE },
                { text: 'B+', value: BLOOD_GROUP.B_POSITIVE },
                { text: 'B-', value: BLOOD_GROUP.B_NEGATIVE },
                { text: 'O+', value: BLOOD_GROUP.O_POSITIVE },
                { text: 'O-', value: BLOOD_GROUP.O_NEGATIVE },
                { text: 'AB+', value: BLOOD_GROUP.AB_POSITIVE },
                { text: 'AB-', value: BLOOD_GROUP.AB_NEGATIVE },
            ],
            position: 9,
            add: { sibling: 4 },
            required: false,


        },
        {
            title: 'Gender',
            Id: 'Gender',
            dataSource: [
                { text: 'Male', value: Gender.MALE },
                { text: 'Female', value: Gender.FEMALE },
                { text: 'Non-Binary', value: Gender.NON_BINARY },
            ],
            position: 8,
            add: { sibling: 4 },
            required: false,


        }, {
            title: 'Religion',
            Id: 'Religion',
            dataSource: [
                { text: 'Islam', value: Religion.ISLAM },
                { text: 'Hinduism', value: Religion.HINDUISM },
                { text: 'Christian', value: Religion.CHRISTIAN },
            ],
            position: 10,
            add: { sibling: 4 },
            required: false,
        }, {
            Id: 'DistrictId',
            add: { sibling: 3 },
            position: 31,
            url: '/District/AutoComplete',
            Type: 'AutoComplete',
            page: { 'PageNumber': 1, 'PageSize': 20, filter: [liveRecord] }
        }
    ];

    function save() {

        if (!formModel.IsValid) {
            alert('Validation Error.');
            return;
        }

        formModel.DateOfBirth = new Date(formModel.dob)?.toISOString();

        Global.Busy('Please Wait while saving data......');
        console.log(['save', formModel]);
            
        if (inputs.ImageURL.files[0]) {

            formModel.Img = { IsFile: true, File: inputs.ImageURL.files[0] }
            Global.Uploader.upload({
                data: formModel,
                url: '/UnApproveStudent/Add',
                onProgress: function (data) {
                    //windowModel.View.find('#progress_ba_container #myBar').css({ width: (data.loaded / data.total) * 100 + '%' });
                    console.log(data);
                },
                onComplete: function (response) {
                    Global.Free();
                    if (!response.IsError) {
                        window.location.href = "/RegistrationSuccesfull";

                    } else {
                        Global.Error.Show(response, { formModel });
                    }
                },
                onError: function (response) {
                    response.Id = -8;
                    Global.Error.Show(response, { formModel });
                }
            });


        } else {

        }
        
    };

 

    function setDRP() {

        drp.forEach((item) => {
            item.elm = $(inputs[item.Id]);
            if (item.Type === 'AutoComplete') {
                Global.AutoComplete.Bind(item);
            } else {
                Global.DropDown.Bind(item);
            }
            console.log(['item', item, drp]);
        });
    };

    inputs = Global.Form.Bind(formModel, form);
    //windowModel.View.find('.btn_cancel').click(close);
    Global.Click(form.find('.btn_save'), save);
    setDRP();

})();

export { StudentRegistration };