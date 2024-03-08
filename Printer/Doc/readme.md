# Usage
## Quick Start

    var xmlLoader = new XMLLoader();
    xmlLoader.LoadStyle(styleString);
    PrinterElement root = xmlLoader.LoadXML(xmlString);

An 'Element' is a single node on the document tree. 
Call the Draw() method of the root element passing in the relevent graphics object.

    protected override void OnPaint(PaintEventArgs e) {
        base.OnPaint(e);
        this.RootElement?.Draw(e.Graphics);
    }

# Development
## Compiling ANTLR - Compiles grammar into c# source files.

java -jar D:\lib\antlr.jar -o generated -Dlanguage=CSharp .\StyleLexer.g4
java -jar D:\lib\antlr.jar -o generated -Dlanguage=CSharp .\StyleParser.g4

# Rectangles
┌────Container──┐
│    Margin     │
│┌───Border────┐│
││   Padding   ││
││┌──Content──┐││
│││           │││
││└───────────┘││
│└─────────────┘│
└───────────────┘

# Styles

Flex_Direction { Default, Row, Row_reverse, Column, Column_reverse }
Justify_Content { Default, Flex_start, Flex_end, Center, Space_between, Space_around, Space_evenly }
Align_Items { Default, Flex_start, Flex_end, Center }

# Implementation Details

All child components are drawn relative to their parent's content box.

|--> Perform layout
    |--> DoSize() -> Set content box width & height to the sum of child widths unless specified in style sheet.
        |    FlexMajor is Row -> content = {sumWidth x maxHeight}
        |    FlexMajor is Col -> content = {maxWidth x sumHeight}
        |    Width/Height is %, size set proportional to parent
        |    Width/Height is px, size set to absolute value
    |--> DoPos() -- set the position of each element
        |--> Setting the location position's the element relative to the parent


Draw() |--> Invoke the Style.Draw() abstract method.
       |--> Invoke any OnDraw delegates.
       |--> Invoke Draw() method on each child element.
