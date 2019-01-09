using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HuffmanCode {
    static class code {

        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }


    public class Huffman {

        private class BinaryTreeNode {
            public BinaryTreeNode leftChild;
            public BinaryTreeNode rightChild;
            
            
            public char symbol {
                get; private set;
            }

            public BinaryTreeNode(char symbol) {
                this.symbol = symbol;
            }
            public BinaryTreeNode(BinaryTreeNode leftChild, BinaryTreeNode rightChild) {
                this.leftChild = leftChild;
                this.rightChild = rightChild;
            }

        }
        private class PriorityQueue <T> {
            Dictionary<T, int> queue;
            
            public PriorityQueue() {
                queue = new Dictionary<T, int>();
            }

            public int Count {
                get {
                    return queue.Count;
                }
            }

            public void Enqueue(T element, int priority) {
                queue.Add(element, priority);
            }

            public (T, int) Dequeue() {
                int min = queue.Values.Min();
                T element = queue.First().Key;

                foreach (T key in queue.Keys) {
                    if (min == queue[key]) {
                        element = key;
                        queue.Remove(key);

                        return (element, min);
                    }
                }

                return (element, min);
            }
        }

        private string text;
        public string huffmanText {
            get; private set;
        }
        public double averageLength {
            get; private set;
        }
        public double compression {
            get; private set;
        }

        Dictionary<char, int> probabilityTable;
        Dictionary<char, string> codes;
        BinaryTreeNode tree;
        
        public Huffman(string huffmanText, Dictionary<char, string> codes) {

        }
        public Huffman(string text) {
            codes = new Dictionary<char, string>();
            this.text = text;

            huffmanText = HuffmanText();
        }
        private string HuffmanText() {
            probabilityTable = GetProbabilityTable(text);
            Dictionary<char, int> tempTable = new Dictionary<char, int>(probabilityTable);
            PriorityQueue<BinaryTreeNode> queue = GetBinaryTrees(tempTable);
            
            while(queue.Count > 1) {
                BinaryTreeNode first, second;
                int prob1, prob2;
                
                (first, prob1) = queue.Dequeue();
                (second, prob2) = queue.Dequeue();

                queue.Enqueue(new BinaryTreeNode(first, second), prob1 + prob2);
            }

            int prob;
            (tree, prob) = queue.Dequeue();
            GetCodes(tree, "");
            
            string tmp = "";
            for (int i = 0; i < text.Length; i++) {
                for (int j = 0; j < codes[text[i]].Length; j++) {
                    tmp += codes[text[i]][j] == '0' ? "0" : "1";
                }
            }

            averageLength = AverageLength(probabilityTable);
            compression = Compression(probabilityTable);

            return tmp;
        }
        
        private double AverageLength(Dictionary<char, int> probabilityTable) {
            averageLength = 0;
            foreach (char key in probabilityTable.Keys) {
                averageLength += probabilityTable[key] * codes[key].Length;
            }
            
            return averageLength / text.Length;
        }
        private double Compression(Dictionary<char, int> probabilityTable) {
            int lenght = 0;
            foreach (char key in probabilityTable.Keys) {
                lenght += probabilityTable[key] * codes[key].Length;
            }

            return (text.Length * 8.0 - lenght) / (text.Length * 8.0) * 100.0;
        }
        private void GetCodes(BinaryTreeNode tree, string c) {
            
            if (tree.leftChild != null) {
                GetCodes(tree.leftChild, c + "0");
                GetCodes(tree.rightChild, c + "1");
            } else {
                codes.Add(tree.symbol, c);
            }
            
        }
        
        private Dictionary<char, int> GetProbabilityTable(string text) {
            Dictionary<char, int> probabilityTable = new Dictionary<char, int>();
            for (int i = 0; i< text.Length; i++) {
                if (!probabilityTable.ContainsKey(text[i])) {
                    probabilityTable.Add(text[i], 0);
                }
                probabilityTable[text[i]]++;
            }

            return probabilityTable;
        }
        private PriorityQueue<BinaryTreeNode> GetBinaryTrees(Dictionary<char, int> probabilityTable) {
            PriorityQueue<BinaryTreeNode> queue = new PriorityQueue<BinaryTreeNode>();
            
            while (probabilityTable.Count > 0) {

                int maxPriority = probabilityTable.Values.Max();
                foreach (char key in probabilityTable.Keys) {
                    if (probabilityTable[key] == maxPriority) {
                        queue.Enqueue(new BinaryTreeNode(key), maxPriority);
                        probabilityTable.Remove(key);
                        break;
                    } 
                }
            }

            return queue;
        }
        
        private string GetOriginalText(BinaryTreeNode root, string huffmanText) {
            string originalText = "";

            BinaryTreeNode currentNode = root;
            for (int i = 0; i < huffmanText.Length; i++) { 
                if (huffmanText[i] == '0') {

                    currentNode = currentNode.leftChild;
                    if (currentNode.leftChild == null) {
                        originalText += currentNode.symbol;
                        currentNode = root;
                    }

                } else {

                    currentNode = currentNode.rightChild;
                    if (currentNode.rightChild == null) {
                        originalText += currentNode.symbol;
                        currentNode = root;
                    }
                }
            }

            return originalText;
        }
        private string GetOriginalText(Dictionary<char, string> codes, string huffmanText) {
            string text = "";

            string lastCode = "";
            for (int i = 0; i < huffmanText.Length; i++) {
                lastCode += huffmanText[i];

                foreach (char key in codes.Keys) {
                    if (lastCode == codes[key]) {
                        text += key;
                        lastCode = "";
                    }
                }
            }

            return text;
        }

        public string GetCodes() {
            string codesText = "";

            foreach(char key in codes.Keys) {
                codesText += $"{key}:\t {codes[key]}{Environment.NewLine}";
            }

            return codesText;
        }
    }
}
