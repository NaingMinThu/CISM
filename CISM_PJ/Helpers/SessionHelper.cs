using System.Web;

namespace CISM_PJ.Helpers
{
    public static class SessionHelper
    {
        public static T Get<T>(string key)
        {
            object sessionObject = HttpContext.Current.Session[key];
            if (sessionObject == null)
            {
                return default(T);
            }
            return (T)HttpContext.Current.Session[key];
        }

        public static T Get<T>(string key, T defaultValue)
        {
            object sessionObject = HttpContext.Current.Session[key];
            if (sessionObject == null)
            {
                HttpContext.Current.Session[key] = defaultValue;
            }

            return (T)HttpContext.Current.Session[key];
        }

        public static void Save<T>(string key, T entity)
        {
            HttpContext.Current.Session[key] = entity;
        }

        public static void Remove(string key)
        {
            HttpContext.Current.Session.Remove(key);
        }
    }
}