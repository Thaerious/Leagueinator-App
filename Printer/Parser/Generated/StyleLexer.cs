//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.13.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from ./StyleLexer.g4 by ANTLR 4.13.1

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

using System;
using System.IO;
using System.Text;
using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
using DFA = Antlr4.Runtime.Dfa.DFA;

[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.13.1")]
[System.CLSCompliant(false)]
public partial class StyleLexer : Lexer {
	protected static DFA[] decisionToDFA;
	protected static PredictionContextCache sharedContextCache = new PredictionContextCache();
	public const int
		OPAR=1, DOT=2, HASH=3, STAR=4, COMMA=5, STRING=6, WS=7, KEY=8, COLON=9, 
		CPAR=10, MM_WS=11, SEMI=12, VALUE=13, VM_WS=14;
	public const int
		KEY_MODE=1, VALUE_MODE=2;
	public static string[] channelNames = {
		"DEFAULT_TOKEN_CHANNEL", "HIDDEN"
	};

	public static string[] modeNames = {
		"DEFAULT_MODE", "KEY_MODE", "VALUE_MODE"
	};

	public static readonly string[] ruleNames = {
		"OPAR", "DOT", "HASH", "STAR", "COMMA", "STRING", "WS", "KEY", "COLON", 
		"CPAR", "MM_WS", "SEMI", "VALUE", "VM_WS"
	};


	public StyleLexer(ICharStream input)
	: this(input, Console.Out, Console.Error) { }

	public StyleLexer(ICharStream input, TextWriter output, TextWriter errorOutput)
	: base(input, output, errorOutput)
	{
		Interpreter = new LexerATNSimulator(this, _ATN, decisionToDFA, sharedContextCache);
	}

	private static readonly string[] _LiteralNames = {
		null, "'{'", "'.'", "'#'", "'*'", "','", null, null, null, "':'", "'}'", 
		null, "';'"
	};
	private static readonly string[] _SymbolicNames = {
		null, "OPAR", "DOT", "HASH", "STAR", "COMMA", "STRING", "WS", "KEY", "COLON", 
		"CPAR", "MM_WS", "SEMI", "VALUE", "VM_WS"
	};
	public static readonly IVocabulary DefaultVocabulary = new Vocabulary(_LiteralNames, _SymbolicNames);

	[NotNull]
	public override IVocabulary Vocabulary
	{
		get
		{
			return DefaultVocabulary;
		}
	}

	public override string GrammarFileName { get { return "StyleLexer.g4"; } }

	public override string[] RuleNames { get { return ruleNames; } }

	public override string[] ChannelNames { get { return channelNames; } }

	public override string[] ModeNames { get { return modeNames; } }

	public override int[] SerializedAtn { get { return _serializedATN; } }

	static StyleLexer() {
		decisionToDFA = new DFA[_ATN.NumberOfDecisions];
		for (int i = 0; i < _ATN.NumberOfDecisions; i++) {
			decisionToDFA[i] = new DFA(_ATN.GetDecisionState(i), i);
		}
	}
	private static int[] _serializedATN = {
		4,0,14,91,6,-1,6,-1,6,-1,2,0,7,0,2,1,7,1,2,2,7,2,2,3,7,3,2,4,7,4,2,5,7,
		5,2,6,7,6,2,7,7,7,2,8,7,8,2,9,7,9,2,10,7,10,2,11,7,11,2,12,7,12,2,13,7,
		13,1,0,1,0,1,0,1,0,1,1,1,1,1,2,1,2,1,3,1,3,1,4,1,4,1,5,4,5,45,8,5,11,5,
		12,5,46,1,6,4,6,50,8,6,11,6,12,6,51,1,6,1,6,1,7,4,7,57,8,7,11,7,12,7,58,
		1,8,1,8,1,8,1,8,1,9,1,9,1,9,1,9,1,10,4,10,70,8,10,11,10,12,10,71,1,10,
		1,10,1,11,1,11,1,11,1,11,1,12,4,12,81,8,12,11,12,12,12,82,1,13,4,13,86,
		8,13,11,13,12,13,87,1,13,1,13,0,0,14,3,1,5,2,7,3,9,4,11,5,13,6,15,7,17,
		8,19,9,21,10,23,11,25,12,27,13,29,14,3,0,1,2,4,4,0,45,45,65,90,95,95,97,
		122,3,0,9,10,13,13,32,32,3,0,9,10,13,13,59,59,2,0,9,10,13,13,94,0,3,1,
		0,0,0,0,5,1,0,0,0,0,7,1,0,0,0,0,9,1,0,0,0,0,11,1,0,0,0,0,13,1,0,0,0,0,
		15,1,0,0,0,1,17,1,0,0,0,1,19,1,0,0,0,1,21,1,0,0,0,1,23,1,0,0,0,2,25,1,
		0,0,0,2,27,1,0,0,0,2,29,1,0,0,0,3,31,1,0,0,0,5,35,1,0,0,0,7,37,1,0,0,0,
		9,39,1,0,0,0,11,41,1,0,0,0,13,44,1,0,0,0,15,49,1,0,0,0,17,56,1,0,0,0,19,
		60,1,0,0,0,21,64,1,0,0,0,23,69,1,0,0,0,25,75,1,0,0,0,27,80,1,0,0,0,29,
		85,1,0,0,0,31,32,5,123,0,0,32,33,1,0,0,0,33,34,6,0,0,0,34,4,1,0,0,0,35,
		36,5,46,0,0,36,6,1,0,0,0,37,38,5,35,0,0,38,8,1,0,0,0,39,40,5,42,0,0,40,
		10,1,0,0,0,41,42,5,44,0,0,42,12,1,0,0,0,43,45,7,0,0,0,44,43,1,0,0,0,45,
		46,1,0,0,0,46,44,1,0,0,0,46,47,1,0,0,0,47,14,1,0,0,0,48,50,7,1,0,0,49,
		48,1,0,0,0,50,51,1,0,0,0,51,49,1,0,0,0,51,52,1,0,0,0,52,53,1,0,0,0,53,
		54,6,6,1,0,54,16,1,0,0,0,55,57,7,0,0,0,56,55,1,0,0,0,57,58,1,0,0,0,58,
		56,1,0,0,0,58,59,1,0,0,0,59,18,1,0,0,0,60,61,5,58,0,0,61,62,1,0,0,0,62,
		63,6,8,2,0,63,20,1,0,0,0,64,65,5,125,0,0,65,66,1,0,0,0,66,67,6,9,3,0,67,
		22,1,0,0,0,68,70,7,1,0,0,69,68,1,0,0,0,70,71,1,0,0,0,71,69,1,0,0,0,71,
		72,1,0,0,0,72,73,1,0,0,0,73,74,6,10,1,0,74,24,1,0,0,0,75,76,5,59,0,0,76,
		77,1,0,0,0,77,78,6,11,3,0,78,26,1,0,0,0,79,81,8,2,0,0,80,79,1,0,0,0,81,
		82,1,0,0,0,82,80,1,0,0,0,82,83,1,0,0,0,83,28,1,0,0,0,84,86,7,3,0,0,85,
		84,1,0,0,0,86,87,1,0,0,0,87,85,1,0,0,0,87,88,1,0,0,0,88,89,1,0,0,0,89,
		90,6,13,1,0,90,30,1,0,0,0,10,0,1,2,46,51,56,58,71,82,87,4,5,1,0,6,0,0,
		5,2,0,4,0,0
	};

	public static readonly ATN _ATN =
		new ATNDeserializer().Deserialize(_serializedATN);


}
