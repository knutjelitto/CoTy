using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;

namespace CoEd
{
    public class Source : TextSource
    {
        public Source(Typeface typeface, double size, Brush foreground)
        {
            RunProperties = new TheRunProperties(typeface, size, foreground);
            ParagraphProperties = new TheParagraphProperties(this);
        }

        public string Text { get; set; }

        public TheRunProperties RunProperties { get; }
        public TheParagraphProperties ParagraphProperties { get; }

        public override TextRun GetTextRun(int textSourceCharacterIndex)
        {
            if (Text == null || textSourceCharacterIndex < 0 ||  textSourceCharacterIndex >= Text.Length)
            {
                return new TextEndOfParagraph(1);
            }

            return new TextCharacters(Text, RunProperties);
        }

        public override TextSpan<CultureSpecificCharacterBufferRange> GetPrecedingText(int textSourceCharacterIndexLimit)
        {
            throw new NotImplementedException();
        }

        public override int GetTextEffectCharacterIndexFromTextSourceCharacterIndex(int textSourceCharacterIndex)
        {
            throw new NotImplementedException();
        }

        public class TheRunProperties : TextRunProperties
        {
            public TheRunProperties(Typeface typeface, double size, Brush foreground)
            {
                Typeface = typeface;
                FontHintingEmSize = size;
                FontRenderingEmSize = size;
                TextDecorations = null;
                ForegroundBrush = foreground;
                BackgroundBrush = null;
                CultureInfo = CultureInfo.InvariantCulture;
                TextEffects = null;

#if false
                var deco = new TextDecoration(
                    TextDecorationLocation.Underline,
                    //new Pen(Brushes.YellowGreen, 2),
                    ErrorPen(),
                    0.0,
                    TextDecorationUnit.Pixel,
                    TextDecorationUnit.Pixel
                    );

                TextDecorations = new TextDecorationCollection(new TextDecoration[] {deco});
#endif
            }

            // ReSharper disable once UnusedMember.Local
            private static Pen ErrorPen()
            {
                var path_pen = new Pen(new SolidColorBrush(Colors.Red), 0.2)
                {
                    EndLineCap = PenLineCap.Square,
                    StartLineCap = PenLineCap.Square
                };

                var path_start = new Point(0, 1);
                var path_segment = new BezierSegment(
                    new Point(1, 0), new Point(2, 2), new Point(3, 1), true);
                var path_figure = new PathFigure(path_start, new PathSegment[] { path_segment }, false);
                var path_geometry = new PathGeometry(new[] { path_figure });

                var squiggly_brush = new DrawingBrush
                {
                    Viewport = new Rect(0, 0, 6, 4),
                    ViewportUnits = BrushMappingMode.Absolute,
                    TileMode = TileMode.Tile,
                    Drawing = new GeometryDrawing(null, path_pen, path_geometry)
                };

                return new Pen(squiggly_brush, 3);
            }

            public override Typeface Typeface { get; }
            public override double FontRenderingEmSize { get; }
            public override double FontHintingEmSize { get; }
            public override TextDecorationCollection TextDecorations { get; }
            public override Brush ForegroundBrush { get; }
            public override Brush BackgroundBrush { get; }
            public override CultureInfo CultureInfo { get; }
            public override TextEffectCollection TextEffects { get; }

            public double LineHeight => FontRenderingEmSize * Typeface.FontFamily.LineSpacing;
        }

        public class TheParagraphProperties : TextParagraphProperties
        {
            private readonly Source textSource;

            public TheParagraphProperties(Source textSource)
            {
                this.textSource = textSource;
            }

            public override FlowDirection FlowDirection => FlowDirection.LeftToRight;
            public override TextAlignment TextAlignment => TextAlignment.Left;
            public override double LineHeight => this.textSource.RunProperties.LineHeight;
            public override bool FirstLineInParagraph => true;
            public override TextRunProperties DefaultTextRunProperties => this.textSource.RunProperties;
            public override TextWrapping TextWrapping => TextWrapping.NoWrap;
            public override TextMarkerProperties TextMarkerProperties => null;
            public override double Indent => 0;
        }
    }
}
