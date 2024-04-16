/* eslint-disable @typescript-eslint/no-var-requires */
const { createProxyMiddleware } = require('http-proxy-middleware');

module.exports = function (app) {
  app.use(
    '/api',
    createProxyMiddleware({
      target: process.env.API_URL || 'http://localhost:5000/',
      changeOrigin: true,
      secure: false,
      // timeout: 2000,
      xfwd: true,
      logLevel: 'debug',
      cookiePathRewrite: '/',
      cookieDomainRewrite: '',
      pathRewrite: function (path) {
        return path;
      },
      onProxyReq: function (proxyReq) {
        proxyReq.setHeader('x-powered-by', 'onProxyReq');
      },
    }),
  );
  app.use(
    '/ogs-internal',
    createProxyMiddleware({
      target: 'http://localhost:5000/api/geoserver',
      changeOrigin: true,
      secure: false,
      // timeout: 2000,
      xfwd: true,
      logLevel: 'debug',
      cookiePathRewrite: '/',
      cookieDomainRewrite: '',
      pathRewrite: {
        '^/ogs-internal/': '/', // remove base path
      },
      onProxyReq: function (proxyReq) {
        proxyReq.setHeader('x-powered-by', 'onProxyReq');
      },
    }),
  );
};
