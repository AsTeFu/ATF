using System;
using System.Collections.Generic;
using System.Linq;
namespace HuffmanCode.Codes {
    using System.IO;

    class HuffmanCompress {

        Dictionary<char, string> codes = new Dictionary<char, string>();
        private string text;

        public HuffmanCompress(string text) {
            this.text = text;
            var frequenceTable = new Dictionary<char, int>();

            for (int i = 0; i < text.Length; i++)
                if (!frequenceTable.Keys.Contains(text[i]))
                    frequenceTable.Add(text[i], 1);
                else frequenceTable[text[i]]++;

            PriorityQueue queue = new PriorityQueue();
            foreach (var letter in frequenceTable.Keys) {
                BinaryTree tree = new BinaryTree(new Node(letter, frequenceTable[letter]));
                queue.Add(tree);
            }
            
            while (queue.Length > 1) 
                queue.Add(new BinaryTree(new Node(queue.Remove().root, queue.Remove().root)));
            

            GetCodes(queue.Remove().root);

            void GetCodes(Node root, string c = "") {
                if (root.leftChild != null) {
                    GetCodes(root.leftChild, c + "0");
                    GetCodes(root.rightChild, c + "1");
                } else {
                    codes.Add(root.letter, c == "" ? "0" : c);
                }
            }
        }
        
    }
}
