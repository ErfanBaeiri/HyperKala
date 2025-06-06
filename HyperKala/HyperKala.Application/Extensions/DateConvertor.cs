﻿using System;
using System.Globalization;

namespace HyperKala.Application.Extensions
{
    public static class DateConvertor
    {
        public static string ToShamsi(this DateTime value)
        {
            PersianCalendar persianCalendar = new PersianCalendar();
            return persianCalendar.GetYear(value) + "/" +
                   persianCalendar.GetMonth(value).ToString("00") + "/" +
                   persianCalendar.GetDayOfMonth(value).ToString("00");
        }
    }
}
