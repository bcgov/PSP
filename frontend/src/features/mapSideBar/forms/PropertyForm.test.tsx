import { render, screen } from '@testing-library/react';
import { ADMINISTRATIVE_AREA_CODE_SET_NAME, PROVINCE_CODE_SET_NAME } from 'constants/API';
import { enableFetchMocks } from 'jest-fetch-mock';
import React from 'react';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';
import leafletMouseSlice from 'store/slices/leafletMouse/LeafletMouseSlice';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { propertiesSlice } from 'store/slices/properties';
import { fillInput } from 'utils/test-utils';
import TestCommonWrapper from 'utils/TestCommonWrapper';

import PropertyForm from './PropertyForm';

enableFetchMocks();

const mockStore = configureMockStore([thunk]);
const defaultStore = mockStore({
  [leafletMouseSlice.name]: {
    mapClickEvent: {
      originalEvent: { timeStamp: 1 },
      latlng: { lat: 1, lng: 2 },
    },
  },
  [lookupCodesSlice.name]: {
    lookupCodes: [
      { name: 'BC', code: 'BC', id: '1', isDisabled: false, type: PROVINCE_CODE_SET_NAME },
      {
        name: 'Victoria',
        id: '1',
        isDisabled: false,
        type: ADMINISTRATIVE_AREA_CODE_SET_NAME,
      },
    ],
  },
  [propertiesSlice.name]: { parcels: [], draftProperties: [] },
});

describe('Property Form functionality', () => {
  const renderContainer = ({ store }: any) =>
    render(
      <TestCommonWrapper store={store ?? defaultStore} organizations={[1 as any]}>
        <PropertyForm formikRef={{} as any} />
      </TestCommonWrapper>,
    );
  it('reformats postal code into expected format', async () => {
    fetchMock.mockResponse(JSON.stringify({ status: 200, body: {} }));
    const { container } = renderContainer({});
    fillInput(container, 'address.postal', 'V8V1V1');
    expect(await screen.findByDisplayValue('V8V 1V1')).toBeInTheDocument();
  });
});
