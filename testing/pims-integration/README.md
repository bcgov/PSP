# PIMS Integration Playwright Tests

This folder contains the Playwright end-to-end test suite for the PIMS integration project.

## Overview

- `playwright.config.ts` defines the test configuration, including parallel execution, reporting, and browser projects.
- Tests are organized under `tests/` with a setup project and separate suites for smoke and end-to-end tests.
- Environment variables are loaded from `.env` using `dotenv`.

## Prerequisites

- Node.js 18+ (or compatible version supporting ES2020 and Playwright)
- npm
- A browser environment available for Playwright
- Local access to the target `BASE_URL` defined in `.env`

## Install dependencies

```bash
cd testing/pims-integration
npm install
```

## Environment setup

The repository does not include a committed `.env` file. Create a local `.env` file in this folder with the required variables:

```env
BASE_URL=
USER_TRANPSP1_ID=
USER_TRANPSP1_PASSWORD=
USER_TRANPSP1_FULLNAME=
USER_TRANPSP1_EMAIL=
```

At a minimum, configure:

- `BASE_URL` – the application URL to test
- `USER_TRANPSP1_ID` / `USER_TRANPSP1_PASSWORD` – credentials for login
- `USER_TRANPSP1_FULLNAME` / `USER_TRANPSP1_EMAIL` – user identity values

> Do not commit secrets to version control. If you need a different environment, create a separate `.env.local` or managed secret file.

## Test structure

- `tests/auth.setup.ts` – setup project for authentication or shared initialization logic.
- `tests/smoke-tests/` – lighter smoke tests validating core flows.
- `tests/e2e/` – full end-to-end scenarios.

The configured projects in `playwright.config.ts` include:

- `setup` – runs `tests/auth.setup.ts`
- `smoke-test` – runs tests in `tests/smoke-tests/`
- `pims-e2e` – runs tests in `tests/e2e/`

Both `smoke-test` and `pims-e2e` depend on `setup`, ensuring shared setup logic runs first.

## Running tests

Run the full Playwright suite:

```bash
npx playwright test
```

Run only smoke tests:

```bash
npx playwright test --project=smoke-test
```

Run only end-to-end tests:

```bash
npx playwright test --project=pims-e2e
```

Run the setup project by itself:

```bash
npx playwright test --project=setup
```

## Viewing reports

After a test run, open the HTML report:

```bash
npx playwright show-report
```

## Code quality

- `npm run lint` – run ESLint over `.ts` and `.js` files
- `npm run lint:fix` – fix lint issues automatically where possible
- `npm run format` – format source files with Prettier
- `npm run format:check` – verify formatting without changing files

## Notes

- The Playwright config uses `trace: 'on-first-retry'`, so tracing is collected only for retry attempts.
- `playwright.config.ts` reads environment variables from `.env` before launching tests.
- If your test environment requires a running local app, start it before executing Playwright.
