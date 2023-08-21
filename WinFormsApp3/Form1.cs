using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace WinFormsApp3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            try
            {
                con = new
                SqlConnection("server=localhost;database=Musteriler; Trusted_connection=True;TrustServerCertificate=true;");
            }
            catch (Exception h)
            {

            }
        }
        SqlConnection con;
        public static Boolean IsTableExistInDb(string tableName, SqlConnection Con)
        {

            try
            {


                SqlCommand Cmd = new SqlCommand("SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = " + "'" + tableName + "'", Con);
                Con.Open();
                Object result = Cmd.ExecuteScalar();
                Con.Close();

                if (result != null && byte.Parse(result.ToString()) == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return true;

            }
            finally
            {
                if (Con.State != ConnectionState.Closed)
                {
                    Con.Close();

                }
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {

            SqlConnection connection;
            try
            {
                connection = new SqlConnection("Server=localhost; Integrated Security=SSPI; database=master");
                //SqlCommand command = new SqlCommand("Create Database if not exists DonerciOtomasyonu", connection);
                SqlCommand command = new SqlCommand($"If(db_id(N'Musteriler') IS NULL) CREATE DATABASE [Musteriler]", connection);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();

            }
            catch (Exception ex)
            {


            }
            finally
            {

            }

            try
            {

                string query = "Create table dbo.Musteriler" +
                    "(" +
                    "[Müþteri No]  nvarchar(6) not null primary key," +
                    "[Adý] nvarchar(50) not null," +
                    "[Soyadý] nvarchar(50) not null," +
                    "[Telefon Numarasý] nvarchar(50) not null," +
                    "[Adresi] nvarchar(300) not null" +
                    ")";
                SqlCommand command1 = new SqlCommand(query, con);
                if (!IsTableExistInDb("dbo.Musteriler", con))
                {
                    con.Open();
                    command1.ExecuteNonQuery();

                }
            }
            catch (Exception ex)
            {



            }
            finally
            {
                if (con.State != ConnectionState.Closed)
                {
                    con.Close();
                }
            }
            try
            {
                con.Open();
                SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Musteriler", con);
                DataTable table = new DataTable();
                da.Fill(table);
                //dataGridView1.AutoGenerateColumns = false;
                dataGridView1.DataSource = table;



            }
            catch (Exception a)
            {

            }
            finally
            {
                if (con.State != ConnectionState.Closed)
                {
                    con.Close();
                }
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool musteriNoZatenVar = false;
            bool musteriNoFormataUygunDegil = false;
            bool musteriAdiFormataUygunDegil = false;
            bool musteriSoyadiFormataUygunDegil = false;
            bool musteriTelefonNumarasýFormataUygunDegil = false;
            bool musteriAdresiFormataUygunDegil = false;
            try
            {
                SqlCommand com = new SqlCommand("Select * from Musteriler", con);

                con.Open();
                SqlDataReader dr = com.ExecuteReader();
                while (dr.Read())
                {
                    if (dr["Müþteri No"].ToString() == textBox1.Text)
                    {
                        MessageBox.Show("Müþteri numarasý baþka bir müþteriye ait");
                        musteriNoZatenVar = true;
                    }
                }


            }
            catch (Exception ex) { }
            finally
            {
                if (con.State != ConnectionState.Closed)
                {
                    con.Close();
                }
            }
            if (!Regex.IsMatch(textBox1.Text, "^[0-9]{6}$"))
            {
                MessageBox.Show("Müþteri No formata uygun deðil, lütfen müþteri no olarak 6 haneli bir rakam giriniz");
                musteriNoFormataUygunDegil = true;
            }
            if (!Regex.IsMatch(textBox2.Text, "^[öüÖÜÐðþÞçÇýÝ|a-z|A-Z]{2,20}(\\s[öüÖÜÐðþÞçÇýÝ|a-z|A-Z]{2,20})?$"))
            {
                MessageBox.Show("Müþteri adý formata uygun deðil");
                musteriAdiFormataUygunDegil = true;
            }
            if (!Regex.IsMatch(textBox3.Text, "^[öüÖÜÐðþÞçÇýÝa-zA-Z]{2,20}$"))
            {
                MessageBox.Show("Müþteri soyadý formata uygun deðil");
                musteriSoyadiFormataUygunDegil = true;
            }
            if (!Regex.IsMatch(textBox4.Text, "^[0]?[0-9]{3}[0-9]{3}[0-9]{2}[0-9]{2}$"))
            {
                MessageBox.Show("Müþteri telefon numarasý formata uygun deðil");
                musteriTelefonNumarasýFormataUygunDegil = true;
            }
            if (
                musteriNoZatenVar == false
                && musteriNoFormataUygunDegil == false
                && musteriAdiFormataUygunDegil == false
                && musteriSoyadiFormataUygunDegil == false
                && musteriTelefonNumarasýFormataUygunDegil == false
                && musteriAdresiFormataUygunDegil == false
               )
            {
                try
                {



                    SqlCommand cmd2 = new SqlCommand("Insert into Musteriler values " +
                        "(@musterino,@adi,@soyadi," +
                        " @telefonnumarasi,@adresi)", con);
                    cmd2.Parameters.AddWithValue("@musterino", textBox1.Text);
                    cmd2.Parameters.AddWithValue("@adi", textBox2.Text);
                    cmd2.Parameters.AddWithValue("@soyadi", textBox3.Text);
                    cmd2.Parameters.AddWithValue("@telefonnumarasi", textBox4.Text);
                    cmd2.Parameters.AddWithValue("@adresi", textBox5.Text);
                    con.Open();
                    cmd2.ExecuteNonQuery();






                }
                catch (Exception a)
                {

                }
                finally
                {
                    if (con.State != ConnectionState.Closed)
                    {
                        con.Close();
                    }
                }
                try
                {
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Musteriler", con);
                    DataTable table = new DataTable();
                    da.Fill(table);
                    //dataGridView1.AutoGenerateColumns = false;
                    dataGridView1.DataSource = table;
                    MessageBox.Show("Kayýt iþlemi baþarýlý");
                    textBox1.Text = "";
                    textBox2.Text = "";
                    textBox3.Text = "";
                    textBox4.Text = "";
                    textBox5.Text = "";
                    textBox6.Text = "";

                }
                catch (Exception a)
                {

                }
                finally
                {
                    if (con.State != ConnectionState.Closed)
                    {
                        con.Close();
                    }
                }
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            bool musteriNoZatenVar = false;
            bool musteriNoFormataUygunDegil = false;
            bool musteriAdiFormataUygunDegil = false;
            bool musteriSoyadiFormataUygunDegil = false;
            bool musteriTelefonNumarasiFormataUygunDegil = false;
            bool musteriAdresiFormataUygunDegil = false;
            bool musteriNoFormataUygunDegil2 = false;
            bool musteriAdiFormataUygunDegil2 = false;
            bool musteriSoyadiFormataUygunDegil2 = false;
            bool musteriTelefonNumarasiFormataUygunDegil2 = false;
            bool musteriAdresiFormataUygunDegil2 = false;
            string musteriadi = "";
            string musterisoyadi = "";
            string musteritelefon = "";
            string musteriadresi = "";
            string musteriadi2 = "";
            string musterisoyadi2 = "";
            string musteritelefon2 = "";
            string musteriadresi2 = "";
            try
            {
                SqlCommand com = new SqlCommand("Select * from Musteriler", con);

                con.Open();
                SqlDataReader dr = com.ExecuteReader();
                while (dr.Read())
                {
                    if (dr["Müþteri No"].ToString() == textBox1.Text)
                    {

                        musteriNoZatenVar = true;
                    }
                }
                if (musteriNoZatenVar == false)
                {
                    MessageBox.Show("Müþteri Numarasý Bulunamadý");
                }


            }
            catch (Exception ex) { }
            finally
            {
                if (con.State != ConnectionState.Closed)
                {
                    con.Close();
                }
            }

            if (!Regex.IsMatch(textBox1.Text, "^[0-9]{6}$") && textBox1.Text != "")
            {
                MessageBox.Show("Müþteri No formata uygun deðil, lütfen müþteri no olarak 6 haneli bir rakam giriniz");

            }
            if (!Regex.IsMatch(textBox2.Text, "^[öüÖÜÐðþÞçÇýÝ|a-z|A-Z]{2,20}(\\s[öüÖÜÐðþÞçÇýÝ|a-z|A-Z]{2,20})?$") && textBox2.Text != "")
            {
                MessageBox.Show("Müþteri adý formata uygun deðil");

            }
            if (!Regex.IsMatch(textBox3.Text, "^[öüÖÜÐðþÞçÇýÝa-zA-Z]{2,20}$") && textBox3.Text != "")
            {
                MessageBox.Show("Müþteri soyadý formata uygun deðil");

            }
            if (!Regex.IsMatch(textBox4.Text, "^[0]?[0-9]{3}[0-9]{3}[0-9]{2}[0-9]{2}$") && textBox4.Text != "")
            {
                MessageBox.Show("Müþteri telefon numarasý formata uygun deðil");

            }

            if (!Regex.IsMatch(textBox1.Text, "^[0-9]{6}$"))
            {

                musteriNoFormataUygunDegil = true;
            }
            if (!Regex.IsMatch(textBox2.Text, "^[öüÖÜÐðþÞçÇýÝ|a-z|A-Z]{2,20}(\\s[öüÖÜÐðþÞçÇýÝ|a-z|A-Z]{2,20})?$"))
            {

                musteriAdiFormataUygunDegil = true;
            }
            if (!Regex.IsMatch(textBox3.Text, "^[öüÖÜÐðþÞçÇýÝa-zA-Z]{2,20}$"))
            {

                musteriSoyadiFormataUygunDegil = true;
            }
            if (!Regex.IsMatch(textBox4.Text, "^[0]?[0-9]{3}[0-9]{3}[0-9]{2}[0-9]{2}$"))
            {

                musteriTelefonNumarasiFormataUygunDegil = true;
            }
            if (musteriNoZatenVar == true)
            {
                try
                {
                    SqlCommand com = new SqlCommand("Select * from Musteriler where [Müþteri No]=@mn", con);
                    com.Parameters.AddWithValue("@mn", textBox1.Text);
                    con.Open();
                    SqlDataReader dr = com.ExecuteReader();
                    while (dr.Read())
                    {
                        if (dr["Müþteri No"].ToString() == textBox1.Text)
                        {
                            musteriadi = dr["Adý"].ToString();
                            musterisoyadi = dr["Soyadý"].ToString();
                            musteritelefon = dr["Telefon Numarasý"].ToString();
                            musteriadresi = dr["Adresi"].ToString();
                        }
                    }


                }
                catch (Exception ex) { }
                finally
                {
                    if (con.State != ConnectionState.Closed)
                    {
                        con.Close();
                    }
                }
            }
            if (textBox2.Text == "")
            { musteriadi2 = musteriadi; }
            else if (textBox2.Text != "" && musteriAdiFormataUygunDegil == false)
            { musteriadi2 = textBox2.Text; }
            else if (textBox2.Text != "" && musteriAdiFormataUygunDegil == true)
            {
                musteriAdiFormataUygunDegil2 = true;
            }
            if (textBox3.Text == "")
            { musterisoyadi2 = musterisoyadi; }
            else if (textBox3.Text != "" && musteriSoyadiFormataUygunDegil == false)
            { musterisoyadi2 = textBox3.Text; }
            else if (textBox2.Text != "" && musteriSoyadiFormataUygunDegil == true)
            {
                musteriSoyadiFormataUygunDegil2 = true;
            }
            if (textBox4.Text == "")
            { musteritelefon2 = musteritelefon; }
            else if (textBox4.Text != "" && musteriTelefonNumarasiFormataUygunDegil == false)
            { musteritelefon2 = textBox4.Text; }
            else if (textBox2.Text != "" && musteriTelefonNumarasiFormataUygunDegil == true)
            {
                musteriTelefonNumarasiFormataUygunDegil2 = true;
            }
            if (textBox5.Text == "")
            { musteriadresi2 = musteriadresi; }
            else if (textBox5.Text != "" && musteriAdresiFormataUygunDegil == false)
            { musteriadresi2 = textBox5.Text; }
            else if (textBox2.Text != "" && musteriAdresiFormataUygunDegil == true)
            {
                musteriAdresiFormataUygunDegil2 = true;
            }
            if (
                musteriNoZatenVar == true
                && musteriNoFormataUygunDegil2 == false
                && musteriAdiFormataUygunDegil2 == false
                && musteriSoyadiFormataUygunDegil2 == false
                && musteriTelefonNumarasiFormataUygunDegil2 == false
                && musteriAdresiFormataUygunDegil2 == false
               )
            {
                try
                {



                    SqlCommand cmd2 = new SqlCommand("Update Musteriler set " +
                        "[Adý]=@adi,[Soyadý]=@soyadi," +
                        " [Telefon Numarasý]=@telefonnumarasi,[Adresi]=@adresi where " +
                        "[Müþteri No]=@musterino", con);
                    cmd2.Parameters.AddWithValue("@musterino", textBox1.Text);
                    cmd2.Parameters.AddWithValue("@adi", musteriadi2);
                    cmd2.Parameters.AddWithValue("@soyadi", musterisoyadi2);
                    cmd2.Parameters.AddWithValue("@telefonnumarasi", musteritelefon2);
                    cmd2.Parameters.AddWithValue("@adresi", musteriadresi2);
                    con.Open();
                    cmd2.ExecuteNonQuery();






                }
                catch (Exception a)
                {

                }
                finally
                {
                    if (con.State != ConnectionState.Closed)
                    {
                        con.Close();
                    }
                }
                try
                {
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Musteriler", con);
                    DataTable table = new DataTable();
                    da.Fill(table);
                    //dataGridView1.AutoGenerateColumns = false;
                    dataGridView1.DataSource = table;
                    MessageBox.Show("Güncelleme iþlemi baþarýlý");
                    textBox1.Text = "";
                    textBox2.Text = "";
                    textBox3.Text = "";
                    textBox4.Text = "";
                    textBox5.Text = "";
                    textBox6.Text = "";

                }
                catch (Exception a)
                {

                }
                finally
                {
                    if (con.State != ConnectionState.Closed)
                    {
                        con.Close();
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            bool musteriNoZatenVar = false;
            bool musteriNoFormataUygunDegil = false;
            bool musteriAdiFormataUygunDegil = false;
            bool musteriSoyadiFormataUygunDegil = false;
            bool musteriTelefonNumarasýFormataUygunDegil = false;
            bool musteriAdresiFormataUygunDegil = false;
            try
            {
                SqlCommand com = new SqlCommand("Select * from Musteriler", con);

                con.Open();
                SqlDataReader dr = com.ExecuteReader();
                while (dr.Read())
                {
                    if (dr["Müþteri No"].ToString() == textBox1.Text)
                    {

                        musteriNoZatenVar = true;
                    }
                }
                if (musteriNoZatenVar == false)
                {
                    MessageBox.Show("Müþteri Numarasý Bulunamadý");
                }


            }
            catch (Exception ex) { }
            finally
            {
                if (con.State != ConnectionState.Closed)
                {
                    con.Close();
                }
            }
            if (musteriNoZatenVar == true)
            {
                DialogResult secim = new DialogResult();
                secim = MessageBox.Show("Bu iþlem yapýlýrsa müþteri kayýtlardan silinecektir\nEmin misiniz?", "UYARI",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (secim == DialogResult.No)
                {

                }
                if (secim == DialogResult.Yes)
                {
                    try
                    {



                        SqlCommand cmd2 = new SqlCommand("Delete from Musteriler where " +
                            "[Müþteri No]=@musterino", con);
                        cmd2.Parameters.AddWithValue("@musterino", textBox1.Text);
                        con.Open();
                        cmd2.ExecuteNonQuery();






                    }
                    catch (Exception a)
                    {

                    }
                    finally
                    {
                        if (con.State != ConnectionState.Closed)
                        {
                            con.Close();
                        }
                    }
                    try
                    {
                        con.Open();
                        SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Musteriler", con);
                        DataTable table = new DataTable();
                        da.Fill(table);
                        //dataGridView1.AutoGenerateColumns = false;
                        dataGridView1.DataSource = table;
                        MessageBox.Show("Silme iþlemi baþarýlý");
                        textBox1.Text = "";
                        textBox2.Text = "";
                        textBox3.Text = "";
                        textBox4.Text = "";
                        textBox5.Text = "";
                        textBox6.Text = "";


                    }
                    catch (Exception a)
                    {

                    }
                    finally
                    {
                        if (con.State != ConnectionState.Closed)
                        {
                            con.Close();
                        }
                    }
                }
            }
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            string aramaparametresi = "";

            if (radioButton1.Checked == true) aramaparametresi = "[Müþteri No]";
            else if (radioButton2.Checked == true) aramaparametresi = "Adý";
            else if (radioButton3.Checked == true) aramaparametresi = "Soyadý";
            else if (radioButton4.Checked == true) aramaparametresi = "[Telefon Numarasý]";
            else if (radioButton5.Checked == true) aramaparametresi = "Adresi";

            try
            {
                con.Open();
                string[] dizi = textBox6.Text.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                int i = 1;
                string addcommand = "";
                while(i<dizi.Length)
                {
                    if (i < dizi.Length)
                    { addcommand += " and " + aramaparametresi + " like '%" + dizi[i] + "%'"; }
                    i++;
                }
                string command = "SELECT * FROM Musteriler where " + aramaparametresi + " like '%" + dizi[0] + "%'" + addcommand;
                //string command = "SELECT * FROM Musteriler where " + aramaparametresi + " like '%" + textBox6.Text + "%'";

                SqlDataAdapter da = new SqlDataAdapter(command, con);

                DataTable table = new DataTable();
                da.Fill(table);
                //dataGridView1.AutoGenerateColumns = false;
                dataGridView1.DataSource = table;




            }
            catch (Exception a)
            {

            }
            finally
            {
                if (con.State != ConnectionState.Closed)
                {
                    con.Close();
                }
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            textBox6.Text = "";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            textBox6.Text = "";
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            textBox6.Text = "";
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            textBox6.Text = "";
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            textBox6.Text = "";
        }
    }
}