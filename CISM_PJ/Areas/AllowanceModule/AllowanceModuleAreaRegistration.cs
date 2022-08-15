using System.Web.Mvc;

namespace CISM_PJ.Areas.AllowanceModule
{
    public class AllowanceModuleAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "AllowanceModule";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "AllowanceModule_default",
                "AllowanceModule/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}