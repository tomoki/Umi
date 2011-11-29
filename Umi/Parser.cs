using System;
using System.Diagnostics;
using Umi.Tree;


namespace Umi{
    public class Parser{
        private Lexer lex;
        private TokenType tokenType;
        //今いるブロック
        public BlockNode Block;

        //デバッグ用メソッド
        [Conditional("DEBUG")]
            public static void ShowDebugMessage(object message){
                Console.WriteLine(message.ToString());
            }

        public Parser(){
            //グローバルなブロック。
            //グローバル変数等を格納する。
            Block = new BlockNode();
            Block.Set("true",new TrueNode());
            Block.Set("false",new FalseNode());
			Block.Set("system",new SystemNode());
        }

        private void GetToken(){
            //次のトークンが存在するならば
            if(lex.Advance()){
                //トークンをlexerから読み込む
                tokenType = lex.Token;
            }else{
                //存在しなければTokenType.EOF
                tokenType = TokenType.EOF;
            }
        }

        public Node Parse(Lexer lexer){
            ShowDebugMessage("Parse Called");
            //Lexerをセット
            lex = lexer;
            //最初のトークンを読み込む
            GetToken();
            //プログラムをパースしはじめる
            return ParseProgram();
        }

        //すべてパース、実行する。
        //TODO:ここは分離したほうがいい。
        public Node ParseAll(Lexer lexer){
            lex = lexer;
            GetToken();
            Node result = null;
            try{
                while(tokenType != TokenType.EOF){
                    Node node = ParseProgram();
                    if(node != null){
                        result = node.eval();
                    }
                }
            }catch (Exception e){
                Console.Error.WriteLine(e.Message);
            }
            return result;
        }

        private Node ParseProgram(){
            ShowDebugMessage("ParseProgram Called");
            //文(通常は一行)をパース
            Node obj = ParseStatement();
            switch(tokenType){
                case TokenType.EOF:
                case TokenType.Newline:
                    break;

                default:
                    throw new UnexpectedException(tokenType,"NewLine or Semicolon");
            }
            return obj;
        }

        private Node ParseStatement(){
            ShowDebugMessage("ParseStatement");
            switch(tokenType){
                case TokenType.EOF:
                    break;
                    //空白行を読み飛ばす
                case TokenType.Newline:
                    GetToken();
                    return ParseStatement();
                default:
                    //式を評価する
                    return ParseExpression();
            }
            return null;
        }

        //Expressionをパースする。
        //ここから他のメソッドが呼ばれていく。
        private Node ParseExpression(){
            return ParseAssignment();
        }

        private Node ParseAssignment(){
            Node node = ParseComparison();
            switch(tokenType){
                case TokenType.Assign:
                    node = ParseAssignment2(node);
                    break;
            }
            return node;
        }

        private Node ParseAssignment2(Node node){
            TokenType op_type = tokenType;
            GetToken();
            Node right = ParseExpression();
            Node op = new AssignNode(node,right,op_type);
            return op;
        }

        private Node ParseComparison(){
            Node node = ParseAdditiveExpression();
            switch(tokenType){
                case TokenType.Equal:
                case TokenType.NotEqual:
                case TokenType.Greater:
                case TokenType.GreaterEqual:
                case TokenType.Less:
                case TokenType.LessEqual:
                    node = ParseComparison2(node);
                    break;
                default:
                    break;
            }
            return node;
        }

        private Node ParseComparison2(Node node){
            TokenType op_type = tokenType;
            GetToken();
            Node right = ParseAdditiveExpression();
            Node compNode = new CompareNode(node,right,op_type);
            return compNode;
        }

        // + と - の処理。
        private Node ParseAdditiveExpression(){
            ShowDebugMessage("ParseAdditiveExpression");
            // +と-の前に*と/の処理。
            Node node =  ParseModExpression();
            while((tokenType == TokenType.Plus) ||
                    (tokenType == TokenType.Minus)){
                TokenType op_type = tokenType;
                GetToken();
                Node right = ParseModExpression();
                node = new OPNode(node,right,op_type);
            }
            return node;
        }
		private Node ParseModExpression(){
			Node node = ParseMultiplicativeExpression();
			while((tokenType == TokenType.Percent)){
				TokenType op_type = tokenType;
                GetToken();
                Node right = ParseMultiplicativeExpression();
                node = new OPNode(node,right,op_type);
            }
            return node;
		}

