#region COPYRIGHT
// File:     userNameFilterAttribute.cs
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

using System.Security.Claims;
using System.Web.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Machete.Web.Helpers
{
    /// <inheritdoc />
    /// <summary>Controller decorator to handle UserName</summary>
	public class UserNameFilter : ActionFilterAttribute
	{
	    public override void OnActionExecuting(ActionExecutingContext filterContext)
	    {
	        const string key = "userName";
	        var userIdentity = new ClaimsIdentity("Cookies");
	 
	        if (filterContext.ActionArguments.ContainsKey(key))
	        {
	            if (userIdentity.IsAuthenticated)
	            {
	                filterContext.ActionArguments[key] = userIdentity.Name;
	            }
	        }	 
	        base.OnActionExecuting(filterContext);
	    }
	}
}