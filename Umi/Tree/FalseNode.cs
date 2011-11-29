using System;

namespace Umi.Tree{
    public class FalseNode:Node{
        public FalseNode():base(){
            SetSlot(TokenMap.GetInfix(TokenType.Equal),this.equal);
            SetSlot(TokenMap.GetPrefix(TokenType.Not),this.not);
            SetSlot("to_s",this.to_s);
        }
        public Node equal(Node node){
            if(node is FalseNode){
                return new TrueNode();
            }else if(node is TrueNode){
                return new FalseNode();
            }else{
                throw new UmiException("Right is not boolean");
            }
        }

        public Node not(Node n){
            return new TrueNode();
        }
        public override Node to_s(Node node){
            return new StringNode("false");
        }
        public override Node eval(){
            return this;
        }
    }
}

