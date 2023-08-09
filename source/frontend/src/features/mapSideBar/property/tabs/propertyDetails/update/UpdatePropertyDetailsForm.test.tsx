import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { Formik } from 'formik';
import { createMemoryHistory } from 'history';

import { mockLookups } from '@/mocks/lookups.mock';
import { Api_Property } from '@/models/api/Property';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { render, RenderOptions } from '@/utils/test-utils';

import { UpdatePropertyDetailsFormModel } from './models';
import { UpdatePropertyDetailsForm } from './UpdatePropertyDetailsForm';

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

const onSubmit = jest.fn();

const mockAxios = new MockAdapter(axios);
const fakeProperty: Api_Property = {
  id: 205,
  propertyType: {
    id: 'TITLED',
    description: 'Titled',
    isDisabled: false,
  },
  anomalies: [
    {
      id: 1,
      propertyId: 205,
      propertyAnomalyTypeCode: {
        id: 'ACCESS',
        description: 'Access',
        isDisabled: false,
      },
      rowVersion: 17,
    },
    {
      id: 3,
      propertyId: 205,
      propertyAnomalyTypeCode: {
        id: 'DISTURB',
        description: 'Disturbance',
        isDisabled: false,
      },
      rowVersion: 12,
    },
  ],
  tenures: [
    {
      id: 453,
      propertyId: 205,
      propertyTenureTypeCode: {
        id: 'HWYROAD',
        description: 'Highway/Road established by',
        isDisabled: false,
      },
      rowVersion: 16,
    },
    {
      id: 454,
      propertyId: 205,
      propertyTenureTypeCode: {
        id: 'ADJLAND',
        description: 'Adjacent Land type',
        isDisabled: false,
      },
      rowVersion: 16,
    },
  ],
  roadTypes: [
    {
      id: 1,
      propertyId: 205,
      propertyRoadTypeCode: {
        id: 'CTRLACC',
        description: 'Controlled Access',
        isDisabled: false,
      },
      rowVersion: 16,
    },
    {
      id: 3,
      propertyId: 205,
      propertyRoadTypeCode: {
        id: 'OIC',
        description: 'Order in Council (OIC)',
        isDisabled: false,
      },
      rowVersion: 13,
    },
  ],
  adjacentLands: [
    {
      id: 1,
      propertyId: 205,
      propertyAdjacentLandTypeCode: {
        id: 'INDIANR',
        description: 'Indian Reserve (IR)',
        isDisabled: false,
      },
      rowVersion: 16,
    },
  ],
  status: {
    id: 'MOTIADMIN',
    description: 'Under MoTI administration',
    isDisabled: false,
  },
  dataSource: {
    id: 'PAIMS',
    description: 'Property Acquisition and Inventory Management System (PAIMS)',
    isDisabled: false,
  },
  region: {
    id: 1,
    description: 'South Coast Region',
    isDisabled: false,
  },
  district: {
    id: 1,
    description: 'Lower Mainland District',
    isDisabled: false,
  },
  dataSourceEffectiveDate: '2021-08-31T00:00:00',
  latitude: 925866.6022023489,
  longitude: 1406876.1727310908,
  isSensitive: false,
  pphStatusUpdateTimestamp: new Date('2020-05-10T20:00'),
  pphStatusUpdateUserGuid: 'A85F259B-FEBF-4508-87A6-1C2419036EFA',
  pphStatusUpdateUserid: 'USER',
  isRwyBeltDomPatent: false,
  pphStatusTypeCode: 'Non-PPH',

  address: {
    id: 1,
    streetAddress1: '45 - 904 Hollywood Crescent',
    streetAddress2: 'Living in a van',
    streetAddress3: 'Down by the River',
    municipality: 'Hollywood North',
    province: {
      id: 1,
      code: 'BC',
      description: 'British Columbia',
      displayOrder: 10,
    },
    country: {
      id: 1,
      code: 'CA',
      description: 'Canada',
      displayOrder: 1,
    },
    postal: 'V6Z 5G7',
    rowVersion: 2,
  },
  pid: 9956727,
  pin: 5498101,
  areaUnit: {
    id: 'M2',
    description: 'Meters sq',
    isDisabled: false,
  },
  landArea: 25000,
  isVolumetricParcel: true,
  volumetricMeasurement: 1000,
  volumetricUnit: {
    id: 'M3',
    description: 'Cubic Meters',
    isDisabled: false,
  },
  volumetricType: {
    id: 'SUBSURF',
    description: 'Sub-surface',
    isDisabled: false,
  },
  municipalZoning: 'Zoning # 1',
  location: {
    coordinate: {
      x: 1406876.1727310908,
      y: 925866.6022023489,
    },
  },
  notes: 'I edited this Test notes for this property - again',
  rowVersion: 5,
};

describe('UpdatePropertyDetailsForm component', () => {
  // render component under test
  const setup = (
    renderOptions: RenderOptions & { initialValues: UpdatePropertyDetailsFormModel },
  ) => {
    const utils = render(
      <Formik onSubmit={onSubmit} initialValues={renderOptions.initialValues}>
        {formikProps => <UpdatePropertyDetailsForm formikProps={formikProps} />}
      </Formik>,
      {
        ...renderOptions,
        store: storeState,
        history,
      },
    );

    return {
      ...utils,
    };
  };

  let initialValues: UpdatePropertyDetailsFormModel;

  beforeEach(() => {
    mockAxios.onGet(new RegExp('users/info/*')).reply(200, {});
    initialValues = UpdatePropertyDetailsFormModel.fromApi(fakeProperty);
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  it('renders as expected', () => {
    const { asFragment } = setup({ initialValues });
    expect(asFragment()).toMatchSnapshot();
  });

  it('shows property address if available', () => {
    const { container } = setup({ initialValues });
    const addressLine1 = container.querySelector(`input[name='address.streetAddress1']`);
    expect(addressLine1).toHaveValue('45 - 904 Hollywood Crescent');
  });
});
