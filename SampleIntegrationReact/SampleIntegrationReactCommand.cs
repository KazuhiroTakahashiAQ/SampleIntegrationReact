using Rhino;
using Rhino.Commands;
using Rhino.UI;

namespace SampleIntegrationReact
{
    public class SampleIntegrationReactCommand : Command
    {
        public SampleIntegrationReactCommand()
        {
            Instance = this;
        }

        public static SampleIntegrationReactCommand Instance { get; private set; }

        public override string EnglishName => "SampleIntegrationReactCommand";

        /// <summary>
        /// コマンドが実行されたときに呼び出されます。
        /// </summary>
        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            var panelId = typeof(MyPanel).GUID;
            Panels.OpenPanel(panelId);

            return Result.Success;
        }
    }
}
