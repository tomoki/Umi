using System;

namespace Umi.Tree{
    public class SymbolNode:Node{
        private string name;
        public string Name{
            get{
                return name;
            }
        }
        public SymbolNode(string name):base(){
            this.name = name;
            SetSlot(TokenMap.GetInfix(TokenType.Assign),this.assign);
            SetSlot("to_s",this.to_s);
        }

        public Node assign(Node node){
            Node n = node.eval();
            Block.Set(name,n);
            return n;
        }
        public Node defineFunc(Node node){
            Block.Set(name,node);
            return node;
        }

        public override Node eval(){
            return Block.Get(name);
        }

        public override Node to_s(Node node){
            return new StringNode(name);
        }

        public override string ToString ()
        {
            return string.Format("(#SymbolNode :{0})",name);
        }
    }
}

