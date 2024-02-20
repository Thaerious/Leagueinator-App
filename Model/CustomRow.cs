using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Tables {
    public class CustomRow(League league, DataRow row) {
        public readonly League League = league;
        public readonly DataRow DataRow = row;
    }

    public class InvalidTableException : Exception {
        public InvalidTableException(string? message) : base(message) {}

        public static void CheckTable<T>(DataRow row) {
            if (typeof(T) != row.Table.GetType()) {
                throw new InvalidTableException(
                    $"Incorrect table in DataRow, expected {typeof(T)}, found {row.Table.GetType()}"
                );
            }
        }
    }
}
