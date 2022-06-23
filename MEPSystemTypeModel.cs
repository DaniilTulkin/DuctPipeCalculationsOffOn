namespace DuctPipeCalculationsOffOn
{
    using Autodesk.Revit.DB;
    using Autodesk.Revit.DB.Mechanical;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using Newtonsoft.Json;

    public class MEPSystemTypeModel : INotifyPropertyChanged
    {
        
        private MEPSystemType mepSystemType;
        [JsonIgnore]
        public MEPSystemType MEPSystemType 
        {
            get
            {
                return mepSystemType;
            }
            set
            {
                mepSystemType = value;
                OnPropertyChange("MEPSystemType");
            }
        }

        private string name;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                OnPropertyChange("Name");
            }
        }

        private string systemCalculationLevel;
        public string SystemCalculationLevel
        {
            get
            {
                return systemCalculationLevel;
            }
            set
            {
                systemCalculationLevel = value;
                OnPropertyChange("SystemCalculationLevel");
            }
        }

        [JsonIgnore]
        public List<string> SystemCalculationLevels { get; set; } = new List<string>();

        public MEPSystemTypeModel(MEPSystemType mepSystemType)
        {
            MEPSystemType = mepSystemType;
            Name = mepSystemType.Name;
            SystemCalculationLevel = SystemCalculationLevelRus.ToRus(mepSystemType.CalculationLevel);

            Array enumData = Enum.GetValues(typeof(SystemCalculationLevel));
            foreach (SystemCalculationLevel item in enumData)
            {
                if (item != Autodesk.Revit.DB.Mechanical.SystemCalculationLevel.Volume)
                {
                    SystemCalculationLevels.Add(SystemCalculationLevelRus.ToRus(item));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChange([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
    }
}
