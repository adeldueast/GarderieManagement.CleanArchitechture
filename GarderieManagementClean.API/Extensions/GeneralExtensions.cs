﻿using Microsoft.AspNetCore.Http;
using System.Linq;

namespace GarderieManagementClean.API.Extensions
{
    public static class GeneralExtensions
    {
        public static string GetUserId(this HttpContext httpContext)
        {
            if (httpContext.User == null)
            {
                return string.Empty;
            }
            return httpContext.User.Claims.Single(x => x.Type == "Id").Value;
        }

        public static string GetUserGarderieId(this HttpContext httpContext)
        {
            if (httpContext.User == null)
            {
                return string.Empty;
            }
            return httpContext.User.Claims.Single(x => x.Type == "GarderieId").Value;
        }
    }
}
