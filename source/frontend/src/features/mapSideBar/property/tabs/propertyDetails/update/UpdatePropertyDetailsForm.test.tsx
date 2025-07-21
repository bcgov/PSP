import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { Formik } from 'formik';
import { createMemoryHistory } from 'history';

import { getEmptyAddress } from '@/mocks/address.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { ApiGen_Concepts_Property } from '@/models/api/generated/ApiGen_Concepts_Property';
import { getEmptyBaseAudit, getEmptyProperty } from '@/models/defaultInitializers';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { render, RenderOptions, waitForEffects } from '@/utils/test-utils';
import { useTenant } from '@/tenants/useTenant';
import defaultTenant from '@/tenants/config/defaultTenant';

import { UpdatePropertyDetailsFormModel } from './models';
import { UpdatePropertyDetailsForm } from './UpdatePropertyDetailsForm';

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

const onSubmit = vi.fn();

const mockAxios = new MockAdapter(axios);
const fakeProperty: ApiGen_Concepts_Property = {
  ...getEmptyProperty(),
  id: 205,
  propertyType: {
    id: 'TITLED',
    description: 'Titled',
    isDisabled: false,
    displayOrder: null,
  },
  anomalies: [
    {
      id: 1,
      propertyId: 205,
      propertyAnomalyTypeCode: {
        id: 'ACCESS',
        description: 'Access',
        isDisabled: false,
        displayOrder: null,
      },
      ...getEmptyBaseAudit(17),
    },
    {
      id: 3,
      propertyId: 205,
      propertyAnomalyTypeCode: {
        id: 'DISTURB',
        description: 'Disturbance',
        isDisabled: false,
        displayOrder: null,
      },
      ...getEmptyBaseAudit(12),
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
        displayOrder: null,
      },
      ...getEmptyBaseAudit(16),
    },
    {
      id: 454,
      propertyId: 205,
      propertyTenureTypeCode: {
        id: 'ADJLAND',
        description: 'Adjacent Land type',
        isDisabled: false,
        displayOrder: null,
      },
      ...getEmptyBaseAudit(16),
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
        displayOrder: null,
      },
      ...getEmptyBaseAudit(16),
    },
    {
      id: 3,
      propertyId: 205,
      propertyRoadTypeCode: {
        id: 'OIC',
        description: 'Order in Council (OIC)',
        isDisabled: false,
        displayOrder: null,
      },
      ...getEmptyBaseAudit(13),
    },
  ],
  status: {
    id: 'MOTIADMIN',
    description: 'Under MoTI administration',
    isDisabled: false,
    displayOrder: null,
  },
  dataSource: {
    id: 'PAIMS',
    description: 'Property Acquisition and Inventory Management System (PAIMS)',
    isDisabled: false,
    displayOrder: null,
  },
  region: {
    id: 1,
    description: 'South Coast Region',
    isDisabled: false,
    displayOrder: null,
  },
  district: {
    id: 1,
    description: 'Lower Mainland District',
    isDisabled: false,
    displayOrder: null,
  },
  dataSourceEffectiveDateOnly: '2021-08-31T00:00:00',
  latitude: 925866.6022023489,
  longitude: 1406876.1727310908,
  pphStatusUpdateTimestamp: '2020-05-10T20:00',
  pphStatusUpdateUserGuid: 'A85F259B-FEBF-4508-87A6-1C2419036EFA',
  pphStatusUpdateUserid: 'USER',
  isRwyBeltDomPatent: false,
  pphStatusTypeCode: 'Non-PPH',
  address: {
    ...getEmptyAddress(),
    id: 1,
    streetAddress1: '45 - 904 Hollywood Crescent',
    streetAddress2: 'Living in a van',
    streetAddress3: 'Down by the River',
    municipality: 'Hollywood North',
    provinceStateId: 1,
    province: {
      id: 1,
      code: 'BC',
      description: 'British Columbia',
      displayOrder: 10,
    },
    countryId: 1,
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
    displayOrder: null,
  },
  landArea: 25000,
  isVolumetricParcel: true,
  volumetricMeasurement: 1000,
  volumetricUnit: {
    id: 'M3',
    description: 'Cubic Meters',
    isDisabled: false,
    displayOrder: null,
  },
  volumetricType: {
    id: 'SUBSURF',
    description: 'Sub-surface',
    isDisabled: false,
    displayOrder: null,
  },
  municipalZoning: 'Zoning # 1',
  location: {
    coordinate: {
      x: 1406876.1727310908,
      y: 925866.6022023489,
    },
  },
  rowVersion: 5,
};

vi.mock('@/tenants/useTenant');
const mockUseTenant = vi.mocked(useTenant);

describe('UpdatePropertyDetailsForm component', () => {
  // render component under test
  const setup = async (
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
    mockUseTenant.mockReturnValue(defaultTenant);
  });

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('renders as expected', async () => {
    const { asFragment } = await setup({ initialValues });
    expect(asFragment()).toMatchSnapshot();
  });

  it('shows property address if available', async () => {
    const { container } = await setup({ initialValues });
    const addressLine1 = container.querySelector(`input[name='address.streetAddress1']`);
    const province = container.querySelector(`select[name='address.provinceStateId']`);

    expect(addressLine1).toHaveValue('45 - 904 Hollywood Crescent');
    expect(province).toHaveValue('1');
  });

  it('province defaults to BC when null', async () => {
    mockUseTenant.mockReturnValue({ ...defaultTenant, provinceStateId: 2 });

    initialValues.address.provinceStateId = null;
    const { container } = await setup({ initialValues });
    waitForEffects();

    const province = container.querySelector(`select[name='address.provinceStateId']`);
    expect(province).toHaveValue('2');
  });

  it('province defaults to BC when null no tenant', async () => {
    mockUseTenant.mockReturnValue({ ...defaultTenant, provinceStateId: null });

    initialValues.address.provinceStateId = null;
    const { container } = await setup({ initialValues });
    await waitForEffects();

    const province = container.querySelector(`select[name='address.provinceStateId']`);
    expect(province).toHaveValue('1');
  });

  it('province defaults to BC when null no tenant', async () => {
    mockUseTenant.mockReturnValue({ ...defaultTenant, provinceStateId: null });

    initialValues.address.provinceStateId = null;
    const { container } = await setup({ initialValues });
    await waitForEffects();

    const province = container.querySelector(`select[name='address.provinceStateId']`);
    expect(province).toHaveValue('1');
  });
});
