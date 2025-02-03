using System.Collections;
using System.Collections.Generic;

namespace FanKit.Layers.Demo
{
    /// <summary/>
    public sealed class DemoLayerCollection : IEnumerable<DemoLayer>, IEnumerator<DemoLayer>
    {
        byte i;

        /// <inheritdoc/>
        object IEnumerator.Current => C();
        /// <inheritdoc/>
        public DemoLayer Current => C();

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => this;
        /// <inheritdoc/>
        public IEnumerator<DemoLayer> GetEnumerator() => this;

        /// <inheritdoc/>
        public void Reset() => this.i = 0;
        /// <inheritdoc/>
        public void Dispose() => this.i = 0;

        /// <inheritdoc/>
        public bool MoveNext()
        {
            switch (this.i)
            {
                case 0: this.i = 1; return true;
                case 1: this.i = 2; return true;

                case 2: this.i = 3; return true;
                case 3: this.i = 4; return true;
                case 4: this.i = 5; return true;
                case 5: this.i = 6; return true;
                case 6: this.i = 7; return true;

                case 7: this.i = 8; return true;
                case 8: this.i = 9; return true;

                case 9: this.i = 10; return true;
                case 10: this.i = 11; return true;
                default:; return false;
            }
        }
        // Create a Layer
        private DemoLayer C()
        {
            switch (this.i)
            {
                case 1: return L("1", 0);
                case 2: return L("2", 0);

                case 3: return G("3", 0);
                case 4: return G("4", 1);
                case 5: return L("5", 2);
                case 6: return L("6", 2);
                case 7: return L("7", 2);

                case 8: return G("8", 0);
                case 9: return L("9", 1);

                case 10: return G("10", 0);
                case 11: return L("11", 1);
                default: return null;
            }
        }

        // Create a Layer
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static DemoLayer L(string t, int d) => new DemoLayer
        {
            Title = t,
            Depth = d
        };
        // Create a Group Layer
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static DemoLayer G(string t, int d) => new DemoLayer
        {
            IsGroup = true,
            Title = t,
            Depth = d,
            IsExpanded = true
        };
    }
}