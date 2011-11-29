using System;

namespace Umi.Tree{
    public class ElseNode:Node{
        BlockNode FalseBlock;
        public ElseNode(BlockNode block){
            FalseBlock = block;
        }
        public override Node eval(){
            return FalseBlock.eval();
        }

    }
}

