﻿using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ToDoList.Domain.Extensions
{
    public static class EnumExention
    {
        public static string GetDisplayName( this System.Enum enumValue)
        {
            return enumValue.GetType()
                .GetMember(enumValue.ToString())
                .First()
                .GetCustomAttribute<DisplayAttribute>()
                ?.GetName() ?? "Unknown";
        }
    }
}
