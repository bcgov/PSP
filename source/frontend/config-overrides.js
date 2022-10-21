// customize CRA configuration, add compression to js.
const CompressionPlugin = require('compression-webpack-plugin'); //gzip

const addCompressionPlugin = (config) => {
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

module.exports = function override(config) {
  const fallback = config.resolve.fallback || {};

  config.resolve.fallback = fallback;
  addCompressionPlugin(config);
  config.ignoreWarnings = [/Failed to parse source map/, /autoprefixer/];
  return config;
};
