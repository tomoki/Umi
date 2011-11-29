using System;
using System.Collections;
using System.Collections.Generic;
using Umi.IO;

namespace Umi{
    /*
     * Lexical Analyzerクラス
     * 文字列を読み込んで、それをトークンに分解する。
     * トークンの種類はTokenType.csを参照。
     * 外から使う場合、Advance()とValue,Tokenを使用する。
     */
    public class Lexer{
        //文字を読み込む為のクラス。
        //詳細はIO.LexerReaderを参照
        private LexerReader reader;
        private TokenType tokenType;
        private object tokenValue;
        private string fileName;
        private Stack<int> beginnings = new Stack<int>();
        private bool beginningOfLine = true;

        //Tokenの種類を返す。
        public TokenType Token{
            get {
                return tokenType;
            }
        }

        //Tokenの値。
        //使用するときは随時キャストする。
        public object Value{
            get {
                return tokenValue;
            }
        }

        //コンストラクタ
        //第一引数は実際のコード、第二引数はファイル名(オプション)
        public Lexer(string code,string fname){
            reader = new LexerReader(code);
            fileName = fname;
            beginnings.Push(0);
        }

        //コンストラクタ
        //第一引数は実際のコード。
        public Lexer(string code){
            reader = new LexerReader(code);
            fileName = "No File(maybe interpreter)";
            beginnings.Push(0);
        }

        //次のトークンを読み込む。
        //読み込めたらtrue,読み込めなかったら(ファイルが終わったら)false
        public bool Advance(){
            return LexToken();
        }

        //次のトークンを読み込む。
        //読み込めたらtrue,読み込めなかったら(ファイルが終わったら)false
        private bool LexToken(){
            SkipWhiteSpace();
            int c = reader.Peek();
            switch(c){
                case -1:
                    return false;
                case '\n':
                    reader.Read();
                    beginningOfLine = true;
                    return LexToken();
                case '#':
                    SkipLineComment();
                    return LexToken();
            }
            LexVisibles();
            return true;
        }

        //空白文字を読み飛ばす。
        private void SkipWhiteSpace(){
            int c = reader.Peek();
            while((c != -1) && (c != '\n') && char.IsWhiteSpace((char)c)){
                reader.Read();
                c = reader.Peek();
            }
        }

        //ラインコメントを読み飛ばす。
        private void SkipLineComment(){
            int c = reader.Peek();
            while((c!=-1) && (c!='\n')){
                reader.Read();
                c = reader.Peek();
            }
        }

        //文字列内のエスケープシーケンスを処理。
        //適当な文字を参照型で渡すと、エスケープシーケンス化して返す。
        private void SetEscapeSequence(ref int c){
            reader.Read();
            c = reader.Peek();
            switch(c){
                case -1:
                    throw new AnalyzerException("EOF inside string.");
                case '\n':
                    throw new AnalyzerException("NewLine inside string.");
                case 'a':
                    c = '\a';
                    break;
                case 'b':
                    c = '\b';
                    break;
                case 'f':
                    c = '\f';
                    break;
                case 'n':
                    c = '\n';
                    break;
                case 't':
                    c = '\t';
                    break;
                case 'v':
                    c = '\v';
                    break;
            }
        }
        /*
         * シンボルの読み込み。
         * 場合によっては、TokenType.If,TokenType.While,TokenType.defも処理する。
         */
        private void LexSymbol(){
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            int c = reader.Peek();
            while((c!=-1) && IsUmiIdentifierPart((char)c)){
                sb.Append((char)reader.Read());
                c = reader.Peek();
            }
            tokenValue = sb.ToString();
            switch((string)tokenValue){
                case "if":
                    tokenType = TokenType.If;
                    break;
                case "else":
                    tokenType = TokenType.Else;
                    break;
                case "elif":
                    tokenType = TokenType.Elif;
                    break;
                case "while":
                    tokenType = TokenType.While;
                    break;
                case "def":
                    tokenType = TokenType.Define;
                    break;
                default:
                    tokenType = TokenType.Symbol;
                    break;
            }
        }

        //Symbolのスタートかの判定。
        //通常の文字、もしくは_ならtrue,数値、それ以外の特殊文字はfalse.
        private bool IsUmiIdentifierStart(char c){
            return (char.IsLetter(c) || (c=='_'));
        }

        //Symbolの途中判定。
        //通常の文字、数字もしくは_ならtrue、それ以外の特殊文字はfalse.
        private bool IsUmiIdentifierPart(char c){
            return (char.IsLetterOrDigit(c) || (c=='_'));
        }

        //数字を読み込む。
        private void LexNumber(){
            int ch = reader.Peek();
            //すでに.が来ているか。小数の判定に使う。
            bool alreadyDoted = false;
            System.Text.StringBuilder sb= new System.Text.StringBuilder();
            while(ch!=-1){
                if(char.IsDigit((char)ch)){
                    sb.Append((char)ch);
                    reader.Read();
                    //.が初めてきたとき
                }else if((char)ch=='.' && !alreadyDoted){
                    alreadyDoted = true;
                    //.の次が数字ならば小数として処理する。
                    if(reader.Peek(1) != -1 && char.IsDigit((char)reader.Peek(1))){
                        sb.Append((char)ch);
                        reader.Read();
                        sb.Append((char)reader.Read());
                    }
                }else{
                    break;
                }
                ch = reader.Peek();
            }
            tokenType = TokenType.Number;
            tokenValue = decimal.Parse(sb.ToString());
        }

