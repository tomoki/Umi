using System;
using System.Collections.Generic;

namespace Umi{
    //トークンと文字列の対応。
    public class TokenMap{
        //infixは、二項間演算子。
        //ex. a + bの+
        private static Dictionary<TokenType,string> infix = new Dictionary<TokenType,string>();
        //prefixは、前につく、単項演算子。
        //ex. -b の -
        private static Dictionary<TokenType,string> prefix = new Dictionary<TokenType, string>();

        static TokenMap (){
            infix[TokenType.Assign]       = "(=)";
            infix[TokenType.LocalAssign]  = "(:=)";
            infix[TokenType.AddAssign]    = "(+=)";
            infix[TokenType.SubAssign]    = "(-=)";
            infix[TokenType.MulAssign]    = "(*=)";
            infix[TokenType.DivAssign]    = "(/=)";
            infix[TokenType.PowAssign]    = "(**=)";
            infix[TokenType.Equal]        = "(==)";
			infix[TokenType.NotEqual]     = "(!=)";
            infix[TokenType.Less]         = "(<)";
            infix[TokenType.Greater]      = "(>)";
            infix[TokenType.LessEqual]    = "(<=)";
            infix[TokenType.GreaterEqual] = "(>=)";
            infix[TokenType.Plus]         = "(+)";
            infix[TokenType.Minus]        = "(-)";
            infix[TokenType.Asterisk]     = "(*)";
            infix[TokenType.Slash]        = "(/)";
			infix[TokenType.Percent]      = "(%)";
            infix[TokenType.Pow]          = "(**)";
            infix[TokenType.And]          = "(&&)";
            infix[TokenType.Or]           = "(||)";

            prefix[TokenType.Plus]        = "(@+)";
            prefix[TokenType.Minus]       = "(@-)";
            prefix[TokenType.Not]         = "(@!)";

        }

        //与えられたトークンの種類に対応する文字列を返す。
        public static string GetInfix(TokenType tokentype){
            return infix[tokentype];
        }

        //与えられたトークンの種類に対応する文字列を返す。
        public static string GetPrefix(TokenType tokentype){
            return prefix[tokentype];
        }
    }
}

