// tsで書く時宣言
// declare global {
//   interface Window {
//     _hostFuncs?: Record<string, (...a: any[]) => any>;
//   }
// }

// グローバル関数を定義してC#からデータを受け取る
// name: 関数名, fn: 関数
export function registerHostFunc(
  name, // string,
  fn // (...args: any[]) => string?,
) {
  window.hostFunc[name] = fn;
}

// 専用名前空間
export const init = () => {
  window.hostFunc ??= {}  
}