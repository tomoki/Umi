using System;
using System.Collections;

namespace Umi.Tree{
    /*
     * 関数の仮引数の為のノード。
     */
    public class ParametersNode : Node{
        private ArrayList parameters;
        public int Length {
            get{
                return parameters.Count;
            }
        }
		public SymbolNode this[int index]{
			get{
				return (SymbolNode)parameters[index];
			}
		}
        public ParametersNode() : base(){
            parameters = new ArrayList();
        }
        //仮引数を追加
        public void Add(Node symbol){
            if(!(symbol is SymbolNode)){
                throw new UmiException("parameter is not Symbol");
            }
            parameters.Add(symbol);
        }

        public void SetBlock(BlockNode block){
            foreach(SymbolNode sn in parameters){
                sn.Block = block;
            }
        }
    }
}

