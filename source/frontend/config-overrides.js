// customize CRA configuration, add compression to js.
const CompressionPlugin = require('compression-webpack-plugin'); //gzip
const path = require('path');

const addCompressionPlugin = config => {
  if (process.env.NODE_ENV === 'production') {
    config.devtool = false;
    config.plugins.push(
      new CompressionPlugin({
        //gzip plugin
        test: /\.(js|css|html|svg)$/,
      }),
    );
  }
  return config;
};

module.exports = {
  // The Webpack config to use when compiling your react app for development or production.
  webpack: function override(config) {
    const fallback = config.resolve.fallback || {};

    config.resolve.fallback = fallback;
    addCompressionPlugin(config);
    config.ignoreWarnings = [/Failed to parse source map/, /autoprefixer/];
    config.resolve.alias = {
      ...config.resolve.alias,
      '@': path.resolve(__dirname, 'src'),
    };
    return config;
  },
  // The Jest config to use when running your jest tests
  jest: function override(config) {
    config.transformIgnorePatterns = ['node_modules/(?!(react-leaflet|@react-leaflet/core)/)'];
    return config;
  },

  devServer: function (configFunction) {
    return function (proxy, allowedHost) {
      // Create the default config by calling configFunction with the proxy/allowedHost parameters
      const config = configFunction(proxy, allowedHost);
      config.headers = {
        'Content-Security-Policy':
          "base-uri 'self'; \
          default-src 'self'; \
          script-src 'self'; \
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
      };
      return config;
    };
  },
};
