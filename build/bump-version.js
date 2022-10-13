const fs = require('fs');
const os = require('os');

const packageJsonLoc = './source/frontend/package.json';
const csprojLoc = './source/backend/api/Pims.Api.csproj';

/**
 * A script for automated version bumps. It requires Node.js to be installed on the server.
 * The script will default to a DRY RUN to avoid unintended changes; run with '--apply' to execute the changes.
 *
 * NOTE:
 * To print the next version without releasing anything, add the '--release-version' flag.
 *
 * USAGE:
 * node ./build/bump-version.js [<new-version> | --major | --minor | --patch | --is | --build] [--apply] [--print-version]
 *
 */

// parse command-line args
const args = process.argv.slice(2);
run(args);

////////////////////////// Helpers /////////////////////////////

function run(args) {
  const IS_DRY_RUN = !args.includes('--apply'); // default to dry-run
  const IS_PRINT_VERSION = args.includes('--print-next-version');

  const releaseType = args[0];
  const packageJson = JSON.parse(fs.readFileSync(packageJsonLoc, 'utf8'));
  const version = packageJson.version;

  let [newVersion, assemblyVersion] = bumpVersion(releaseType, version);

  if (IS_PRINT_VERSION) {
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
  fs.writeFileSync(csprojLoc, csproj, 'utf8');
}

// version format: <major>.<minor>.<patch>-<IS_number>.<build>
// e.g. 0.2.0-7.3
function isValidVersion(version) {
  if (!version) {
    return false;
  }
  return /^(0|[1-9]\d*)\.(0|[1-9]\d*)\.(0|[1-9]\d*)-(0|[1-9]\d*)\.(0|[1-9]\d*)$/.test(version);
}

function parse(version) {
  const [semVer, metadata] = version.split('-');
  const [major, minor, patch] = semVer?.split('.');
  const [isNumber, build] = metadata?.split('.');

  return [major, minor, patch, isNumber, build];
}

function bumpVersion(releaseType, version) {
  let major, minor, patch, isNumber, build;

  // Support setting version to a fixed value; e.g. "0.1.0-8.5"
  if (isValidVersion(releaseType)) {
    [major, minor, patch, isNumber, build] = parse(releaseType);
  } else if (version) {
    [major, minor, patch, isNumber, build] = parse(version);

    switch (releaseType) {
      case '--major':
        major = parseInt(major, 10) + 1;
        minor = 0;
        patch = 0;
        build = 0;
        break;

      case '--minor':
        minor = parseInt(minor, 10) + 1;
        patch = 0;
        build = 0;
        break;

      case '--patch':
        patch = parseInt(patch, 10) + 1;
        build = 0;
        break;

      case '--is':
        isNumber = parseInt(isNumber, 10) + 1;
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
    isNumber = 1;
    build = 0;
  }

  let newVersion = `${major}.${minor}.${patch}-${isNumber}.${build}`;
  let assemblyVersion = `${major}.${minor}.${patch}.${isNumber}`;

  return [newVersion, assemblyVersion];
}
