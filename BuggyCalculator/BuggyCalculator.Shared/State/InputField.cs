using System;
using System.Diagnostics;
using System.Globalization;

namespace BuggyCalculator.State
{
    class InputField
    {
        private decimal currentVal = 0;
        private byte digitsAfterPoint = 0;
        private bool isPointAlreadyPressed = false;
        private bool isDisplayingExternalValue = false;

        private CultureInfo culture = CultureInfo.CurrentUICulture;

        public decimal Value
        {
            get
            {
                return currentVal;
            }

            set
            {
                currentVal = value;
                isDisplayingExternalValue = true;
            }
        }

        public override string ToString()
        {
            if (isDisplayingExternalValue)
            {
                int nDecimals = BitConverter.GetBytes(Decimal.GetBits(currentVal)[3])[2];
                return currentVal.ToString("N" + nDecimals, culture);
            }
            else
            {
                return currentVal.ToString("N" + digitsAfterPoint, culture);
            }
        }

        public InputField()
        {
            Reset();
        }

        public void Reset()
        {
            currentVal = 0;
            digitsAfterPoint = 0;
            isPointAlreadyPressed = false;
            isDisplayingExternalValue = false;
        }

        public void HandleNumberKey(int n)
        {
            Debug.Assert(n >= 0 && n <= 9);

            if (isDisplayingExternalValue)
            {
                Reset();
            }

            if (! isPointAlreadyPressed)
            {
                currentVal = currentVal * 10 + n;
            }
            else
            {
                digitsAfterPoint++;
                currentVal = currentVal + new decimal(n, 0, 0, false, digitsAfterPoint);
            }
        }

        public void HandlePointKey()
        {
            if (isDisplayingExternalValue)
            {
                Reset();
            }

            isPointAlreadyPressed = true;
            digitsAfterPoint = 0;
        }

        public void HandleOppositeKey()
        {
            currentVal = -currentVal;
        }
    }
}
