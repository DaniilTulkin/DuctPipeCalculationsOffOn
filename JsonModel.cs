namespace DuctPipeCalculationsOffOn
{
    public class JsonModel
    {
        public string Name { get; set; }
        public string SystemCalculationLevel { get; set; }

        public JsonModel(string name, string systemCalculationLevel)
        {
            Name = name;
            SystemCalculationLevel = systemCalculationLevel;
        }
    }
}
