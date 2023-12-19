﻿namespace Leagueinator_App {
    public enum Change { VALUE, COMPOSITION};

    public class ModelUpdateEventHandlerArgs {
        public readonly Change Change;
        public readonly string Field;

        public ModelUpdateEventHandlerArgs(Change change, string field) {
            this.Change = change;
            this.Field = field;
        }

        public ModelUpdateEventHandlerArgs(Change change) {
            this.Change = change;
            this.Field = "N/A";
        }
    }

    public class LeagueSingleton {
        public delegate void ModelUpdateEventHandler(object? source, ModelUpdateEventHandlerArgs args);
        public static event ModelUpdateEventHandler ModelUpdate = delegate { };

        public static void Invoke(object? source, ModelUpdateEventHandlerArgs args) {
            ModelUpdate?.Invoke(source, args);
        }
    }
}
