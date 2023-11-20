using Leagueinator.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1 {
    public class CI<T> {
        public T? Left, Right, Top, Bottom;

        public CI() {
            Left = default;
            Right = default;
            Top = default;
            Bottom = default;
        }

        public CI(T value) {
            Left = value;
            Right = value;
            Top = value;
            Bottom = value;
        }

        public CI(T top, T right, T bottom, T left) {
            this.Left = left;
            this.Right = right;
            this.Top = top;
            this.Bottom = bottom;
        }

        public static void TryParse(CI<T> target, string source) {
            var method = typeof(T).GetMethod(
                "TryParse",
                BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy,
                new Type[] { typeof(string), typeof(T).MakeByRefType() }
            );

            if (method != null) HasTryParse(target, source, method);
            if (typeof(T).IsEnum) IsEnum(target, source);
        }

        private static void HasTryParse(CI<T> target, string source, MethodInfo method) {
            string[] split = source.Split();

            if (split.Length == 1) {
                object?[] argsTop = new object?[] { split[0], default };
                method.Invoke(null, argsTop);
                target.Top = (T?)argsTop[1];
                target.Right = (T?)argsTop[1];
                target.Bottom = (T?)argsTop[1];
                target.Left = (T?)argsTop[1];
            }
            if (split.Length >= 2) {
                object?[] argsRight = new object?[] { split[1], default };
                method.Invoke(null, argsRight);
                target.Right = (T?)argsRight[1];
            }
            if (split.Length >= 3) {
                object?[] argsBottom = new object?[] { split[2], default };
                method.Invoke(null, argsBottom);
                target.Bottom = (T?)argsBottom[1];
            }
            if (split.Length >= 4) {
                object?[] argsLeft = new object?[] { split[3], default };
                method.Invoke(null, argsLeft);
                target.Left = (T?)argsLeft[1];
            }
        }

        private static void IsEnum(CI<T> target, string source) {
            string[] split = source.Split();

            if (split.Length == 1) {
                target.Top = (T?)Enum.Parse(typeof(T), split[0]);
                target.Right = (T?)Enum.Parse(typeof(T), split[0]);
                target.Bottom = (T?)Enum.Parse(typeof(T), split[0]);
                target.Left = (T?)Enum.Parse(typeof(T), split[0]);
            }
            if (split.Length >= 2) {
                target.Right = (T?)Enum.Parse(typeof(T), split[1]);
            }
            if (split.Length >= 3) {
                target.Bottom = (T?)Enum.Parse(typeof(T), split[2]);
            }
            if (split.Length >= 4) {
                target.Left = (T?)Enum.Parse(typeof(T), split[3]);
            }
        }

        public override string ToString() {
            return $"Cardinal<{typeof(int).Name}>({this.Top} {this.Right} {this.Bottom} {this.Left})";
        }
    }
}
