import { rest } from 'msw';

import {
  getMockAddresses,
  getMockDescription,
  getMockLandChars,
  getMockLegalDescriptions,
  getMockSales,
  getMockValues,
} from '@/mocks/bcAssessment.mock';

export const handlers = [
  rest.get('https://delivery.apps.gov.bc.ca/ext/sgw/geo.bca', (req, res, ctx) => {
    const search = req.url.search;
    if (search.includes('BCA_FOLIO_ADDRESSES_SV')) {
      return res(ctx.delay(500), ctx.status(200), ctx.json(getMockAddresses()));
    } else if (search.includes('BCA_FOLIO_LEGAL_DESCRIPTS_SV')) {
      return res(ctx.delay(5000), ctx.status(200), ctx.json(getMockLegalDescriptions()));
    } else if (search.includes('BCA_FOLIO_GNRL_PROP_VALUES_SV')) {
      return res(ctx.delay(500), ctx.status(200), ctx.json(getMockValues()));
    } else if (search.includes('BCA_FOLIO_SALES_SV')) {
      return res(ctx.delay(500), ctx.status(200), ctx.json(getMockSales()));
    } else if (search.includes('BCA_FOLIO_DESCRIPTIONS_SV')) {
      return res(ctx.delay(500), ctx.status(200), ctx.json(getMockDescription()));
    } else if (search.includes('BCA_FOLIO_LAND_CHARS_SV')) {
      return res(ctx.delay(500), ctx.status(200), ctx.json(getMockLandChars()));
    }
  }),
];
