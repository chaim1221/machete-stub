using System;
using System.Configuration;
using AutoMapper;
using Machete.Data;
using Machete.Data.Infrastructure;
using Machete.Domain;
using Machete.Service;
using Machete.Web.Maps;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Machete.Test.Integration.DotNetCore
{
    [TestClass]
    public class PersonTests
    {
        const string connString = "Server=localhost,1433; Database=machete_db; User=SA; Password=passw0rD!;";

        public PersonTests(){
            var databaseFactory = new DatabaseFactory(connString);
            var personRepository = new PersonRepository(databaseFactory);
            var unitOfWork = new UnitOfWork(databaseFactory);
            var lookupRepository = new LookupRepository(databaseFactory);
            var mapper = new Mapper(new MapperConfigurationFactory().Config);
            personService = new PersonService(personRepository, unitOfWork, lookupRepository, mapper);
        }

        private PersonService personService { get; }

        [TestMethod]
        public void SavePerson()
        {
            try {
                var person = personService.Create(new Person() {
                    active = true,
                    address1 = "123 Any St.",
                    address2 = "Apt. Any",
                    email = "person@any.com",
                    state = "AK",
                    facebook = "is a scam",
                    cellphone = "123-456-7890",
                    city = "Any City",
                    createdby = "jadmin",
                    datecreated = DateTime.Now,
                    dateupdated = DateTime.Now,
                    nickname = "Fluffy",
                    lastname1 = "Isa"
                    ,lastname2 = "Person",
                    updatedby = "jadmin",
                    firstname1 = "Any",
                    firstname2 = "Person",
                    fullName = "Immigrant Rights are Human Rights"
                }, "jadmin");
                Assert.IsNotNull(person);
            } catch (Exception ex) {
                throw ex;
            }
        }
    }
}
