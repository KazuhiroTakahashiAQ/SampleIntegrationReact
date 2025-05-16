/**
 * C#実行用の汎用関数
 * @param method 呼び出すC#側のメソッド名
 * @param params メソッドに渡すパラメータ
 */
export function callHost(
  method: string,
  params: Record<string, string | number | boolean> = {},
): void {
  // URLクエリパラメータに変換
  const queryParams = Object.entries(params)
    .map(([key, value]) => `${encodeURIComponent(key)}=${encodeURIComponent(String(value))}`)
    .join('&');
  
  // rhino:// プロトコルでC#側に通信
  window.location.href = `rhino://${method}${queryParams ? `?${queryParams}` : ''}`;
}
