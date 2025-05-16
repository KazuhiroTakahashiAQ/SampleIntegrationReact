using Eto.Forms;
using Newtonsoft.Json;
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

            e.Cancel = true;                    
            var method = e.Uri.Host;           

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

        public Task<string> CallJs(string method, object[] args)
        {
            var argList = string.Join(",", args.Select(ToJsLiteral));
            var script  = $"window.hostFunc.{method}({argList});";

            var res = _webView.ExecuteScriptAsync(script);

            return res;
        }

        string ToJsLiteral(object v) => v switch
        {
            string  str => JsEscape(str),          // "text"
            bool    b   => b.ToString().ToLower(), // true / false
            byte or sbyte or short or ushort or int or uint or long or ulong or
            float or double or decimal => Convert.ToString(v, System.Globalization.CultureInfo.InvariantCulture),
            _ => JsonConvert.SerializeObject(v)    // fallback
        };

        string JsEscape(string s) => HttpUtility.JavaScriptStringEncode(s, addDoubleQuotes: true);
    }


}
