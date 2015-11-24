using System;
using System.Windows.Forms;
using ServiceStack.Redis;
using System.IO;

namespace redis_tools
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Now restore everything back
            RedisClient client = new RedisClient(txtHost.Text, 6379, txtAuth.Text.Trim(), int.Parse(txtDBNo.Text));
            var reader = new StreamReader("dump.txt");
            int i = 1;
            while (true)
            {
                string key = reader.ReadLine();
                if (key == null)
                    break;
                Byte[] value = File.ReadAllBytes(i.ToString() + ".txt");
                client.Restore(key, 0, value);
                i++;
            }
            MessageBox.Show("Redis has been successfully restored.");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // This is to dump everything
            RedisClient client = new RedisClient(txtHost.Text, 6379, txtAuth.Text.Trim(), int.Parse(txtDBNo.Text));
            var all_keys = client.GetAllKeys();
            var sw = new StreamWriter("dump.txt");
            int i = 1;
            foreach (var key in all_keys)
            {
                //write key
                sw.WriteLine(key);
                var value = client.Dump(key);
                var file = File.OpenWrite(i.ToString() + ".txt");
                file.Write(value, 0, value.Length);
                file.Close();
                i++;
            }
            sw.Close();
            MessageBox.Show("Done");
        }
    }
}
