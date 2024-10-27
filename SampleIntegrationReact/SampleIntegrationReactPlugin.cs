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
    public class SampleIntegrationReactPlugin : Rhino.PlugIns.PlugIn
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
        private WebView _webView;
        private Guid _objectId; // 操作対象のオブジェクトID
        private BoundingBox _originalBoundingBox; // 元のオブジェクトのバウンディングボックス
        private double _originalSize; // 元のオブジェクトのサイズ（対角線の長さ）

        public MyPanel()
        {
            InitializeComponents();

            // Rhinoのイベントハンドラを設定
            RhinoDoc.SelectObjects += OnSelectObjects;
        }

        private void InitializeComponents()
        {
            var serverUrl = "http://localhost:3000";

            _webView = new WebView
            {
                Url = new Uri(serverUrl),
            };

            // DocumentTitleChangedイベントをハンドル
            _webView.DocumentTitleChanged += OnDocumentTitleChanged;

            Content = _webView;
        }

        // オブジェクト選択イベントハンドラ
        private void OnSelectObjects(object sender, RhinoObjectSelectionEventArgs e)
        {
            if (e.Selected)
            {
                // 最初に選択されたオブジェクトのIDとサイズを保存
                _objectId = e.RhinoObjects[0].Id;
                var obj = e.RhinoObjects[0];
                _originalBoundingBox = obj.Geometry.GetBoundingBox(true);
                _originalSize = _originalBoundingBox.Diagonal.Length;
            }
        }

        // DocumentTitleChangedイベントでサイズ変更を検知
        private void OnDocumentTitleChanged(object sender, WebViewTitleEventArgs e)
        {
            if (e.Title.StartsWith("size:"))
            {
                var sizeStr = e.Title.Substring(5);
                UpdateObjectSize(sizeStr);
            }
        }

        // オブジェクトのサイズを更新
        private void UpdateObjectSize(string sizeStr)
        {
            if (_objectId == Guid.Empty || _originalSize == 0) return;

            if (double.TryParse(sizeStr, out double scaleFactor))
            {
                var doc = RhinoDoc.ActiveDoc;
                var obj = doc.Objects.FindId(_objectId);
                if (obj != null)
                {
                    // 現在のバウンディングボックス
                    var currentBoundingBox = obj.Geometry.GetBoundingBox(true);
                    var currentSize = currentBoundingBox.Diagonal.Length;

                    // 必要なスケール倍率を計算
                    double requiredScale = (scaleFactor * _originalSize) / currentSize;

                    // スケーリングの変換行列を作成
                    var center = _originalBoundingBox.Center;
                    var xform = Transform.Scale(center, requiredScale);

                    // オブジェクトをスケーリング
                    doc.Objects.Transform(_objectId, xform, true);
                    doc.Views.Redraw();
                }
            }
        }
    }
}