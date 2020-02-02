using System;
using System.Diagnostics;
using System.Globalization;

namespace BuggyCalculator.State
{
    /// <summary>
    /// This is the view model and main state machine of the calculator
    /// </summary>
    public class Calculator : Notifier
    {
        private readonly string primaryDisplayName = "PrimaryDisplay";
        private readonly string secondaryDisplayName = "SecondaryDisplay";
        private readonly Settings settings;

        public string PrimaryDisplay
        {
            get
            {
                return inputField.ToString();
            }
        }

        public string SecondaryDisplay
        {
            get
            {
                return (root != null) ? root.ToFormula() + " ≈" : "";
            }
        }

        public string DecimalSeparator
        {
            get
            {
                return CultureInfo.CurrentUICulture.NumberFormat.CurrencyDecimalSeparator;
            }
        }

        #region Actual state of the calculator

        private enum Operation
        {
            ADD,
            SUBSTRACT,
            MULTIPLY,
            DIVIDE
        }
        private Model.Evaluable root = null;
        private Operation? pendingOp = null;

        // This is an auxiliary class to collect and display the input
        private readonly InputField inputField = new InputField();

        #endregion

        public Calculator(Settings settings)
        {
            this.settings = settings;

            ResetStateMachine();
        }

        private void ResetStateMachine()
        {
            root = null;
            pendingOp = null;
        }

        public void HandleResetKey()
        {
            ResetStateMachine();
            inputField.Reset();

            Notify(primaryDisplayName);
            Notify(secondaryDisplayName);
        }

        public void HandleClearKey()
        {
            inputField.Reset();

            Notify(primaryDisplayName);
        }

        public void HandleNumberKey(int n)
        {
            inputField.HandleNumberKey(n);

            Notify(primaryDisplayName);
        }

        public void HandlePointKey()
        {
            inputField.HandlePointKey();

            Notify(primaryDisplayName);
        }

        public void HandleOppositeKey()
        {
            inputField.HandleOppositeKey();

            Notify(primaryDisplayName);
        }

        private bool IsFormulaBeingConstructed()
        {
            return root != null;
        }

        private void QueueInitialValue()
        {
            Debug.Assert(!IsFormulaBeingConstructed());

            root = new Model.Value(inputField.Value);
        }

        private void QueuePendingOperation()
        {
            Debug.Assert(IsFormulaBeingConstructed());
            Debug.Assert(pendingOp.HasValue);

            switch (pendingOp.Value)
            {
                case Operation.ADD:
                    root = new Model.Add(root, new Model.Value(inputField.Value));
                    break;

                case Operation.SUBSTRACT:
                    root = new Model.Substract(root, new Model.Value(inputField.Value));
                    break;

                case Operation.MULTIPLY:
                    root = new Model.Multiply(root, new Model.Value(inputField.Value));
                    break;
                
                case Operation.DIVIDE:
                    root = new Model.Divide(root, new Model.Value(inputField.Value));
                    break;

                default:
                    throw new InvalidOperationException();
            }
        }

        private void HandleUserInput()
        {
            if (!IsFormulaBeingConstructed())
            {
                QueueInitialValue();
            }
            else
            {
                QueuePendingOperation();
            }

            Debug.Assert(IsFormulaBeingConstructed());
        }

        private void HandleOperationKey(Operation op)
        {
            HandleUserInput();

            // Save the Op as next pending operation
            pendingOp = op;

            // Display the partial result
            inputField.Value = root.Evaluate();

            Notify(primaryDisplayName);
            Notify(secondaryDisplayName);
        }

        public void HandleAddKey()
        {
            HandleOperationKey(Operation.ADD);
        }

        public void HandleSubstructKey()
        {
            HandleOperationKey(Operation.SUBSTRACT);
        }

        public void HandleMultiplyKey()
        {
            HandleOperationKey(Operation.MULTIPLY);
        }

        public void HandleDivideKey()
        {
            HandleOperationKey(Operation.DIVIDE);
        }
        
        public void HandleResultKey()
        {
            HandleUserInput();

            // This is the final result
            // and where the error should be applied :-)
            decimal correctResult = root.Evaluate();
            decimal error = CalculateError(correctResult);
            inputField.Value = AddErrorAndRound(correctResult, error);

            // Update the UI
            Notify(primaryDisplayName);
            Notify(secondaryDisplayName);

            // Reset the internal state, but not the input field
            ResetStateMachine();
        }

        private decimal CalculateError(decimal result)
        {
            decimal factor = settings.PseudoRandomError ? (decimal)(Math.Abs(result.GetHashCode())) / int.MaxValue : 1;

            return settings.ErrorFixedDecimal() + result * factor * settings.ErrorProportionalDecimal();
        }

        private decimal AddErrorAndRound(decimal result, decimal error)
        {
            if (!settings.SmartRounding)
            {
                return result + error;
            }
            else
            {
                int nDecimals = BitConverter.GetBytes(Decimal.GetBits(result)[3])[2];
                return Math.Round(result + error, nDecimals);
            }
        }
    }
}
