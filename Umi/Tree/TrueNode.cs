using System;

namespace Umi.Tree{
    public class TrueNode:Node {
        public TrueNode():base(){
            SetSlot(TokenMap.GetInfix(TokenType.Equal),this.equal);
            SetSlot(TokenMap.GetPrefix(TokenType.Not),this.not);
            SetSlot("to_s",this.to_s);
        }
        public Node equal(Node node){
            if(node is TrueNode){
                return new TrueNode();
            }else if(node is FalseNode){
                return new FalseNode();
            }else{
                throw new UmiException("Right is not boolean");
            }
        }
        public Node not(Node n){
            return new FalseNode();
        }
        public override Node to_s(Node node){
            return new StringNode("true");
        }

        public override Node eval(){
            return this;
        }

    }
}
