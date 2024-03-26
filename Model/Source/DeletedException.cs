namespace Leagueinator.Model {

    public interface IDeleted {
        public bool Deleted { get; }
    }

    public class DeletedException : Exception {
        public DeletedException() : base("Model component has been deleted") { }

        public DeletedException(string type) : base($"Model component has been deleted: {type}") { }

        public static void ThrowIf(IDeleted obj) {
            if (obj.Deleted) throw new DeletedException(obj.GetType().Name);
        }
    }
}
