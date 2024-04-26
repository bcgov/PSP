import { createMemoryHistory } from 'history';

import { Claims, PropertyTenureTypes } from '@/constants/index';
import { getEmptyAddress } from '@/mocks/address.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { ApiGen_Concepts_Property } from '@/models/api/generated/ApiGen_Concepts_Property';
import { getEmptyBaseAudit, getEmptyProperty } from '@/models/defaultInitializers';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { toTypeCodeNullable } from '@/utils/formUtils';
import { RenderOptions, act, render } from '@/utils/test-utils';

import { useApiProperties } from '@/hooks/pims-api/useApiProperties';
import { useApiPropertyOperation } from '@/hooks/pims-api/useApiPropertyOperation';
import { PropertyDetailsTabView } from './PropertyDetailsTabView';
import { toFormValues } from './PropertyDetailsTabView.helpers';
import { usePimsPropertyRepository } from '@/hooks/repositories/usePimsPropertyRepository';
import { IResponseWrapper } from '@/hooks/util/useApiRequestWrapper';
import { AxiosResponse } from 'axios';

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

// mock keycloak auth library

const mockGetProperty = vi.fn();
vi.mock('@/hooks/repositories/usePimsPropertyRepository');
vi.mocked(usePimsPropertyRepository).mockReturnValue({
  getPropertyWrapper: { execute: mockGetProperty } as unknown as IResponseWrapper<
    (id: number) => Promise<AxiosResponse<ApiGen_Concepts_Property, any>>
  >,
} as unknown as ReturnType<typeof usePimsPropertyRepository>);

vi.mock('@/hooks/pims-api/useApiPropertyOperation');
const getPropertyOperationsApiMock = vi.fn();
vi.mocked(useApiPropertyOperation).mockImplementation(
  () =>
    ({
      getPropertyOperationsApi: getPropertyOperationsApiMock,
    } as unknown as ReturnType<typeof useApiPropertyOperation>),
);

vi.mock('@/hooks/pims-api/useApiProperties');
const getPropertyConceptWithIdApiMock = vi.fn();
vi.mocked(useApiProperties).mockImplementation(
  () =>
    ({
      getPropertyConceptWithIdApi: getPropertyConceptWithIdApiMock,
    } as unknown as ReturnType<typeof useApiProperties>),
);

describe('PropertyDetailsTabView component', () => {
  // render component under test
  const setup = (renderOptions: RenderOptions & { property?: ApiGen_Concepts_Property } = {}) => {
    const { property, ...rest } = renderOptions;
    const formValues = toFormValues(property);
    const component = render(<PropertyDetailsTabView property={formValues} loading={false} />, {
      ...rest,
      store: storeState,
      useMockAuthentication: true,
      claims: renderOptions?.claims ?? [],
      history,
    });

    return { ...component };
  };

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('renders as expected when provided valid data object', async () => {
    const { asFragment } = setup({ property: mockPropertyInfo });
    await act(async () => {});
    expect(asFragment()).toMatchSnapshot();
  });

  it('does not throw an exception for an invalid data object', async () => {
    const { getByText } = setup({ property: {} as ApiGen_Concepts_Property });
    await act(async () => {});
    expect(getByText(/property attributes/i)).toBeVisible();
  });

  it('shows highway/road multi-select when tenure status is Highway/Road', async () => {
    const property: ApiGen_Concepts_Property = {
      ...mockPropertyInfo,
      tenures: [
        {
          propertyTenureTypeCode: toTypeCodeNullable(PropertyTenureTypes.HighwayRoad),
          id: 0,
          propertyId: mockPropertyInfo.id,
          ...getEmptyBaseAudit(),
        },
      ],
    };

    const { getByText } = setup({ property });
    await act(async () => {});
    expect(getByText('Highway / Road Details:')).toBeVisible();
  });

  it('does not show highway/road multi-select when tenure status is not Highway/Road', async () => {
    const property: ApiGen_Concepts_Property = {
      ...mockPropertyInfo,
      tenures: [
        {
          propertyTenureTypeCode: toTypeCodeNullable(PropertyTenureTypes.Unknown),
          id: 0,
          propertyId: mockPropertyInfo.id,
          ...getEmptyBaseAudit(),
        },
      ],
    };

    const { queryByText } = setup({ property });
    await act(async () => {});
    expect(queryByText('Highway / Road Details:')).toBeNull();
  });

  it('shows first nations information when tenure status is Indian Reserve', async () => {
    const property: ApiGen_Concepts_Property = {
      ...mockPropertyInfo,
      tenures: [
        {
          propertyTenureTypeCode: toTypeCodeNullable(PropertyTenureTypes.IndianReserve),
          id: 0,
          propertyId: mockPropertyInfo.id,
          ...getEmptyBaseAudit(),
        },
      ],
    };

    const { getByText } = setup({ property });
    await act(async () => {});
    expect(getByText(/First Nations Information/i)).toBeVisible();
  });

  it('does not show first nations information when tenure status is not Indian Reserve', async () => {
    const property: ApiGen_Concepts_Property = {
      ...mockPropertyInfo,
    };

    const { queryByText } = setup({ property });
    await act(async () => {});
    expect(queryByText(/First Nations Information/i)).toBeNull();
  });

  it('shows additional volume measurements for volumetric parcels', async () => {
    const property: ApiGen_Concepts_Property = {
      ...mockPropertyInfo,
      isVolumetricParcel: true,
    };

    const { getByText } = setup({ property });
    await act(async () => {});
    expect(getByText(/Volume/)).toBeVisible();
  });

  it('shows Provincial public hwy field', async () => {
    const property: ApiGen_Concepts_Property = {
      ...mockPropertyInfo,
      pphStatusTypeCode: 'NONPPH',
    };

    const { getByText } = setup({ property });
    await act(async () => {});
    expect(getByText(/Non-Provincial Public Highway/i)).toBeVisible();
  });

  it('does not show shows additional volume measurements for non-volumetric parcels', async () => {
    const property: ApiGen_Concepts_Property = {
      ...mockPropertyInfo,
      isVolumetricParcel: false,
    };

    const { queryByText } = setup({ property });
    await act(async () => {});
    expect(queryByText(/Volume/)).toBeNull();
  });

  it('shows property address if available', async () => {
    const property: ApiGen_Concepts_Property = {
      ...mockPropertyInfo,
    };

    const { getByText } = setup({ property });
    await act(async () => {});
    expect(getByText(/456 Souris Street/i)).toBeVisible();
  });

  it('shows a warning message if no address found', async () => {
    const property: ApiGen_Concepts_Property = {
      ...mockPropertyInfo,
    };
    property.address = null;

    const { getByText } = setup({ property });
    await act(async () => {});
    expect(getByText(/Property address not available/i)).toBeVisible();
  });

  it('should display the Edit button if the user has permissions', async () => {
    const property: ApiGen_Concepts_Property = {
      ...mockPropertyInfo,
    };
    const { getByTitle, queryByTestId } = setup({ property, claims: [Claims.PROPERTY_EDIT] });
    await act(async () => {});
    expect(getByTitle(/Edit property details/)).toBeVisible();
    expect(queryByTestId('tooltip-icon-property-retired-tooltip')).toBeNull();
  });

  it('should not display the Edit button if the user does not have permissions', async () => {
    const property: ApiGen_Concepts_Property = {
      ...mockPropertyInfo,
    };
    const { queryByTitle } = await setup({ property, claims: [] });
    await act(async () => {});
    expect(queryByTitle(/Edit property details/)).toBeNull();
  });

  it('should render the retired tooltip instead of the Edit button for retired properties', async () => {
    const property: ApiGen_Concepts_Property = {
      ...mockPropertyInfo,
      isRetired: true,
    };
    const { queryByTitle, getByTestId } = await setup({ property, claims: [Claims.PROPERTY_EDIT] });
    await act(async () => {});
    expect(queryByTitle(/Edit property details/)).toBeNull();
    expect(getByTestId('tooltip-icon-property-retired-tooltip')).toBeInTheDocument();
  });
});

