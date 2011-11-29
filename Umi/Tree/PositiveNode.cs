using System;

namespace Umi.Tree{
    public class PositiveNode:Node{
        Node node;
        public PositiveNode(Node node){
            this.node = node;
        }
        public override Node eval(){
            return send_message(node.eval(),null,TokenMap.GetPrefix(TokenType.Plus));
        }
        public override string ToString(){
            return string.Format("(#PositiveNode: {0})",this.node);
        }
    }
}

