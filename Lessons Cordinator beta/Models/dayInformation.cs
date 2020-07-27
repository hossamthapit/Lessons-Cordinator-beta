using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lessons_Cordinator_beta.Models {
    class dayInformation {
        [Key]
        public int ID { get; set; }
        public int studentID { get; set; }
        public DateTime date { get; set; }
        public bool absent { get; set; }
        public double mark { get; set; }
        public string note { get; set; }
    }
}