        //文字列を読み込む
        private void LexString(){
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            while(true){
                int c = reader.Peek();
                //文字列終わり。
                if(c=='"'){
                    reader.Read();
                    break;
                }
                switch(c){
                    case -1:
                        throw new AnalyzerException("EOF inside string.");
                    case '\n':
                        throw new AnalyzerException("NewLine inside string.");
                        //エスケープシーケンス。
                    case '\\':
                        SetEscapeSequence(ref c);
                        break;
                }
                reader.Read();
                sb.Append((char)c);
            }
            tokenType = TokenType.String;
            tokenValue = sb.ToString();
        }

        //普通の見える文字。(空白、改行以外)の処理。
        private void LexVisibles(){
            int c = reader.Peek();
            switch(c){
                case '$':
                case '%':
                case '(':
                case ')':
                case ',':
                case '.':
                case '?':
                case '@':
                case '[':
                case ']':
                case '^':
                case '`':
                case '{':
                case '}':
                case '\'':
                    reader.Read();
                    //TokenTypeとASCIIはある程度互換性がある。
                    tokenType = (TokenType)c;
                    break;
                    //セミコロンは
                case ';':
                    reader.Read();
                    tokenType = TokenType.Newline;
                    break;
                case '&':
                    reader.Read();
                    // &&
                    if(reader.Peek()=='&'){
                        reader.Read();
                        tokenType = TokenType.And;
                    }else{
                        tokenType = TokenType.Ampersand;
                    }
                    break;
                case '*':
                    reader.Read();
                    switch(reader.Peek()){
                        //*=
                        case '=':
                            reader.Read();
                            tokenType = TokenType.MulAssign;
                            break;
                            // **
                        case '*':
                            reader.Read();
                            if(reader.Peek()=='='){
                                reader.Read();
                                tokenType = TokenType.PowAssign;
                            }else{
                                tokenType = TokenType.Pow;
                            }
                            break;
                        default:
                            tokenType = TokenType.Asterisk;
                            break;
                    }
                    break;
                case '+':
                    reader.Read();
                    switch(reader.Peek()){
                        case '=':
                            reader.Read();
                            tokenType = TokenType.AddAssign;
                            break;
                        case '+':
                            reader.Read();
                            tokenType = TokenType.Increment;
                            break;
                        default:
                            tokenType = TokenType.Plus;
                            break;
                    }
                    break;
                case '-':
                    reader.Read();
                    switch(reader.Peek()){
                        case '=':
                            reader.Read();
                            tokenType = TokenType.SubAssign;
                            break;
                        case '-':
                            reader.Read();
                            tokenType = TokenType.Decrement;
                            break;
                        default:
                            tokenType = TokenType.Minus;
                            break;
                    }
                    break;
                case '/':
                    reader.Read();
                    // /=
                    if(reader.Peek()=='='){
                        reader.Read();
                        tokenType = TokenType.DivAssign;
                    }else{
                        tokenType = TokenType.Slash;
                    }
                    break;
                case ':':
                    reader.Read();
                    if(reader.Peek() == '='){
                        reader.Read();
                        tokenType = TokenType.LocalAssign;
                    }else{
                        tokenType = TokenType.Colon;
                    }
                    break;
                case '!':
                    reader.Read();
                    if(reader.Peek() == '='){
                        reader.Read();
                        tokenType = TokenType.NotEqual;
                    }else{
                        tokenType = TokenType.Not;
                    }
                    break;
                case '<':
                    reader.Read();
                    if(reader.Peek() == '='){
                        reader.Read();
                        tokenType = TokenType.LessEqual;
                    }else{
                        tokenType = TokenType.Less;
                    }
                    break;
                case '=':
                    reader.Read();
                    switch(reader.Peek()){
                        case '>':
                            reader.Read();
                            tokenType = TokenType.DoubleArrow;
                            break;
                        case '=':
                            reader.Read();
                            tokenType = TokenType.Equal;
                            break;
                        default:
                            tokenType = TokenType.Assign;
                            break;
                    }
                    break;
                case '>':
                    reader.Read();
                    if(reader.Peek()=='='){
                        reader.Read();
                        tokenType = TokenType.GreaterEqual;
                    }else{
                        tokenType = TokenType.Greater;
                    }
                    break;
                case '|':
                    reader.Read();
                    if(reader.Peek()=='|'){
                        reader.Read();
                        tokenType = TokenType.Or;
                    }else{
                        tokenType = TokenType.Pipe;
                    }
                    break;
                case '"':
                    reader.Read();
                    LexString();
                    break;
                    //記号類で始まらないとき。
                default:
                    //最初が数字なら、数字として処理
                    if(char.IsDigit((char)c)){
                        LexNumber();
                        //シンボルを構成する文字の場合は、シンボルとして処理
                    }else if(IsUmiIdentifierStart((char)c)){
                        LexSymbol();
                    }else{
                        string output = string.Format("unknown char: {0}",(char)c);
                        throw new AnalyzerException(output);
                    }
                    break;
            }
        }
    }
}

