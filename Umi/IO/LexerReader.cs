using System;
using System.Collections;

namespace Umi.IO{
    /*
     * Lexerのための文字列処理クラス。
     * Peek():次の文字を予測する。
     * Peek(int n):n文字先の文字を予測する。
     * Read():次の文字を読み込む。
     */
    public class LexerReader{
        //インデントによる構文を使用していたときの名残。未使用。
        private const int tabWidth = 4;
        //エラー処理用。未使用。
        private int lineNumber = 1;
        private int index = 0;

        /*
         * 実際のコードを保存しておくArrayList
         * 文字はint型に変換しておく。
         * なぜint型かというと、Tokenの多くはASCIIと対応しているから。
         * C#のchar型はunicodeが入る大きさ。
         */
        private ArrayList code;

        public LexerReader(){
            code = new ArrayList();
        }

        public LexerReader(string str){
            code = new ArrayList();
            //与えられた文字列をintに変換してArrayListに入れる。
            foreach(char ch in str){
                code.Add((int)ch);
            }
        }

        /*
         * (n+1)文字先(n文字先?)の文字を予測する
         * 引数なしの場合はPeek(0)と同じ意味。
         * ex. ArrayListに[1,2,3]が入っているとき
         * Peek(0)orPeek()は、1,Peek(1)は2を返す。
         * オーバーするときは-1を返す。
         */
        public int Peek(int n){
            //
            if(n>=code.Count){
                return -1;
            }else{
                return (int)code[n];
            }
        }
        public int Peek(){
            return Peek(0);
        }

        //実際に次の文字を読み込む。
        public int Read(){
            int ch = Peek();
            if(ch=='\t'){
                this.index = this.index + tabWidth;
            }else{
                this.index++;
            }
            if(ch=='\n'){
                this.index = 0;
                this.lineNumber++;
            }
            if(ch != -1){
                code.RemoveAt(0);
            }
            return ch;
        }
    }
}
