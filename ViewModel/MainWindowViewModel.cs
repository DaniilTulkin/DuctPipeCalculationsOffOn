namespace DuctPipeCalculationsOffOn.ViewModel
{
    using Autodesk.Revit.DB.Mechanical;
    using Autodesk.Revit.DB.Plumbing;
    using Autodesk.Revit.UI;
    using DuctPipeCalculationsOffOn.MVVM;
    using DuctPipeCalculationsOffOn.Services;
    using System.Collections.Generic;
    using System.IO;
    using System.Windows.Input;

    public class MainWindowViewModel : ModelBase
    {
        private MEPSystemService mepSystemService;
        private IEnumerable<MEPSystemTypeModel> mepSystemTypes;
        private string revitFileName;

        public List<MEPSystemTypeModel> MechanicalSystemTypes { get; set; } = new List<MEPSystemTypeModel>();
        public List<MEPSystemTypeModel> PipingSystemTypes { get; set; } = new List<MEPSystemTypeModel>();

        private bool isEnable;
        public bool IsEnable
        {
            get
            {
                return isEnable;
            }
        }

        public MainWindowViewModel(UIApplication app)
        {
            mepSystemService = new MEPSystemService(app);
            mepSystemTypes = mepSystemService.GetMEPSystemTypes();
            revitFileName = mepSystemService.revitFileName;

            if (!File.Exists(Json.jsonFolderPath + $"\\{revitFileName}.json"))
            {
                Json.WriteJson(mepSystemTypes, revitFileName);
            }

            foreach (var mepSystemType in mepSystemTypes)
            {
                if (mepSystemType.MEPSystemType is MechanicalSystemType)
                {
                    MechanicalSystemTypes.Add(mepSystemType);
                }
                else if (mepSystemType.MEPSystemType is PipingSystemType)
                {
                    PipingSystemTypes.Add(mepSystemType);
                }
            }
        }

        public ICommand rdbSwitchOn => new RelayCommandWithoutParameter(OnrdbSwitchOn);
        private void OnrdbSwitchOn()
        {
            isEnable = false;
            OnPropertyChanged("IsEnable");

            foreach (var m in mepSystemTypes)
            {
                m.SystemCalculationLevel = SystemCalculationLevelRus.ToRus(SystemCalculationLevel.All);
            }
        }

        public ICommand rdbSwitchOff => new RelayCommandWithoutParameter(OnrdbSwitchOff);
        private  void OnrdbSwitchOff()
        {
            isEnable = false;
            OnPropertyChanged("IsEnable");

            foreach (var m in mepSystemTypes)
            {
                m.SystemCalculationLevel = SystemCalculationLevelRus.ToRus(SystemCalculationLevel.None);
            }
        }

        public ICommand rdbSwitchCustom => new RelayCommandWithoutParameter(OnrdbSwitchCustom);
        private void OnrdbSwitchCustom()
        {
            isEnable = true;
            OnPropertyChanged("IsEnable");

            foreach (var Old in mepSystemTypes)
            {
                foreach (var New in mepSystemService.GetMEPSystemTypes())
                {
                    if (Old.Name == New.Name)
                    {
                        Old.SystemCalculationLevel = New.SystemCalculationLevel;
                    }
                }
            }
        }

        public ICommand btnSave => new RelayCommandWithoutParameter(OnbtnSave);
        private void OnbtnSave()
        {
            Json.WriteJson(mepSystemTypes, revitFileName);
        }

        public ICommand btnLoad => new RelayCommandWithoutParameter(OnbtnLoad);
        private void OnbtnLoad()
        {
            List<JsonModel> jsonModels = Json.ReadJson<List<JsonModel>>(revitFileName);
            foreach (var Old in mepSystemTypes)
            {
                foreach (var New in jsonModels)
                {
                    if (Old.Name == New.Name)
                    {
                        Old.SystemCalculationLevel = New.SystemCalculationLevel;
                    }
                }
            }
        }

        public ICommand btnApply => new RelayCommandWithoutParameter(OnbtnApply);
        private void OnbtnApply()
        {
            mepSystemService.revitEvent.Run(app => mepSystemService.ChangeCalculationType(mepSystemTypes));
        }
    }
}
