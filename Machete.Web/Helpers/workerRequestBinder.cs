#region COPYRIGHT
// File:     workerRequestBinder.cs
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

using Machete.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Machete.Web.Helpers
{
    public class workerRequestBinder : IModelBinder
    {
//        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
//        {
//        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            // see if there is an existing model to update and create one if not
            List<WorkerRequest> model = (List<WorkerRequest>) bindingContext.Model ?? new List<WorkerRequest>();
            // find out if the value provider has the required prefix
            bool hasPrefix = bindingContext.ValueProvider
                .ContainsPrefix(bindingContext.ModelName);
            string searchPrefix = (hasPrefix) ? bindingContext.ModelName + "." : "";
            // Get the raw attempted value from the value provider
            ValueProviderResult vpr = bindingContext.ValueProvider.GetValue("workerRequests2");
            var incomingData = vpr.FirstValue; //.AttemptedValue;
            model = incomingData.Split(new char[1] {','}).Select(data => new WorkerRequest
            {
                WorkerID = int.Parse(data)
            }).ToList();
            bindingContext.ModelState.SetModelValue(bindingContext.ModelName, vpr);
            return Task.FromResult(model);
        }
    }
}