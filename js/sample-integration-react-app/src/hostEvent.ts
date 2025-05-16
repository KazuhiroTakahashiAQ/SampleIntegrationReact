// グローバル関数を定義してC#からデータを受け取る
type HostFunction = (...args: any[]) => void;

/**
 * C#側から呼び出し可能な関数を登録します
 * @param name 関数名
 * @param fn 実行される関数
 */
export function registerHostFunc(
  name: string,
  fn: HostFunction,
): void {
  window.hostFunc[name] = fn;
}

/**
 * C#との通信用のグローバル名前空間を初期化します
 */
export const init = (): void => {
  if (!window.hostFunc) {
    window.hostFunc = {};
  }
}
