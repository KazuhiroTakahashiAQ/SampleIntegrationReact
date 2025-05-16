# Rhino Integration React App

このプロジェクトは、Rhinoと統合するためのReactアプリケーションです。Vite + TypeScriptで構築されています。

## 機能

- RhinoのC#アプリケーションとWebViewを通じて通信
- リアクティブなUIコンポーネント
- オブジェクトのプロパティ表示と編集
- TypeScriptによる型安全なコード

## 技術スタック

- React 18
- TypeScript
- Vite
- Vitest (テスト)

## 開発環境のセットアップ

### 必要条件

- Node.js 16.x以上
- npm 7.x以上

### インストール

```bash
# 依存関係のインストール
npm install
```

## 利用可能なスクリプト

プロジェクトディレクトリで以下のコマンドを実行できます：

### `npm run dev`

開発モードでアプリを実行します。\
[http://localhost:5173](http://localhost:5173) を開いてブラウザで表示します。

コードを変更すると、ページは自動的にリロードされます。

### `npm run build`

本番用にアプリをビルドし、`build`フォルダに出力します。\
高いパフォーマンスのために最適化された本番モードでReactをバンドルします。

ビルドはミニファイされ、ファイル名にはハッシュが含まれます。\
アプリはデプロイする準備ができています！

### `npm run lint`

ESLintを使用してコードをリントします。

### `npm run test`

インタラクティブなウォッチモードでテストランナーを起動します。

### `npm run test:run`

すべてのテストを一度だけ実行します。

### `npm run coverage`

コードカバレッジレポートを生成します。

## Rhinoとの統合

このアプリケーションは、Rhinoと以下の方法で通信します：

1. **C#からJavaScriptへの呼び出し**: `window.hostFunc`名前空間に登録された関数を通じて、C#側からJavaScriptの関数を呼び出します。
2. **JavaScriptからC#への呼び出し**: `rhino://`プロトコルを使用して、JavaScriptからC#メソッドを呼び出します。

## プロジェクト構造

- `src/` - ソースコード
  - `App.tsx` - メインのReactコンポーネント
  - `hostCall.ts` - C#への呼び出しを処理するユーティリティ
  - `hostEvent.ts` - C#からの呼び出しを処理するユーティリティ
- `public/` - 静的なアセット
- `build/` - ビルド成果物

### `npm run eject`

**Note: this is a one-way operation. Once you `eject`, you can't go back!**

If you aren't satisfied with the build tool and configuration choices, you can `eject` at any time. This command will remove the single build dependency from your project.

Instead, it will copy all the configuration files and the transitive dependencies (webpack, Babel, ESLint, etc) right into your project so you have full control over them. All of the commands except `eject` will still work, but they will point to the copied scripts so you can tweak them. At this point you're on your own.

You don't have to ever use `eject`. The curated feature set is suitable for small and middle deployments, and you shouldn't feel obligated to use this feature. However we understand that this tool wouldn't be useful if you couldn't customize it when you are ready for it.

## Learn More

You can learn more in the [Create React App documentation](https://facebook.github.io/create-react-app/docs/getting-started).

To learn React, check out the [React documentation](https://reactjs.org/).

### Code Splitting

This section has moved here: [https://facebook.github.io/create-react-app/docs/code-splitting](https://facebook.github.io/create-react-app/docs/code-splitting)

### Analyzing the Bundle Size

This section has moved here: [https://facebook.github.io/create-react-app/docs/analyzing-the-bundle-size](https://facebook.github.io/create-react-app/docs/analyzing-the-bundle-size)

### Making a Progressive Web App

This section has moved here: [https://facebook.github.io/create-react-app/docs/making-a-progressive-web-app](https://facebook.github.io/create-react-app/docs/making-a-progressive-web-app)

### Advanced Configuration

This section has moved here: [https://facebook.github.io/create-react-app/docs/advanced-configuration](https://facebook.github.io/create-react-app/docs/advanced-configuration)

### Deployment

This section has moved here: [https://facebook.github.io/create-react-app/docs/deployment](https://facebook.github.io/create-react-app/docs/deployment)

### `npm run build` fails to minify

This section has moved here: [https://facebook.github.io/create-react-app/docs/troubleshooting#npm-run-build-fails-to-minify](https://facebook.github.io/create-react-app/docs/troubleshooting#npm-run-build-fails-to-minify)
