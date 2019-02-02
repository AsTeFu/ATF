using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuffmanCode.Codes
{
    class BinaryTree {
        public int Frequence {
            get {
                return root.frequence;
            }
        }

        public readonly Node root;

        public BinaryTree(Node root) {
            this.root = root;
        }
    }

    public class Node {
        public int frequence;
        public readonly char letter;
        public Node leftChild;
        public Node rightChild;

        public Node(char letter, int frequence) {
            this.frequence = frequence;
            this.letter = letter;
        }
        public Node() {
            frequence = 0;
        }
        public Node(Node leftChild, Node rightChild) {
            frequence = 0;
            AddChild(leftChild);
            AddChild(rightChild);
        }

        public void AddChild(Node newChild) {
            if (leftChild == null)
                leftChild = newChild;
            else if (leftChild.frequence <= newChild.frequence)
                rightChild = newChild;
            else {
                rightChild = leftChild;
                leftChild = newChild;
            }

            frequence += newChild.frequence;
        }
        public bool isLeaf() {
            return leftChild == null && rightChild == null;
        }
    }
}
