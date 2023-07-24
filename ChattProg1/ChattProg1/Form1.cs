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

namespace ChattProg1
{
    public partial class Form1 : Form
    {
        TcpClient client = new TcpClient();
        int port = 23456;
        public Form1()
        {
            InitializeComponent();
            client.NoDelay = true;
        }
        public async void StartConnection() 
        {
            try 
            { 
                IPAddress adress = IPAddress.Parse("127.0.0.1");
                await client.ConnectAsync(adress, port);
                button2.Enabled = false;
                button2.BackColor = Color.Green;
            }
            catch (Exception error) 
            { 
                MessageBox.Show(error.Message, Text); return;  
            }
        }

        public async void SendMessage(string message)
        {
            byte[] outData = Encoding.Unicode.GetBytes(message);
            try 
            { 
                await client.GetStream().WriteAsync(outData, 0 , outData.Length);
            }
            catch (Exception error) 
            { 
                MessageBox.Show(error.Message, this.Text);
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
        }
        private void button3_Click(object sender, EventArgs e) // Avsluta
        {
            this.Close();

        }

        private void button1_Click(object sender, EventArgs e) //Skicka 
        {
            if (client.Connected) 
            {
                listBox1.Items.Add("You: " + richTextBox1.Text);
                SendMessage("Client: " + richTextBox1.Text);
                richTextBox1.Clear();
            } else { MessageBox.Show("Du måste vara uppkopplad"); }
        }

        private void button2_Click(object sender, EventArgs e) // Connect
        {
            if (!client.Connected) 
            {
                StartConnection();
                StartListener();
                if (client.Connected) 
                {
                    listBox1.Text = "Du är uppkopplad!";
                }
            }
        }
    }
}
