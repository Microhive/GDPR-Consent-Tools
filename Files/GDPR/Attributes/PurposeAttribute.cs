using GDPR.Core;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace GDPR.Attributes
{
    public class PurposeAttribute : Attribute, IActionFilter
    {
        public string Purpose { get; set; }

        public string PII { get; set; }

        public PurposeAttribute(string name, string pii)
        {
            Purpose = name;
            PII = pii;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var controller = (IGDPRController)context.Controller;
            controller.CurrentPurpose = Purpose;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            var controller = context.Controller as IGDPRController;
            controller.CurrentPurpose = "";
        }
    }
}
