import { http, HttpResponse } from 'msw';

import {
  getMockAddresses,
  getMockDescription,
  getMockLandChars,
  getMockLegalDescriptions,
  getMockSales,
  getMockValues,
} from '@/mocks/bcAssessment.mock';

import { mockDistrictLayerResponse } from '../districtLayerResponse.mock';
import { mockMotiRegionLayerResponse } from '../index.mock';

export const handlers = [
  http.get('https://delivery.apps.gov.bc.ca/ext/sgw/geo.bca', ({ request }) => {
    const search = new URL(request.url).search;
    if (search.includes('BCA_FOLIO_ADDRESSES_SV')) {
      return HttpResponse.json(getMockAddresses(), { status: 200 });
    } else if (search.includes('BCA_FOLIO_LEGAL_DESCRIPTS_SV')) {
      return HttpResponse.json(getMockLegalDescriptions(), { status: 200 });
    } else if (search.includes('BCA_FOLIO_GNRL_PROP_VALUES_SV')) {
      return HttpResponse.json(getMockValues(), { status: 200 });
    } else if (search.includes('BCA_FOLIO_SALES_SV')) {
      return HttpResponse.json(getMockSales(), { status: 200 });
    } else if (search.includes('BCA_FOLIO_DESCRIPTIONS_SV')) {
      return HttpResponse.json(getMockDescription(), { status: 200 });
    } else if (search.includes('BCA_FOLIO_LAND_CHARS_SV')) {
      return HttpResponse.json(getMockLandChars(), { status: 200 });
    }
  }),
  http.get('https://maps.th.gov.bc.ca/geoV05', ({ request }) => {
    const search = new URL(request.url).search;
    if (search.includes('hwy:DSA_REGION_BOUNDARY')) {
      return HttpResponse.json(mockMotiRegionLayerResponse, { status: 200 });
    } else if (search.includes('hwy:DSA_DISTRICT_BOUNDARY')) {
      return HttpResponse.json(mockDistrictLayerResponse, { status: 200 });
    }
  }),
];
