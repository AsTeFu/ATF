using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuffmanCode.Codes {
    class PriorityQueue {
        private List<BinaryTree> data;
        public int Length {
            get {
                return data.Count;
            }
        }

        public PriorityQueue() {
            data = new List<BinaryTree>();
        }

        public void Add(BinaryTree tree) {
            data.Add(tree);
            data.Sort((a, b) => {
                return a.Frequence > b.Frequence ? 1 : (a.Frequence < b.Frequence ? -1 : 0);
            });
        }

        public BinaryTree Remove() {
            BinaryTree tmp = data.First();
            data.Remove(tmp);
            return tmp;
        }
    }
}
