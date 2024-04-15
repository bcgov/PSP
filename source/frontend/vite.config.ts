/// <reference types="vitest" />
import react from '@vitejs/plugin-react';
import path from 'path';
import { defineConfig } from 'vite';
import viteCompression from 'vite-plugin-compression';
import eslint from 'vite-plugin-eslint';
import svgr from 'vite-plugin-svgr';
import viteTsconfigPaths from 'vite-tsconfig-paths';

// https://vitejs.dev/config/
export default defineConfig({
  test: {
    setupFiles: ['./src/setupTests.ts'],
    environment: 'jsdom',
    coverage: {
      reporter: [['lcov'], ['text'], ['json', { file: 'coverage-final.json' }]],
      include: ['src/**/*.{js,jsx,ts,tsx}'],
      exclude: [
        'node_modules/**',
        'test-config',
        'src/interfaces/**',
        'jestGlobalMocks.ts',
        '*.module.ts',
        '<rootDir>/src/index.tsx',
        '*.mock.ts',
        '*.ignore.ts',
        'msw.ts',
        'setupProxy.js',
      ],
    },
    outputFile: 'coverage/sonar-report.xml',
    globals: true,
    testTimeout: 10000,
    reporters: ['default', ['vitest-sonar-reporter', { outputFile: 'test-report.xml' }]],
    poolOptions: {
      threads: {
        useAtomics: true,
        isolate: false,
      },
    },
    deps: {
      optimizer: {
        web: {
          enabled: true,
        },
      },
    },
    pool: 'vmThreads',
    maxConcurrency: 8,
  },
  resolve: {
    alias: [{ find: '@', replacement: path.resolve(__dirname, 'src') }],
  },
  server: {
    open: true,
    port: 3000,
    headers: {
      'Content-Security-Policy':
        "base-uri 'self'; \
         default-src 'self'; \
         script-src 'self' 'sha256-8ZgGo/nOlaDknQkDUYiedLuFRSGJwIz6LAzsOrNxhmU='; \
         connect-src 'self' https://maps.gov.bc.ca/arcgis/rest/ https://server.arcgisonline.com/ArcGIS/rest/ https://dev.loginproxy.gov.bc.ca/ https://dev-pims.th.gov.bc.ca/api/ https://openmaps.gov.bc.ca/ https://delivery.apps.gov.bc.ca/; \
         img-src 'self' data: blob: https://openmaps.gov.bc.ca/ https://maps.gov.bc.ca/ https://server.arcgisonline.com/; \
         style-src 'self' 'unsafe-inline'; \
         form-action 'self'; \
         font-src 'self'; \
         frame-src 'self' https://dev.loginproxy.gov.bc.ca/; \
         frame-ancestors 'self'; \
         ",
      'Strict-Transport-Security': ' "max-age=86400; includeSubDomains"',
      'X-Content-Type-Options': ' "nosniff"',
      'X-XSS-Protection': '1',
      'X-Frame-Options': 'DENY',
      'Cache-Control': '"no-cache, no-store, must-revalidate"',
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
    viteCompression({
      filter: /\.(js|mjs|css|html)$/i,
    }),
  ],
});
