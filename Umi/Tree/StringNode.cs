using System;

namespace Umi.Tree{
    public class StringNode : Node{
        public string Value;
        public StringNode(string str) : base(){
            Value = str;
            SetSlot(TokenMap.GetInfix(TokenType.Plus),this.plus);
            SetSlot("to_s",this.to_s);
        }

        public Node plus(Node node){
            Node n = node.eval();
            if(!(node is StringNode)){
                throw new UmiException("Right is not String");
            }
            return new StringNode(Value+(((StringNode)n).Value));
        }

        public override Node eval(){
            return this;
        }
        public override Node to_s(Node node){
            return new StringNode(Value);
        }
        public override string ToString(){
            return string.Format("[StringNode: \"{0}\"]",Value);
        }
    }
}
