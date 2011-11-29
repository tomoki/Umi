using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace Umi.Tree{
    public class BlockNode : Node{
        public BlockNode UpperBlock;
        private ArrayList ChildNodes;
        private Dictionary<string,Node> Name;

        //一番上のブロックノード
        public BlockNode(){
            UpperBlock = null;
            ChildNodes = new ArrayList();
            Name = new Dictionary<string,Node>();
        }

        //ブロックノードの下のブロックノード
        public BlockNode(BlockNode upper){
            UpperBlock = upper;
            ChildNodes = new ArrayList();
            Name = new Dictionary<string,Node>();
        }

        public void Add(Node n){
            ChildNodes.Add(n);
        }

        private bool containName(string str){
            return Name.ContainsKey(str);
        }

        public Node Get(string str){
            if(containName(str)){
                return Name[str];
            }else if(UpperBlock != null){
                return UpperBlock.Get(str);
            }else{
                throw new UmiException(string.Format("Undefined Symbol: {0}",str));
            }
        }

        private bool existName(string str){
            if(containName(str)){
                return true;
            }else if(UpperBlock != null){
                return UpperBlock.existName(str);
            }else{
                return false;
            }
        }

        public void Set(string str,Node n){
            if(existName(str)){
                SetSub(str,n);
            }else{
                Name[str] = n;
            }
        }

        private void SetSub(string str,Node n){
            if(containName(str)){
                Name[str] = n;
                return;
            }else{
                UpperBlock.SetSub(str,n);
            }
        }

        public override Node eval(){
            Node returnNode = null;
            foreach(Node n in ChildNodes){
                returnNode = n.eval();
            }
            return returnNode;
        }

        public override string ToString(){
            StringBuilder sb = new StringBuilder();
            sb.Append("(#BlockNode:{");
            foreach(Node n in ChildNodes){
                sb.Append("-"+n.ToString());
            }
            sb.Append("})");
            return sb.ToString();
        }
    }
}

