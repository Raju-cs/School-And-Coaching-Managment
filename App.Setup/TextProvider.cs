using System;
using System.Collections.Generic;
using System.Text;

namespace App.Setup
{

    public class TextProvider
    {
        public static class Response
        {
            public static string Success { get { return @"Success"; } }
            public static string Fail { get { return @"Fail"; } }
            public static string AlreadySignout { get { return @"Already Signout."; } }
            public static string WrongUserNameOrPassword { get { return @"Phone number or password does not match."; } }
        }

        public static class MESSAGE_TYPES
        {
            public static string REGISTER_OTP { get; private set; } = "REGISTER_OTP";
            public static string LOGIN_OTP { get; private set; } = "LOGIN_OTP";
        }

        public static class CONTACT_STATUS
        {
            public static string PENDING { get; private set; } = "Pending";
        }

        public static class REVIEW_STATUS
        {
            public static string PENDING { get; private set; } = "Pending";
            public static string APPROVED { get; private set; } = "Approved";
            public static string DENIED { get; private set; } = "Denied";
        }

        public static class RESUME_STATUS
        {
            public static string PENDING { get; private set; } = "Pending";
        }

        public static class MESSAGE_REFERNCE_NAME
        {
            public static string CUSTOMER { get; private set; } = "CUSTOMER";
        }

        public static class PLACE_TYPE
        {
            public static string ROOM { get; private set; } = "Room";
            public static string COTTAGE { get; private set; } = "Cottage";
        }
    }
}
