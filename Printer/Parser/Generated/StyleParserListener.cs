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

using Antlr4.Runtime.Misc;
using IParseTreeListener = Antlr4.Runtime.Tree.IParseTreeListener;
using IToken = Antlr4.Runtime.IToken;

/// <summary>
/// This interface defines a complete listener for a parse tree produced by
/// <see cref="StyleParser"/>.
/// </summary>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.13.1")]
[System.CLSCompliant(false)]
public interface IStyleParserListener : IParseTreeListener {
	/// <summary>
	/// Enter a parse tree produced by <see cref="StyleParser.styles"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterStyles([NotNull] StyleParser.StylesContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="StyleParser.styles"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitStyles([NotNull] StyleParser.StylesContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="StyleParser.style"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterStyle([NotNull] StyleParser.StyleContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="StyleParser.style"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitStyle([NotNull] StyleParser.StyleContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="StyleParser.selectors"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterSelectors([NotNull] StyleParser.SelectorsContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="StyleParser.selectors"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitSelectors([NotNull] StyleParser.SelectorsContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="StyleParser.selector"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterSelector([NotNull] StyleParser.SelectorContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="StyleParser.selector"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitSelector([NotNull] StyleParser.SelectorContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="StyleParser.property"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterProperty([NotNull] StyleParser.PropertyContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="StyleParser.property"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitProperty([NotNull] StyleParser.PropertyContext context);
}
