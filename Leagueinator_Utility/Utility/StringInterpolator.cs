namespace Leagueinator.Utility {
    public class StringInterpolator : Dictionary<string, string> {

        public string Interpolate(string input) {
            foreach (string key in this.Keys) {
                input = input.Replace("${" + key + "}", this[key]);
            }

            return input;
        }
    }
}
