using System;
using Rhino;
using Rhino.PlugIns;
using Rhino.UI;
using Eto.Forms;
using System.Runtime.InteropServices;
using Rhino.DocObjects;
using Rhino.Geometry;

namespace SampleIntegrationReact
{
    public class SampleIntegrationReactPlugin : PlugIn
    {
        public SampleIntegrationReactPlugin()
        {
            Instance = this;
        }

        /// <summary>
        /// パネルのIDを定義します。
        /// </summary>
        //public static readonly Guid PanelId = new Guid("9F96C4A4-87F3-46F6-B29E-B041ABC40F57");

        /// <summary>
        /// プラグインのロード時に呼び出されます。
        /// </summary>
        protected override LoadReturnCode OnLoad(ref string errorMessage)
        {
            Panels.RegisterPanel(this, typeof(MyPanel), "React Panel", null);
            return LoadReturnCode.Success;
        }

        ///<summary>Gets the only instance of the SampleIntegrationReactPlugin plug-in.</summary>
        public static SampleIntegrationReactPlugin Instance { get; private set; }
    }

    /// <summary>
    /// Reactアプリケーションを表示するパネルクラス
    /// </summary>
    [Guid("9F96C4A4-87F3-46F6-B29E-B041ABC40F57")]
    public class MyPanel : Panel
    {
        private WebViewAdapter _webViewAdapter;
        private Guid _objectId; // 操作対象のオブジェクトID
        private BoundingBox _originalBoundingBox; // 元のオブジェクトのバウンディングボックス
        private double _originalSize; // 元のオブジェクトのサイズ（対角線の長さ）

        public MyPanel()
        {
            _webViewAdapter = new();
            InitializeComponents();

            // Rhinoのイベントハンドラを設定
            RhinoDoc.SelectObjects += OnSelectObjects;
        }

        private void InitializeComponents()
        {
            // Reactから実行される関数の登録
            _webViewAdapter.Register("UpdateObjectSize", (query) =>
            {
                // React側からのオブジェクトサイズ更新要求を処理
                var sizeStr = query["size"];
                if (!double.TryParse(sizeStr, out double size))
                {
                    RhinoApp.WriteLine($"Invalid size value: {sizeStr}");
                }
                UpdateObjectSize(size);
            });

            Content = _webViewAdapter.WebView;
        }

        // オブジェクト選択イベントハンドラ
        private void OnSelectObjects(object sender, RhinoObjectSelectionEventArgs e)
        {
            if (e.Selected)
            {
                _objectId = e.RhinoObjects[0].Id;
                var obj = e.RhinoObjects[0];
                _originalBoundingBox = obj.Geometry.GetBoundingBox(true);
                _originalSize = _originalBoundingBox.Diagonal.Length;

                string objectName = obj.Name ?? "";
                string layerName = obj.Document.Layers[obj.Attributes.LayerIndex].Name;
                System.Drawing.Color color = obj.Attributes.DrawColor(obj.Document);

                string colorString = $"rgb({color.R}, {color.G}, {color.B})";

                // jsの処理を呼ぶ
                _webViewAdapter.CallJs("setRhinoProperty", new object[] { objectName, layerName, colorString });
            }
        }

        private void UpdateObjectSize(double size)
        {
            if (_objectId == Guid.Empty) return;
            if (_originalSize == 0) return;

            var doc = RhinoDoc.ActiveDoc;
            var obj = doc.Objects.FindId(_objectId);
            if (obj == null) return;

            var currentBoundingBox = obj.Geometry.GetBoundingBox(true);
            var currentSize = currentBoundingBox.Diagonal.Length;

            double requiredScale = (size * _originalSize) / currentSize;

            var center = _originalBoundingBox.Center;
            var xform = Transform.Scale(center, requiredScale);

            doc.Objects.Transform(_objectId, xform, true);
            doc.Views.Redraw();
        }

    }
}