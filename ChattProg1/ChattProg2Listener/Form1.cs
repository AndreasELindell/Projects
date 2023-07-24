using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Threading;
using System.Net.Sockets;

namespace ChattProg2Listener
{
    public partial class Form1 : Form
    {
        TcpClient client = new TcpClient();
        TcpListener server;
        IPAddress ipadress = IPAddress.Parse("127.0.0.1");
        int port = 23456;
        public void StartServer() 
        {
            try
            {
                server = new TcpListener(ipadress, port);
                server.Start();
                button2.Enabled = false;
                button2.BackColor = Color.Green;
                button2.Text = "Uppkopplad!";
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, Text);
                button2.Text = "FEL";
                button2.BackColor = Color.Red;
                return;
            }
        }
        public async void StartReceiver()
        {
            try
            {
                client = await server.AcceptTcpClientAsync();
                listView1.Items.Add(client.ToString());
                StartListener();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, Text);
                return;
            }
        }
        public async void StartListener()
        {
            byte[] receivedMessage = new byte[1024];
            try
            {
                while (true) //Loop för att fortsätta lyssna
                {
                    int i = await client.GetStream().ReadAsync(receivedMessage, 0, receivedMessage.Length);
                    string nosMsg = Encoding.Unicode.GetString(receivedMessage, 0, i);
                    listBox1.Items.Add(nosMsg);
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, Text);
                return;
            }
            finally
            {
                server.Stop();
            }
        }
        private async void SendMessage(string message)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(message);
            try
            {
                await client.GetStream().WriteAsync(bytes, 0, bytes.Length);
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, Text);
                return;
            }
        }
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) // Skicka
        {
            
                listBox1.Items.Add("You: " + richTextBox1.Text);
                SendMessage("Server: " + richTextBox1.Text);
                richTextBox1.Clear();
        }

        private void button2_Click(object sender, EventArgs e) // Starta server
        {
            StartServer();
            StartReceiver();
        }

        private void button3_Click(object sender, EventArgs e) //Avsluta
        {
            this.Close();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
