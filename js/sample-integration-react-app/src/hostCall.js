
  // C#実行用の汎用関数
  // method: 関数名, parames: パラメータ名と値のRecord
  export function callHost(
    method, // string
    params = {}, // Record<string, string | number | boolean>
  ) {
    const q = Object.entries(params)
      .map(([k, v]) => `${encodeURIComponent(k)}=${encodeURIComponent(String(v))}`)
      .join('&');
    window.location.href = `rhino://${method}${q ? `?${q}` : ''}`;
  }
