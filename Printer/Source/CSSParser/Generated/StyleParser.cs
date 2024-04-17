//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.13.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from ./StyleParser.g4 by ANTLR 4.13.1

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
using System.Diagnostics;
using System.Collections.Generic;
using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using DFA = Antlr4.Runtime.Dfa.DFA;

[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.13.1")]
[System.CLSCompliant(false)]
public partial class StyleParser : Parser {
	protected static DFA[] decisionToDFA;
	protected static PredictionContextCache sharedContextCache = new PredictionContextCache();
	public const int
		OPAR=1, MULT_SELECTOR=2, CLASS_SELECTOR=3, ID_SELECTOR=4, TAG_SELECTOR=5, 
		ALL_SELECTOR=6, COMMA=7, GT=8, I_DIR=9, QUOTED_STRING=10, COMMENT=11, 
		WS=12, NL=13, K_COMMENT=14, KEY=15, COLON=16, CPAR=17, MM_WS=18, SEMI=19, 
		VALUE=20, VM_WS=21, NEWLINE=22, COMMENT_VALUE=23;
	public const int
		RULE_styles = 0, RULE_import_dir = 1, RULE_style = 2, RULE_selectors = 3, 
		RULE_selector = 4, RULE_line = 5, RULE_comment = 6, RULE_property = 7;
	public static readonly string[] ruleNames = {
		"styles", "import_dir", "style", "selectors", "selector", "line", "comment", 
		"property"
	};

	private static readonly string[] _LiteralNames = {
		null, "'{'", null, null, null, null, "'*'", "','", "'>'", "'@import'", 
		null, null, null, null, null, null, "':'", "'}'", null, "';'"
	};
	private static readonly string[] _SymbolicNames = {
		null, "OPAR", "MULT_SELECTOR", "CLASS_SELECTOR", "ID_SELECTOR", "TAG_SELECTOR", 
		"ALL_SELECTOR", "COMMA", "GT", "I_DIR", "QUOTED_STRING", "COMMENT", "WS", 
		"NL", "K_COMMENT", "KEY", "COLON", "CPAR", "MM_WS", "SEMI", "VALUE", "VM_WS", 
		"NEWLINE", "COMMENT_VALUE"
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

	public override string GrammarFileName { get { return "StyleParser.g4"; } }

	public override string[] RuleNames { get { return ruleNames; } }

	public override int[] SerializedAtn { get { return _serializedATN; } }

	static StyleParser() {
		decisionToDFA = new DFA[_ATN.NumberOfDecisions];
		for (int i = 0; i < _ATN.NumberOfDecisions; i++) {
			decisionToDFA[i] = new DFA(_ATN.GetDecisionState(i), i);
		}
	}

		public StyleParser(ITokenStream input) : this(input, Console.Out, Console.Error) { }

		public StyleParser(ITokenStream input, TextWriter output, TextWriter errorOutput)
		: base(input, output, errorOutput)
	{
		Interpreter = new ParserATNSimulator(this, _ATN, decisionToDFA, sharedContextCache);
	}

	public partial class StylesContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode Eof() { return GetToken(StyleParser.Eof, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public Import_dirContext[] import_dir() {
			return GetRuleContexts<Import_dirContext>();
		}
		[System.Diagnostics.DebuggerNonUserCode] public Import_dirContext import_dir(int i) {
			return GetRuleContext<Import_dirContext>(i);
		}
		[System.Diagnostics.DebuggerNonUserCode] public StyleContext[] style() {
			return GetRuleContexts<StyleContext>();
		}
		[System.Diagnostics.DebuggerNonUserCode] public StyleContext style(int i) {
			return GetRuleContext<StyleContext>(i);
		}
		public StylesContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_styles; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override void EnterRule(IParseTreeListener listener) {
			IStyleParserListener typedListener = listener as IStyleParserListener;
			if (typedListener != null) typedListener.EnterStyles(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override void ExitRule(IParseTreeListener listener) {
			IStyleParserListener typedListener = listener as IStyleParserListener;
			if (typedListener != null) typedListener.ExitStyles(this);
		}
	}

	[RuleVersion(0)]
	public StylesContext styles() {
		StylesContext _localctx = new StylesContext(Context, State);
		EnterRule(_localctx, 0, RULE_styles);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 19;
			ErrorHandler.Sync(this);
			_la = TokenStream.LA(1);
			while (_la==I_DIR) {
				{
				{
				State = 16;
				import_dir();
				}
				}
				State = 21;
				ErrorHandler.Sync(this);
				_la = TokenStream.LA(1);
			}
			State = 25;
			ErrorHandler.Sync(this);
			_la = TokenStream.LA(1);
			while ((((_la) & ~0x3f) == 0 && ((1L << _la) & 18556L) != 0)) {
				{
				{
				State = 22;
				style();
				}
				}
				State = 27;
				ErrorHandler.Sync(this);
				_la = TokenStream.LA(1);
			}
			State = 28;
			Match(Eof);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class Import_dirContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode I_DIR() { return GetToken(StyleParser.I_DIR, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode QUOTED_STRING() { return GetToken(StyleParser.QUOTED_STRING, 0); }
		public Import_dirContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_import_dir; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override void EnterRule(IParseTreeListener listener) {
			IStyleParserListener typedListener = listener as IStyleParserListener;
			if (typedListener != null) typedListener.EnterImport_dir(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override void ExitRule(IParseTreeListener listener) {
			IStyleParserListener typedListener = listener as IStyleParserListener;
			if (typedListener != null) typedListener.ExitImport_dir(this);
		}
	}

	[RuleVersion(0)]
	public Import_dirContext import_dir() {
		Import_dirContext _localctx = new Import_dirContext(Context, State);
		EnterRule(_localctx, 2, RULE_import_dir);
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 30;
			Match(I_DIR);
			State = 31;
			Match(QUOTED_STRING);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class StyleContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public SelectorsContext selectors() {
			return GetRuleContext<SelectorsContext>(0);
		}
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode OPAR() { return GetToken(StyleParser.OPAR, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode CPAR() { return GetToken(StyleParser.CPAR, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public LineContext[] line() {
			return GetRuleContexts<LineContext>();
		}
		[System.Diagnostics.DebuggerNonUserCode] public LineContext line(int i) {
			return GetRuleContext<LineContext>(i);
		}
		[System.Diagnostics.DebuggerNonUserCode] public CommentContext comment() {
			return GetRuleContext<CommentContext>(0);
		}
		public StyleContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_style; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override void EnterRule(IParseTreeListener listener) {
			IStyleParserListener typedListener = listener as IStyleParserListener;
			if (typedListener != null) typedListener.EnterStyle(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override void ExitRule(IParseTreeListener listener) {
			IStyleParserListener typedListener = listener as IStyleParserListener;
			if (typedListener != null) typedListener.ExitStyle(this);
		}
	}

	[RuleVersion(0)]
	public StyleContext style() {
		StyleContext _localctx = new StyleContext(Context, State);
		EnterRule(_localctx, 4, RULE_style);
		int _la;
		try {
			State = 44;
			ErrorHandler.Sync(this);
			switch (TokenStream.LA(1)) {
			case MULT_SELECTOR:
			case CLASS_SELECTOR:
			case ID_SELECTOR:
			case TAG_SELECTOR:
			case ALL_SELECTOR:
				EnterOuterAlt(_localctx, 1);
				{
				State = 33;
				selectors(0);
				State = 34;
				Match(OPAR);
				State = 38;
				ErrorHandler.Sync(this);
				_la = TokenStream.LA(1);
				while ((((_la) & ~0x3f) == 0 && ((1L << _la) & 51200L) != 0)) {
					{
					{
					State = 35;
					line();
					}
					}
					State = 40;
					ErrorHandler.Sync(this);
					_la = TokenStream.LA(1);
				}
				State = 41;
				Match(CPAR);
				}
				break;
			case COMMENT:
			case K_COMMENT:
				EnterOuterAlt(_localctx, 2);
				{
				State = 43;
				comment();
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class SelectorsContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public SelectorContext selector() {
			return GetRuleContext<SelectorContext>(0);
		}
		[System.Diagnostics.DebuggerNonUserCode] public SelectorsContext selectors() {
			return GetRuleContext<SelectorsContext>(0);
		}
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode COMMA() { return GetToken(StyleParser.COMMA, 0); }
		public SelectorsContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_selectors; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override void EnterRule(IParseTreeListener listener) {
			IStyleParserListener typedListener = listener as IStyleParserListener;
			if (typedListener != null) typedListener.EnterSelectors(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override void ExitRule(IParseTreeListener listener) {
			IStyleParserListener typedListener = listener as IStyleParserListener;
			if (typedListener != null) typedListener.ExitSelectors(this);
		}
	}

	[RuleVersion(0)]
	public SelectorsContext selectors() {
		return selectors(0);
	}

	private SelectorsContext selectors(int _p) {
		ParserRuleContext _parentctx = Context;
		int _parentState = State;
		SelectorsContext _localctx = new SelectorsContext(Context, _parentState);
		SelectorsContext _prevctx = _localctx;
		int _startState = 6;
		EnterRecursionRule(_localctx, 6, RULE_selectors, _p);
		try {
			int _alt;
			EnterOuterAlt(_localctx, 1);
			{
			{
			State = 47;
			selector(0);
			}
			Context.Stop = TokenStream.LT(-1);
			State = 54;
			ErrorHandler.Sync(this);
			_alt = Interpreter.AdaptivePredict(TokenStream,4,Context);
			while ( _alt!=2 && _alt!=global::Antlr4.Runtime.Atn.ATN.INVALID_ALT_NUMBER ) {
				if ( _alt==1 ) {
					if ( ParseListeners!=null )
						TriggerExitRuleEvent();
					_prevctx = _localctx;
					{
					{
					_localctx = new SelectorsContext(_parentctx, _parentState);
					PushNewRecursionContext(_localctx, _startState, RULE_selectors);
					State = 49;
					if (!(Precpred(Context, 1))) throw new FailedPredicateException(this, "Precpred(Context, 1)");
					State = 50;
					Match(COMMA);
					State = 51;
					selector(0);
					}
					} 
				}
				State = 56;
				ErrorHandler.Sync(this);
				_alt = Interpreter.AdaptivePredict(TokenStream,4,Context);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			UnrollRecursionContexts(_parentctx);
		}
		return _localctx;
	}

	public partial class SelectorContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode MULT_SELECTOR() { return GetToken(StyleParser.MULT_SELECTOR, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode CLASS_SELECTOR() { return GetToken(StyleParser.CLASS_SELECTOR, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode ID_SELECTOR() { return GetToken(StyleParser.ID_SELECTOR, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode TAG_SELECTOR() { return GetToken(StyleParser.TAG_SELECTOR, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode ALL_SELECTOR() { return GetToken(StyleParser.ALL_SELECTOR, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public SelectorContext[] selector() {
			return GetRuleContexts<SelectorContext>();
		}
		[System.Diagnostics.DebuggerNonUserCode] public SelectorContext selector(int i) {
			return GetRuleContext<SelectorContext>(i);
		}
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode GT() { return GetToken(StyleParser.GT, 0); }
		public SelectorContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_selector; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override void EnterRule(IParseTreeListener listener) {
			IStyleParserListener typedListener = listener as IStyleParserListener;
			if (typedListener != null) typedListener.EnterSelector(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override void ExitRule(IParseTreeListener listener) {
			IStyleParserListener typedListener = listener as IStyleParserListener;
			if (typedListener != null) typedListener.ExitSelector(this);
		}
	}

	[RuleVersion(0)]
	public SelectorContext selector() {
		return selector(0);
	}

	private SelectorContext selector(int _p) {
		ParserRuleContext _parentctx = Context;
		int _parentState = State;
		SelectorContext _localctx = new SelectorContext(Context, _parentState);
		SelectorContext _prevctx = _localctx;
		int _startState = 8;
		EnterRecursionRule(_localctx, 8, RULE_selector, _p);
		try {
			int _alt;
			EnterOuterAlt(_localctx, 1);
			{
			State = 63;
			ErrorHandler.Sync(this);
			switch (TokenStream.LA(1)) {
			case MULT_SELECTOR:
				{
				State = 58;
				Match(MULT_SELECTOR);
				}
				break;
			case CLASS_SELECTOR:
				{
				State = 59;
				Match(CLASS_SELECTOR);
				}
				break;
			case ID_SELECTOR:
				{
				State = 60;
				Match(ID_SELECTOR);
				}
				break;
			case TAG_SELECTOR:
				{
				State = 61;
				Match(TAG_SELECTOR);
				}
				break;
			case ALL_SELECTOR:
				{
				State = 62;
				Match(ALL_SELECTOR);
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
			Context.Stop = TokenStream.LT(-1);
			State = 72;
			ErrorHandler.Sync(this);
			_alt = Interpreter.AdaptivePredict(TokenStream,7,Context);
			while ( _alt!=2 && _alt!=global::Antlr4.Runtime.Atn.ATN.INVALID_ALT_NUMBER ) {
				if ( _alt==1 ) {
					if ( ParseListeners!=null )
						TriggerExitRuleEvent();
					_prevctx = _localctx;
					{
					State = 70;
					ErrorHandler.Sync(this);
					switch ( Interpreter.AdaptivePredict(TokenStream,6,Context) ) {
					case 1:
						{
						_localctx = new SelectorContext(_parentctx, _parentState);
						PushNewRecursionContext(_localctx, _startState, RULE_selector);
						State = 65;
						if (!(Precpred(Context, 2))) throw new FailedPredicateException(this, "Precpred(Context, 2)");
						State = 66;
						Match(GT);
						State = 67;
						selector(3);
						}
						break;
					case 2:
						{
						_localctx = new SelectorContext(_parentctx, _parentState);
						PushNewRecursionContext(_localctx, _startState, RULE_selector);
						State = 68;
						if (!(Precpred(Context, 1))) throw new FailedPredicateException(this, "Precpred(Context, 1)");
						State = 69;
						selector(2);
						}
						break;
					}
					} 
				}
				State = 74;
				ErrorHandler.Sync(this);
				_alt = Interpreter.AdaptivePredict(TokenStream,7,Context);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			UnrollRecursionContexts(_parentctx);
		}
		return _localctx;
	}

	public partial class LineContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public PropertyContext property() {
			return GetRuleContext<PropertyContext>(0);
		}
		[System.Diagnostics.DebuggerNonUserCode] public CommentContext comment() {
			return GetRuleContext<CommentContext>(0);
		}
		public LineContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_line; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override void EnterRule(IParseTreeListener listener) {
			IStyleParserListener typedListener = listener as IStyleParserListener;
			if (typedListener != null) typedListener.EnterLine(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override void ExitRule(IParseTreeListener listener) {
			IStyleParserListener typedListener = listener as IStyleParserListener;
			if (typedListener != null) typedListener.ExitLine(this);
		}
	}

	[RuleVersion(0)]
	public LineContext line() {
		LineContext _localctx = new LineContext(Context, State);
		EnterRule(_localctx, 10, RULE_line);
		try {
			State = 77;
			ErrorHandler.Sync(this);
			switch (TokenStream.LA(1)) {
			case KEY:
				EnterOuterAlt(_localctx, 1);
				{
				State = 75;
				property();
				}
				break;
			case COMMENT:
			case K_COMMENT:
				EnterOuterAlt(_localctx, 2);
				{
				State = 76;
				comment();
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class CommentContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode COMMENT() { return GetToken(StyleParser.COMMENT, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode COMMENT_VALUE() { return GetToken(StyleParser.COMMENT_VALUE, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode NEWLINE() { return GetToken(StyleParser.NEWLINE, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode K_COMMENT() { return GetToken(StyleParser.K_COMMENT, 0); }
		public CommentContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_comment; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override void EnterRule(IParseTreeListener listener) {
			IStyleParserListener typedListener = listener as IStyleParserListener;
			if (typedListener != null) typedListener.EnterComment(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override void ExitRule(IParseTreeListener listener) {
			IStyleParserListener typedListener = listener as IStyleParserListener;
			if (typedListener != null) typedListener.ExitComment(this);
		}
	}

	[RuleVersion(0)]
	public CommentContext comment() {
		CommentContext _localctx = new CommentContext(Context, State);
		EnterRule(_localctx, 12, RULE_comment);
		try {
			State = 85;
			ErrorHandler.Sync(this);
			switch (TokenStream.LA(1)) {
			case COMMENT:
				EnterOuterAlt(_localctx, 1);
				{
				State = 79;
				Match(COMMENT);
				State = 80;
				Match(COMMENT_VALUE);
				State = 81;
				Match(NEWLINE);
				}
				break;
			case K_COMMENT:
				EnterOuterAlt(_localctx, 2);
				{
				State = 82;
				Match(K_COMMENT);
				State = 83;
				Match(COMMENT_VALUE);
				State = 84;
				Match(NEWLINE);
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public partial class PropertyContext : ParserRuleContext {
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode KEY() { return GetToken(StyleParser.KEY, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode COLON() { return GetToken(StyleParser.COLON, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode VALUE() { return GetToken(StyleParser.VALUE, 0); }
		[System.Diagnostics.DebuggerNonUserCode] public ITerminalNode SEMI() { return GetToken(StyleParser.SEMI, 0); }
		public PropertyContext(ParserRuleContext parent, int invokingState)
			: base(parent, invokingState)
		{
		}
		public override int RuleIndex { get { return RULE_property; } }
		[System.Diagnostics.DebuggerNonUserCode]
		public override void EnterRule(IParseTreeListener listener) {
			IStyleParserListener typedListener = listener as IStyleParserListener;
			if (typedListener != null) typedListener.EnterProperty(this);
		}
		[System.Diagnostics.DebuggerNonUserCode]
		public override void ExitRule(IParseTreeListener listener) {
			IStyleParserListener typedListener = listener as IStyleParserListener;
			if (typedListener != null) typedListener.ExitProperty(this);
		}
	}

	[RuleVersion(0)]
	public PropertyContext property() {
		PropertyContext _localctx = new PropertyContext(Context, State);
		EnterRule(_localctx, 14, RULE_property);
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 87;
			Match(KEY);
			State = 88;
			Match(COLON);
			State = 89;
			Match(VALUE);
			State = 90;
			Match(SEMI);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			ErrorHandler.ReportError(this, re);
			ErrorHandler.Recover(this, re);
		}
		finally {
			ExitRule();
		}
		return _localctx;
	}

	public override bool Sempred(RuleContext _localctx, int ruleIndex, int predIndex) {
		switch (ruleIndex) {
		case 3: return selectors_sempred((SelectorsContext)_localctx, predIndex);
		case 4: return selector_sempred((SelectorContext)_localctx, predIndex);
		}
		return true;
	}
	private bool selectors_sempred(SelectorsContext _localctx, int predIndex) {
		switch (predIndex) {
		case 0: return Precpred(Context, 1);
		}
		return true;
	}
	private bool selector_sempred(SelectorContext _localctx, int predIndex) {
		switch (predIndex) {
		case 1: return Precpred(Context, 2);
		case 2: return Precpred(Context, 1);
		}
		return true;
	}

	private static int[] _serializedATN = {
		4,1,23,93,2,0,7,0,2,1,7,1,2,2,7,2,2,3,7,3,2,4,7,4,2,5,7,5,2,6,7,6,2,7,
		7,7,1,0,5,0,18,8,0,10,0,12,0,21,9,0,1,0,5,0,24,8,0,10,0,12,0,27,9,0,1,
		0,1,0,1,1,1,1,1,1,1,2,1,2,1,2,5,2,37,8,2,10,2,12,2,40,9,2,1,2,1,2,1,2,
		3,2,45,8,2,1,3,1,3,1,3,1,3,1,3,1,3,5,3,53,8,3,10,3,12,3,56,9,3,1,4,1,4,
		1,4,1,4,1,4,1,4,3,4,64,8,4,1,4,1,4,1,4,1,4,1,4,5,4,71,8,4,10,4,12,4,74,
		9,4,1,5,1,5,3,5,78,8,5,1,6,1,6,1,6,1,6,1,6,1,6,3,6,86,8,6,1,7,1,7,1,7,
		1,7,1,7,1,7,0,2,6,8,8,0,2,4,6,8,10,12,14,0,0,97,0,19,1,0,0,0,2,30,1,0,
		0,0,4,44,1,0,0,0,6,46,1,0,0,0,8,63,1,0,0,0,10,77,1,0,0,0,12,85,1,0,0,0,
		14,87,1,0,0,0,16,18,3,2,1,0,17,16,1,0,0,0,18,21,1,0,0,0,19,17,1,0,0,0,
		19,20,1,0,0,0,20,25,1,0,0,0,21,19,1,0,0,0,22,24,3,4,2,0,23,22,1,0,0,0,
		24,27,1,0,0,0,25,23,1,0,0,0,25,26,1,0,0,0,26,28,1,0,0,0,27,25,1,0,0,0,
		28,29,5,0,0,1,29,1,1,0,0,0,30,31,5,9,0,0,31,32,5,10,0,0,32,3,1,0,0,0,33,
		34,3,6,3,0,34,38,5,1,0,0,35,37,3,10,5,0,36,35,1,0,0,0,37,40,1,0,0,0,38,
		36,1,0,0,0,38,39,1,0,0,0,39,41,1,0,0,0,40,38,1,0,0,0,41,42,5,17,0,0,42,
		45,1,0,0,0,43,45,3,12,6,0,44,33,1,0,0,0,44,43,1,0,0,0,45,5,1,0,0,0,46,
		47,6,3,-1,0,47,48,3,8,4,0,48,54,1,0,0,0,49,50,10,1,0,0,50,51,5,7,0,0,51,
		53,3,8,4,0,52,49,1,0,0,0,53,56,1,0,0,0,54,52,1,0,0,0,54,55,1,0,0,0,55,
		7,1,0,0,0,56,54,1,0,0,0,57,58,6,4,-1,0,58,64,5,2,0,0,59,64,5,3,0,0,60,
		64,5,4,0,0,61,64,5,5,0,0,62,64,5,6,0,0,63,57,1,0,0,0,63,59,1,0,0,0,63,
		60,1,0,0,0,63,61,1,0,0,0,63,62,1,0,0,0,64,72,1,0,0,0,65,66,10,2,0,0,66,
		67,5,8,0,0,67,71,3,8,4,3,68,69,10,1,0,0,69,71,3,8,4,2,70,65,1,0,0,0,70,
		68,1,0,0,0,71,74,1,0,0,0,72,70,1,0,0,0,72,73,1,0,0,0,73,9,1,0,0,0,74,72,
		1,0,0,0,75,78,3,14,7,0,76,78,3,12,6,0,77,75,1,0,0,0,77,76,1,0,0,0,78,11,
		1,0,0,0,79,80,5,11,0,0,80,81,5,23,0,0,81,86,5,22,0,0,82,83,5,14,0,0,83,
		84,5,23,0,0,84,86,5,22,0,0,85,79,1,0,0,0,85,82,1,0,0,0,86,13,1,0,0,0,87,
		88,5,15,0,0,88,89,5,16,0,0,89,90,5,20,0,0,90,91,5,19,0,0,91,15,1,0,0,0,
		10,19,25,38,44,54,63,70,72,77,85
	};

	public static readonly ATN _ATN =
		new ATNDeserializer().Deserialize(_serializedATN);


}