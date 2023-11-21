using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBModel
{
    public class TextTabContext : DbContext
    {
        public DbSet<TextTab> TextTab { get; set; }
        public DbSet<QuestionAndAnswer> QuestionAndAnswer { get; set; }

        protected override void OnConfiguring(
            DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(
                "Data Source=textTabs.db");
            //optionsBuilder.UseLazyLoadingProxies();
        }
    }
}
