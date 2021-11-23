#!/usr/bin/env node

const output = require('../frontend/package.json').version;
process.stdout.write(output);
