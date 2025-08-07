module.exports = {
  default: {
    require: [
      "features/**/*.js",
      "support/hooks.js",
      "./custom-world.js", // ⬅️ Add this line
    ],
    requireModule: ["dotenv/config"], // ⬅️ Add this line to load .env
    paths: ["features/**/*.feature"],
    format: ["progress"],
    publishQuiet: true,
  },
};
