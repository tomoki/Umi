using System;

namespace Umi.Tree{
    public class NegativeNode :Node{
        Node node;
        public NegativeNode(Node node){
            this.node = node;
        }
        public override Node eval(){
            return send_message(node.eval(),null,TokenMap.GetPrefix(TokenType.Minus));
        }
        public override string ToString(){
            return string.Format("(#NegativeNode: {0})",this.node);
        }

    }
}

