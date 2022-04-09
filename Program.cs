using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace mod7_1302204100
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            ProgramConfig config = new ProgramConfig();

            if (config.conf.lang == "en")
            {
                Console.Write("Please insert the amount of money to transfer : ");
            }
            else
            {
                Console.Write("Masukkan jumlah uang yang akan di-transfer : ");
            }

            string biayaStr = Console.ReadLine();
            int biaya = int.Parse(biayaStr);

            if (biaya <= config.conf.transfer.threshold)
            {
                biaya += config.conf.transfer.low_fee;
            }
            else
            {
                biaya += config.conf.transfer.high_fee;
            }

            if (config.conf.lang == "en")
            {
                Console.WriteLine("Total amount : " + biaya);
                printMethods();
                Console.Write("Select transfer method : ");
            }
            else
            {
                Console.WriteLine("Total biaya : " + biaya);
                printMethods();
                Console.Write("Pilih metode transfer : ");
            }

            string inputMethodsStr = Console.ReadLine();
            int inputMethods = int.Parse(inputMethodsStr);

            if (config.conf.lang == "en")
            {
                Console.Write("Please type " + config.conf.confirmation.en + " to confirm the transaction : ");
            }
            else
            {
                Console.Write("Ketik " + config.conf.confirmation.id + " untuk mengkonfirmasi transaksi : ");
            }

            string inputConfirmation = Console.ReadLine();

            if (inputConfirmation == config.conf.confirmation.en)
            {
                Console.WriteLine("The transfer is completed");
            }
            else if (inputConfirmation == config.conf.confirmation.id)
            {
                Console.WriteLine("Proses transfer berhasil");
            }
            else
            {
                if (config.conf.lang == "en")
                {
                    Console.WriteLine("Transfer is cancelled");
                }
                else
                {
                    Console.WriteLine("transfer dibatalkan");
                }
            }

            void printMethods()
            {
                ProgramConfig config = new ProgramConfig();
                int index = 1;
                foreach (var item in config.conf.methods)
                {
                    Console.WriteLine(index++ + ". " + item);
                }
            }
        }



    }

    class ProgramConfig
    {
        public BankTransferConfig conf;
        public string path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
        public string configFileName = "bank_transfer_config.json";
        public ProgramConfig()
        {
            try
            {
                ReadConfigFile();
            }
            catch
            {
                SetDefault();
                WriteNewConfigFile();
            }
        }

        private void SetDefault()
        {
            TransferConfig transfer = new TransferConfig(25000000, 6500, 15000);
            ConfirmationConfig confirmation = new ConfirmationConfig("yes", "ya");
            List<string> methods = new List<string>() { "RTO (real-time)", "SKN", "RTGS", "BI FAST" };
            conf = new BankTransferConfig("en", transfer, methods, confirmation);

        }

        private BankTransferConfig ReadConfigFile()
        {
            string jsonFromFile = File.ReadAllText(path + '/' + configFileName);
            conf = JsonSerializer.Deserialize<BankTransferConfig>(jsonFromFile);
            return conf;
        }

        private void WriteNewConfigFile()
        {
            JsonSerializerOptions options = new JsonSerializerOptions()
            {
                WriteIndented = true
            };

            String jsonString = JsonSerializer.Serialize(conf, options);
            string fullPath = path + '/' + configFileName;
            File.WriteAllText(fullPath, jsonString);
        }
    }

    class BankTransferConfig
    {
        public string lang { get; set; }
        public TransferConfig transfer { get; set; }
        public List<string> methods { get; set; }
        public ConfirmationConfig confirmation { get; set; }

        public BankTransferConfig() { }

        public BankTransferConfig(string lang, TransferConfig transfer, List<string> methods, ConfirmationConfig confirmation)
        {
            this.lang = lang;
            this.transfer = transfer;
            this.methods = methods;
            this.confirmation = confirmation;
        }
    }

    class TransferConfig
    {
        public int threshold { get; set; }
        public int low_fee { get; set; }
        public int high_fee { get; set; }

        public TransferConfig() { }

        public TransferConfig(int threshold, int low_fee, int high_fee)
        {
            this.threshold = threshold;
            this.low_fee = low_fee;
            this.high_fee = high_fee;
        }
    }

    class ConfirmationConfig
    {
        public string en { get; set; }
        public string id { get; set; }

        public ConfirmationConfig() { }

        public ConfirmationConfig(string en, string id)
        {
            this.en = en;
            this.id = id;
        }
    }
}