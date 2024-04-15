import axios, { AxiosResponse } from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { FormikProps } from 'formik';
import { createMemoryHistory } from 'history';
import { createRef } from 'react';

import { usePimsPropertyRepository } from '@/hooks/repositories/usePimsPropertyRepository';
import {
  IMapLayerResults,
  useQueryMapLayersByLocation,
} from '@/hooks/repositories/useQueryMapLayersByLocation';
import { getEmptyAddress } from '@/mocks/address.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { ApiGen_Concepts_Address } from '@/models/api/generated/ApiGen_Concepts_Address';
import { ApiGen_Concepts_CodeType } from '@/models/api/generated/ApiGen_Concepts_CodeType';
import { ApiGen_Concepts_Property } from '@/models/api/generated/ApiGen_Concepts_Property';
import { getEmptyBaseAudit, getEmptyProperty } from '@/models/defaultInitializers';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, render, RenderOptions, userEvent, waitFor } from '@/utils/test-utils';

import { UpdatePropertyDetailsFormModel } from './models';
import {
  IUpdatePropertyDetailsContainerProps,
  UpdatePropertyDetailsContainer,
} from './UpdatePropertyDetailsContainer';
import { IResponseWrapper } from '@/hooks/util/useApiRequestWrapper';
import { LatLngLiteral } from 'leaflet';

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

const onSuccess = vi.fn();

const DEFAULT_PROPS: IUpdatePropertyDetailsContainerProps = {
  id: 205,
  onSuccess,
};

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
  isSensitive: false,
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
  notes: 'I edited this Test notes for this property - again',
  rowVersion: 5,
};

// Mock API service calls
vi.mock('@/hooks/repositories/usePimsPropertyRepository');
vi.mock('@/hooks/repositories/useQueryMapLayersByLocation');

const getProperty = vi.fn(() => ({ ...fakeProperty }));
const updateProperty = vi.fn(() => ({ ...fakeProperty }));
vi.mocked(usePimsPropertyRepository).mockReturnValue({
  getPropertyWrapper: {
    execute: getProperty as unknown as (id: number) => Promise<ApiGen_Concepts_Property>,
    loading: false,
  } as IResponseWrapper<(id: number) => Promise<AxiosResponse<ApiGen_Concepts_Property, any>>>,
  updatePropertyWrapper: {
    execute: updateProperty as unknown as (
      property: ApiGen_Concepts_Property,
    ) => Promise<ApiGen_Concepts_Property>,
    updatePropertyLoading: false,
  } as unknown as IResponseWrapper<
    (property: ApiGen_Concepts_Property) => Promise<AxiosResponse<ApiGen_Concepts_Property, any>>
  >,
} as ReturnType<typeof usePimsPropertyRepository>);

vi.mocked(useQueryMapLayersByLocation).mockReturnValue({
  queryAll: vi.fn(() => ({
    isALR: null,
    motiRegion: null,
    highwaysDistrict: null,
    electoralDistrict: null,
    firstNations: null,
  })) as unknown as (location: LatLngLiteral) => Promise<IMapLayerResults>,
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
    vi.clearAllMocks();
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

    const expectedValues = expect.objectContaining<Partial<ApiGen_Concepts_Property>>({
      address: expect.objectContaining<Partial<ApiGen_Concepts_Address>>({
        streetAddress1: fakeProperty.address?.streetAddress1,
        streetAddress2: fakeProperty.address?.streetAddress2,
        streetAddress3: fakeProperty.address?.streetAddress3,
        municipality: fakeProperty.address?.municipality,
        postal: fakeProperty.address?.postal,
        country: expect.objectContaining<Partial<ApiGen_Concepts_CodeType>>({
          id: fakeProperty.address!.country!.id,
        }),
        province: expect.objectContaining<Partial<ApiGen_Concepts_CodeType>>({
          id: fakeProperty.address!.province!.id,
        }),
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
    await act(async () => {
      await act(async () => userEvent.clear(addressLine1));
      await act(async () => userEvent.paste(addressLine1, '123 Mock St'));
      formikRef.current?.submitForm();
    });

    await waitFor(() => {
      const expectedValues = expect.objectContaining<Partial<ApiGen_Concepts_Property>>({
        address: expect.objectContaining<Partial<ApiGen_Concepts_Address>>({
          streetAddress1: '123 Mock St',
          streetAddress2: fakeProperty.address?.streetAddress2,
          streetAddress3: fakeProperty.address?.streetAddress3,
          municipality: fakeProperty.address?.municipality,
          postal: fakeProperty.address?.postal,
          country: expect.objectContaining<Partial<ApiGen_Concepts_CodeType>>({
            id: fakeProperty.address!.country!.id,
          }),
          province: expect.objectContaining<Partial<ApiGen_Concepts_CodeType>>({
            id: fakeProperty.address!.province!.id,
          }),
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

    await act(async () => {
      await act(async () => userEvent.clear(addressLine1));
      await act(async () => userEvent.clear(addressLine2));
      await act(async () => userEvent.clear(addressLine3));
      await act(async () => userEvent.clear(municipality));
      await act(async () => userEvent.clear(postal));
      formikRef.current?.submitForm() as Promise<void>;
    });

    await waitFor(() => {
      const expectedValues = expect.objectContaining<Partial<ApiGen_Concepts_Property>>({
        address: null,
      });

      expect(updateProperty).toBeCalledWith(expectedValues);
      expect(onSuccess).toBeCalled();
    });
  });
});
