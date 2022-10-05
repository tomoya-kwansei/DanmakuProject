﻿
using UnityEngine;

public struct ScriptToken
{
    public Type type;
    public string user_defined_symbol;
    public int int_val;
    public float float_val;

    /**
     * 与えられたトークンの情報から適切にトークンを生成する。
     */
    public static ScriptToken GenerateToken(string snippet, Type token)
    {
        switch (token)
        {
            case Type.SYMBOL_ID:
                return GenerateUserDefinedSymbolToken(snippet);
            case Type.INT_LITERAL:
                return GenerateIntValToken(snippet);
            case Type.FLOAT_LITERAL:
                return GenerateFloatValToken(snippet);
            default:
                return GenerateReservedToken(token);
        }
    }
    public static ScriptToken EndOfFile => new(){ type = Type.END_OF_FILE };
    public enum Type
    {
        NONE,
        BEHAVIOR,
        BULLET,
        ACTION,
        PHASE_SEPARATOR,
        ID_NAVIGATOR,
        INT,
        FLOAT,
        REPEAT,
        BREAK,
        INFINITY,
        IF,
        ELSE,
        PLUS,
        SUB,
        MULTIPLY,
        DIVIDE,
        MOD,
        AND,
        OR,
        NOT,
        GREATER_EQUAL,
        LESS_EQUAL,
        GREATER_THAN,
        LESS_THAN,
        BRACKET_LEFT,
        BRACKET_RIGHT,


        CURLY_BRACKET_LEFT,
        CURLY_BRACKET_RIGHT,

        COLON,

        ASSIGNMENT,
        EQUAL,
        NOT_EQUAL,
        COMMA,

        SYMBOL_ID,
        INT_LITERAL,
        FLOAT_LITERAL,

        END_OF_FILE

    };


    private static ScriptToken GenerateReservedToken(Type giventype)
    {
        var token = new ScriptToken();
        token.type = giventype;
        return token;
    }
    private static ScriptToken GenerateUserDefinedSymbolToken(string given_user_defined_symbol)
    {
        var token = new ScriptToken();
        token.type = Type.SYMBOL_ID;
        token.user_defined_symbol = given_user_defined_symbol;
        return token;
    }
    private static ScriptToken GenerateIntValToken(string given_int_val)
    {
        var token = new ScriptToken();
        token.type = Type.INT_LITERAL;
        token.int_val = int.Parse(given_int_val);
        return token;
    }
    private static ScriptToken GenerateFloatValToken(string given_float_val)
    {
        var token = new ScriptToken();
        token.type = Type.FLOAT_LITERAL;
        var trimmedFloatVal = given_float_val.TrimEnd('f');
        token.float_val = float.Parse(trimmedFloatVal);
        return token;
    }
    
    
}
