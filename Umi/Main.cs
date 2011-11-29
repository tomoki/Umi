using System;

namespace Umi{
    class MainClass{
        public static void Main (string[] args){
            //引数がひとつ。
            if(args.Length==1){
                //それをファイルパスと思って実行。
                Evaluator.ExecuteFile(args[0]);
            }else{
                //引数がひとつでなければ、
                //インタプリタを起動する。
                Evaluator.Interpret();
            }
        }
    }
}
