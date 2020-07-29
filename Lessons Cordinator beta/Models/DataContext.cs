using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lessons_Cordinator_beta.Models {
    class DataContext : DbContext {
        public DataContext() : base("name=CordinatorDB") {

        }
        public DbSet<Student> students { get; set; }
        public DbSet<Groups> groups { get; set; }
        public DbSet<dayInformation> dayInfoList { get; set; }
    }
}
