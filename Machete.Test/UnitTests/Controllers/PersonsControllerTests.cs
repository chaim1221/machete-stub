#region COPYRIGHT
// File:     PersonsControllerTests.cs
// Author:   Savage Learning, LLC.
// Created:  2012/06/17 
// License:  GPL v3
// Project:  Machete.Test
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
using AutoMapper;
using Machete.Data.Infrastructure;
using Machete.Domain;
using Machete.Service;
using Machete.Web.Controllers;
using Machete.Web.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;

namespace Machete.Test.Unit.Controller
{
    /// <summary>
    /// Summary description for PersonControllerUnitTests
    /// </summary>

    [TestClass]
    public class PersonTests
    {
        Mock<IPersonService> _serv;
        Mock<IDatabaseFactory> dbfactory;
        Mock<IDefaults> def;
        Mock<IMapper> map;
        PersonController _ctrlr;
        FormCollection fakeform;

        [TestInitialize]
        public void TestInitialize()
        {
            _serv = new Mock<IPersonService>();
            dbfactory = new Mock<IDatabaseFactory>();
            def = new Mock<IDefaults>();
            map = new Mock<IMapper>();
            _ctrlr = new PersonController(_serv.Object, def.Object, map.Object);
            _ctrlr.SetFakeControllerContext();
            fakeform = new FormCollection();
            fakeform.Add("ID", "12345");
            fakeform.Add("firstname1", "Ronald");
            fakeform.Add("lastname1", "Reagan");
            // TODO: Include Lookups in Dependency Injection, remove initialize statements
        }
        //
        //   Testing /Index functionality
        //
        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Persons)]
        public void index_get_returns_ActionResult()
        {
            //Arrange
            //Act
            var result = (ViewResult)_ctrlr.Index();
            //Assert
            Assert.IsInstanceOfType(result, typeof(ActionResult));
        }
        //
        //   Testing /Create functionality
        //
        #region createtests
        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Persons)]
        public void create_get_returns_person()
        {
            //Arrange
            var p = new Machete.Web.ViewModel.Person();
            map.Setup(x => x.Map<Domain.Person, Machete.Web.ViewModel.Person>(It.IsAny<Domain.Person>()))
                .Returns(p);
            //Act
            var result = (PartialViewResult)_ctrlr.Create();
            //Assert
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(Web.ViewModel.Person));
        }
        [Ignore]
        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Persons)]
        public void create_post_valid_returns_JSON()
        {
            //Arrange
            var person = new Person();
            _serv.Setup(p => p.Create(person, "UnitTest")).Returns(person);
            _ctrlr.ValueProvider = fakeform.ToValueProvider();
            //Act
            var result = (JsonResult)_ctrlr.Create(person, "UnitTest");
            //Assert
            IDictionary<string, object> data = new RouteValueDictionary(result.Data);
            Assert.AreEqual(12345, data["iNewID"]);
            Assert.AreEqual("Ronald Reagan", data["sNewLabel"]);
            Assert.AreEqual("/Person/Edit/12345", data["sNewRef"]);
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Persons)]
        [ExpectedException(typeof(InvalidOperationException),
            "An invalid UpdateModel was inappropriately allowed.")]
        public void create_post_invalid_throws_exception()
        {
            //Arrange
            var person = new Person();
            _serv.Setup(p => p.Create(person, "UnitTest")).Returns(person);
            fakeform.Remove("firstname1");
            _ctrlr.ValueProvider = fakeform.ToValueProvider();
            //Act
            _ctrlr.Create(person, "UnitTest");
            //Assert
        }
        #endregion
        //
        //   Testing /Edit functionality
        //
        #region edittests
        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Persons)]
        public void edit_get_returns_person()
        {
            //Arrange
            var pp = new Machete.Web.ViewModel.Person();
            map.Setup(x => x.Map<Domain.Person, Machete.Web.ViewModel.Person>(It.IsAny<Domain.Person>()))
                .Returns(pp);
            _serv = new Mock<IPersonService>();
            int testid = 4242;
            Person fakeperson = new Person();
            _serv.Setup(p => p.Get(testid)).Returns(fakeperson);
            var _ctrlr = new PersonController(_serv.Object, def.Object, map.Object);
            //Act
            var result = (PartialViewResult)_ctrlr.Edit(testid);
            //Assert
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(Web.ViewModel.Person));
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Persons)]
        public void edit_post_valid_updates_model_returns_JSON()
        {
            //Arrange
            _serv = new Mock<IPersonService>();
            int testid = 4242;
            FormCollection fakeform = new FormCollection();
            fakeform.Add("ID", testid.ToString());
            fakeform.Add("firstname1", "blah");     //Every required field must be populated,
            fakeform.Add("lastname1", "UnitTest");  //or result will be null.
            fakeform.Add("gender", "47");
            Person fakeperson = new Person();
            Person savedperson = new Person();
            string user = "";
            _serv.Setup(p => p.Get(testid)).Returns(fakeperson);
            _serv.Setup(x => x.Save(It.IsAny<Person>(),
                                          It.IsAny<string>())
                                         ).Callback((Person p, string str) =>
                                                {
                                                    savedperson = p;
                                                    user = str;
                                                });
            var _ctrlr = new PersonController(_serv.Object, def.Object, map.Object);
            _ctrlr.SetFakeControllerContext();
            _ctrlr.ValueProvider = fakeform.ToValueProvider();
            //Act
            var result = _ctrlr.Edit(testid, "UnitTest") as JsonResult;
            //Assert
            IDictionary<string, object> data = new RouteValueDictionary(result.Data);
            Assert.AreEqual("OK", data["status"]);
            Assert.AreEqual(fakeperson, savedperson);
            Assert.AreEqual(savedperson.firstname1, "blah");
            Assert.AreEqual(savedperson.lastname1, "UnitTest");
            Assert.AreEqual(savedperson.gender, 47);
        }

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Persons)]
        [ExpectedException(typeof(InvalidOperationException),
            "An invalid UpdateModel was inappropriately allowed.")]
        public void edit_post_invalid_throws_exception()
        {
            //Arrange
            var person = new Person();
            int testid = 12345;
            //
            // Mock service and setup SavePerson mock
            _serv.Setup(p => p.Save(person, "UnitTest"));
            _serv.Setup(p => p.Get(testid)).Returns(person);
            //
            // Mock HttpContext so that ModelState and FormCollection work
            fakeform.Remove("firstname1");
            _ctrlr.ValueProvider = fakeform.ToValueProvider();
            //
            //Act
            _ctrlr.Edit(testid, "UnitTest");
            //Assert
            //IDictionary<string, object> data = new RouteValueDictionary(result.Data);
            //Assert.AreEqual("Controller UpdateModel failure on recordtype: person", data["status"]);
        }
        #endregion

        //
        // Testing /Delete functionality
        //

        [TestMethod, TestCategory(TC.UT), TestCategory(TC.Controller), TestCategory(TC.Persons)]
        public void delete_post_returns_JSON()
        {
            //Arrange
            _serv = new Mock<IPersonService>();
            int testid = 4242;
            FormCollection fakeform = new FormCollection();
            var _ctrlr = new PersonController(_serv.Object, def.Object, map.Object);
            _ctrlr.SetFakeControllerContext();
            _ctrlr.ValueProvider = fakeform.ToValueProvider();
            //Act
            JsonResult result = _ctrlr.Delete(testid, "UnitTest") as JsonResult;
            //Assert
            IDictionary<string, object> data = new RouteValueDictionary(result.Data);
            Assert.AreEqual("OK", data["status"]);
            Assert.AreEqual(4242, data["deletedID"]);            
        }
    }
}
