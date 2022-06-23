namespace DuctPipeCalculationsOffOn
{
    using Autodesk.Revit.DB.Mechanical;

    public class SystemCalculationLevelRus
    {
        public static string ToRus(SystemCalculationLevel systemCalculationLevel)
        {
            switch (systemCalculationLevel)
            {
                case SystemCalculationLevel.All:
                    return "Все";
                case SystemCalculationLevel.None:
                    return "Нет";
                case SystemCalculationLevel.Flow:
                    return "Только расход";
                case SystemCalculationLevel.Performance:
                    return "Производительность";
                case SystemCalculationLevel.Volume:
                    return string.Empty;
                default:
                    return string.Empty;
            }
        }
        
        public static SystemCalculationLevel ToEng(string rus)
        {
            switch (rus)
            {
                case "Все":
                    return SystemCalculationLevel.All;
                case "Нет":
                    return SystemCalculationLevel.None;
                case "Только расход":
                    return SystemCalculationLevel.Flow;
                case "Производительность":
                    return SystemCalculationLevel.Performance;
                default:
                    return SystemCalculationLevel.None;
            }
        }
    }
}
