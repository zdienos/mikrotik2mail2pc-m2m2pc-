﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace mail_mikrotik
{
public partial class Form1 : Form
    {
        string path_log = @"E:\!email-mikrotik\";
        string path_log_out = @"E:\!email-mikrotik\!!!out\";
        string path_conf_file = @"E:\!email-mikrotik\!!!out\hosts.conf";
        public Form1()
        {
            InitializeComponent();
        }
        // перенос файлов из одного каталога в другой
public void move2path(string @name_old,string @path)
        {    //откуда копируем
            string Dir1 = @path;
            //куда копируем
            string Dir2 = @path + @"\" + @name_old;
            if (!Directory.Exists(Dir2)) Directory.CreateDirectory(Dir2);
               try
              {
                  DirectoryInfo dirInfo = new DirectoryInfo(Dir1);
                  foreach (FileInfo file in dirInfo.GetFiles(name_old + "*.txt"))
                  {File.Move(file.FullName, @Dir2 + @"\" + file.Name);}
              }
              catch (Exception ex)
              { MessageBox.Show(ex.Message+Environment.NewLine); }
        }
private void button1_Click(object sender, EventArgs e)
        {         
            string path_m = @"E:\!Source\Repos\mail_mikrotik\console_mail2dir\mail2dirr\mail2dirr\bin\Debug\mail2dirr.exe";
            if(File.Exists(path_m)) //запускаем клиента получения вложений почты
            { var startInfo = new System.Diagnostics.ProcessStartInfo
            { FileName = path_m,// + @" /dir "+@path_log,  // Путь к приложению
            Arguments= @" /dir " + @path_log,
            UseShellExecute = false, CreateNoWindow = true};
            System.Diagnostics.Process.Start(startInfo);
                // label2.Text = DateTime.Now.ToString()+"_OK";
            }           
            timer1.Enabled = true;//animation
            timer2.Enabled = true;// wait file "mail_OK"
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            System.IO.DirectoryInfo info = new System.IO.DirectoryInfo(@path_log);
            System.IO.DirectoryInfo[] dirs = info.GetDirectories();
            System.IO.FileInfo[] files = info.GetFiles();
            //foreach (string s in files.name) label1.Text += s;
            //создание таблицы с именами всех фыйлов, разделенных на столбцы по знаку "_"
            // name_date_time_uptimeHour_uptimeMinutes_uptimeSecund
            DataTable dt = new DataTable("tab0");
            int st = 0;
            //dt.Clear();
            //dt = new DataTable("tab0");
            DataColumn a0 = new DataColumn(st++.ToString(), typeof(String));
            DataColumn a1 = new DataColumn(st++.ToString(), typeof(String));
            DataColumn a2 = new DataColumn(st++.ToString(), typeof(String));
            DataColumn a3 = new DataColumn(st++.ToString(), typeof(String));
            DataColumn a4 = new DataColumn(st++.ToString(), typeof(String));
            DataColumn a5 = new DataColumn(st++.ToString(), typeof(String));
            DataColumn a6 = new DataColumn(st++.ToString(), typeof(String));
            // download_upload
            DataColumn a7 = new DataColumn(st++.ToString(), typeof(String));
            DataColumn a8 = new DataColumn(st++.ToString(), typeof(String));
            DataColumn a9 = new DataColumn(st++.ToString(), typeof(String));
            DataColumn a10 = new DataColumn(st++.ToString(), typeof(String));
            dt.Columns.AddRange(new DataColumn[] { a0, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10 });

            string[] tab0Values = null;
            DataRow dr = null;
            /////////////////////////////////


            for (int i = 0; i < files.Length; i++)
            {
                textBox2.Text += files[i].Name + Environment.NewLine;
                tab0Values = files[i].Name.Split('_');
                dr = dt.NewRow();
                for (int ii = 0; ii < 6; ii++) { dr[ii] = tab0Values[ii]; }
                //считываем содержимое файла              
                /* формат файла
                 Flags: D - dynamic, X - disabled, R - running, S - slave 
                 #     NAME               RX-BYTE           TX-BYTE     RX-PACKET     TX-PACKET
                 0  R  Lan         71 292 359 093 1 034 318 413 528   322 675 085   780 861 923
                 1  R  Sky      1 064 572 143 276    83 572 080 930   786 977 972   327 838 501
                 2  X  ether3                   0                 0             0             0
                 3  X  ether4                   0                 0             0             0
                 4  X  globa...                 0                 0             0             0
                 5  X  globa...                 0                 0             0             0
                 6  R  pptp-... 1 020 807 199 464    63 961 725 927   784 864 546   327 233 856
                 идея два пробела меняем на знак подчеркивания, 
                 1. разделить на столбцы, понятно что столбец 4-5-3-7 заканчивается 
                 на количество символов, равное на заголовок 2-й строки
                 #     NAME               RX-BYTE           TX-BYTE     RX-PACKET     TX-PACKET
                фиксированная ширина поля 
                2 6 14 34 52 66 80
                 *///
                string[] readText = File.ReadAllLines(path_log+ files[i].Name);
                for(int ii=0;ii<readText.Length;ii++)
                {// ищем строку с номером интерфейса из конфига и считываем поле 34 и 52
   //!!! no working
                    int j;
                 //   for (j = 0; j < dataGridView2.RowCount; j++)
                   //     if ((dataGridView2.Rows[1].Cells[j].ToString() == readText[0])
                     //       && (dataGridView2.Rows[1].Cells[j].ToString()==readText[0])
                       //     ) dr[6] = dataGridView2.Rows[1].Cells[j].ToString().Substring(14,20);
                  //  if (readText[ii].IndexOf("name:") >0) dr[6] = readText[ii];
                  //if (readText[ii].IndexOf("driver-rx-byte:") > 0) dr[7] = readText[ii];
                  //if (readText[ii].IndexOf("tx-bytes:") > 0) dr[8] = readText[ii];
                 }
                 dt.Rows.Add(dr);}
                 dataGridView1.DataSource = dt;
                 // подумать если пусто
                 if(dt.Rows.Count>0)
                {//записываем текущие данные
                string dates = DateTime.Now.ToString();
                dates = dates.Replace(" ", "_");dates = dates.Replace(":", "-");dates = dates.Replace(".", "_");
                if (!Directory.Exists(@path_log_out)) Directory.CreateDirectory(@path_log_out);
                label4.Text = dates; dt.WriteXml(@path_log_out + dates + @"out.xml");
                // ищем уникальніе имена
                string name_u =dt.Rows[0][0].ToString(),name_old = "";
                for(int i=0;i<dt.Rows.Count;i++)
                 {name_u = dt.Rows[i][0].ToString();
                  if (name_u != name_old)
                  { name_old = name_u;
                        //label3.Text += name_old;
                        //кидаем в соотвующую дирректорию
                        /////////////////////
                        //откуда копируем
                        move2path(name_old, path_log);
                      }
            /////////////
        }}}

                        private void button5_Click(object sender, EventArgs e)
                        {
                        }
                        //string zagruz = "загрузка................загрузка............загрузка......",za;
                        string zagruz = "загрузка................загрузка............", za;
                        int i = 0;

                        private void timer2_Tick(object sender, EventArgs e)
                        {
                            string path_f = path_log+"ok.ok";
                            if (File.Exists(path_f))
                            {
                                File.Delete(path_f);
                                timer1.Enabled = false;
                                timer2.Enabled = false;
                                label3.Text = "Данные принято от_" + DateTime.Now.ToString();
                            }
                        }

        private void button6_Click(object sender, EventArgs e)
        {
            dataGridView1.Columns.Clear();
            System.IO.DirectoryInfo info = new System.IO.DirectoryInfo(@path_log);
            System.IO.DirectoryInfo[] dirs = info.GetDirectories();
            //формируем таблицу для ссотвевий
            DataTable dt = new DataTable("tab0");
            int st = 0;
            //dt.Clear();
            //dt = new DataTable("tab0");
            DataColumn a0 = new DataColumn(st++.ToString(), typeof(String));
            a0.ColumnName = "хост";
            DataColumn a1 = new DataColumn(st++.ToString(), typeof(String));
            a1.ColumnName = "интерфейс";
            DataColumn a2 = new DataColumn(st++.ToString(), typeof(String));         
            dt.Columns.AddRange(new DataColumn[] { a0, a1, a2});
            string[] tab0Values = null;
            DataRow dr = null;
            //   не работает...

            for (int i = 1; i < dirs.Length; i++)
            {
                //tab0Values[0] = dirs[i].Name;
                dr = dt.NewRow();
                dr[0] = dirs[i].Name;
                //ищкем в конфиге это имя, и добавляем в таблицу параметр для него
                // а е сли не пришло сообщения?!"?
                //думем....


                dt.Rows.Add(dr);
            }
           dataGridView2.DataSource = dt;
            writeCSV(dataGridView2, path_conf_file);
            // + Environment.NewLine;
            // считываем файл !!!!!! пока не рабоает...
            
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            init_config(dataGridView2);
        }

        private void timer1_Tick(object sender, EventArgs e)
                        {
                            za = zagruz.Substring(i++, 20);
                            //if (flag == 1) i++; else i--;
                            if (i == 24) i=0;
                            //if (i == 0) flag = 1;
                            label3.Text= za;
                            //if (label3.Text.Length > 32) label3.Text = "загрузка";
                        }
        public void init_config(DataGridView gridIn)
        {
            if (!File.Exists(path_conf_file)) File.Create(path_conf_file);          
            FileInfo f = new FileInfo(path_conf_file);
            if (f.Length > 1)
            {   //описываем виртуальную таблицу 
                DataTable dt2 = new DataTable("конфиг сбора данных");
                DataColumn a02 = new DataColumn("хост", typeof(String));
                DataColumn a12 = new DataColumn("номер интерфейса", typeof(String));
                DataColumn a22 = new DataColumn("примечание", typeof(String));
                dt2.Columns.AddRange(new DataColumn[] { a02, a12, a22 });
                string[] tab0 = File.ReadAllLines(path_conf_file, Encoding.UTF8);
                string[] tab1Values = null;
                DataRow dr1 = null;
                //помещаем файл в виртуальную таблицу
                for (int i = 0; i < tab0.Length; i++)
                {
                    if (!String.IsNullOrEmpty(tab0[i]))
                    {
                        tab1Values = tab0[i].Split(',');
                        //создаём новую строку
                        dr1 = dt2.NewRow();
                        //j должно быть колыо строк!!!
                        for (int j = 0; j < tab1Values.Length; j++)
                        {
                            string valp = tab1Values[j];
                            dr1[j] = valp;
                        }
                        dt2.Rows.Add(dr1);
                    }
                }
                gridIn.DataSource = dt2;
            }

        }

        private void button7_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView2_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            writeCSV(dataGridView2, path_conf_file);
        }

        private void button8_Click(object sender, EventArgs e)
        {//ищем в конфиге хосты их списка полученных файлов, 
         //и если не находим, добавляем в конфиг





        }

        public void writeCSV(DataGridView gridIn, string outputFile)
        {
            //test to see if the DataGridView has any rows
            if (gridIn.RowCount > 0)
            {
                string value = "";
                DataGridViewRow dr = new DataGridViewRow();
                StreamWriter swOut = new StreamWriter(outputFile);

                //write header rows to csv
                /*    for (int i = 0; i <= gridIn.Columns.Count - 1; i++)
                    {
                        if (i > 0)
                        {
                            swOut.Write(",");
                        }
                        swOut.Write(gridIn.Columns[i].HeaderText);
                    }
                    swOut.WriteLine();
    */
                //write DataGridView rows to csv
                for (int j = 0; j <= gridIn.Rows.Count - 1; j++)
                {
                    if (j > 0)
                    {
                        swOut.WriteLine();
                    }

                    dr = gridIn.Rows[j];

                    for (int i = 0; i <= gridIn.Columns.Count-1 ; i++)
                    {
                        if (i > 0){swOut.Write(","); }

                        //value = dr.Cells[i].Value.ToString();
                        //value = dr.Cells[i].ToString();
                        value = dr.Cells[i].Value.ToString();
                        //replace comma's with spaces
                        value = value.Replace(',', ' ');
                        //replace embedded newlines with spaces
                        value = value.Replace(Environment.NewLine, " ");

                        swOut.Write(value);
                    }
                }
                swOut.Close();
            }
        }
    }
                }
