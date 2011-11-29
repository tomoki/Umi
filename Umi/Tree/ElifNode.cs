using System;

namespace Umi.Tree{
    public class ElifNode :Node{
        private Node boolean;
        private Node TrueBlock;
        private bool excuted;
        public bool Excuted{
            get{
                return excuted;
            }
        }
        public ElifNode(Node boolean,Node TrueBlock){
            this.boolean = boolean;
            this.TrueBlock = TrueBlock;
            excuted = false;
        }

        public override Node eval(){
            if(this.boolean.eval() is TrueNode){
                excuted = true;
                return ((BlockNode)TrueBlock).eval();
            }else{
                excuted = false;
                return null;
            }
        }
    }
}

