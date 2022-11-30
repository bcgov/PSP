// customize CRA configuration, add compression to js.
const CompressionPlugin = require('compression-webpack-plugin'); //gzip

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
    return config;
  },
  // The Jest config to use when running your jest tests
  jest: function override(config) {
    config.transformIgnorePatterns = ['node_modules/(?!(react-leaflet|@react-leaflet/core)/)'];
    return config;
  },
};
