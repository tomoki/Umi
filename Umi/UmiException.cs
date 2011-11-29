using System;

namespace Umi{
    //Umi用の例外。
    class UmiException : Exception{
        public UmiException(string message) : base("error : " + message){
        }
    }
    //Lexical Analyzer用の例外。
    class AnalyzerException : UmiException{
        public AnalyzerException(string message) : base(message){
        }
    }
    //パーサー用の例外。
    class ParserException : Exception{
        public ParserException(string message):
            base("Syntax Error : \n\t" + message){
            }
    }
    //パーサー用
    //予期しないものが来た時の例外。
    //ex.かっこの対応が取れていない、とか。
    class UnexpectedException : ParserException{
        public UnexpectedException(TokenType unexpectedToken,string expectedToken) :
            base(string.Format("Unexpected {0} : expected {1}",
                               unexpectedToken,
                               expectedToken))
            {
            }
    }
}
