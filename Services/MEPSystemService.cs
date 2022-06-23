namespace DuctPipeCalculationsOffOn.Services
{
    using Autodesk.Revit.DB;
    using Autodesk.Revit.DB.Mechanical;
    using Autodesk.Revit.DB.Plumbing;
    using Autodesk.Revit.UI;
    using DuctPipeCalculationsOffOn;
    using DuctPipeCalculationsOffOn.ExternalEvents;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class MEPSystemService
    {
        public UIApplication uiapp;
        public UIDocument uidoc;
        public Document doc;
        public RevitEvent revitEvent;
        public string revitFileName;

        public MEPSystemService(UIApplication uiapp)
        {
            this.uiapp = uiapp;
            uidoc = uiapp.ActiveUIDocument;
            doc = uidoc.Document;
            revitEvent = new RevitEvent();

            string filePath = string.Empty;
            if (doc.IsWorkshared)
            {
                ModelPath centralPath = doc.GetWorksharingCentralModelPath();
                filePath = ModelPathUtils.ConvertModelPathToUserVisiblePath(centralPath);
            }
            else
            {
                filePath = doc.PathName;
            }
            revitFileName = Path.GetFileName(filePath);
        }

        public void ChangeCalculationType(IEnumerable<MEPSystemTypeModel> mepSystemTypesList)
        {
            using (Transaction t = new Transaction(doc, "Изменение типов расчётов"))
            {
                t.Start();
                foreach (MEPSystemTypeModel mepSystemType in mepSystemTypesList)
                {
                    SystemCalculationLevel systemCalculationLevel = SystemCalculationLevelRus.ToEng(mepSystemType.SystemCalculationLevel);
                    mepSystemType.MEPSystemType.CalculationLevel = systemCalculationLevel;
                }
                t.Commit();
            }
        }

        public IEnumerable<MEPSystemTypeModel> GetMEPSystemTypes()
        {
            var mechanicalSystemType = new FilteredElementCollector(doc)
                .OfClass(typeof(MechanicalSystemType))
                .Cast<MechanicalSystemType>()
                .Select(p => new MEPSystemTypeModel(p))
                .ToList();

            var pipingSystemType = new FilteredElementCollector(doc)
                .OfClass(typeof(PipingSystemType))
                .Cast<PipingSystemType>()
                .Select(p => new MEPSystemTypeModel(p))
                .ToList();

            return mechanicalSystemType.Concat(pipingSystemType);
        }
    }
}