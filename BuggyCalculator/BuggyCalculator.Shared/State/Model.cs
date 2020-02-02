using System;
using System.Globalization;

namespace BuggyCalculator.State.Model
{
    abstract class Evaluable
    {
        public abstract decimal Evaluate();
        public abstract string ToFormula();
        public abstract int Precedence();

        protected string Parenthesis(string s, int precedence)
        {
            if (precedence < Precedence())
            {
                return "(" + s + ")";
            }
            else
            {
                return s;
            }
        }
    }

    class Value : Evaluable
    {
        private decimal value;

        private CultureInfo culture = CultureInfo.CurrentUICulture;

        public Value(decimal value)
        {
            this.value = value;
        }

        public override decimal Evaluate()
        {
            return value;
        }

        public override string ToFormula()
        {
            int nDecimals = BitConverter.GetBytes(Decimal.GetBits(value)[3])[2];
            return value.ToString("N" + nDecimals, culture);
        }

        public override int Precedence()
        {
            return 2;
        }
    }

    class Add : Evaluable
    {
        private Evaluable v1;
        private Evaluable v2;

        public Add(Evaluable v1, Evaluable v2)
        {
            this.v1 = v1;
            this.v2 = v2;
        }

        public override decimal Evaluate()
        {
            return v1.Evaluate() + v2.Evaluate();
        }

        public override string ToFormula()
        {
            string s1 = v1.ToFormula();
            string s2 = v2.ToFormula();

            return Parenthesis(s1, v1.Precedence()) + " + " + Parenthesis(s2, v2.Precedence());
        }

        public override int Precedence()
        {
            return 0;
        }
    }

    class Substract : Evaluable
    {
        private Evaluable v1;
        private Evaluable v2;

        public Substract(Evaluable v1, Evaluable v2)
        {
            this.v1 = v1;
            this.v2 = v2;
        }

        public override decimal Evaluate()
        {
            return v1.Evaluate() - v2.Evaluate();
        }

        public override string ToFormula()
        {
            string s1 = v1.ToFormula();
            string s2 = v2.ToFormula();

            return Parenthesis(s1, v1.Precedence()) + " - " + Parenthesis(s2, v2.Precedence());
        }

        public override int Precedence()
        {
            return 0;
        }
    }

    class Multiply : Evaluable
    {
        private Evaluable v1;
        private Evaluable v2;

        public Multiply(Evaluable v1, Evaluable v2)
        {
            this.v1 = v1;
            this.v2 = v2;
        }

        public override decimal Evaluate()
        {
            return this.v1.Evaluate() * this.v2.Evaluate();
        }

        public override string ToFormula()
        {
            string s1 = v1.ToFormula();
            string s2 = v2.ToFormula();

            return Parenthesis(s1, v1.Precedence()) + " × " + Parenthesis(s2, v2.Precedence());
        }

        public override int Precedence()
        {
            return 1;
        }
    }

    class Divide : Evaluable
    {
        private Evaluable v1;
        private Evaluable v2;

        public Divide(Evaluable v1, Evaluable v2)
        {
            this.v1 = v1;
            this.v2 = v2;
        }

        public override decimal Evaluate()
        {
            return v1.Evaluate() / v2.Evaluate();
        }

        public override string ToFormula()
        {
            string s1 = v1.ToFormula();
            string s2 = v2.ToFormula();

            return Parenthesis(s1, v1.Precedence()) + " : " + Parenthesis(s2, v2.Precedence());
        }

        public override int Precedence()
        {
            return 1;
        }
    }
}
