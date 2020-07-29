using Lessons_Cordinator_beta.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Core.Objects;
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
        List<Student> currentViewStudents = new List<Student>();
        List <dayInformation> currentStudentInfo = new List<dayInformation>();
        dayInformation currentEditInfo = new dayInformation();

        public Home() {
            InitializeComponent();
        }

        void getStudents(string str) {

            List<Student> Students = data.students.Where(st => st.name.Contains(str) == true).ToList();
            List<Groups> _groups = data.groups.ToList();
            currentViewStudents = Students;

            foreach (Student st in Students) {

                if (st.absence == null) {
                    st.absence = new List<absencePair>();
                    st.absence.Add(new absencePair() { absent = true, date = DateTime.Now });
                }
                var info = data.dayInfoList.ToList();

                gridViewStudents.Rows.Add(st.name, st.phone1, st.phone2,
                    _groups.FirstOrDefault(m => m.ID == st.groupID).ToString(),
                    info.Count(inf => inf.studentID == st.ID && inf.absent == false), "معلومات", "تعديل","حذف");
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
            tabControl.SelectedIndex = 1;
            cleanAddStudentTab();
        }

        private void studentsBtn_Click(object sender, EventArgs e) {
            tabControl.SelectedIndex = 2;
            cleanStudentsTab();
        }

        private void addGroupBtn_Click(object sender, EventArgs e) {
            tabControl.SelectedIndex = 0;
            cleanAddGroupTab();
        }

        private void groupsBtn_Click(object sender, EventArgs e) {
            tabControl.SelectedIndex = 3;
            cleanGroupsTab();
            getgroups();
        }

        private void absenceBtn_Click(object sender, EventArgs e) {
            tabControl.SelectedIndex = 7;
            cleanAbsentTab();
        }

        private void Home_Load(object sender, EventArgs e) {
            tabControl.Appearance = TabAppearance.FlatButtons;
            tabControl.ItemSize = new Size(0, 1);
            tabControl.SizeMode = TabSizeMode.Fixed;
            cleanAddGroupTab();
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
            cleanAddStudentTab();
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
        }

        private void btnStudentsSearch_Click(object sender, EventArgs e) {

            string searchCritierta = textBoxStudentsSearch.Text;
            cleanStudentsTab();
            getStudents(searchCritierta);
        }

        private void gridViewStudents_CellClick(object sender, DataGridViewCellEventArgs e) {

            cleanEditStudentTab();

            int col = e.RowIndex, row = e.ColumnIndex;
            Student stu = currentViewStudents[col];
            editableStudent = stu;

            if (row == 6) {
                tabControl.SelectedIndex = 4;
                cleanStudentRecordTab();

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
            }
            else if (row == 5) {
                tabControl.SelectedIndex = 5;
                dataGridViewStudentRecord.Rows.Clear();
                foreach (var rec in data.dayInfoList.Where(inf => inf.studentID == stu.ID).ToList()) {

                    string date = rec.date.Day.ToString() + "/" + rec.date.Month.ToString() + "/" +
                        rec.date.Year.ToString();
                    dataGridViewStudentRecord.Rows.Add(date, rec.mark, rec.absent == true ? "حاضر" : "غائب", rec.note,"تعديل", "حذف");
                }

                bool color = true;
                foreach (DataGridViewRow rr in dataGridViewStudentRecord.Rows) {
                    if (color) rr.DefaultCellStyle.BackColor = Color.LightGray;
                    else rr.DefaultCellStyle.BackColor = Color.LightBlue;
                    color = !color;
                }

                currentStudentInfo = data.dayInfoList.Where(inf => inf.studentID == stu.ID).ToList();
            }
            else if (row == 7) {
                data.students.Remove(stu);
                data.SaveChanges();
                MessageBox.Show("تم الحذف");

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
            studentsBtn_Click(sender, e);
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
                cleanEditGroupTab();
            }
        }

        private void buttonEditGroupSave_Click(object sender, EventArgs e) {
            editableGroup.hour = int.Parse(textBoxEditGroupHour.Text);
            editableGroup.minutes = int.Parse(textBoxEditGroupMinutes.Text);
            editableGroup.day = (Models.Day)Enum.Parse(typeof(Models.Day), comboBoxEditGroupDay.Text);
            data.SaveChanges();
            MessageBox.Show("تم الحفظ");
            groupsBtn_Click(sender, e);
        }

        private void comboBoxsSearchByGroup_SelectedIndexChanged(object sender, EventArgs e) {
            Groups group = data.groups.ToList()[comboBoxsSearchByGroup.SelectedIndex];

            gridViewStudents.Rows.Clear();
            List<Student> Students = data.students.Where(st => st.groupID == group.ID).ToList();
            currentViewStudents = Students;

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
                dataGridViewAbsent.Rows.Add(st.name, "0");
            }
        }

        private void buttonAbsentSave_Click(object sender, EventArgs e) {
            Groups group = data.groups.ToList()[comboBoxAbsentGroup.SelectedIndex];
            List<Student> stus = data.students.Where(st => st.groupID == group.ID).ToList();

            for (int i = 0; i < dataGridViewAbsent.Rows.Count; i++) {

                dayInformation info = new dayInformation();
                info.date = dateTimePickerAbsent.Value;


                if(dataGridViewAbsent[1,i].Value != null)
                    info.mark = int.Parse(dataGridViewAbsent[1, i].Value.ToString());
                else 
                    info.mark = 0;

                if (dataGridViewAbsent[3, i].Value != null)
                    info.note = dataGridViewAbsent[3, i].Value.ToString();
                else
                    info.note = "";

                if (dataGridViewAbsent[2, i].Value != null)
                    info.absent = !bool.Parse(dataGridViewAbsent[2, i].Value.ToString());
                else 
                    info.absent = true;

                info.studentID = stus[i].ID;

                data.dayInfoList.Add(info);
            }
            data.SaveChanges();
            MessageBox.Show("تم الحفظ");
        }
        public void cleanAddGroupTab() {

            comboBoxDay.Items.Clear();
            genderCombo.Items.Clear();

            foreach (Models.Day d in Enum.GetValues(typeof(Models.Day))) {
                comboBoxDay.Items.Add(d);
            }

            hourBox.Text = minutesBox.Text = "";

            foreach (Gender d in Enum.GetValues(typeof(Gender))) {
                genderCombo.Items.Add(d);
            }

        }
        public void cleanAddStudentTab() {

            groupCombo.Items.Clear();

            List<Groups> _groups = data.groups.ToList();
            foreach (Groups g in _groups)
                groupCombo.Items.Add(g.day.ToString() + ", " + g.hour.ToString() + ":" + g.minutes.ToString() + " " + g.gender.ToString());

            nameBox.Text = phone1Box.Text = phone2Box.Text = whatsAppBox.Text = "";
            addressBox.Text = schoolBox.Text = "";
        }
        public void cleanStudentsTab() {

            gridViewStudents.Rows.Clear();
            comboBoxsSearchByGroup.Items.Clear();

            foreach (var g in data.groups.ToList()) {
                comboBoxsSearchByGroup.Items.Add(g.ToString());
            }

            textBoxStudentsSearch.Text = "";
        }
        public void cleanGroupsTab() {
            groupsGridView.Rows.Clear();
        }
        public void cleanEditGroupTab() {

            comboBoxEditGroupDay.Items.Clear();

            foreach (Models.Day d in Enum.GetValues(typeof(Models.Day))) {
                comboBoxEditGroupDay.Items.Add(d);
            }

            textBoxEditGroupHour.Text = textBoxEditGroupMinutes.Text = "";
        }
        public void cleanEditStudentTab() {
            textBoxStuAddress.Text = textBoxStuFatherPhone.Text = textBoxStuName.Text = textBoxStuPhone.Text = "";
            textBoxStuSchool.Text = textBoxStuWhatsNumber.Text = "";

            comboBoxStuGroup.Items.Clear();
        }
        public void cleanAbsentTab() {

            comboBoxAbsentGroup.Items.Clear();
            dataGridViewAbsent.Rows.Clear();

            foreach (var g in data.groups.ToList()) {
                comboBoxAbsentGroup.Items.Add(g.ToString());
            }
        }

        public void cleanStudentRecordTab() {
            dataGridViewStudentRecord.Rows.Clear();
        }

        private void dataGridViewStudentRecord_CellClick(object sender, DataGridViewCellEventArgs e) {

            int col = e.ColumnIndex, row = e.RowIndex;

            if(col == 5) {
                data.dayInfoList.Remove(currentStudentInfo[row]);
                currentStudentInfo.Remove(currentStudentInfo[row]);
                data.SaveChanges();

                dataGridViewStudentRecord.Rows.Clear();
                foreach (var rec in currentStudentInfo) {

                    string date = rec.date.Day.ToString() + "/" + rec.date.Month.ToString() + "/" +
                        rec.date.Year.ToString();
                    dataGridViewStudentRecord.Rows.Add(date, rec.mark, rec.absent == true ? "حاضر" : "غائب", rec.note);
                }
            }  
            else if(col == 4) {
                tabControl.SelectedIndex = 8;

                currentEditInfo = currentStudentInfo[row];

                checkBoxEditRecordAbsent.Checked = !currentEditInfo.absent;
                textBoxEditRecordMark.Text = currentEditInfo.mark.ToString();
                textBoxEditRecordNote.Text = currentEditInfo.note;

            }
        }

        private void buttonEditRecordSave_Click(object sender, EventArgs e) {
            currentEditInfo.absent = !checkBoxEditRecordAbsent.Checked;
            currentEditInfo.mark = double.Parse(textBoxEditRecordMark.Text);
            currentEditInfo.note = textBoxEditRecordNote.Text;
            data.SaveChanges();
            MessageBox.Show("تم الحفظ");

        }
    }
}
