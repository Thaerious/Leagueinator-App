namespace Leagueinator.Utility {
    public class NoSuchElementException : Exception {
        public NoSuchElementException() { }

        public NoSuchElementException(string message) : base(message) { }
    }
}
