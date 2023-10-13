//  (               )   (         )     (               *      (      (     (             (               )
//  )\ )         ( /(   )\ )   ( /(     )\ )          (  `     )\ )   )\ )  )\ )          )\ )         ( /(
// (()/(   (     )\()) (()/(   )\())   (()/(    (     )\))(   (()/(  (()/( (()/(    (    (()/(   (     )\())
//  /(_))  )\   ((_)\   /(_)) ((_)\     /(_))   )\   ((_)()\   /(_))  /(_)) /(_))   )\    /(_))  )\   ((_)/
//  _(_)  ((_)   _((_) (_))    _((_)   (_))_   ((_)  (_()((_) (_))   (_))  (_))_   ((_)  (_))   ((_)   _((_)
// |   \  | __| | \| | |_ _|  |_  /     |   \  | __| |  \/  | |_ _|  | _ \  |   \  | __| | |    | __| | \| |
// | |) | | _|  | .` |  | |    / /      | |) | | _|  | |\/| |  | |   |   /  | |) | | _|  | |__  | _|  | .` |
// |___/  |___| |_|\_| |___|  /___|     |___/  |___| |_|  |_| |___|  |_|_\  |___/  |___| |____| |___| |_|\_|
//
// Copyright (c) 2023 Deniz DEMIRDELEN

using System.Data.SqlServerCe;
using System.Globalization;
using System.Reflection;
using System.Xml;

namespace KKTCMB
{
    public partial class Form1 : Form
    {

        private readonly HttpClient client = new();
        private readonly SqlCeConnection connection = new($"Data Source={Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? "", "Database.sdf")}");

        public Form1()
        {
            InitializeComponent();

            dateTimePicker.Value = DateTime.Today;
            dateTimePicker.MaxDate = DateTime.Today;
        }

        private async void HandleButtons()
        {
            await connection.OpenAsync();

            try {
                var selectCommand = new SqlCeCommand($"SELECT tarih FROM kktcmb WHERE tarih = '{dateTimePicker.Value:yyyy-MM-dd}'", connection);
                var reader = await selectCommand.ExecuteReaderAsync();
                var exists = await reader.ReadAsync();

                saveButton.Enabled = dataGridView.Rows.Count != 0;
                showButton.Enabled = exists;

            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }

            connection.Close();
        }

        private void dateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            saveButton.Enabled = false;
            dataGridView.Rows.Clear();
            dataGridView.Focus();

            HandleButtons();
        }

        private void todayButton_Click(object sender, EventArgs e)
        {
            dateTimePicker.Value = DateTime.Today;
        }

        private async void getButton_Click(object sender, EventArgs e)
        {
            if (dataGridView.Rows.Count != 0) {
                if (MessageBox.Show("Seçili tarihe ait döviz bilgileri zaten tabloda mevcut. Yinede veriler yenilensin mi?", "KKTCMB Döviz Kurları", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    return;
            }

            try {
                var selectedDate = dateTimePicker.Value;

                var response = await client.GetAsync($"http://www.mb.gov.ct.tr/kur/tarih/{dateTimePicker.Value:yyyyMMdd}");
                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException("Kur bilgileri alınırken bir hata oluştu.");

                var document = new XmlDocument();
                var content = await new StreamReader(await response.Content.ReadAsStreamAsync()).ReadToEndAsync();
                if (content == "")
                    throw new Exception("Seçili tarih için kur bilgileri bulunamadı.");

                document.LoadXml(content);
                dataGridView.Rows.Clear();
                dataGridView.Focus();

                var culture = new CultureInfo("tr-TR", false);
                foreach (XmlElement kur in document.GetElementsByTagName("Resmi_Kur"))
                    dataGridView.Rows.Add(kur["Birim"]?.InnerText, kur["Sembol"]?.InnerText, culture.TextInfo.ToTitleCase(kur["Isim"]?.InnerText.ToLower(culture) ?? "Bulunamadı"),
                        kur["Doviz_Alis"]?.InnerText, kur["Doviz_Satis"]?.InnerText, kur["Efektif_Alis"]?.InnerText, kur["Efektif_Satis"]?.InnerText);

                saveButton.Enabled = true;
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);

                dataGridView.Rows.Clear();
                saveButton.Enabled = false;
            }
        }

        private async void saveButton_Click(object sender, EventArgs e)
        {
            if (!saveButton.Enabled)
                return;

            await connection.OpenAsync();

            try {
                var selectCommand = new SqlCeCommand($"SELECT tarih FROM kktcmb WHERE tarih = '{dateTimePicker.Value:yyyy-MM-dd}'", connection);
                var reader = await selectCommand.ExecuteReaderAsync();
                if (await reader.ReadAsync()) {
                    if (MessageBox.Show("Veritabanında bu tarihe ait bir kayıt zaten mevcut. Üstüne yazılsın mı?", "KKTCMB Döviz Kurları", MessageBoxButtons.YesNo) != DialogResult.Yes) {
                        connection.Close();
                        return;
                    }

                    var deleteCommand = new SqlCeCommand($"DELETE FROM kktcmb WHERE tarih = '{dateTimePicker.Value:yyyy-MM-dd}'", connection);
                    await deleteCommand.ExecuteNonQueryAsync();
                }

                foreach (DataGridViewRow row in dataGridView.Rows) {
                    var command = new SqlCeCommand("INSERT INTO KKTCMB (tarih, siralama, birim, sembol, isim, doviz_alis, doviz_satis, efektif_alis, efektif_satis) " +
                        $"VALUES ('{dateTimePicker.Value:yyyy-MM-dd}', {row.Index}, {row.Cells[0].Value}, '{row.Cells[1].Value}', '{row.Cells[2].Value}', {row.Cells[3].Value}, {row.Cells[4].Value}, {row.Cells[5].Value}, {row.Cells[6].Value})",
                        connection);

                    await command.ExecuteNonQueryAsync();
                }

                MessageBox.Show($"{dateTimePicker.Value.ToLongDateString()} gününe ait döviz kurları bilgileri veritabanına kaydedildi.", "KKTCMB Döviz Kurları");
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }

            connection.Close();
        }

        private async void showButton_Click(object sender, EventArgs e)
        {
            if (!showButton.Enabled)
                return;

            if (dataGridView.Rows.Count != 0) {
                MessageBox.Show("Seçili tarihe ait döviz bilgileri zaten tabloda mevcut.", "KKTCMB Döviz Kurları");
                return;
            }

            await connection.OpenAsync();

            try {
                var command = new SqlCeCommand(
                $"SELECT birim, sembol, isim, doviz_alis, doviz_satis, efektif_alis, efektif_satis FROM kktcmb WHERE tarih = '{dateTimePicker.Value:yyyy-MM-dd}' ORDER BY siralama ASC",
                connection);

                var reader = await command.ExecuteReaderAsync();

                dataGridView.Rows.Clear();

                while (await reader.ReadAsync())
                    dataGridView.Rows.Add(reader[0], reader[1], reader[2], reader[3], reader[4], reader[5], reader[6]);
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }

            await connection.CloseAsync();
        }

        private void aboutButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show($"Sürüm: {Assembly.GetExecutingAssembly().GetName().Version}\nDeniz Demirdelen © 2023", "KKTCMB Döviz Kurları");
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}