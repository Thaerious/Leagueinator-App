namespace Test_Style {
    internal class Dummy {
        private bool _invalid;

        public bool Invalid {
            get => _invalid;
            set {
                Console.WriteLine($"Set Invalid {value}");
                _invalid = value;
            }
        }

        public bool Foo { get; [Validated] set; }

        [Validated] public bool Bar = false;

        [Validated]
        public void HelloWorld() {
            Console.WriteLine("Hello World");
        }

    }
}
