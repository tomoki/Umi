using System;

namespace Umi.Tree{
    public class WhileNode:Node{
        Node Boolean;
        Node block;
        public WhileNode(Node boolean,Node Block){
            Boolean = boolean;
            block = Block;
        }
        public override Node eval(){
            Node returnnode = null;
            while(Boolean.eval() is TrueNode){
				returnnode = block.eval();

            }
            return returnnode;
        }
    }
}

