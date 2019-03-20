using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Web;

namespace App_Dev_A2_Client_Calendar
{
    public partial class Form1 : Form
    {
        string httpServerAddress = "http://slimapp/api/";
        string selectedDay;
        string selectedMonth;
        string selectedYear;
        string usersID;

        //bool NameIsHere = false;
        //bool TimeIsHere = false;
        //bool DescriptionIsHere = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            EventNameLabel.Visible = false;
            EventNameTextBox.Visible = false;
            TimeLabel.Visible = false;
            TimeTextBox.Visible = false;
            DescriptionLabel.Visible = false;
            EventDescriptionTextBox.Visible = false;
            monthCalendar1.Visible = false;
            AddEventButton.Visible = false;
            DeleteEventButton.Visible = false;
            SaveEventButton.Visible = false;
        }

        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            GetRequest();
        }

        private void AddEventButton_Click(object sender, EventArgs e)
        {
            RestClient rClient = new RestClient();
            rClient.endPoint = httpServerAddress + "event/add";

            rClient.httpMethod = httpVerb.POST;

            string[] dateArray = monthCalendar1.SelectionRange.Start.ToString().Split('/');
            selectedDay = dateArray[0];
            selectedMonth = dateArray[1];
            selectedYear = dateArray[2];

            rClient.postJSON = "{\"user_id\":\"" + usersID + "\",\"event_name\":\"" + EventNameTextBox.Text + "\",\"event_description\":\"" + EventDescriptionTextBox.Text + "\",\"time\":\"" + TimeTextBox.Text + "\",\"day\":\"" + selectedDay + "\",\"month\":\"" + selectedMonth + "\",\"year\":\"" + selectedYear + "\"}";
            rClient.makeRequest();
            //recievedRequest = recievedRequest.Replace("\n", "\\n");
            //JsonConvert.SerializeObject(rClient.makeRequest(), Formatting.Indented);
            GetRequest();
        }

        private void deserialiseJSON(string strJSON)
        {
            try
            {
                var jPerson = JsonConvert.DeserializeObject<dynamic>(strJSON);

                EventNameTextBox.Text = jPerson.event_name;
                TimeTextBox.Text = jPerson.time;
                EventDescriptionTextBox.Text = jPerson.event_description;
            }
            catch (Exception ex)
            {
                EventDescriptionTextBox.Text = "error occoured: " + ex.Message.ToString();
                EventDescriptionTextBox.Text = string.Empty;
            }
        }

        private void GetRequest()
        {
            ClearTextBoxes();

            RestClient rClient = new RestClient();
            rClient.httpMethod = httpVerb.GET;

            string[] dateArray = monthCalendar1.SelectionRange.Start.ToString().Split('/');
            selectedDay = dateArray[0];
            selectedMonth = dateArray[1];
            selectedYear = dateArray[2];
            selectedYear = selectedYear.Substring(0, 4);

            rClient.endPoint = httpServerAddress + "event/" + usersID + "/" + selectedDay + "/" + selectedMonth + "/" + selectedYear;

            string strResponse = string.Empty;
            strResponse = rClient.makeRequest();

            deserialiseJSON(strResponse);

            //strResponse = strResponse.Replace("\r\n", "\n");
            //strResponse = strResponse.Replace("\n", "\\n");

            if (EventNameTextBox.Text == string.Empty && TimeTextBox.Text == string.Empty && EventDescriptionTextBox.Text == string.Empty)
            {
                AddEventButton.Enabled = true;
                SaveEventButton.Enabled = false;
                DeleteEventButton.Enabled = false;
            }
            else
            {
                AddEventButton.Enabled = false;
                SaveEventButton.Enabled = true;
                DeleteEventButton.Enabled = true;
            }
        }

        private void ClearTextBoxes()
        {
            EventNameTextBox.Text = null;
            TimeTextBox.Text = null;
            EventDescriptionTextBox.Text = null;
        }

        private void SaveEventButton_Click(object sender, EventArgs e)
        {
            RestClient rClient = new RestClient();

            rClient.httpMethod = httpVerb.PUT;

            string[] dateArray = monthCalendar1.SelectionRange.Start.ToString().Split('/');
            selectedDay = dateArray[0];
            selectedMonth = dateArray[1];
            selectedYear = dateArray[2];
            selectedYear = selectedYear.Substring(0, 4);

            rClient.endPoint = httpServerAddress + "event/update/" + usersID + "/" + selectedDay + "/" + selectedMonth + "/" + selectedYear;

            rClient.putJSON = "{\"user_id\":\"" + usersID + "\",\"event_name\":\"" + EventNameTextBox.Text + "\",\"event_description\":\"" + EventDescriptionTextBox.Text + "\",\"time\":\"" + TimeTextBox.Text + "\",\"day\":\"" + selectedDay + "\",\"month\":\"" + selectedMonth + "\",\"year\":\"" + selectedYear + "\"}";
            rClient.makeRequest();
            GetRequest();
        }

        private void DeleteEventButton_Click(object sender, EventArgs e)
        {
            RestClient rClient = new RestClient();

            rClient.httpMethod = httpVerb.DELETE;

            string[] dateArray = monthCalendar1.SelectionRange.Start.ToString().Split('/');
            selectedDay = dateArray[0];
            selectedMonth = dateArray[1];
            selectedYear = dateArray[2];
            selectedYear = selectedYear.Substring(0, 4);

            rClient.endPoint = httpServerAddress + "deleteEvent/" + usersID + "/" + selectedDay + "/" + selectedMonth + "/" + selectedYear;

            rClient.deleteJSON = string.Empty;
            rClient.makeRequest();
            GetRequest();
        }

        private void EventNameTextBox_TextChanged(object sender, EventArgs e)
        {
            //if (EventNameTextBox.Text == string.Empty)
            //{
            //    NameIsHere = false;
            //    checkBox1.Checked = false;
            //}
            //else
            //{
            //    NameIsHere = true;
            //    checkBox1.Checked = true;
            //}
        }

        private void TimeTextBox_TextChanged(object sender, EventArgs e)
        {
            //if (TimeTextBox.Text == string.Empty)
            //{
            //    TimeIsHere = false;
            //    checkBox2.Checked = false;
            //}
            //else
            //{
            //    TimeIsHere = true;
            //    checkBox2.Checked = true;
            //}
        }

        private void EventDescriptionTextBox_TextChanged(object sender, EventArgs e)
        {
            //if (EventDescriptionTextBox.Text == string.Empty)
            //{
            //    DescriptionIsHere = false;
            //    checkBox3.Checked = false;
            //}
            //else
            //{
            //    DescriptionIsHere = true;
            //    checkBox3.Checked = true;
            //}
        }

        private void UserAuthenticationButton_Click(object sender, EventArgs e)
        {
            string InputUser = UserNameTextBox.Text;
            string InputPass = UserPasswordTextBox.Text;

            InputUser = string.Format("\"{0}\"", InputUser);
            InputPass = string.Format("\"{0}\"", InputPass);

            RestClient rClient = new RestClient();
            rClient.httpMethod = httpVerb.GET;

            rClient.endPoint = httpServerAddress + "user/" + InputUser + "/" + InputPass;

            string strResponse = string.Empty;
            strResponse = rClient.makeRequest();

            deserialiseUserJSON(strResponse);
        }

        private void deserialiseUserJSON(string strJSON)
        {
            try
            {
                var jPerson = JsonConvert.DeserializeObject<dynamic>(strJSON);

                usersID = jPerson.id;
                DisplayNameLabel.Text = "Welcome " + jPerson.name;

                EventNameLabel.Visible = true;
                EventNameTextBox.Visible = true;
                TimeLabel.Visible = true;
                TimeTextBox.Visible = true;
                DescriptionLabel.Visible = true;
                EventDescriptionTextBox.Visible = true;
                monthCalendar1.Visible = true;
                AddEventButton.Visible = true;
                DeleteEventButton.Visible = true;
                SaveEventButton.Visible = true;

                GetRequest();
            }
            catch (Exception ex)
            {
                EventDescriptionTextBox.Text = "error occoured: " + ex.Message.ToString();
            }
        }
    }
}

class JSONAttributes
{
    public string eventName { get; set; }
    public string eventDescription { get; set; }
    public string time { get; set; }
    public string day { get; set; }
    public string month { get; set; }
    public string year { get; set; }
}