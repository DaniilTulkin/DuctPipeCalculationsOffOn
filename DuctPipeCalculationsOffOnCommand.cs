namespace DuctPipeCalculationsOffOn.Model
{
    using Autodesk.Revit.Attributes;
    using Autodesk.Revit.UI;
    using global::DuctPipeCalculationsOffOn.View;
    using System;

    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class DuctPipeCalculationsOffOn : IExternalEventHandler
    {
        public void Execute(UIApplication app)
        {
            try
            {
                MainWindow mainWindow = new MainWindow(app);
                mainWindow?.Show();
            }
            catch (Exception ex)
            {
            }
        }

        public string GetName() => nameof(DuctPipeCalculationsOffOn);
    }
}