        //*と/の処理。
        private Node ParseMultiplicativeExpression(){
            ShowDebugMessage("ParseMultiplicativeExpression");
            Node node = ParseUnaryExpression();
            while((tokenType == TokenType.Asterisk) ||
                    (tokenType == TokenType.Slash)){
                TokenType op_type = tokenType;
                GetToken();
                Node right = ParseUnaryExpression();
                node = new OPNode(node,right,op_type);
            }
            return node;
        }

        private Node ParseUnaryExpression(){
            Node node = null;
            switch(tokenType){
                case TokenType.Plus:
                case TokenType.Not:
                case TokenType.Minus:
                    node = ParseUnaryExpression2();
                    break;
                default:
                    node = ParseFirst();
                    break;
            }
            return node;
        }

        private Node ParseUnaryExpression2(){
            TokenType type = tokenType;
            GetToken();
            Node node = ParseUnaryExpression();
            switch(type){
                case TokenType.Plus:
                    node = new PositiveNode(node);
                    break;
                case TokenType.Minus:
                    node = new NegativeNode(node);
                    break;
                case TokenType.Not:
                    node = new NotNode(node);
                    break;
            }
            return node;
        }

        //数値とか、文字列とか、一番最初にパースされるもの
        //もしくはカッコの中。
        private Node ParseFirst(){
            ShowDebugMessage("ParseFirst");
            Node node = null;
            switch(tokenType){
                case TokenType.Number:
                    node = new NumberNode((decimal)lex.Value);
                    GetToken();
                    break;
                case TokenType.String:
                    node = new StringNode((string)lex.Value);
                    GetToken();
                    break;
                case TokenType.Symbol:
                    node = new SymbolNode((string)lex.Value);
                    node.Block = Block;

                    GetToken();
                    if(tokenType == TokenType.OpenParen){
                        node = CallFunction((SymbolNode)node);
                    }
                    break;
                case TokenType.Define:
                    GetToken ();
                    node = DefineFunction();
                    break;
                case TokenType.If:
                    GetToken();
                    node = ParseIf();
                    break;
                case TokenType.While:
                    GetToken();
                    node = ParseWhile();
                    break;
                case TokenType.OpenBrace:
                    GetToken();
                    node = ParseBlock();
                    break;
                case TokenType.OpenParen:
                    GetToken();
                    node = ParseParentheses();
                    break;
                default:
                    break;
            }
            if(tokenType==TokenType.Dot){
                GetToken();
                node = ParseMethod(node);
            }
            return node;
        }
        private SymbolNode ParseSymbol(){
            SymbolNode node = new SymbolNode((string)lex.Value);
            node.Block = Block;
            GetToken();
            return node;
        }


        private Node ParseParentheses(){
            Node n = ParseExpression();
            if(tokenType != TokenType.CloseParen){
                throw new UnexpectedException(tokenType,"CloseParen");
            }
            GetToken();
            return n;
        }

        //ブロックを処理する。
        private Node ParseBlock(){
            //新しいブロックを今いるブロックの下に作る。
            BlockNode innerBlockNode = new BlockNode(Block);
            //今いるブロックを新しいブロックに。
            Block = innerBlockNode;
            //閉じ括弧がくるまで、文をブロックに入れる。
            while(tokenType != TokenType.CloseBrace){
                Node n = ParseStatement();
                if(n==null){
                    break;
                }
                innerBlockNode.Add(n);
            }
            if(tokenType != TokenType.CloseBrace){
                throw new UnexpectedException(tokenType,"CloseBrase(})");
            }
            GetToken();
            //中のブロックから、もともといたブロックに出る。
            Block = innerBlockNode.UpperBlock;
            return innerBlockNode;
        }

