using Lessons_Cordinator_beta.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lessons_Cordinator_beta {
    public partial class Home : Form {

        DataContext data = new DataContext();
        Student editableStudent = new Student();
        Groups editableGroup = new Groups();

        public Home() {
            InitializeComponent();
            //setUp();
        }


        void setUp() {

            foreach (Models.Day d in Enum.GetValues(typeof(Models.Day))) {
                comboBoxDay.Items.Add(d);
            }
            foreach (Models.Day d in Enum.GetValues(typeof(Models.Day))) {
                comboBoxEditGroupDay.Items.Add(d);
            }
            foreach (var g in data.groups.ToList()) {
                comboBoxsSearchByGroup.Items.Add(g.ToString());
            }
            foreach (var g in data.groups.ToList()) {
                comboBoxAbsentGroup.Items.Add(g.ToString());
            }
            foreach (Gender d in Enum.GetValues(typeof(Gender))) {
                genderCombo.Items.Add(d);
            }

        }

        void getStudents(string str) {

            gridViewStudents.Rows.Clear();
            List<Student> Students = data.students.Where(st => st.name.Contains(str) == true).ToList();
            List<Groups> _groups = data.groups.ToList();

            foreach (Student st in Students) {

                if (st.absence == null) {
                    st.absence = new List<absencePair>();
                    st.absence.Add(new absencePair() { absent = true, date = DateTime.Now });
                }
                var info = data.dayInfoList.ToList();

                gridViewStudents.Rows.Add(st.name, st.phone1, st.phone2,
                    _groups.FirstOrDefault(m => m.ID == st.groupID).ToString(),
                    info.Count(inf => inf.studentID == st.ID && inf.absent == false), "معلومات", "تعديل");
            }
            bool color = true;
            foreach (DataGridViewRow row in gridViewStudents.Rows) {
                if (color) row.DefaultCellStyle.BackColor = Color.LightGray;
                else row.DefaultCellStyle.BackColor = Color.LightBlue;
                color = !color;
            }
        }

        void getgroups() {
            List<Groups> _groups = data.groups.ToList();
            foreach (Groups g in _groups)
                groupsGridView.Rows.Add(g.day.ToString(), g.hour.ToString() + ":" + g.minutes.ToString(),
                    g.gender.ToString(), data.students.Count(st => st.groupID == g.ID), "تعديل", "حذف");

            bool color = true;
            foreach (DataGridViewRow row in groupsGridView.Rows) {
                if (color) row.DefaultCellStyle.BackColor = Color.LightGray;
                else row.DefaultCellStyle.BackColor = Color.LightBlue;
                color = !color;
            }
        }


        private void addStudentBtn_Click(object sender, EventArgs e) {
            groupCombo.Items.Clear();
            List<Groups> _groups = data.groups.ToList();
            foreach (Groups g in _groups)
                groupCombo.Items.Add(g.day.ToString() + ", " + g.hour.ToString() + ":" + g.minutes.ToString() + " " + g.gender.ToString());

            tabControl.SelectedIndex = 1;
        }

        private void studentsBtn_Click(object sender, EventArgs e) {
            tabControl.SelectedIndex = 2;
            dataGridViewStudentRecord.Rows.Clear();
            gridViewStudents.Rows.Clear();
        }

        private void addGroupBtn_Click(object sender, EventArgs e) {
            tabControl.SelectedIndex = 0;
        }

        private void groupsBtn_Click(object sender, EventArgs e) {
            tabControl.SelectedIndex = 3;
            groupsGridView.Rows.Clear();
            getgroups();
        }

        private void absenceBtn_Click(object sender, EventArgs e) {
            tabControl.SelectedIndex = 7;
        }

        private void Home_Load(object sender, EventArgs e) {
            tabControl.Appearance = TabAppearance.FlatButtons;
            tabControl.ItemSize = new Size(0, 1);
            tabControl.SizeMode = TabSizeMode.Fixed;
        }

        private void btnAddStudent_Click(object sender, EventArgs e) {
            List<Groups> _groups = data.groups.ToList();
            Student stu = new Student {
                name = nameBox.Text,
                address = addressBox.Text,
                whatsAppNumber = whatsAppBox.Text,
                phone1 = phone1Box.Text,
                phone2 = phone2Box.Text,
                school = schoolBox.Text,
                groupID = _groups[groupCombo.SelectedIndex].ID,
            };
            data.students.Add(stu);
            data.SaveChanges();
            MessageBox.Show("تمت اضافه الطالب/ه بنجاح");
        }

        private void newGroupBtn_Click(object sender, EventArgs e) {
            Gender _gender = (Gender)Enum.Parse(typeof(Gender), genderCombo.Text);
            Models.Day _day = (Models.Day)Enum.Parse(typeof(Models.Day), comboBoxDay.Text);

            Groups gr = new Groups {
                hour = int.Parse(hourBox.Text),
                minutes = int.Parse(minutesBox.Text),
                gender = _gender,
                day = _day
            };
            data.groups.Add(gr);
            data.SaveChanges();
            MessageBox.Show("تمت اضافه المجموعه بنجاح");
            setUp();
        }

        private void btnStudentsSearch_Click(object sender, EventArgs e) {
            getStudents(textBoxStudentsSearch.Text);
        }

        private void gridViewStudents_CellClick(object sender, DataGridViewCellEventArgs e) {
            comboBoxStuGroup.Items.Clear();

            List<Student> Students = data.students.Where(st => st.name.Contains(textBoxStudentsSearch.Text) == true).ToList();
            int col = e.RowIndex, row = e.ColumnIndex;
            Student stu = Students[col];
            editableStudent = stu;

            if (row == 6) {
                tabControl.SelectedIndex = 4;
                textBoxStuName.Text = stu.name;
                textBoxStuWhatsNumber.Text = stu.whatsAppNumber;
                textBoxStuAddress.Text = stu.address;
                textBoxStuFatherPhone.Text = stu.phone2;
                textBoxStuPhone.Text = stu.phone1;
                textBoxStuSchool.Text = stu.school;

                int selectMe = 0, cnt = 0;

                foreach (Groups group in data.groups.ToList()) {
                    string groupName = group.gender.ToString() + "  " + group.day.ToString();
                    groupName += "  " + group.hour.ToString() + ":" + group.minutes.ToString();
                    comboBoxStuGroup.Items.Add(groupName);
                    if (group.ID == stu.groupID)
                        selectMe = cnt;
                    cnt++;
                }
                data.SaveChanges();
            }
            else if (row == 5) {
                tabControl.SelectedIndex = 5;
                dataGridViewStudentRecord.Rows.Clear();
                foreach (var rec in data.dayInfoList.Where(inf => inf.studentID == stu.ID).ToList()) {

                    string date = rec.date.Day.ToString() + "/" + rec.date.Month.ToString() + "/" +
                        rec.date.Year.ToString();
                    dataGridViewStudentRecord.Rows.Add(date, rec.mark, rec.absent == true ? "حاضر" : "غائب", rec.note);
                }
            }
            else if (row == 7) {
                data.students.Remove(stu);
                data.SaveChanges();

                gridViewStudents.Rows.Clear();
                getStudents(textBoxStudentsSearch.Text);

            }
        }

        private void btnStuSavetData_Click(object sender, EventArgs e) {
            Student stu = editableStudent;

            stu.name = textBoxStuName.Text;
            stu.whatsAppNumber = textBoxStuWhatsNumber.Text;
            stu.address = textBoxStuAddress.Text;
            stu.phone2 = textBoxStuFatherPhone.Text;
            stu.phone1 = textBoxStuPhone.Text;
            stu.school = textBoxStuSchool.Text;
            stu.groupID = data.groups.ToList()[comboBoxStuGroup.SelectedIndex].ID;
            data.SaveChanges();
            MessageBox.Show("تم الحفظ");
        }

        private void groupsGridView_CellClick(object sender, DataGridViewCellEventArgs e) {
            int row = e.RowIndex, col = e.ColumnIndex;

            if (col == 5) {

                data.groups.Remove(data.groups.ToList()[row]);
                data.SaveChanges();

                groupsBtn_Click(sender, e);

            }
            if (col == 4) {

                editableGroup = data.groups.ToList()[row];

                textBoxEditGroupHour.Text = editableGroup.hour.ToString();
                textBoxEditGroupMinutes.Text = editableGroup.minutes.ToString();
                comboBoxEditGroupDay.Text = editableGroup.day.ToString();

                tabControl.SelectedIndex = 6;
            }
        }

        private void buttonEditGroupSave_Click(object sender, EventArgs e) {
            editableGroup.hour = int.Parse(textBoxEditGroupHour.Text);
            editableGroup.minutes = int.Parse(textBoxEditGroupMinutes.Text);
            editableGroup.day = (Models.Day)Enum.Parse(typeof(Models.Day), comboBoxEditGroupDay.Text);
        }

        private void comboBoxsSearchByGroup_SelectedIndexChanged(object sender, EventArgs e) {
            Groups group = data.groups.ToList()[comboBoxsSearchByGroup.SelectedIndex];

            gridViewStudents.Rows.Clear();
            List<Student> Students = data.students.Where(st => st.groupID == group.ID).ToList();

            foreach (Student st in Students) {

                if (st.absence == null) {
                    st.absence = new List<absencePair>();
                    st.absence.Add(new absencePair() { absent = true, date = DateTime.Now });
                }
                var info = data.dayInfoList.ToList();

                gridViewStudents.Rows.Add(st.name, st.phone1, st.phone2,
                    group.ToString(), info.Count(inf => inf.studentID == st.ID && inf.absent == false),
                    "معلومات", "تعديل");
            }
            bool color = true;
            foreach (DataGridViewRow row in gridViewStudents.Rows) {
                if (color) row.DefaultCellStyle.BackColor = Color.LightGray;
                else row.DefaultCellStyle.BackColor = Color.LightBlue;
                color = !color;
            }
        }

        private void comboBoxAbsentGroup_SelectedIndexChanged(object sender, EventArgs e) {
            Groups group = data.groups.ToList()[comboBoxAbsentGroup.SelectedIndex];

            dataGridViewAbsent.Rows.Clear();
            List<Student> stus = data.students.Where(st => st.groupID == group.ID).ToList();
            foreach (var st in stus) {
                dataGridViewAbsent.Rows.Add(st.name, "1-");
            }
        }

        private void buttonAbsentSave_Click(object sender, EventArgs e) {
            Groups group = data.groups.ToList()[comboBoxAbsentGroup.SelectedIndex];
            List<Student> stus = data.students.Where(st => st.groupID == group.ID).ToList();

            for (int i = 0; i < dataGridViewAbsent.Rows.Count; i++) {

                dayInformation info = new dayInformation();
                info.date = dateTimePickerAbsent.Value;
                info.mark = int.Parse(dataGridViewAbsent[1, i].Value.ToString());
                info.note = dataGridViewAbsent[3, i].Value.ToString();
                info.absent = !bool.Parse(dataGridViewAbsent[2, i].Value.ToString());
                info.studentID = stus[i].ID;

                data.dayInfoList.Add(info);
                data.SaveChanges();
            }
        }


    }
}
