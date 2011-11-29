using System;

namespace Umi.Tree{
    public class FunctionNode : Node{
        public SymbolNode name;
        public ParametersNode paraNode;
        BlockNode func;
        public FunctionNode(Node name,Node paraNode,Node func) : base(){
            this.name = (SymbolNode)name;
            this.paraNode = (ParametersNode)paraNode;
            this.func = (BlockNode)func;
            this.paraNode.SetBlock(this.func);
        }
        public override Node eval(){
            return ((SymbolNode)name).defineFunc(this);
        }
        public Node call(Node node){
            ArgumentsNode argsNode = (ArgumentsNode)node;
            if(argsNode.Length != paraNode.Length){
                throw new UmiException("Call failed because of args'length");
            }
            for(int i=0;i<argsNode.Length;i++){
                paraNode[i].assign(argsNode[i].eval());
            }
            return func.eval();
        }
    }
}
