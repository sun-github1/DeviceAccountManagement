using DeviceCredentialManagement.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeviceCredentialManagement.Services
{
    public class AppDBContext: IdentityDbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) :base(options)
        {

        }

        public DbSet<DeviceAccountCredential> CandidAccountCredentials { get; set; }

    }
}
