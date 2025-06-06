﻿using System.ComponentModel.DataAnnotations;
using System.Reflection;


namespace HyperKala.Application.Extensions
{
    public static class CommonExtensions
    {
        public static string GetEnumName(this System.Enum dataEnum)
        {
            var enumDisplayName = dataEnum.GetType().GetMember(dataEnum.ToString()).FirstOrDefault();

            if (enumDisplayName != null)
            {
                return enumDisplayName.GetCustomAttribute<DisplayAttribute>()?.GetName();
            }

            return "";
        }
    }
}
