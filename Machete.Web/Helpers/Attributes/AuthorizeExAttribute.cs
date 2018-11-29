﻿#region COPYRIGHT
// File:     AuthorizeExAttribute.cs
// Author:   Savage Learning, LLC.
// Created:  2012/06/25 
// License:  GPL v3
// Project:  Machete.Web
// Contact:  savagelearning
// 
// Copyright 2011 Savage Learning, LLC., all rights reserved.
// 
// This source file is free software, under either the GPL v3 license or a
// BSD style license, as supplied with this software.
// 
// This source file is distributed in the hope that it will be useful, but 
// WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY 
// or FITNESS FOR A PARTICULAR PURPOSE. See the license files for details.
//  
// For details please refer to: 
// http://www.savagelearning.com/ 
//    or
// http://www.github.com/jcii/machete/
// 
#endregion

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Machete.Helpers
{
    public class AuthorizeExAttribute : AuthorizeAttribute
    {        
        public void OnAuthorization(AuthorizationHandlerContext filterContext)
        {
            CheckIfUserIsAuthenticated(filterContext);
        }

        private void CheckIfUserIsAuthenticated(AuthorizationHandlerContext filterContext)
        {
            // Framework 4.5
//            // If result is null, we're authorized
//            if (filterContext.Result == null) return;
//
//            // If here, you're getting an HTTP 401 status code
//            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
//            {
//                
//                ViewResult result = new ViewResult();
//                result.ViewName = "Error";
//                filterContext.Result = result;
//            }
            
            // .NET Core 2.1
            if (filterContext.HasSucceeded) return;

            // If here, you're getting an HTTP 401 status code
            if (!filterContext.User.Identity.IsAuthenticated) return;
            ViewResult result = new ViewResult();
            result.ViewName = "Error";
            filterContext.Fail();
        }

    }
}