#!/usr/bin/env node

const output = require('../../../source/frontend/package.json').version;
process.stdout.write(output);
