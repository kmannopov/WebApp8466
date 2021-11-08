using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DSCC_CW1_MVCWebApp_8466.Models;

namespace DSCC_CW1_MVCWebApp_8466.Data
{
    public class DSCC_CW1_MVCWebApp_8466Context : DbContext
    {
        public DSCC_CW1_MVCWebApp_8466Context (DbContextOptions<DSCC_CW1_MVCWebApp_8466Context> options)
            : base(options)
        {
        }

        public DbSet<DSCC_CW1_MVCWebApp_8466.Models.Book> Book { get; set; }

        public DbSet<DSCC_CW1_MVCWebApp_8466.Models.Genre> Genre { get; set; }
    }
}
