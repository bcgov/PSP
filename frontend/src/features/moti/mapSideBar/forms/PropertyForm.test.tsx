import { render, screen } from '@testing-library/react';
import {
  ADMINISTRATIVE_AREA_CODE_SET_NAME,
  MOTI_CLASSIFICATION_CODE_SET_NAME,
  MOTI_REGION_CODE_SET_NAME,
  PROVINCE_CODE_SET_NAME,
  PURPOSE_CODE_SET_NAME,
  RURAL_AREA_CODE_SET_NAME,
} from 'constants/API';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';
import leafletMouseSlice from 'store/slices/leafletMouse/LeafletMouseSlice';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { propertiesSlice } from 'store/slices/properties';
import TestCommonWrapper from 'utils/TestCommonWrapper';
import { fillInput } from 'utils/testUtils';

import PropertyForm from './PropertyForm';

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
      {
        name: 'rural',
        id: '1',
        isDisabled: false,
        type: RURAL_AREA_CODE_SET_NAME,
      },
      {
        name: 'classification',
        id: '1',
        isDisabled: false,
        type: MOTI_CLASSIFICATION_CODE_SET_NAME,
      },
      {
        name: 'region',
        id: '1',
        isDisabled: false,
        type: MOTI_REGION_CODE_SET_NAME,
      },
      {
        name: 'purpose',
        id: '1',
        isDisabled: false,
        type: PURPOSE_CODE_SET_NAME,
      },
    ],
  },
  [propertiesSlice.name]: { parcels: [], draftParcels: [] },
});
describe('Property Form functionality', () => {
  const renderContainer = ({ store }: any) =>
    render(
      <TestCommonWrapper store={store ?? defaultStore} agencies={[1 as any]}>
        <PropertyForm formikRef={{} as any} />
      </TestCommonWrapper>,
    );
  it('reformats postal code into expected format', async () => {
    const { container } = renderContainer({});
    await fillInput(container, 'address.postal', 'V8V1V1');
    expect(screen.getByDisplayValue('V8V 1V1')).toBeInTheDocument();
  });
});
