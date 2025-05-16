/// <reference types="vite/client" />

interface Window {
  hostFunc: Record<string, (...args: any[]) => any>;
}
