module.exports = {
  default: {
    require: ["support/world.js", "support/hooks.js", "features/**/*.steps.js"],
    requireModule: ["dotenv/config"],
    paths: ["features/**/*.feature"],
    format: ["progress"],
    publishQuiet: true,
  },
};
