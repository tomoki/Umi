using System;

namespace Umi.Tree{
    public class OPNode : Node{
        Node Right,Left;
        TokenType OPType;
        public OPNode(Node left,Node right,TokenType op_type){
            this.Left = left;
            this.Right = right;
            OPType = op_type;
        }
        public override Node eval(){
            return send_message(Left.eval(),Right.eval(),TokenMap.GetInfix(OPType));
        }
        public override string ToString(){
            return string.Format("[OPNode:{0}]\n|-{1}\n|-{2}",OPType,Left,Right);
        }
    }
}
