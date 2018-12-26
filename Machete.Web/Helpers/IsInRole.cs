#region COPYRIGHT
// File:     IsInRole.cs
// Author:   Savage Learning, LLC.
// Created:  2012/06/17 
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

using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Machete.Web.Helpers
{
    //http://stackoverflow.com/questions/4649795/hiding-column-in-table-based-on-role-in-mvc
    public static class IsInRoleHelper
    {
        public static bool IsInRole(this IHtmlHelper instance, params string[] roles)
        {
            var user = instance.ViewContext.HttpContext.User;
            return roles.Any(role => user.IsInRole(role));
        }
    }
}