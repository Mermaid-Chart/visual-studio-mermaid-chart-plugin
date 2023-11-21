using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MermaidChart.Utils
{
    public class Either<TL,TR>
    {
        internal TL Left;
        internal TR Right;
        internal bool IsLeft;

        public Either(TL left)
        {
            this.Left = left;
            this.IsLeft = true;
        }

        public Either(TR right)
        {
            this.Right = right;
            this.IsLeft = false;
        }

        public T Match<T>(Func<TL, T> leftFunc, Func<TR, T> rightFunc)
            => this.IsLeft ? leftFunc(this.Left) : rightFunc(this.Right);

        public static implicit operator Either<TL, TR>(TL left) => new Either<TL, TR>(left);

        public static implicit operator Either<TL, TR>(TR right) => new Either<TL, TR>(right);
    }

    public class EitherE<T> : Either<Exception, T>
    {
        public EitherE(Exception left) : base(left)
        {
        }

        public EitherE(T right) : base(right)
        {
        }
    }
}
