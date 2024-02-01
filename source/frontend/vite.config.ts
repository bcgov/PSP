import react from '@vitejs/plugin-react';
import path from 'path';
import { defineConfig } from 'vite';
import eslint from 'vite-plugin-eslint';
import svgr from 'vite-plugin-svgr';
import viteTsconfigPaths from 'vite-tsconfig-paths';

// https://vitejs.dev/config/
export default defineConfig({
  resolve: {
    alias: [{ find: '@', replacement: path.resolve(__dirname, 'src') }],
  },
  server: {
    open: true,
    port: 3000,
    headers: {
      Pragma: '"no-cache"',
      Expires: '"0"',
    },
    proxy: {
      '/api': {
        target: 'http://localhost:5000/',
        changeOrigin: true,
        rewrite: (path: string) => path,
        xfwd: true,
        cookiePathRewrite: '/',
        cookieDomainRewrite: '',
      },
      '/ogs-internal': {
        target: 'http://localhost:5000/api/geoserver',
        changeOrigin: true,
        rewrite: (path: string) => path.replace(/^\/ogs-internal/, '/'),
        xfwd: true,
        cookiePathRewrite: '/',
        cookieDomainRewrite: '',
      },
    },
  },
  build: {
    outDir: 'build',
  },
  plugins: [
    react(),
    eslint(),
    viteTsconfigPaths(),
    svgr({
      include: '**/*.svg?react',
    }),
  ],
});
