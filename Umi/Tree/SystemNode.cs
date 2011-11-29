using System;

namespace Umi.Tree{
    //システム関連のノード。
    //組み込み関数とか。
    public class SystemNode:Node{
        public SystemNode(){
            SetSlot("print",this.print);
        }
        public override Node eval(){
            return this;
        }
        public Node print(Node node){
			if(node is ArgumentsNode){
				Node s = null;
				for(int i=0;i<((ArgumentsNode)node).Length;i++){
					s = send_message((((ArgumentsNode)node)[i]).eval(),null,"to_s");
					Console.WriteLine(((StringNode)s).Value);
				}
				return s;
			}else{
				Node s = send_message(node.eval(),null,"to_s");
				Console.WriteLine(((StringNode)s).Value);
				return s;
			}
            return null;
        }
        public override string ToString(){
            return string.Format("[System]");
        }
    }
}

