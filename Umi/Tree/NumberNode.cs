using System;

namespace Umi.Tree{
    public class NumberNode : Node{
        decimal Value;
        public NumberNode(decimal d):base(){
            Value = d;
            // (+)
            SetSlot(TokenMap.GetInfix(TokenType.Plus),this.plus);
            // (-)
            SetSlot(TokenMap.GetInfix(TokenType.Minus),this.minus);
            // (*)
            SetSlot(TokenMap.GetInfix(TokenType.Asterisk),this.mul);
            // (/)
            SetSlot(TokenMap.GetInfix(TokenType.Slash),this.div);
            SetSlot (TokenMap.GetInfix(TokenType.Percent),this.mod);
            // (==)
            SetSlot(TokenMap.GetInfix(TokenType.Equal),this.equal);
            SetSlot(TokenMap.GetInfix(TokenType.NotEqual),this.notequal);
            SetSlot(TokenMap.GetInfix(TokenType.Greater),this.greater);
            SetSlot(TokenMap.GetInfix(TokenType.GreaterEqual),this.greaterequal);
            SetSlot(TokenMap.GetInfix(TokenType.Less),this.less);
            SetSlot(TokenMap.GetInfix(TokenType.LessEqual),this.lessequal);

            SetSlot(TokenMap.GetPrefix(TokenType.Plus),this.positive);
            SetSlot(TokenMap.GetPrefix(TokenType.Minus),this.negative);

            SetSlot("to_s",this.to_s);
        }

        public Node plus(Node node){
            Node n = node.eval();
            if(!(node is NumberNode)){
                throw new Exception("Right is not Number");
            }
            return new NumberNode(Value+(((NumberNode)n).Value));
        }

        public Node minus(Node node){
            Node n = node.eval();
            if(!(node is NumberNode)){
                throw new Exception("Right is not Number");
            }
            return new NumberNode(Value-(((NumberNode)n).Value));
        }

        public Node mul(Node node){
            Node n = node.eval();
            if(!(node is NumberNode)){
                throw new Exception("Right is not Number");
            }
            return new NumberNode(Value*(((NumberNode)n).Value));
        }

        public Node div(Node node){
            Node n = node.eval();
            if(!(node is NumberNode)){
                throw new Exception("Right is not Number");
            }
            return new NumberNode(Value/(((NumberNode)n).Value));
        }
        public Node mod(Node node){
            Node n = node.eval();
            if(!(node is NumberNode)){
                throw new Exception("Right is not Number");
            }
            return new NumberNode(Value % (((NumberNode)n).Value));
        }


        public Node equal(Node node){
            Node n = node.eval();
            if(!(node is NumberNode)){
                throw new Exception("Right is not Number");
            }
            if(((NumberNode)n).Value == this.Value){
                return new TrueNode();
            }else{
                return new FalseNode();
            }
        }

        public Node notequal(Node node){
            Node n = node.eval();
            if(!(node is NumberNode)){
                throw new Exception("Right is not Number");
            }
            if(((NumberNode)n).Value != this.Value){
                return new TrueNode();
            }else{
                return new FalseNode();
            }
        }

        public Node greater(Node node){
            Node n = node.eval();
            if(!(node is NumberNode)){
                throw new Exception("Right is not Number");
            }
            if(((NumberNode)n).Value < this.Value){
                return new TrueNode();
            }else{
                return new FalseNode();
            }
        }


        public Node greaterequal(Node node){
            Node n = node.eval();
            if(!(node is NumberNode)){
                throw new Exception("Right is not Number");
            }
            if(((NumberNode)n).Value <= this.Value){
                return new TrueNode();
            }else{
                return new FalseNode();
            }
        }


        public Node less(Node node){
            Node n = node.eval();
            if(!(node is NumberNode)){
                throw new Exception("Right is not Number");
            }
            if(((NumberNode)n).Value > this.Value){
                return new TrueNode();
            }else{
                return new FalseNode();
            }
        }


        public Node lessequal(Node node){
            Node n = node.eval();
            if(!(node is NumberNode)){
                throw new Exception("Right is not Number");
            }
            if(((NumberNode)n).Value >= this.Value){
                return new TrueNode();
            }else{
                return new FalseNode();
            }
        }


        public Node positive(Node node){
            return new NumberNode(1*Value);
        }

        public Node negative(Node node){
            return new NumberNode(-1*Value);
        }

        public override Node eval(){
            return this;
        }

        public override Node to_s(Node node){
            return new StringNode(Value.ToString());
        }

        public override string ToString ()
        {
            return string.Format ("[NumberNode:{0}]",Value);
        }
    }
}
