namespace Liminal.Editor.TemplateSetup
{
    public abstract class TemplateSetupPage
    {
        public abstract bool CanProceed { get; }
        public abstract string Name { get; }
        public abstract void DrawPage();
    }
}