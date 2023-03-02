import {
  getMockAddresses,
  getMockLandChars,
  getMockLegalDescriptions,
  getMockValues,
} from 'mocks/bcAssessmentMock';
// src/mocks.js
// 1. Import the library.
import { rest, setupWorker } from 'msw';

import { getMockDescription, getMockSales } from './bcAssessmentMock';

// 2. Describe network behavior with request handlers.
export const worker = setupWorker(
  rest.get('https://delivery.apps.gov.bc.ca/ext/sgw/geo.bca', (req, res, ctx) => {
    if (req.url.search.includes('BCA_FOLIO_ADDRESSES_SV')) {
      return res(ctx.delay(500), ctx.status(200), ctx.json(getMockAddresses()));
    } else if (req.url.search.includes('BCA_FOLIO_LEGAL_DESCRIPTS_SV')) {
      return res(ctx.delay(500), ctx.status(200), ctx.json(getMockLegalDescriptions()));
    } else if (req.url.search.includes('BCA_FOLIO_GNRL_PROP_VALUES_SV')) {
      return res(ctx.delay(500), ctx.status(200), ctx.json(getMockValues()));
    } else if (req.url.search.includes('BCA_FOLIO_SALES_SV')) {
      return res(ctx.delay(500), ctx.status(200), ctx.json(getMockSales()));
    } else if (req.url.search.includes('BCA_FOLIO_DESCRIPTIONS_SV')) {
      return res(ctx.delay(500), ctx.status(200), ctx.json(getMockDescription()));
    } else if (req.url.search.includes('BCA_FOLIO_LAND_CHARS_SV')) {
      return res(ctx.delay(500), ctx.status(200), ctx.json(getMockLandChars()));
    }
  }),
);
