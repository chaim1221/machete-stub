#region COPYRIGHT
// File:     EmployerController.cs
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

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AutoMapper;
using Machete.Domain;
using Machete.Service;
using Machete.Service.DTO;
using Machete.Web.Helpers;
using Machete.Web.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Employer = Machete.Domain.Employer;

namespace Machete.Web.Controllers
{

    [ElmahHandleError]
    public class EmployerController : MacheteController
    {
        private readonly IEmployerService serv;
        private readonly IMapper map;
        private readonly IDefaults def;
        private CultureInfo CI;

        public EmployerController(
            IEmployerService employerService, 
            IDefaults def,
            IMapper map
        ) {
            serv = employerService;
            this.map = map;
            this.def = def;
        }
        protected override void Initialize(ActionContext requestContext)
        {
            base.Initialize(requestContext);
            CI = (CultureInfo)Session["Culture"];
            ViewBag.idPrefix = "employer";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// GET: /Employer/AjaxHandler
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public JsonResult AjaxHandler(jQueryDataTableParam param)
        {
            var vo = map.Map<jQueryDataTableParam, viewOptions>(param);
            vo.CI = CI;
            dataTableResult<EmployersList> list = serv.GetIndexView(vo);
            //return what's left to datatables
            var result = list.query
                .Select(e => map.Map<EmployersList, EmployerList>(e))
                .AsEnumerable();
            return Json(new
            {
                param.sEcho,
                iTotalRecords = list.totalCount,
                iTotalDisplayRecords = list.filteredCount,
                aaData = result
            });
        }
        /// <summary>
        /// GET: /Employer/Create
        /// </summary>
        /// <returns>PartialView</returns>
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public ActionResult Create()
        {
            var m = map.Map<Employer, ViewModel.Employer>(new Employer
            {
                active = true,
                blogparticipate = false,
                referredby = def.getDefaultID(LCategory.emplrreference)
            });
            m.def = def;
            return PartialView("Create", m);
        }
        /// <summary>
        /// POST: /Employer/Create
        /// </summary>
        /// <param name="employer"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public JsonResult Create(Employer employer, string userName)
        {
            UpdateModel(employer);
            Employer newEmployer = serv.Create(employer, userName);                          
            var result = map.Map<Employer, ViewModel.Employer>(newEmployer);
            return Json(new
            {
                sNewRef = result.tabref,
                sNewLabel = result.tablabel,    
                iNewID = result.ID,
                jobSuccess = true
            });
        }

        /// <summary>
        /// GET: /Employer/Edit/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public ActionResult Edit(int id)
        {
            var e = serv.Get(id);
            var m = map.Map<Employer, ViewModel.Employer>(e);
            m.def = def;
            return PartialView("Edit", m);
        }
        /// <summary>
        /// POST: /Employer/Edit/5
        /// </summary>
        /// <param name="id"></param>
        /// <param name="collection"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public JsonResult Edit(int id, FormCollection collection, string userName)
        {
            Employer employer = serv.Get(id);
            UpdateModel(employer);
            serv.Save(employer, userName);                            
            return Json(new
            {
                jobSuccess = true
            });            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="UserName"></param>
        /// <returns></returns>
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator")]
        public JsonResult Delete(int id, string userName)
        {
            serv.Delete(id, userName);

            return Json(new
            {
                status = "OK",
                jobSuccess = true,
                deletedID = id
            });
        }

        private List<Dictionary<string, string>> DuplicateEmployers(string name, string address, 
            string phone, string city, string zipcode)
        {
            //Get all the records            
            IEnumerable<Employer> list = serv.GetAll();
            var employersFound = new List<Dictionary<string, string>>();
            name = name.Replace(" ", "");
            address = address.Replace(" ", "");
            phone = string.IsNullOrEmpty(phone) ? "x" : phone;
            city = city.Replace(" ","");
            zipcode = zipcode.Replace(" ","");

            foreach (var employer in list)
            {
                var employer_Name = employer.name.Replace(" ", "");
                var employer_Address = employer.address1.Replace(" ", "");
                var employer_Phone = string.IsNullOrEmpty(employer.phone) ? "y" : employer.phone;
                var employer_City = employer.city.Replace(" ","");
                var employer_Zipcode = employer.zipcode.Replace(" ","");

                //checking if person already exists in dbase
                var matchCount = 0;
                if (employer_Name.Equals(name, StringComparison.CurrentCultureIgnoreCase)) matchCount++;
                if (employer_Address.Equals(address, StringComparison.CurrentCultureIgnoreCase)) matchCount++;
                if (employer_Phone.Equals(phone, StringComparison.CurrentCultureIgnoreCase)) matchCount++;
                if (employer_Zipcode.Equals(zipcode, StringComparison.CurrentCultureIgnoreCase)) matchCount++;
                if (employer_City.Equals(city, StringComparison.CurrentCultureIgnoreCase)) matchCount++;
                

                if (matchCount >= 3)
                {
                    var employerFound = new Dictionary<string, string>();
                    employerFound.Add("Name", employer.name);
                    employerFound.Add("Address", employer.address1);
                    employerFound.Add("Phone", employer.phone);
                    employerFound.Add("City", employer.city);
                    employerFound.Add("ZipCode", employer.zipcode);
                    employerFound.Add("ID", employer.ID.ToString());

                    employersFound.Add(employerFound);
                }
            }

            return employersFound;
        }

        [Authorize(Roles = "Administrator, Manager, PhoneDesk")]
        public JsonResult GetDuplicates(string name, string address,
            string phone, string city, string zipcode )
        {
            var duplicateFound = DuplicateEmployers(name, address, phone, city, zipcode);
            return Json(new { duplicates = duplicateFound });
        }
    }
}
