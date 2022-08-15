using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace CISM_PJ.Models
{
    public class MyEnum
    {
        #region Student Status
        public enum StudentStatus : byte
        {
            Active = 1, InActive
        }
        #endregion
    }
    public enum WeeksDay : byte
    {
        [Description("Sunday")] Sunday = 1, [Description("Monday")] Monday,
        [Description("Tursday")] Tursday, [Description("Wednesday")] Wednesday,
        [Description("Thusday")] Thusday, [Description("Friday")] Friday,
        [Description("Saturday")] Saturday
    }
    public class EnumClass
    {
        public static string GetDescriptionFromEnum(Enum value)
        {
            DescriptionAttribute attribute = value.GetType()
                .GetField(value.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                .SingleOrDefault() as DescriptionAttribute;
            return attribute == null ? value.ToString() : attribute.Description;
        }
    }
}