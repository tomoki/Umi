namespace Umi{
    /*トークンの種類を定義。
     * 33-256はASCIIコードと対応。
     * それ以降は対応なし。
     */
    public enum TokenType{
        // !
        Not = 33,
        // "
        DoubleQuote,
        // #
        Hash,
        // $
        Dollar,
        // %
        Percent,
        // &
        Ampersand,
        // '
        Quote,
        // (
        OpenParen,
        // )
        CloseParen,
        // *
        Asterisk,
        // +
        Plus,
        // ,
        Comma,
        // -
        Minus,
        // .
        Dot,
        // /
        Slash,
        // :
        Colon = 58,
        // ;
        SemiColon,
        // <
        Less,
        // =
        Assign,
        // >
        Greater,
        // ?
        Question,
        // @
        At,
        // [
        OpenBracket = 91,
        // \
        BackSlash,
        // ]
        CloseBracket,
        // ^
        Hat,
        // _
        UnderScore,
        // `
        QuasiQuote,
        // {
        OpenBrace=123,
        // |
        Pipe,
        // }
        CloseBrace,

        EOF=256,
        Newline,
        End,

        // [0-9]+.?[0-9]+
        Number,
        // ([a-zA-Z]|_)([a-z]|[A-Z][0-9])*
        Symbol,
        // \".*\"
        String,

        // =>
        DoubleArrow,
        // :=
        LocalAssign,
        // *=
        MulAssign,
        // +=
        AddAssign,
        // -=
        SubAssign,
        // /=
        DivAssign,
        // **=
        PowAssign,
        // **
        Pow,
        // ==
        Equal,
        // !=
        NotEqual,
        // <=
        LessEqual,
        // >=
        GreaterEqual,
        // &&
        And,
        // ||
        Or,
        // ++
        Increment,
        // --
        Decrement,

        // def
        Define,

        // if
        If,
        // else
        Else,
        // elif
        Elif,
        // while
        While,
    }
}

