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
		ALL_SELECTOR=6, COMMA=7, GT=8, WS=9, NL=10, COMMENT=11, KEY=12, COLON=13, 
		CPAR=14, MM_WS=15, SEMI=16, VALUE=17, VM_WS=18, NEWLINE=19, COMMENT_VALUE=20;
	public const int
		RULE_styles = 0, RULE_style = 1, RULE_selectors = 2, RULE_selector = 3, 
		RULE_line = 4, RULE_comment = 5, RULE_property = 6;
	public static readonly string[] ruleNames = {
		"styles", "style", "selectors", "selector", "line", "comment", "property"
	};

	private static readonly string[] _LiteralNames = {
		null, "'{'", null, null, null, null, "'*'", "','", "'>'", null, null, 
		null, null, "':'", "'}'", null, "';'"
	};
	private static readonly string[] _SymbolicNames = {
		null, "OPAR", "MULT_SELECTOR", "CLASS_SELECTOR", "ID_SELECTOR", "TAG_SELECTOR", 
		"ALL_SELECTOR", "COMMA", "GT", "WS", "NL", "COMMENT", "KEY", "COLON", 
		"CPAR", "MM_WS", "SEMI", "VALUE", "VM_WS", "NEWLINE", "COMMENT_VALUE"
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
			State = 17;
			ErrorHandler.Sync(this);
			_la = TokenStream.LA(1);
			while ((((_la) & ~0x3f) == 0 && ((1L << _la) & 124L) != 0)) {
				{
				{
				State = 14;
				style();
				}
				}
				State = 19;
				ErrorHandler.Sync(this);
				_la = TokenStream.LA(1);
			}
			State = 20;
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
		EnterRule(_localctx, 2, RULE_style);
		int _la;
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 22;
			selectors(0);
			State = 23;
			Match(OPAR);
			State = 27;
			ErrorHandler.Sync(this);
			_la = TokenStream.LA(1);
			while (_la==COMMENT || _la==KEY) {
				{
				{
				State = 24;
				line();
				}
				}
				State = 29;
				ErrorHandler.Sync(this);
				_la = TokenStream.LA(1);
			}
			State = 30;
			Match(CPAR);
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
		int _startState = 4;
		EnterRecursionRule(_localctx, 4, RULE_selectors, _p);
		try {
			int _alt;
			EnterOuterAlt(_localctx, 1);
			{
			{
			State = 33;
			selector(0);
			}
			Context.Stop = TokenStream.LT(-1);
			State = 40;
			ErrorHandler.Sync(this);
			_alt = Interpreter.AdaptivePredict(TokenStream,2,Context);
			while ( _alt!=2 && _alt!=global::Antlr4.Runtime.Atn.ATN.INVALID_ALT_NUMBER ) {
				if ( _alt==1 ) {
					if ( ParseListeners!=null )
						TriggerExitRuleEvent();
					_prevctx = _localctx;
					{
					{
					_localctx = new SelectorsContext(_parentctx, _parentState);
					PushNewRecursionContext(_localctx, _startState, RULE_selectors);
					State = 35;
					if (!(Precpred(Context, 1))) throw new FailedPredicateException(this, "Precpred(Context, 1)");
					State = 36;
					Match(COMMA);
					State = 37;
					selector(0);
					}
					} 
				}
				State = 42;
				ErrorHandler.Sync(this);
				_alt = Interpreter.AdaptivePredict(TokenStream,2,Context);
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
		int _startState = 6;
		EnterRecursionRule(_localctx, 6, RULE_selector, _p);
		try {
			int _alt;
			EnterOuterAlt(_localctx, 1);
			{
			State = 49;
			ErrorHandler.Sync(this);
			switch (TokenStream.LA(1)) {
			case MULT_SELECTOR:
				{
				State = 44;
				Match(MULT_SELECTOR);
				}
				break;
			case CLASS_SELECTOR:
				{
				State = 45;
				Match(CLASS_SELECTOR);
				}
				break;
			case ID_SELECTOR:
				{
				State = 46;
				Match(ID_SELECTOR);
				}
				break;
			case TAG_SELECTOR:
				{
				State = 47;
				Match(TAG_SELECTOR);
				}
				break;
			case ALL_SELECTOR:
				{
				State = 48;
				Match(ALL_SELECTOR);
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
			Context.Stop = TokenStream.LT(-1);
			State = 58;
			ErrorHandler.Sync(this);
			_alt = Interpreter.AdaptivePredict(TokenStream,5,Context);
			while ( _alt!=2 && _alt!=global::Antlr4.Runtime.Atn.ATN.INVALID_ALT_NUMBER ) {
				if ( _alt==1 ) {
					if ( ParseListeners!=null )
						TriggerExitRuleEvent();
					_prevctx = _localctx;
					{
					State = 56;
					ErrorHandler.Sync(this);
					switch ( Interpreter.AdaptivePredict(TokenStream,4,Context) ) {
					case 1:
						{
						_localctx = new SelectorContext(_parentctx, _parentState);
						PushNewRecursionContext(_localctx, _startState, RULE_selector);
						State = 51;
						if (!(Precpred(Context, 2))) throw new FailedPredicateException(this, "Precpred(Context, 2)");
						State = 52;
						Match(GT);
						State = 53;
						selector(3);
						}
						break;
					case 2:
						{
						_localctx = new SelectorContext(_parentctx, _parentState);
						PushNewRecursionContext(_localctx, _startState, RULE_selector);
						State = 54;
						if (!(Precpred(Context, 1))) throw new FailedPredicateException(this, "Precpred(Context, 1)");
						State = 55;
						selector(2);
						}
						break;
					}
					} 
				}
				State = 60;
				ErrorHandler.Sync(this);
				_alt = Interpreter.AdaptivePredict(TokenStream,5,Context);
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
		EnterRule(_localctx, 8, RULE_line);
		try {
			State = 63;
			ErrorHandler.Sync(this);
			switch (TokenStream.LA(1)) {
			case KEY:
				EnterOuterAlt(_localctx, 1);
				{
				State = 61;
				property();
				}
				break;
			case COMMENT:
				EnterOuterAlt(_localctx, 2);
				{
				State = 62;
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
		EnterRule(_localctx, 10, RULE_comment);
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 65;
			Match(COMMENT);
			State = 66;
			Match(COMMENT_VALUE);
			State = 67;
			Match(NEWLINE);
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
		EnterRule(_localctx, 12, RULE_property);
		try {
			EnterOuterAlt(_localctx, 1);
			{
			State = 69;
			Match(KEY);
			State = 70;
			Match(COLON);
			State = 71;
			Match(VALUE);
			State = 72;
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
		case 2: return selectors_sempred((SelectorsContext)_localctx, predIndex);
		case 3: return selector_sempred((SelectorContext)_localctx, predIndex);
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
		4,1,20,75,2,0,7,0,2,1,7,1,2,2,7,2,2,3,7,3,2,4,7,4,2,5,7,5,2,6,7,6,1,0,
		5,0,16,8,0,10,0,12,0,19,9,0,1,0,1,0,1,1,1,1,1,1,5,1,26,8,1,10,1,12,1,29,
		9,1,1,1,1,1,1,2,1,2,1,2,1,2,1,2,1,2,5,2,39,8,2,10,2,12,2,42,9,2,1,3,1,
		3,1,3,1,3,1,3,1,3,3,3,50,8,3,1,3,1,3,1,3,1,3,1,3,5,3,57,8,3,10,3,12,3,
		60,9,3,1,4,1,4,3,4,64,8,4,1,5,1,5,1,5,1,5,1,6,1,6,1,6,1,6,1,6,1,6,0,2,
		4,6,7,0,2,4,6,8,10,12,0,0,77,0,17,1,0,0,0,2,22,1,0,0,0,4,32,1,0,0,0,6,
		49,1,0,0,0,8,63,1,0,0,0,10,65,1,0,0,0,12,69,1,0,0,0,14,16,3,2,1,0,15,14,
		1,0,0,0,16,19,1,0,0,0,17,15,1,0,0,0,17,18,1,0,0,0,18,20,1,0,0,0,19,17,
		1,0,0,0,20,21,5,0,0,1,21,1,1,0,0,0,22,23,3,4,2,0,23,27,5,1,0,0,24,26,3,
		8,4,0,25,24,1,0,0,0,26,29,1,0,0,0,27,25,1,0,0,0,27,28,1,0,0,0,28,30,1,
		0,0,0,29,27,1,0,0,0,30,31,5,14,0,0,31,3,1,0,0,0,32,33,6,2,-1,0,33,34,3,
		6,3,0,34,40,1,0,0,0,35,36,10,1,0,0,36,37,5,7,0,0,37,39,3,6,3,0,38,35,1,
		0,0,0,39,42,1,0,0,0,40,38,1,0,0,0,40,41,1,0,0,0,41,5,1,0,0,0,42,40,1,0,
		0,0,43,44,6,3,-1,0,44,50,5,2,0,0,45,50,5,3,0,0,46,50,5,4,0,0,47,50,5,5,
		0,0,48,50,5,6,0,0,49,43,1,0,0,0,49,45,1,0,0,0,49,46,1,0,0,0,49,47,1,0,
		0,0,49,48,1,0,0,0,50,58,1,0,0,0,51,52,10,2,0,0,52,53,5,8,0,0,53,57,3,6,
		3,3,54,55,10,1,0,0,55,57,3,6,3,2,56,51,1,0,0,0,56,54,1,0,0,0,57,60,1,0,
		0,0,58,56,1,0,0,0,58,59,1,0,0,0,59,7,1,0,0,0,60,58,1,0,0,0,61,64,3,12,
		6,0,62,64,3,10,5,0,63,61,1,0,0,0,63,62,1,0,0,0,64,9,1,0,0,0,65,66,5,11,
		0,0,66,67,5,20,0,0,67,68,5,19,0,0,68,11,1,0,0,0,69,70,5,12,0,0,70,71,5,
		13,0,0,71,72,5,17,0,0,72,73,5,16,0,0,73,13,1,0,0,0,7,17,27,40,49,56,58,
		63
	};

	public static readonly ATN _ATN =
		new ATNDeserializer().Deserialize(_serializedATN);


}
