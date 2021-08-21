const fs = require('fs');
const os = require('os');

const packageJsonLoc = './frontend/package.json';
const csprojLoc = './backend/api/Pims.Api.csproj';

/**
 * USAGE:
 *
 * node ./build/bump-version.js [<new-version> | major | minor | patch | is | build] [--apply] [--release-version]
 *
 * To print the next version without releasing anything, add the '--release-version' flag.
 */

// parse command-line args
const args = process.argv.slice(2);
run(args);

////////////////////////// Helpers /////////////////////////////

function run(args) {
  const IS_DRY_RUN = !args.includes('--apply'); // default to dry-run
  const IS_RELEASE_VERSION = args.includes('--release-version');

  const releaseType = args[0];
  const packageJson = JSON.parse(fs.readFileSync(packageJsonLoc, 'utf8'));
  const version = packageJson.version;
  //   const version = require(packageJsonLoc).version;

  let [newVersion, assemblyVersion] = bumpVersion(releaseType, version);

  if (IS_RELEASE_VERSION) {
    console.info(newVersion);
    process.exit(0);
  }

  // bump version numbers
  if (!IS_DRY_RUN) {
    packageJson.version = newVersion;
    updateJson(packageJson);

    let csproj = fs.readFileSync(csprojLoc, 'utf8');
    let modifiedCsproj = csproj
      .replace(/<Version>(.+?)<\/Version>/gi, `<Version>${newVersion}</Version>`)
      .replace(
        /<AssemblyVersion>(.+?)<\/AssemblyVersion>/gi,
        `<AssemblyVersion>${assemblyVersion}</AssemblyVersion>`,
      );
    updateCsproj(modifiedCsproj);

    console.info(`    success`);
  }

  console.info(`==> Version bumped to: "${newVersion}"`);

  if (IS_DRY_RUN) {
    console.info('');
    console.info(`THIS WAS A DRY RUN, SO NO FILE WAS ACTUALLY CHANGED`);
    console.info(`Run this again with '--apply' to execute the changes`);
  }
}

function updateJson(packageJson) {
  console.info(`==> Applying changes to ${packageJsonLoc}`);
  fs.writeFileSync(packageJsonLoc, `${JSON.stringify(packageJson, undefined, 2)}${os.EOL}`, 'utf8');
}

function updateCsproj(csproj) {
  console.info(`==> Applying changes to ${csprojLoc}`);
  fs.writeFileSync(csprojLoc, `${csproj}${os.EOL}`, 'utf8');
}

// version format: <major>.<minor>.<patch>-<IS_number>.<build>
// e.g. 0.2.0-7.3
function bumpVersion(releaseType, version) {
  let major, minor, patch, is, build;

  if (version) {
    const [semVer, metadata] = version.split('-');
    [major, minor, patch] = semVer.split('.');
    [is, build] = metadata.split('.');

    switch (releaseType) {
      case 'major':
        major = parseInt(major, 10) + 1;
        minor = 0;
        patch = 0;
        build = 0;
        break;

      case 'minor':
        minor = parseInt(minor, 10) + 1;
        patch = 0;
        build = 0;
        break;

      case 'patch':
        patch = parseInt(patch, 10) + 1;
        build = 0;
        break;

      case 'is':
        is = parseInt(is, 10) + 1;
        build = 0;
        break;

      default:
        build = parseInt(build, 10) + 1;
    }
  } else {
    // default
    major = 0;
    minor = 1;
    patch = 0;
    is = 1;
    build = 0;
  }

  let newVersion = `${major}.${minor}.${patch}-${is}.${build}`;
  let assemblyVersion = `${major}.${minor}.${patch}.${is}`;

  return [newVersion, assemblyVersion];
}
