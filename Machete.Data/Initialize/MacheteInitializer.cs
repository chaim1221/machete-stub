#region COPYRIGHT
// File:     MacheteInitializer.cs
// Author:   Savage Learning, LLC.
// Created:  2012/06/17 
// License:  GPL v3
// Project:  Machete.Data
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
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Machete.Data
{
    //public class MacheteInitializer : MigrateDatabaseToLatestVersion<MacheteContext, MacheteConfiguration> {}
    //public class TestInitializer : MigrateDatabaseToLatestVersion<MacheteContext, MacheteConfiguration> {}
    public class MacheteConfiguration // : DbMigrationsConfiguration<MacheteContext>
    {
        private MacheteContext _db;

        public MacheteConfiguration(MacheteContext db, bool seed = false) // : base()
        {
            //AutomaticMigrationsEnabled = true;         // TODO: Add migrations, ignoring on first pass
            //AutomaticMigrationDataLossAllowed = false; // https://stackoverflow.com/a/42302429/2496266
            _db = db;
            if (seed) Seed();
        }

        //protected override void Seed(MacheteContext DB)
        private void Seed()
        {
            if (_db.Lookups.Count() == 0)
            {
                MacheteLookup.Initialize(_db);
            }
            if (_db.TransportProviders.Count() == 0 || _db.TransportProvidersAvailability.Count() == 0)
            {
                MacheteTransports.Initialize(_db);
            }
            if (_db.Users.Count() == 0)   MacheteUsers.Initialize(_db);
            // MacheteConfigs.Initialize assumes Configs table has been populated by script
            if (_db.Configs.Count() == 0) MacheteConfigs.Initialize(_db);
            if (_db.TransportRules.Count() == 0) MacheteRules.Initialize(_db);
            if (_db.ReportDefinitions.Count() != MacheteReportDefinitions.cache.Count) MacheteReportDefinitions.Initialize(_db);
        }
    }   
}
