using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Setup.LogService
{
    public class Log
    {
        public static void Error(ActionExecutingContext context) {

        }
        public static void Error(ActionExecutedContext context)
        {

        }
        public static void Error(ActionExecutingContext context, ActionExecutionDelegate next)
        {

        }
        public static void Error(Exception exp, HttpRequest request)
        {
            Console.WriteLine(new {exp, request});
        }
        public static void Error(Exception exp)
        {
            Console.WriteLine(exp);
        }
    }
}
