using System;
using System.Collections.Generic;
using System.Text;

namespace App.Setup
{
    public class Connection
    {
        //local
        public static string DBName { get { return "Coaching"; } }
        public static string ConnectionString = @"data source=DESKTOP-2UPH23H;initial catalog=" + DBName + @";persist security info=True;user id=raju;password=123;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=True";

        //demo
        //public static string DBName { get { return "Coaching_Demo"; } }
        //public static string ConnectionString = @"data source=WIN-ONDMT4I5R10;initial catalog=" + DBName + @";persist security info=True;user id=Iqra@DataStore2022.;password=IqData@Store#^&.2022;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=True";


        /*  public static string DBName { get { return "dreamers"; } }
          public static string ConnectionString = @"data source=103.108.140.160,1434;initial catalog=" + DBName + ";persist security info=True;user id=devrohan;password=dev.rohan!@#123;MultipleActiveResultSets=True";
  */
        //live
        //public static string DBName { get { return "IqraCoaching"; } }
        //public static string ConnectionString = @"data source= WIN-U21E8GOG7IV;initial catalog=" + DBName + ";persist security info=True;user id=Iqra@Coachi$% ;password=iQra$20*^CoA#@;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=True";

        public static string ReportConnectionString = ConnectionString;
    }
}
