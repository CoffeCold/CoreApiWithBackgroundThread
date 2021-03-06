﻿using System;
using CoreAPI.Models;
using System.Linq;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.DataProtection;

namespace CreateLocalDB
{
    class Program
    {
        static void Main()
        {
            using var context = new JobManagementDBContext();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            PopulateDB(context);
            context.SaveChanges();


        }

        private static void PopulateDB(JobManagementDBContext context)
        {
                
             context.Add(new Job() { JobId = new Guid("69562d2a-6b52-47a4-8089-203efa02a3f0"),  Description = "foo",  ExecutionDomain = ExecutionDomain.Batch });
             context.Add(new JobLog() { LogId = new Guid("78562d2a-6b52-47a4-8089-203efa02a3f0"),  JobId = new Guid("69562d2a-6b52-47a4-8089-203efa02a3f0"), Logcomment="bar", Logdate = DateTime.Now});
        }
    }
}
