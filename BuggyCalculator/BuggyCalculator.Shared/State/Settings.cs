using System;
using Windows.Storage;

namespace BuggyCalculator.State
{
    public class Settings : Notifier
    {
        private readonly string settingsFolderName = "Settings";

        private readonly string errorFixedName = "ErrorFixed";
        private readonly string errorFixedAsTextName = "ErrorFixedAsText";
        private readonly string errorProportionalName = "ErrorProportional";
        private readonly string errorProportionalAsTextName = "ErrorProportionalAsText";
        private readonly string pseudoRandomErrorName = "PseudoRandomError";
        private readonly string smartRoundingName = "SmartRounding";

        private double errorFixed;
        private double errorProportional;
        private bool pseudoRandomError;
        private bool smartRounding;

        public double ErrorFixed
        {
            get { return errorFixed; }
            set { errorFixed = value; Notify(errorFixedName); Notify(errorFixedAsTextName); }
        }

        public string ErrorFixedAsText
        {
            get { return errorFixed.ToString("N2"); }
        }

        public double ErrorProportional
        {
            get { return errorProportional; }
            set { errorProportional = value; Notify(errorProportionalName); Notify(errorProportionalAsTextName); }
        }

        public string ErrorProportionalAsText
        {
            get { return (errorProportional / 100).ToString("P0"); }
        }

        public bool PseudoRandomError
        {
            get { return pseudoRandomError; }
            set { pseudoRandomError = value; Notify(pseudoRandomErrorName); }
        }

        public bool SmartRounding
        {
            get { return smartRounding; }
            set { smartRounding = value; Notify(smartRoundingName); }
        }

        public Settings()
        {
            Default();
        }

        public void Default()
        {
            ErrorFixed = 0;
            ErrorProportional = 0;
            PseudoRandomError = false;
            SmartRounding = true;
        }

        public void Save()
        {
            var localSettings = ApplicationData.Current.LocalSettings;
            var localValues = localSettings.CreateContainer(settingsFolderName, ApplicationDataCreateDisposition.Always).Values;

            localValues[errorFixedName] = ErrorFixed;
            localValues[errorProportionalName] = ErrorProportional;
            localValues[pseudoRandomErrorName] = PseudoRandomError;
            localValues[smartRoundingName] = SmartRounding;
        }

        public void Restore()
        {
            Default();

            var localSettings = ApplicationData.Current.LocalSettings;

            if (localSettings.Containers.ContainsKey(settingsFolderName))
            {
                var localValues = localSettings.Containers[settingsFolderName].Values;

                if (localValues.ContainsKey(errorFixedName))
                {
                    ErrorFixed = (double)localValues[errorFixedName];
                }

                if (localValues.ContainsKey(errorProportionalName))
                {
                    ErrorProportional = (double)localValues[errorProportionalName];
                }

                if (localValues.ContainsKey(pseudoRandomErrorName))
                {
                    PseudoRandomError = (bool)localValues[pseudoRandomErrorName];
                }

                if (localValues.ContainsKey(smartRoundingName))
                {
                    SmartRounding = (bool)localValues[smartRoundingName];
                }
            }
        }

        public decimal ErrorFixedDecimal()
        {
            // Precision is up to 2 decimal digits, we round it at 4
            return (decimal)Math.Round(errorFixed, 4);
        }

        public decimal ErrorProportionalDecimal()
        {
            // Precision is 2 decimal digits, we round it at 4
            return (decimal)Math.Round(errorProportional / 100, 4);
        }
    }
}
