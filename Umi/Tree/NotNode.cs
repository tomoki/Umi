using System;

namespace Umi.Tree{
	public class NotNode:Node{
        Node node;
		public NotNode(Node node){
            this.node = node;
		}
        public override Node eval(){
            return send_message(node.eval(),null,TokenMap.GetPrefix(TokenType.Not));
        }
        public override string ToString(){
            return string.Format("(#NotNode :{0}",this.node);
        }
    }
}

