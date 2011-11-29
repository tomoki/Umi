using System;

namespace Umi.Tree{
    /*
     * Symbolに何かを代入する。
     * Token.Equal以外にも使える。
     */
    public class AssignNode :Node{
        Node Right,Left;
        //演算子
        TokenType OPType;
        public AssignNode(Node left,Node right,TokenType op_type){
            Right = right;
            Left = left;
            OPType = op_type;
        }
        public override Node eval(){
            //send_messageは基底オブジェクトで定義されている。
            //Leftはevalせず、そのまま扱うのがポイント。
            return send_message(Left,Right.eval(),TokenMap.GetInfix(OPType));
        }
        public override string ToString(){
            return string.Format("[AssignNode:{0}]\n|-{1}\n|-{2}",OPType,Left,Right);
        }
    }
}

