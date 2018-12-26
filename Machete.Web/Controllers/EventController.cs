#region COPYRIGHT
// File:     EventController.cs
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
using System.Globalization;
using System.Linq;
using AutoMapper;
using Machete.Domain;
using Machete.Service;
using Machete.Service.DTO;
using Machete.Web.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Machete.Web.Controllers.Helpers;

namespace Machete.Web.Controllers
{
    [ElmahHandleError]
    public class EventController : MacheteController
    {
        private readonly IEventService serv;
        private readonly IImageService iServ;
        private readonly IMapper map;
        private readonly IDefaults def;
        CultureInfo CI;
        //
        //
        public EventController(IEventService eventService, 
            IImageService imageServ, 
            IDefaults def,
            IMapper map)
        {
            serv = eventService;
            iServ = imageServ;
            this.map = map;
            this.def = def;
        }
        protected override void Initialize(ActionContext requestContext)
        {
            base.Initialize(requestContext);
            CI = (CultureInfo)Session["Culture"];
        }
        //
        //
        [Authorize(Roles = "Manager, Administrator")]
        public ActionResult AjaxHandler(jQueryDataTableParam param)
        {            
            //Get all the records
            var vo = map.Map<jQueryDataTableParam, viewOptions>(param);
            vo.CI = CI;
            dataTableResult<EventList> list = serv.GetIndexView(vo);
            //return what's left to datatables
            var result = list.query
                .Select(e => map.Map<EventList, ViewModel.EventList>(e))
                .AsEnumerable();
            return Json(new
            {
                param.sEcho,
                iTotalRecords = list.totalCount,
                iTotalDisplayRecords = list.filteredCount,
                aaData = result
            });
        }
        //
        // GET: /Event/Create
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Create(int PersonID)
        {
            var m = map.Map<Event, ViewModel.Event>(new Event
            {
                dateFrom = DateTime.Today,
                dateTo = DateTime.Today,
                PersonID = PersonID
            });
            m.def = def;
            return PartialView("Create", m);
        }

        //
        // POST: /Event/Create
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Manager, Administrator")]
        public ActionResult Create(Event evnt, string userName)
        {
            UpdateModel(evnt);
            Event newEvent = serv.Create(evnt, userName);
            var result = map.Map<Event, ViewModel.Event>(newEvent);
            return Json(new
            {
                sNewRef = result.tabref,
                sNewLabel = result.tablabel,
                iNewID = newEvent.ID,
                jobSuccess = true
            });
        }

        //
        // GET: /Event/Edit/5
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Edit(int id)
        {
            var m = map.Map<Event, ViewModel.Event>(serv.Get(id));
            m.def = def;
            return PartialView("Edit", m);
        }

        //
        // POST: /Event/Edit/5

        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Edit(int id, string userName)
        {
            Event evnt = serv.Get(id);
            UpdateModel(evnt);
            serv.Save(evnt, userName);
  
            return Json(new { status = "OK" });
        }
        //
        // AddImage
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult AddImage(int id, string userName, IFormFile imagefile)
        {
            if (imagefile == null) throw new MacheteNullObjectException("AddImage called with null imagefile");
            JoinEventImage joiner = new JoinEventImage();
            Event evnt = serv.Get(id);
            // TODO:The following code should be in the Service layer
            Image image = new Image();
            image.ImageMimeType = imagefile.ContentType;
            image.parenttable = "Events";
            image.filename = imagefile.FileName;
            image.recordkey = id.ToString();
            image.ImageData = new byte[imagefile.Length];
            imagefile.OpenReadStream();//(image.ImageData,
                                       //0,
                                       //imagefile.Length);
            // TODO read the stream, close the file
            Image newImage = iServ.Create(image, userName);
            joiner.ImageID = newImage.ID;
            joiner.EventID = evnt.ID;
            joiner.datecreated = DateTime.Now;
            joiner.dateupdated = DateTime.Now;
            joiner.updatedby = userName;
            joiner.createdby = userName;
            // TODO: This tightly couples the MVC straight down to EF. 
            // breaks layering. Should be abstracted.
            evnt.JoinEventImages.Add(joiner);
            serv.Save(evnt, userName);
            //var foo = iServ.Get(newImage.ID).ImageData;
            
            return Json(new
            {
                status = "OK"                
            });
        }
        //
        // GET: /Event/Delete/5
        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Delete(int id, string user)
        {   
            serv.Delete(id, user);
            return Json(new
            {
                status = "OK",
                deletedID = id
            });
        }

        [HttpPost, UserNameFilter]
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult DeleteImage(int evntID, int jeviID, string user)
        {
            int deletedJEVI = 0;
            Event evnt = serv.Get(evntID);
            JoinEventImage jevi = evnt.JoinEventImages.Single(e => e.ID == jeviID);
            deletedJEVI = jevi.ID;
            iServ.Delete(jevi.ImageID, user);
            evnt.JoinEventImages.Remove(jevi);

            return Json(new
            {
                status = "OK",
                deletedID = deletedJEVI
            });
        }
    }
}
