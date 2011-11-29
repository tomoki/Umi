using System;
using System.Collections;

namespace Umi.Tree{
    /*
     * 実引数ノード。
     * 実引数は、関数が呼び出されるときにじっさいに受け取る引数。
     * func(1,2);
     * の1と2。
     */
    public class ArgumentsNode:Node{
        //実引数を保存するArrayList
        private ArrayList arguments;
        /*
         * 長さにアクセスするプロパティ。
         * 関数呼び出し時に関数の仮引数の数と比較する。
         */
        public int Length{
            get{
                return arguments.Count;
            }
        }
        //ノードにアクセスするためのプロパティ。
        public Node this[int index]{
            get{
                return (Node)arguments[index];
            }
        }
        public ArgumentsNode():base(){
            arguments = new ArrayList();
        }
        //実引数を追加。
        public void Add(Node n){
            arguments.Add(n);
        }
    }
}

