using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Setup.Handler
{
    public class Helper
    {
        public static string UnAuthorisedUrl { get { return @"/HandlerArea/UnAuthorised/"; } }
        public static string NotFoundUrl { get { return @"/HandlerArea/NotFound"; } }

    }
}
