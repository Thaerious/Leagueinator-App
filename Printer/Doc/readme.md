# Usage

## DOM
    <docroot>
        <head>
            <stylesheet asset="" file="">
            <style></style>
        </head>

        <template></template>

        ... non-rendered content ...

        <body>
            ... rendered content ...
        <body>
    </docroot>

## Reserved Tags
### Docroot
The document root, any elements can go here, but are not rendered.

### Stylesheet
The <stylesheet> tag defines the relationship between the current document and an external stylesheet.
If the stylesheet is an asset put the name in the asset field.
If the stylesheet is a file put the name in the file field.

### Style
The text contents are read in as a style sheet.

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

Flex_Axis { Default, Row, Column}
Justify_Content { Default, Flex_start, Flex_end, Center, Space_between, Space_around, Space_evenly }
Align_Items { Default, Flex_start, Flex_end, Center }
Flex_Direction { Forward, Reverse }

The relative location Left, Right, Top, Bottom only apply in absolute positioning.

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

## Paging

The draw method get's called with a page number.  
When the style sheet property "overflow" is set to "paged" all child elements that do no fit within
the content box of the parent get printed on the next page.  Styling acts as if only the elements on 
the given page are in the parent.

The root element is always drawn, because overflow effects child elements.  Each page will be assigned
at least one element even if it overflows.

## Query Selectors

| Selector  | Example  | Specificity | Description    |
|-----------|----------|-------------|----------------|
| #id       | #first   | 1           | identity       |
| *         | *        | 5           | all            |
| .         | .bold    | 3           | class          |
| tag       | div      | 4           | element name   |
| tag.class | div.bold | 2           | name and class | 