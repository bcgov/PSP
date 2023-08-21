import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { FormikProps } from 'formik';
import { createMemoryHistory } from 'history';
import { createRef } from 'react';

import { usePimsPropertyRepository } from '@/hooks/repositories/usePimsPropertyRepository';
import {
  IMapLayerResults,
  useQueryMapLayersByLocation,
} from '@/hooks/repositories/useQueryMapLayersByLocation';
import { mockLookups } from '@/mocks/lookups.mock';
import { Api_Property } from '@/models/api/Property';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, render, RenderOptions, userEvent, waitFor } from '@/utils/test-utils';

import { UpdatePropertyDetailsFormModel } from './models';
import {
  IUpdatePropertyDetailsContainerProps,
  UpdatePropertyDetailsContainer,
} from './UpdatePropertyDetailsContainer';

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

const onSuccess = jest.fn();

const DEFAULT_PROPS: IUpdatePropertyDetailsContainerProps = {
  id: 205,
  onSuccess,
};

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

// Mock API service calls
jest.mock('@/hooks/repositories/usePimsPropertyRepository');
jest.mock('@/hooks/repositories/useQueryMapLayersByLocation');

const getProperty = jest.fn(() => ({ ...fakeProperty }));
const updateProperty = jest.fn(() => ({ ...fakeProperty }));
(usePimsPropertyRepository as jest.Mock).mockReturnValue({
  getPropertyWrapper: {
    execute: getProperty,
    loading: false,
  },
  updatePropertyWrapper: {
    execute: updateProperty,
    updatePropertyLoading: false,
  },
});

(useQueryMapLayersByLocation as jest.Mock).mockReturnValue({
  queryAll: jest.fn<IMapLayerResults, any[]>(() => ({
    isALR: null,
    motiRegion: null,
    highwaysDistrict: null,
    electoralDistrict: null,
    firstNations: null,
  })),
});

describe('UpdatePropertyDetailsContainer component', () => {
  // render component under test
  const setup = (
    props: IUpdatePropertyDetailsContainerProps = { ...DEFAULT_PROPS },
    renderOptions: RenderOptions = {},
  ) => {
    const formikRef = createRef<FormikProps<UpdatePropertyDetailsFormModel>>();
    const utils = render(<UpdatePropertyDetailsContainer ref={formikRef} {...props} />, {
      ...renderOptions,
      store: storeState,
      history,
    });

    return {
      ...utils,
      formikRef,
    };
  };

  beforeEach(() => {
    mockAxios.onGet(new RegExp('users/info/*')).reply(200, {});
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  it('renders as expected', async () => {
    const { asFragment, findByTitle } = setup();
    expect(await findByTitle('Down by the River')).toBeInTheDocument();
    expect(asFragment()).toMatchSnapshot();
  });

  it('saves the form with minimal data', async () => {
    const { findByTitle, formikRef } = setup();
    expect(await findByTitle('Down by the River')).toBeInTheDocument();
    await act(async () => formikRef.current?.submitForm() as Promise<void>);

    const expectedValues = expect.objectContaining<Api_Property>({
      address: expect.objectContaining({
        streetAddress1: fakeProperty.address?.streetAddress1,
        streetAddress2: fakeProperty.address?.streetAddress2,
        streetAddress3: fakeProperty.address?.streetAddress3,
        municipality: fakeProperty.address?.municipality,
        postal: fakeProperty.address?.postal,
        country: { id: fakeProperty.address?.country?.id },
        province: { id: fakeProperty.address?.province?.id },
      }),
    });

    expect(updateProperty).toBeCalledWith(expectedValues);
    expect(onSuccess).toBeCalled();
  });

  it('saves the form with updated values', async () => {
    const { findByTitle, formikRef } = setup();
    expect(await findByTitle('Down by the River')).toBeInTheDocument();

    const addressLine1 = document.querySelector(
      `input[name='address.streetAddress1']`,
    ) as HTMLElement;
    act(() => {
      userEvent.clear(addressLine1);
      userEvent.paste(addressLine1, '123 Mock St');
      formikRef.current?.submitForm();
    });

    await waitFor(() => {
      const expectedValues = expect.objectContaining<Api_Property>({
        address: expect.objectContaining({
          streetAddress1: '123 Mock St',
          streetAddress2: fakeProperty.address?.streetAddress2,
          streetAddress3: fakeProperty.address?.streetAddress3,
          municipality: fakeProperty.address?.municipality,
          postal: fakeProperty.address?.postal,
          country: { id: fakeProperty.address?.country?.id },
          province: { id: fakeProperty.address?.province?.id },
        }),
      });
      expect(updateProperty).toBeCalledWith(expectedValues);
      expect(onSuccess).toBeCalled();
    });
  });

  it('sends no address when all fields are cleared', async () => {
    const { findByTitle, formikRef } = setup();
    expect(await findByTitle('Down by the River')).toBeInTheDocument();

    const addressLine1 = document.querySelector(
      `input[name='address.streetAddress1']`,
    ) as HTMLElement;
    const addressLine2 = document.querySelector(
      `input[name='address.streetAddress2']`,
    ) as HTMLElement;
    const addressLine3 = document.querySelector(
      `input[name='address.streetAddress3']`,
    ) as HTMLElement;
    const municipality = document.querySelector(
      `input[name='address.municipality']`,
    ) as HTMLElement;
    const postal = document.querySelector(`input[name='address.postal']`) as HTMLElement;

    act(() => {
      userEvent.clear(addressLine1);
      userEvent.clear(addressLine2);
      userEvent.clear(addressLine3);
      userEvent.clear(municipality);
      userEvent.clear(postal);
      formikRef.current?.submitForm() as Promise<void>;
    });

    await waitFor(() => {
      const expectedValues = expect.objectContaining<Api_Property>({
        address: undefined,
      });

      expect(updateProperty).toBeCalledWith(expectedValues);
      expect(onSuccess).toBeCalled();
    });
  });
});
