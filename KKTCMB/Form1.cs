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

using Newtonsoft.Json;

using System.Data.SqlServerCe;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Xml;

namespace KKTCMB
{
    public partial class Form1 : Form
    {

        private readonly HttpClient client = new();
        private readonly SqlCeConnection connection = new($"Data Source={Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? "", "Database.sdf")}");

        private enum DataSource { KKTCMB, TCMB };
        private DataSource currentSource = DataSource.KKTCMB;

        public Form1()
        {
            InitializeComponent();

            dateTimePicker.Value = DateTime.Today;
            dateTimePicker.MaxDate = DateTime.Today;

            HandleChanges();
        }

        private async void HandleChanges()
        {
            saveButton.Enabled = false;
            dataGridView.Rows.Clear();
            dataGridView.Focus();

            await connection.OpenAsync();

            try {
                var selectCommand = new SqlCeCommand($"SELECT tarih FROM kktcmb WHERE tarih = '{dateTimePicker.Value:yyyy-MM-dd}' AND kaynak = {(int)currentSource}", connection);
                var reader = await selectCommand.ExecuteReaderAsync();
                var exists = await reader.ReadAsync();

                saveButton.Enabled = dataGridView.Rows.Count != 0;
                showButton.Enabled = exists;

            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }

            connection.Close();
        }

        private void genericValueChangedEvent(object sender, EventArgs e)
        {
            if (sender == sourceComboBox)
                currentSource = sourceComboBox.Text == "KKTCMB" ? DataSource.KKTCMB : DataSource.TCMB;

            HandleChanges();
        }

        private void dateTimePicker_CloseUp(object sender, EventArgs e)
        {
            HandleChanges();
        }

