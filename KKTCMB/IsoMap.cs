using System.Globalization;

namespace KKTCMB
{
    internal class IsoMap
    {
        private static readonly Dictionary<string, string> Iso4217CurrencyMap = new() {
            { "AMERİKAN DOLARI", "USD" },
            { "ALMAN MARKI", "DEM" },
            { "AVUSTRALYA DOLARI", "AUD" },
            { "AVUSTURYA ŞİLİNİ", "ATS" },
            { "BELÇİKA FRANGI", "BEF" },
            { "DANİMARKA KRONU" , "DKK"},
            { "FRANSIZ FRANGI", "FRF" },
            { "FİNLANDİYA MARKKASI", "FIM" },
            { "HOLLANDA FLORİNİ" , "NLG"},
            { "STERLİN", "GBP" },
            { "İNGİLİZ STERLİNİ", "GBP" },
            { "İRLANDA LİRASİ", "IEP" },
            { "İSPANYOL PEZETASI", "ESP" },
            { "İSVEÇ KRONU", "SEK" },
            { "İSVİÇRE FRANGI", "CHF" },
            { "İTALYAN LİRETİ", "ITL" },
            { "JAPON YENİ", "JPY" },
            { "KANADA DOLARI", "CAD" },
            { "KUVEYT DİNARI", "KWD" },
            { "LÜKSEMBURG FRANGI", "LUF" },
            { "NORVEÇ KRONU", "NOK" },
            { "ESKÜDO", "PTE" },
            { "SUUDİ ARABİSTAN RİYALİ", "SAR" },
            { "EURO", "EUR" },
            { "DRAHMİ", "GRD" },
            { "KIBRIS LİRASI",  "CYP"},
            { "İRAN RİYALİ", "IRR" },
            { "SURİYE LİRASI", "SYP" },
            { "ÜRDÜN DİNARI", "JOD" },
            { "YENİ İSRAİL ŞEKERİ", "ILS" },
            { "İSRAİL ŞEKERİ (YENİ)", "ILS" }
        };

        public static string GetISOCode(string currency)
        {
            var keys = new List<string>(Iso4217CurrencyMap.Keys);

            foreach(var key in keys) {
                if (LevenshteinDistance(key, currency) <= 3)
                    return Iso4217CurrencyMap[key];
            }

            return "";
        }

        public static string GetCurrencyName(string isoCode, CultureInfo culture)
        {
            return culture.TextInfo.ToTitleCase(Iso4217CurrencyMap.LastOrDefault(x => x.Value == isoCode).Key.ToLower(culture));
        }

        //https://stackoverflow.com/a/13793600
        private static int LevenshteinDistance(string s, string t)
        {
            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            if (n == 0)
                return m;

            if (m == 0)
                return n;

            for (int i = 0; i <= n; d[i, 0] = i++) {
            }

            for (int j = 0; j <= m; d[0, j] = j++) {
            }

            for (int i = 1; i <= n; i++) {
                for (int j = 1; j <= m; j++) {
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;

                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }

            return d[n, m];
        }
    }
}
