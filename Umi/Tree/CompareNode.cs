using System;

namespace Umi.Tree{
    public class CompareNode :Node{
        Node Right,Left;
        TokenType OPType;
        public CompareNode(Node left,Node right,TokenType op_type){
            Left = left;
            Right = right;
            OPType = op_type;
        }
        public override Node eval(){
            return send_message(Left.eval(),Right.eval(),TokenMap.GetInfix(OPType));
        }
        public override string ToString(){
            return string.Format("[CompareNode:{0}]\n|-{1}\n|-{2}",OPType,Left,Right);
        }

    }
}

