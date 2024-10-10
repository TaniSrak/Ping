using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.NetworkInformation;
using System.IO;

namespace Ping
{
    public partial class Form1 : Form
    {
        Process calcProcess, cmdProcess, wordProcess, excelProcess, ieProcess, browserProcess;
        List<Process> processes;
        System.Net.NetworkInformation.Ping ping = new System.Net.NetworkInformation.Ping();

        public Form1()
        {
            InitializeComponent();

            timer1.Interval = 120000;
            timer1.Tick += Timer1_Tick;
            timer1.Start();

        }

        private async void timer1_Tick(object sender, EventArgs e)
        {
            await CheckNodeAvailability(textBox1.Text, "out1.txt", panel1);
            await CheckNodeAvailability(textBox2.Text, "out2.txt", panel2);
            await CheckNodeAvailability(textBox3.Text, "out3.txt", panel3);
        }

        private async void Timer1_Tick(object sender, EventArgs e)
        {
            //await CheckNodeAvailability(textBox1.Text, panel1);
            //await CheckNodeAvailability(textBox2.Text, panel2);
            //await CheckNodeAvailability(textBox3.Text, panel3);
        }

        private async Task CheckNodeAvailability(string address, string outputFile, Panel statusPanel)
        {
            // Формируем команду для запуска
            string pingCommand = $"/C ping {address} > {outputFile}";

            // Вызываем командную строку и запускаем пинг
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",  // Запускаем командную строку
                    Arguments = pingCommand,  // Передаем команду пинг с перенаправлением вывода в файл
                    RedirectStandardOutput = false,  // Вывод не нужен
                    UseShellExecute = true,  // Выполняем в системной оболочке
                    CreateNoWindow = true  // Без окна консоли
                }
            };

            try
            {
                process.Start();
                await process.WaitForExitAsync();

                // Проверяем результат пинга через чтение файла
                string result = File.ReadAllText(outputFile);

                if (result.Contains("TTL="))  // Если пинг успешен
                {
                    statusPanel.BackColor = Color.Green;  // Узел доступен
                }
                else
                {
                    statusPanel.BackColor = Color.Red;  // Узел недоступен
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при пинге узла {address}: {ex.Message}");
                statusPanel.BackColor = Color.Red;  // В случае ошибки
            }
        }
    }
}
