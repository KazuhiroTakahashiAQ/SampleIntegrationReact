using Eto.Forms;
using Rhino;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SampleIntegrationReact
{
    public class WebViewAdapter
    {

        private WebView _webView;
        public WebView WebView => _webView;

        // Reactから呼び出す関数を登録しておく辞書
        private readonly Dictionary<string, Action<NameValueCollection>> _handlers = new(StringComparer.OrdinalIgnoreCase);

        public WebViewAdapter()
        {
            var serverUrl = "http://localhost:3000";

            _webView = new WebView
            {
                Url = new Uri(serverUrl),
            };

            _webView.DocumentLoading += _webView_DocumentLoading;
        }

        public void Register(string method, Action<NameValueCollection> handler) => _handlers[method] = handler;

        private void _webView_DocumentLoading(object _, WebViewLoadingEventArgs e)
        {
            if (e.Uri.Scheme != "rhino") return;

            e.Cancel = true;                                // 表示はキャンセル
            var method = e.Uri.Host;                        // openFile 等

            var query = HttpUtility.ParseQueryString(e.Uri.Query);

            if (_handlers.TryGetValue(method, out var h))
            {
                h(query);
            }
            else
            {
                RhinoApp.WriteLine($"Unknown host call: {method}");
            }
        }

        public Task<string> ExecuteJs()
        {

        }

    }
}
