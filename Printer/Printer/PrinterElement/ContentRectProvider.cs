namespace Leagueinator.Printer {
    public class ContentRectProvider : HasContentRect {
        public delegate RectangleF ProvideRectangle();
        private ProvideRectangle source;
        
        public ContentRectProvider(ProvideRectangle source) {
            this.source = source;
        }

        public RectangleF ContentRect => source();
    }
}