        //関数定義
        private Node DefineFunction(){
            ShowDebugMessage("Define Function");
            if(tokenType != TokenType.Symbol){
                throw new UnexpectedException(tokenType,"Symbol");
            }
            //関数名を読み込む。
            SymbolNode functionSymbol = ParseSymbol();
            ShowDebugMessage(functionSymbol);
            Node parameters = ParseParameters();
            Node block = ParseFirst();
            FunctionNode fn = new FunctionNode(functionSymbol,parameters,block);
            return fn;
        }

        private Node ParseIf(){
            IfElseNode ifelse = new IfElseNode();
            ShowDebugMessage("If");
            if(tokenType!=TokenType.OpenParen){
                throw new UnexpectedException(tokenType,"OpenParen");
            }
            GetToken();
            Node boolean = ParseStatement();
            if(tokenType!=TokenType.CloseParen){
                throw new UnexpectedException(tokenType,"CloseParen");
            }
            GetToken();
            Node block = ParseFirst();
            ifelse.SetIfNode(new IfNode(boolean,block));
            while(tokenType==TokenType.Elif){
                GetToken();
                if(tokenType!=TokenType.OpenParen){
                    throw new UnexpectedException(tokenType,"OpenParen");
                }
                GetToken();
                boolean = ParseStatement();
                if(tokenType!=TokenType.CloseParen){
                    throw new UnexpectedException(tokenType,"CloseParen");
                }
                GetToken();
                block = ParseFirst();
                ifelse.AddElifNode(new ElifNode(boolean,block));
            }
            if(tokenType == TokenType.Else){
                GetToken();
                ifelse.SetElseNode(new ElseNode((BlockNode)ParseFirst()));
            }
            return ifelse;
        }

        private Node ParseWhile(){
            ShowDebugMessage("While");
            if(tokenType!=TokenType.OpenParen){
                throw new UnexpectedException(tokenType,"OpenParen");
            }
            GetToken();
            Node boolean = ParseStatement();
            if(tokenType!=TokenType.CloseParen){
                throw new UnexpectedException(tokenType,"CloseParen");
            }
            GetToken();
            Node block = ParseFirst();
            WhileNode whilenode = new WhileNode(boolean,block);
            return whilenode;
        }

        //関数呼び出し。
        private Node CallFunction(SymbolNode symbol){
            Node f = symbol.eval();
            if(!(f is FunctionNode)){
                throw new UnexpectedException(tokenType,"not paren");
            }
            FunctionNode func = (FunctionNode)f;
            ArgumentsNode args = (ArgumentsNode)ParseArguments();
            return func.call(args);
        }
        private Node ParseMethod(Node node){
			SymbolNode name = ParseSymbol();
            Node argnode = ParseArguments();
            return new MethodNode(node,argnode,name.Name);
        }

        //仮引数ノードを構成する。
        private Node ParseParameters(){
            ParametersNode parameters = new ParametersNode();
            if(tokenType != TokenType.OpenParen){
                throw new UnexpectedException(tokenType,"OpenParen");
            }
            GetToken ();
            while(tokenType != TokenType.CloseParen){
                if(tokenType != TokenType.Symbol){
                    throw new UnexpectedException(tokenType,"Symbol");
                }
                Node n = ParseSymbol();
                parameters.Add(n);
                if(tokenType == TokenType.Comma){
                    GetToken();
                }
            }
            GetToken ();
            return parameters;
        }

        //実引数ノードを構成する。
        private Node ParseArguments(){
            ArgumentsNode arguments = new ArgumentsNode();
            if(tokenType != TokenType.OpenParen){
                throw new UnexpectedException(tokenType,"OpenParen");
            }
            GetToken();
            while(tokenType != TokenType.CloseParen){
                Node n = ParseExpression();
                arguments.Add(n);
                if(tokenType == TokenType.Comma){
                    GetToken();
                }
            }
            GetToken();
            return arguments;
        }
    }
}

