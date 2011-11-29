using System;
using System.IO;
using System.Collections.Generic;
using Umi.Tree;

namespace Umi{
    //評価をする為のクラス。
    //インスタンス化は行わない。
    public class Evaluator{
        //インタプリタ。
        //一行ずつ読み込んで、それを実行する。
        public static void Interpret(){
            Parser parser = new Parser();
            while(true){
                Console.Error.Write("Umi > ");
                string input = Console.ReadLine()+"\n";
                try{
                    Node obj = parser.Parse(new Lexer(input));
                    Console.WriteLine(obj);
                    if(!(obj==null)){
                        Node result = obj.eval();
                        Console.Error.WriteLine(" => {0}",result);
                    }
                }
                catch (Exception e){
                    Console.Error.WriteLine(e.Message);
                }
            }
        }


        //文字列を読み込んで、実行する。
        public static void Execute(string s){
            Lexer lexer = new Lexer(s);
            Parser parser = new Parser();
            parser.ParseAll(lexer);
        }
        //ファイルのパスを受け取って、ファイルを実行する。
        public static void ExecuteFile(string path){
            StreamReader r = new StreamReader(path);
            string s = r.ReadToEnd();
            Execute(s);
        }
    }
}
