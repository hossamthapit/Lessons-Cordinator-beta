using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lessons_Cordinator_beta.Models {
    struct absencePair {
        public DateTime date;
        public bool absent;
        public double mark;
        public string note;
    }
    class Student {
        [Key]
        public int ID { get; set; }
        public string name { get; set; }
        public string phone1 { get; set; }
        public string phone2 { get; set; }
        public string whatsAppNumber { get; set; }
        public int groupID { get; set; }
        public string school { get; set; }
        public string address { get; set; }
        public Gender gender { get; set; }
        public List<absencePair> absence { get; set; }

        public Student() { }
        public Student(string name, string phone1, int groupID, string school, Gender gender) {
            this.name = name;
            this.phone1 = phone1;
            this.groupID = groupID;
            this.school = school;
            this.gender = gender;
        }
        public Student(string name, string phone1, int groupID, string school, Gender gender, string phone2, string whatsapp, string address)
            : this(name, phone1, groupID, school, gender) {
            this.phone2 = phone2;
            this.whatsAppNumber = whatsapp;
            this.address = address;
        }
    }
}
