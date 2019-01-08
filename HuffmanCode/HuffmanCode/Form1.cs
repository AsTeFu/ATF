using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HuffmanCode {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        //private void button1_Click(object sender, EventArgs e) {
        //    Huffman huffman = new Huffman("beep boop beer!");
        //    Huffman huffman1 = new Huffman("шласашапошоссе");

        //    //textBox1.Text = huffman1.lol();
            
        //}

        private void saveHuffmanTextToolStripMenuItem_Click(object sender, EventArgs e) {

        }

        private void compressButton_Click(object sender, EventArgs e) {
            if (input.Text.Length > 1) {
                Huffman huffman = new Huffman(input.Text);

                output.Text = $"Cредняя длина кода: {huffman.averageLength:f3}{Environment.NewLine}" +
                              $"Cжатие: {huffman.compression:f0}%{Environment.NewLine}" +
                              $"{huffman.huffmanText}";
                codesText.Text = huffman.GetCodes();
            }
        }

        private void uncompressButton_Click(object sender, EventArgs e) {
            if (input.Text.Length > 1) {
                //Huffman huffman = new Huffman();
            }
        }
    }
}
