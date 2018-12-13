using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace KriptaS {
    static class Program {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main() {
            Start();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        private static int size = 26;
        //private static string closeText = "";
        
        //private static char[,] tabl = new char[size, size];
        private static char[] defAlphabet = new char[size];
        //private static char[] nonChekedSymbol = { '.', ',', '\'', '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '_', '-', '=', '+', '', '', '', '', '', '', '', '', }

        private static char[] rndAlphabet = new char[size];

        private static void Start() {
            //message = message.ToUpper();

            defAlphabet = GetAlphaget();
            rndAlphabet = GetAlphaget();
            
            //rndAlphabet = RandomAlphabet(alphabet, 1000);
            //tabl = GetTabl(alphabet);
            //printTabl();
            //closeText = GetCloseText(AddKript(), message);
        }

        public static void LoadAlphabet(bool random) {
            if (random) {
                rndAlphabet = RandomAlphabet(defAlphabet, 1000);
            } 
            else
                rndAlphabet = defAlphabet;
        }

        public static void LoadAlphabet(char[] alphabet) {
            rndAlphabet = alphabet;
        }
        public static string SaveAlphabet() {
            string tmp = "";
            for (int i = 0; i < size; i++) {
                tmp += rndAlphabet[i];
            }

            return tmp;
        }
        
        public static string GetCloseText(string message, string key) {
            return GetCloseText(rndAlphabet, AddKript(message, key), message);
        }
        public static string GetOpenText(string closeText, string key) {
            return GetOpenText(rndAlphabet, AddKript(closeText, key), closeText);
        }

        private static char[] GetAlphaget() {
            char[] tmp = new char[size];
            for (int i = 0; i < size; i++) {
                tmp[i] = (char)(i + 65);
            }

            return tmp;
        }

        private static char[] RandomAlphabet(char[] alphabet, int rnd) {
            char[] tmp = new char[alphabet.Length];
            for (int i = 0; i < tmp.Length; i++) {
                tmp[i] = alphabet[i];
            }

            Random rndObj = new Random();
            for (int i = 0; i < rnd; i++) {
                int posFirst = rndObj.Next(0, tmp.Length);
                int posSecond = rndObj.Next(0, tmp.Length);

                char tmpch = tmp[posFirst];
                tmp[posFirst] = tmp[posSecond];
                tmp[posSecond] = tmpch;
            }

            return tmp;
        }
        
        //private static char[,] GetTabl(char[] alphabet) {
        //    char[,] tmp = new char[size, size];
        //    int n = size;

        //    int tmpInt, jt = 0, ch = 0;
        //    for (int i = 0; i < n * 2 - 1; i++, ch++) {
        //        if (i == n) ch = 0;
        //        if (i > n - 1) {
        //            tmpInt = n - 1;
        //            jt++;
        //        } else {
        //            tmpInt = i;
        //        }

        //        for (int j = jt; j <= i - jt; j++, tmpInt--) {
        //            tmp[tmpInt, j] = alphabet[ch];
        //        }
        //    }

        //    return tmp;
        //}

        //private static void printTabl() {
        //    string str = "";
        //    for (int i = 0; i < size; i++) {
        //        for (int j = 0; j < size; j++) {
        //            str += tabl[i, j];
        //        }
        //        str += "\n";
        //    }
        //    MessageBox.Show(str);
        //}


        //

        public static string GetRanddomKey() {
            string tmp = "";
            Random rnd = new Random();
            for (int i = 0; i < 15; i++) {
                tmp += defAlphabet[rnd.Next(0, size)];
            }
            return tmp;
        }

        private static string AddKript(string message, string key) {
            int tmp = 0;
            string keyText = "";

            for (int i = 0; i < message.Length; i++) {
                if (NonCheckedSymbol(message[i], defAlphabet)) {
                    keyText += key[tmp];
                    if (tmp < key.Length - 1) tmp++;
                    else tmp = 0;
                } else keyText += message[i];
            }

            return keyText;
        }

        private static string GetCloseText(char[] alphabet, string keyText, string message) {
            string tmpCloseText = "";

            for (int i = 0; i < message.Length; i++) {
                if (NonCheckedSymbol(message[i], alphabet)) {
                    tmpCloseText += alphabet[((GetSymbolID(alphabet, keyText[i])) + (GetSymbolID(alphabet, message[i]))) % size]; //tabl[GetSymbolID(alphabet, keyText[i]), GetSymbolID(alphabet, message[i])];
                } else tmpCloseText += message[i];
            }
                return tmpCloseText;
        }
        
        private static int GetSymbolID(char[] alphabet, char symbol) {
            for (int i = 0; i < alphabet.Length; i++) {
                if (symbol == alphabet[i]) {
                    return i;
                }
            }

            return -1;
        }

        private static bool NonCheckedSymbol(char symbol, char[] alphabet) {
            for (int i = 0; i < alphabet.Length; i++) {
                if (symbol == alphabet[i])
                    return true;
            }
            return false;
        }

        //

        private static string GetOpenText(char[] alphabet, string keyText, string closeText) {
            string tmpOpenText = "";

            for (int i = 0; i < closeText.Length; i++) {
                if (NonCheckedSymbol(closeText[i], alphabet)) {
                    tmpOpenText += alphabet[Math.Abs((GetSymbolID(alphabet, closeText[i]) - GetSymbolID(alphabet, keyText[i]) + size) % size)];
                } else tmpOpenText += closeText[i];
            }
            return tmpOpenText;
        }
    }
}
