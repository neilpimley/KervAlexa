using System;
using System.Text.RegularExpressions;

namespace Kerv.Common
{
    public class Money
    {
        private int pence;
        private static readonly Regex moneyRegex = new Regex(@"\d+\.\d\d");

        public Money(int pence)
        {
            this.pence = pence;
        }

        public Money(float pounds)
        {
            Pounds = pounds;
        }

        public Money(String amount)
        {
            var match = moneyRegex.Match(amount);
            if (match.Success)
            {
                Pounds = float.Parse(match.Value);
            }
            else
            {
                pence = 0;
            }
        }

        public float Pounds
        {
            get => pence / 100.0f;
            private set
            {
                pence = (int)Math.Round(value * 100);
            }
        }

        public int Pence
        {
            get => pence;
        }

        public override string ToString()
        {
            return String.Format("£{0:F2}", Pounds);
        }

    }
}