        private async void getButton_Click(object sender, EventArgs e)
        {
            if (dataGridView.Rows.Count != 0) {
                if (MessageBox.Show("Seçili tarihe ait döviz bilgileri zaten tabloda mevcut. Yinede veriler yenilensin mi?", "Merkez Bankası Döviz Kurları", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    return;
            }

            var selectedDate = dateTimePicker.Value;

            var isTCMB = sourceComboBox.Text == "TCMB";
            var isHTML = selectedDate < new DateTime(2011, 4, 9);
            var isISO = selectedDate < new DateTime(2008, 2, 8);

            try {
                var response = await client.GetAsync((isTCMB ? "https://api.demirdelen.net/tcmb/kur/tarih/" : "http://www.mb.gov.ct.tr/kur/tarih/") + $"{selectedDate:yyyyMMdd}");
                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException("Kur bilgileri alınırken bir hata oluştu.");

                var buffer = await response.Content.ReadAsByteArrayAsync();
                var content = Encoding.GetEncoding(isHTML && !isTCMB ? (isISO ? "ISO-8859-9" : "UTF-16") : "UTF-8").GetString(buffer, 0, buffer.Length);

                if (content == "")
                    throw new Exception("Seçili tarih için kur bilgileri alınamadı." + (isHTML ? "\nBu tarihin bir resmi tatile veya haftasonuna denk gelmediğinden emin olun." : ""));

                if (isTCMB) {
                    ParseJSON(content);
                    return;
                }

                if (isHTML) {
                    ParseHTML(content);
                    return;
                }

                var document = new XmlDocument();
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

        private void ParseHTML(string content)
        {
            dataGridView.Rows.Clear();
            dataGridView.Focus();

            var lines = content.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines.Skip(Array.FindLastIndex(lines, x => x.Contains("_____________________________")) + 1)) {
                if (line.Replace(" ", "").Contains("ÇAPRAZ"))
                    break;

                var chunks = line.Split("  ", StringSplitOptions.RemoveEmptyEntries);
                var birim = chunks[0].Split(" ", 2);
                var sadeceEf = chunks.Length < 5;

                var culture = new CultureInfo("tr-TR", false);
                dataGridView.Rows.Add(birim[0], IsoMap.GetISOCode(birim[1]), culture.TextInfo.ToTitleCase(birim[1].ToLower(culture)),
                    sadeceEf ? "" : chunks[1], sadeceEf ? "" : chunks[2], chunks.ElementAtOrDefault(sadeceEf ? 1 : 3) ?? "", chunks.ElementAtOrDefault(sadeceEf ? 2 : 4) ?? "");
            }

            saveButton.Enabled = true;
        }

        private void ParseJSON(string content)
        {
            dataGridView.Rows.Clear();
            dataGridView.Focus();

            var json = JsonConvert.DeserializeObject<dynamic>(content)
                ?? throw new Exception("JSON yanıtı ayrıştırılamadı.");

            var items = json.items[0];
            var kurlar = new string[] { "USD", "AUD", "DKK", "EUR", "GBP", "CHF", "SEK", "CAD", "KWD", "NOK", "SAR", "JPY" };

            var culture = new CultureInfo("tr-TR", false);
            foreach (var kur in kurlar)
                dataGridView.Rows.Add(kur == "JPY" ? 100 : 1, kur, IsoMap.GetCurrencyName(kur, culture),
                    items[$"TP_DK_{kur}_A"], items[$"TP_DK_{kur}_S"], items[$"TP_DK_{kur}_A_EF"], items[$"TP_DK_{kur}_S_EF"]);

            saveButton.Enabled = true;
        }

        private async void saveButton_Click(object sender, EventArgs e)
        {
            if (!saveButton.Enabled)
                return;

            await connection.OpenAsync();

            try {
                var selectCommand = new SqlCeCommand($"SELECT tarih FROM kktcmb WHERE tarih = '{dateTimePicker.Value:yyyy-MM-dd}' AND kaynak = {(int)currentSource}", connection);
                var reader = await selectCommand.ExecuteReaderAsync();
                if (await reader.ReadAsync()) {
                    if (MessageBox.Show("Veritabanında bu tarihe ait bir kayıt zaten mevcut. Üstüne yazılsın mı?", "Merkez Bankası Döviz Kurları", MessageBoxButtons.YesNo) != DialogResult.Yes) {
                        connection.Close();
                        return;
                    }

                    var deleteCommand = new SqlCeCommand($"DELETE FROM kktcmb WHERE tarih = '{dateTimePicker.Value:yyyy-MM-dd}' AND kaynak = {(int)currentSource}", connection);
                    await deleteCommand.ExecuteNonQueryAsync();
                }

                foreach (DataGridViewRow row in dataGridView.Rows) {
                    var command = new SqlCeCommand("INSERT INTO kktcmb (tarih, kaynak, siralama, birim, sembol, isim, doviz_alis, doviz_satis, efektif_alis, efektif_satis) " +
                        $"VALUES ('{dateTimePicker.Value:yyyy-MM-dd}', {(int)currentSource}, {row.Index}, {row.Cells[0].Value}, '{row.Cells[1].Value}', '{row.Cells[2].Value}', '{row.Cells[3].Value}', '{row.Cells[4].Value}', '{row.Cells[5].Value}', '{row.Cells[6].Value}')",
                        connection);

                    await command.ExecuteNonQueryAsync();
                }

                MessageBox.Show($"{dateTimePicker.Value.ToLongDateString()} gününe ve {sourceComboBox.Text} kaynağına ait döviz kurları bilgileri veritabanına kaydedildi.", "Merkez Bankası Döviz Kurları");
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
                MessageBox.Show("Seçili tarihe ait döviz bilgileri zaten tabloda mevcut.", "Merkez Bankası Döviz Kurları");
                return;
            }

            await connection.OpenAsync();

            try {
                var command = new SqlCeCommand(
                $"SELECT birim, sembol, isim, doviz_alis, doviz_satis, efektif_alis, efektif_satis FROM kktcmb WHERE tarih = '{dateTimePicker.Value:yyyy-MM-dd}' AND kaynak = {(int)currentSource} ORDER BY siralama ASC",
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
            MessageBox.Show($"Sürüm: {Assembly.GetExecutingAssembly().GetName().Version}\nDeniz Demirdelen © 2023", "Merkez Bankası Döviz Kurları");
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}