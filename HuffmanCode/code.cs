using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HuffmanCode {
    static class Program {

        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }


    public class Huffman {

        public class BinaryTree {
            private BinaryTree root;
            public BinaryTree leftChild {
                get; private set;
            }
            public BinaryTree rightChild {
                get; private set;
            }

            public int probabality {
                get; private set;
            }
            public char symbol {
                get; private set;
            }

            public BinaryTree(char symbol, int probabality) {
                this.symbol = symbol;
                this.probabality = probabality;
            }

            public BinaryTree(BinaryTree leftChild, BinaryTree rightChild, int value) {
                this.leftChild = leftChild;
                this.rightChild = rightChild;

                probabality = value;
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

            public T Dequeue() {
                int min = queue.Values.Min();
                T element = queue.First().Key;
                foreach (T key in queue.Keys) {
                    if (min == queue[key]) {
                        element = key;
                        queue.Remove(key);

                        return element;
                    }
                }

                return element;
            }
        }

        private string text;
        public string huffmanText {
            get; private set;
        }
        public double averageLength {
            get; private set;
        }

        Dictionary<char, int> probabilityTable;
        Dictionary<char, string> codes;
        BinaryTree tree;
        
        public Huffman(string text) {
            codes = new Dictionary<char, string>();
            this.text = text;

            huffmanText = HuffmanText();
        }
        private string HuffmanText() {
            probabilityTable = GetProbabilityTable(text);
            Dictionary<char, int> tempTable = new Dictionary<char, int>(probabilityTable);
            PriorityQueue<BinaryTree> queue = GetBinaryTrees(tempTable);
            
            while(queue.Count > 1) {
                BinaryTree first, second;

                first = queue.Dequeue();
                second = queue.Dequeue();

                queue.Enqueue(new BinaryTree(first, second, first.probabality + second.probabality), first.probabality + second.probabality);
                
            }

            tree = queue.Dequeue();
            GetCodes(tree, "");
            
            string tmp = "";
            for (int i = 0; i < text.Length; i++) {
                for (int j = 0; j < codes[text[i]].Length; j++) {
                    tmp += codes[text[i]][j] == '0' ? "0" : "1";
                }
            }

            averageLength = AverageLength(probabilityTable);

            return tmp;
        }
        
        private double AverageLength(Dictionary<char, int> probabilityTable) {
            averageLength = 0;
            foreach (char key in probabilityTable.Keys) {
                averageLength += probabilityTable[key] * codes[key].Length;
            }

            return averageLength / text.Length;
        }
        private void GetCodes(BinaryTree tree, string c) {
            
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
        private PriorityQueue<BinaryTree> GetBinaryTrees(Dictionary<char, int> probabilityTable) {
            PriorityQueue<BinaryTree> queue = new PriorityQueue<BinaryTree>();
            
            while (probabilityTable.Count > 0) {

                int maxPriority = probabilityTable.Values.Max();
                foreach (char key in probabilityTable.Keys) {
                    if (probabilityTable[key] == maxPriority) {
                        queue.Enqueue(new BinaryTree(key, maxPriority), maxPriority);
                        probabilityTable.Remove(key);
                        break;
                    } 
                }
            }

            return queue;
        }
        
        public string GetOriginalText(BinaryTree root, string huffmanText) {
            string originalText = "";

            BinaryTree currentNode = root;
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
    }
}
