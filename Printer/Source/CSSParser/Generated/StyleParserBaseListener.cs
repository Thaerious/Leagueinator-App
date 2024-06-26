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
using IErrorNode = Antlr4.Runtime.Tree.IErrorNode;
using ITerminalNode = Antlr4.Runtime.Tree.ITerminalNode;
using IToken = Antlr4.Runtime.IToken;
using ParserRuleContext = Antlr4.Runtime.ParserRuleContext;

/// <summary>
/// This class provides an empty implementation of <see cref="IStyleParserListener"/>,
/// which can be extended to create a listener which only needs to handle a subset
/// of the available methods.
/// </summary>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.13.1")]
[System.Diagnostics.DebuggerNonUserCode]
[System.CLSCompliant(false)]
public partial class StyleParserBaseListener : IStyleParserListener {
	/// <summary>
	/// Enter a parse tree produced by <see cref="StyleParser.styles"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterStyles([NotNull] StyleParser.StylesContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="StyleParser.styles"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitStyles([NotNull] StyleParser.StylesContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="StyleParser.import_dir"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterImport_dir([NotNull] StyleParser.Import_dirContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="StyleParser.import_dir"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitImport_dir([NotNull] StyleParser.Import_dirContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="StyleParser.style"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterStyle([NotNull] StyleParser.StyleContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="StyleParser.style"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitStyle([NotNull] StyleParser.StyleContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="StyleParser.selectors"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterSelectors([NotNull] StyleParser.SelectorsContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="StyleParser.selectors"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitSelectors([NotNull] StyleParser.SelectorsContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="StyleParser.selector"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterSelector([NotNull] StyleParser.SelectorContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="StyleParser.selector"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitSelector([NotNull] StyleParser.SelectorContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="StyleParser.line"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterLine([NotNull] StyleParser.LineContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="StyleParser.line"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitLine([NotNull] StyleParser.LineContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="StyleParser.comment"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterComment([NotNull] StyleParser.CommentContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="StyleParser.comment"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitComment([NotNull] StyleParser.CommentContext context) { }
	/// <summary>
	/// Enter a parse tree produced by <see cref="StyleParser.property"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterProperty([NotNull] StyleParser.PropertyContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="StyleParser.property"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitProperty([NotNull] StyleParser.PropertyContext context) { }

	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void EnterEveryRule([NotNull] ParserRuleContext context) { }
	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void ExitEveryRule([NotNull] ParserRuleContext context) { }
	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void VisitTerminal([NotNull] ITerminalNode node) { }
	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void VisitErrorNode([NotNull] IErrorNode node) { }
}
