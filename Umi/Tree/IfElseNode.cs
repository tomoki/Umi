using System;
using System.Collections;

namespace Umi.Tree{
    public class IfElseNode:Node{
        IfNode ifnode;
        ArrayList elifnodes;
        ElseNode elsenode;
        public IfElseNode(IfNode ifn,ElseNode elsen){
            ifnode = ifn;
            elsenode = elsen;
			elifnodes = new ArrayList();
        }
        public IfElseNode(){
			elifnodes = new ArrayList();
        }
        public void SetIfNode(IfNode ifnode){
            this.ifnode = ifnode;
        }
        public void SetElseNode(ElseNode elsenode){
            this.elsenode = elsenode;
        }
        public void AddElifNode(ElifNode elif){
            elifnodes.Add(elif);
        }
        public override Node eval(){
            Node returnnode = null;
            //elseを評価すべき？
            bool toEvalElse = true;
            //if文を評価。ifn.Excutedは実行されたかどうか
            //(条件式がTrueかどうか)
            returnnode = ifnode.eval();
            //elifを回す
            if(!(ifnode.Excuted)){
                foreach(ElifNode elif in elifnodes){
                    returnnode = elif.eval();
                    if(elif.Excuted){
                        toEvalElse = false;
                        break;
                    }
                }
                if(toEvalElse&&elsenode!=null){
                    returnnode = elsenode.eval();
                }
            }
            return returnnode;
        }
    }
}

