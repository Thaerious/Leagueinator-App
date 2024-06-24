using System.Windows.Controls;

namespace Leagueinator.Controls {
    public class DataButton<T>() : Button {
        public T? Data { get; set; } = default;
    }
}
