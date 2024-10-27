using System;
using System.Threading.Tasks;
using Rhino;
using Rhino.PlugIns;
using Rhino.UI;
using Eto.Forms;
using System.Runtime.InteropServices;

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

        public MyPanel()
        {
            // ローカルサーバーのURLを指定します。
            var serverUrl = "http://localhost:3000";

            // WebViewを初期化します。
            _webView = new WebView
            {
                Url = new Uri(serverUrl),
            };

            // WebViewをパネルのコンテンツに設定します。
            Content = _webView;
        }


        // C#からJavaScriptへデータを送信
        public async Task SendTextToJs(string text)
        {
            var script = $"window.setText('{text}');";
            await _webView.ExecuteScriptAsync(script);
        }

        // JavaScriptからC#へデータを取得
        public async Task<string> GetTextFromJs()
        {
            var script = "return window.returnText();";
            var result = await _webView.ExecuteScriptAsync(script);
            return result;
        }
    }
}