// Copyright (c) 2014, Łukasz Walukiewicz <lukasz@walukiewicz.eu>. Some Rights Reserved.
// Licensed under CC BY-NC-SA 4.0 <http://creativecommons.org/licenses/by-nc-sa/4.0/>.
// Part of the XiconfLabelPrinter project <http://lukasz.walukiewicz.eu/p/XiconfLabelPrinter>

using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.OpenSsl;

namespace MSYS.Xiconf.LabelPrinter
{
    public static class LicenseInfo
    {
        public static string Id = "-";

        public static string Product = "-";

        public static string Version = "-";

        public static string Date = "-";

        public static string Licensee = "-";

        public static string Error = "NO_KEY";

        public static string ErrorMessage
        {
            get
            {
                switch (Error)
                {
                    case "":
                        return "-";

                    case "NO_SERVER":
                        return "Brak adresu do serwera zewnętrznego.";

                    case "NO_KEY":
                        return "Brak klucza licencyjnego.";

                    case "APP_ID":
                        return "Ustawiony klucz licencji nie został przydzielony do uruchomionej aplikacji.";

                    case "APP_VERSION":
                        return "Ustawiony klucz licencji nie obejmuje aktualnie uruchomionej wersji aplikacji.";

                    case "VALIDATION":
                        return "Walidacji klucza licencyjnego nie powiodła się.";

                    case "UNKNOWN_LICENSE":
                        return "Zewnętrzny serwer licencji nie rozpoznał wybranego klucza licencyjnego.";

                    case "DUPLICATE_LICENSE":
                        return "Zewnętrzny serwer licencji wykrył w użyciu zduplikowane klucze licencyjne.";

                    default:
                        return Error;
                }
            }
        }

        public static bool IsValid()
        {
            return Error.Length == 0;
        }

        public static void Validate()
        {
            if (Properties.Settings.Default.LicenseServer == "")
            {
                Error = "NO_SERVER";
            }
            else if (Id == "-" || Product == "-" || Version == "-" || Date == "-" || Licensee == "-")
            {
                Error = "NO_KEY";
            }
            else if (Product != "XiconfLabelPrinter")
            {
                Error = "APP_ID";
            }
            else
            {
                Error = "";
            }
        }

        public static void ValidateRemotely()
        {
            var requestData = Encoding.UTF8.GetBytes(@"{""uuid"":""" + EncryptId() + @""",""id"":""" + Environment.MachineName + @"""}");
            var req = (HttpWebRequest)WebRequest.Create(Properties.Settings.Default.LicenseServer + "/licenses/" + Id + ";ping");

            req.Timeout = 5000;
            req.KeepAlive = false;
            req.Method = "POST";
            req.Accept = "text/plain";
            req.ContentType = "application/json";
            req.ContentLength = requestData.Length;
            req.UserAgent = LicenseInfo.Product;

            using (var reqStream = req.GetRequestStream())
            {
                reqStream.Write(requestData, 0, requestData.Length);
                reqStream.Close();
            }

            try
            {
                req.GetResponse();
            }
            catch (WebException x)
            {
                using (var resStream = x.Response.GetResponseStream())
                {
                    Error = new StreamReader(resStream, Encoding.UTF8).ReadToEnd().Split('\n')[0];

                    resStream.Close();
                }
            }
        }

        public static void ReadFromSettings()
        {
            try
            {
                ReadFromRawKey(Properties.Settings.Default.LicenseKey);
            }
            catch (Exception)
            {
                Reset();
            }
        }

        public static void ReadFromFile(string licenseKeyPath)
        {
            ReadFromRawKey(File.ReadAllText(licenseKeyPath, Encoding.UTF8));
        }

        public static void Reset()
        {
            Properties.Settings.Default.LicenseKey = "";

            Id = "-";
            Product = "-";
            Version = "-";
            Date = "-";
            Licensee = "-";

            Validate();
        }

        private static void ReadFromRawKey(string rawKey)
        {
            Reset();

            var encryptedKey = Convert.FromBase64String(Regex.Replace(rawKey, "-+.*?-+", "").Replace("\r", "").Replace("\n", ""));
            var cipher = new Pkcs1Encoding(new RsaEngine());

            cipher.Init(false, CreateCipherParameters());

            var decryptedKey = cipher.ProcessBlock(encryptedKey, 0, encryptedKey.Length);
            var licenseInfo = Encoding.UTF8.GetString(decryptedKey).Split('\n');
 
            Product = licenseInfo[0];
            Version = licenseInfo[1];
            Date = string.Format("20{0}-{1}-{2}", licenseInfo[2].Substring(0, 2), licenseInfo[2].Substring(2, 2), licenseInfo[2].Substring(4, 2));
            Id = licenseInfo[3];
            Licensee = licenseInfo[4];

            Validate();

            Properties.Settings.Default.LicenseKey = IsValid() ? rawKey : "";
        }

        private static string EncryptId()
        {
            var cipher = new OaepEncoding(new RsaEngine());

            cipher.Init(true, CreateCipherParameters());

            var decryptedId = Encoding.UTF8.GetBytes(Id);
            var encryptedId = cipher.ProcessBlock(decryptedId, 0, decryptedId.Length);

            return Convert.ToBase64String(encryptedId);
        }

        private static ICipherParameters CreateCipherParameters()
        {
            var pamReader = new PemReader(new StringReader(Encoding.UTF8.GetString(Properties.Resources.licenseEdPublicPem)));
            var cipherParameters = (AsymmetricKeyParameter)pamReader.ReadObject();

            return cipherParameters;
        }
    }
}
