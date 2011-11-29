using System;
using System.Collections.Generic;

namespace Umi.Tree{
    /*
     * ノードが持つ、メソッド用のdelegate(関数オブジェクト)。
     * 単項演算子などの渡すものがない関数ではnullを渡す。
     */
    public delegate Node Function(Node node);
    /*
     * Node用抽象(abstract)クラス
     */
    public abstract class Node{
        // このノードが所属するブロック。
        // Symbol以外は
        // わざわざ定義する必要はない。
        private BlockNode block;
        // slotsにはメソッド、メンバー変数を保存する。
        // ただし、それを呼ぶ方法は未実装。2011/11/25
        private Dictionary<string,Function> slots;

        public BlockNode Block{
            get{
                return block;
            }
            set{
                block = value;
            }
        }

        public Node(){
            slots = new Dictionary<string,Function>();
            SetSlot("to_s",this.to_s);
        }

        //評価されるときに呼ばれるメソッド。
        //仮想メソッドであり、派生オブジェクトで適宜overrideする。
        public virtual Node eval(){
            return this;
        }

        public virtual Node to_s(Node node){
            return new StringNode("Node");
        }

        //send_messageは、senderのGetSlot(message)した関数にrecvを渡して実行する。
        //これはSmallTalkのオブジェクト指向。
        public Node send_message(Node sender,Node recv,string message){
            return sender.recv_message(recv,message);
        }

        //messageを受け取る。
        public Node recv_message(Node recv,string message){
            if(recv==null){
                return GetSlot(message)(null);
            }else{
                return GetSlot(message)(recv);
            }
        }
        public void SetSlot(string s,Function func){
            slots[s]  = func;
        }
        public Function GetSlot(string s){
            return slots[s];
        }

        public override int GetHashCode(){
            return base.GetHashCode();
        }
    }
}