export const mockPropertyInfo: ApiGen_Concepts_Property = {
  ...getEmptyProperty(),
  id: 1,
  propertyType: {
    id: 'TITLED',
    description: 'Titled',
    isDisabled: false,
    displayOrder: null,
  },
  anomalies: [
    {
      propertyAnomalyTypeCode: {
        id: 'BLDGLIENS',
        description: 'Building liens',
        isDisabled: false,
        displayOrder: null,
      },
      id: 0,
      propertyId: 1,
      ...getEmptyBaseAudit(),
    },
  ],
  tenures: [
    {
      propertyTenureTypeCode: {
        id: 'CLOSEDRD',
        description: 'Closed Road',
        isDisabled: false,
        displayOrder: null,
      },
      id: 0,
      propertyId: 1,
      ...getEmptyBaseAudit(),
    },
  ],
  roadTypes: [
    {
      propertyRoadTypeCode: {
        id: 'GAZSURVD',
        description: 'Gazetted (Surveyed)',
        isDisabled: false,
        displayOrder: null,
      },
      id: 0,
      propertyId: 0,
      ...getEmptyBaseAudit(),
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
  dataSourceEffectiveDateOnly: '2021-08-31T00:00:00',
  latitude: 1088851.4995,
  longitude: 924033.5004,
  isSensitive: false,
  isRetired: false,
  address: {
    ...getEmptyAddress(),
    id: 204,
    streetAddress1: '456 Souris Street',
    streetAddress2: 'PO Box 250',
    streetAddress3: 'A Hoot and a holler from the A&W',
    municipality: 'North Podunk',
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
    postal: 'IH8 B0B',
    rowVersion: 1,
  },
  pid: 7723385,
  pin: 90069930,
  areaUnit: {
    id: 'HA',
    description: 'Hectare',
    isDisabled: false,
    displayOrder: null,
  },
  landArea: 1,
  isVolumetricParcel: false,
  volumetricMeasurement: 250000,
  volumetricUnit: {
    id: 'M3',
    description: 'Cubic Meters',
    isDisabled: false,
    displayOrder: null,
  },
  volumetricType: {
    id: 'AIRSPACE',
    description: 'Airspace',
    isDisabled: false,
    displayOrder: null,
  },
  municipalZoning: 'Some municipal zoning comments',
  zoning: 'Lorem ipsum',
  notes:
    'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam porttitor nisl at elit vestibulum vestibulum. Nullam eget consectetur felis, id porta eros. Proin at massa rutrum, molestie lorem a, congue lorem.',
  rowVersion: 16,
};
