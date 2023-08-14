import { createMemoryHistory } from 'history';

import { Claims, PropertyTenureTypes } from '@/constants/index';
import { mockLookups } from '@/mocks/lookups.mock';
import { Api_Property } from '@/models/api/Property';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { render, RenderOptions } from '@/utils/test-utils';

import { PropertyDetailsTabView } from './PropertyDetailsTabView';
import { toFormValues } from './PropertyDetailsTabView.helpers';

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

// mock keycloak auth library
jest.mock('@react-keycloak/web');

describe('PropertyDetailsTabView component', () => {
  // render component under test
  const setup = (renderOptions: RenderOptions & { property?: Api_Property } = {}) => {
    const { property, ...rest } = renderOptions;
    const formValues = toFormValues(property);
    const component = render(<PropertyDetailsTabView property={formValues} loading={false} />, {
      ...rest,
      store: storeState,
      claims: [Claims.PROPERTY_EDIT],
      history,
    });

    return {
      ...component,
    };
  };

  it('renders as expected when provided valid data object', () => {
    const { asFragment } = setup({ property: mockPropertyInfo });
    expect(asFragment()).toMatchSnapshot();
  });

  it('does not throw an exception for an invalid data object', () => {
    const { getByText } = setup({ property: {} as Api_Property });
    expect(getByText(/property attributes/i)).toBeVisible();
  });

  it('shows highway/road multi-select when tenure status is Highway/Road', () => {
    const property: Api_Property = {
      ...mockPropertyInfo,
      tenures: [{ propertyTenureTypeCode: { id: PropertyTenureTypes.HighwayRoad } }],
    };

    const { getByText } = setup({ property });
    expect(getByText('Highway / Road Details:')).toBeVisible();
  });

  it('does not show highway/road multi-select when tenure status is not Highway/Road', () => {
    const property: Api_Property = {
      ...mockPropertyInfo,
      tenures: [{ propertyTenureTypeCode: { id: PropertyTenureTypes.Unknown } }],
    };

    const { queryByText } = setup({ property });
    expect(queryByText('Highway / Road Details:')).toBeNull();
  });

  it('shows first nations information when tenure status is Indian Reserve', () => {
    const property: Api_Property = {
      ...mockPropertyInfo,
      tenures: [{ propertyTenureTypeCode: { id: PropertyTenureTypes.IndianReserve } }],
    };

    const { getByText } = setup({ property });
    expect(getByText(/First Nations Information/i)).toBeVisible();
  });

  it('does not show first nations information when tenure status is not Indian Reserve', () => {
    const property: Api_Property = {
      ...mockPropertyInfo,
    };

    const { queryByText } = setup({ property });
    expect(queryByText(/First Nations Information/i)).toBeNull();
  });

  it('shows additional volume measurements for volumetric parcels', () => {
    const property: Api_Property = {
      ...mockPropertyInfo,
      isVolumetricParcel: true,
    };

    const { getByText } = setup({ property });
    expect(getByText(/Volume/)).toBeVisible();
  });

  it('shows Provincial public hwy field', () => {
    const property: Api_Property = {
      ...mockPropertyInfo,
      pphStatusTypeCode: 'NONPPH',
    };

    const { getByText } = setup({ property });
    expect(getByText(/Non-Provincial Public Highway/i)).toBeVisible();
  });

  it('does not show shows additional volume measurements for non-volumetric parcels', () => {
    const property: Api_Property = {
      ...mockPropertyInfo,
      isVolumetricParcel: false,
    };

    const { queryByText } = setup({ property });
    expect(queryByText(/Volume/)).toBeNull();
  });

  it('shows property address if available', () => {
    const property: Api_Property = {
      ...mockPropertyInfo,
    };

    const { getByText } = setup({ property });
    expect(getByText(/456 Souris Street/i)).toBeVisible();
  });

  it('shows a warning message if no address found', () => {
    const property: Api_Property = {
      ...mockPropertyInfo,
    };
    property.address = undefined;

    const { getByText } = setup({ property });
    expect(getByText(/Property address not available/i)).toBeVisible();
  });
});

export const mockPropertyInfo: Api_Property = {
  id: 1,
  propertyType: {
    id: 'TITLED',
    description: 'Titled',
    isDisabled: false,
  },
  anomalies: [
    {
      propertyAnomalyTypeCode: {
        id: 'BLDGLIENS',
        description: 'Building liens',
        isDisabled: false,
      },
    },
  ],
  tenures: [
    {
      propertyTenureTypeCode: {
        id: 'CLOSEDRD',
        description: 'Closed Road',
        isDisabled: false,
      },
    },
  ],
  roadTypes: [
    {
      propertyRoadTypeCode: {
        id: 'GAZSURVD',
        description: 'Gazetted (Surveyed)',
        isDisabled: false,
      },
    },
  ],
  adjacentLands: [
    {
      propertyAdjacentLandTypeCode: {
        id: 'PRIVATE',
        description: 'Private (Fee Simple)',
        isDisabled: false,
      },
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
  dataSourceEffectiveDate: '2021-08-31T00:00:00',
  latitude: 1088851.4995,
  longitude: 924033.5004,
  isSensitive: false,
  address: {
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
  },
  landArea: 1,
  isVolumetricParcel: false,
  volumetricMeasurement: 250000,
  volumetricUnit: {
    id: 'M3',
    description: 'Cubic Meters',
    isDisabled: false,
  },
  volumetricType: {
    id: 'AIRSPACE',
    description: 'Airspace',
    isDisabled: false,
  },
  municipalZoning: 'Some municipal zoning comments',
  zoning: 'Lorem ipsum',
  notes:
    'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nam porttitor nisl at elit vestibulum vestibulum. Nullam eget consectetur felis, id porta eros. Proin at massa rutrum, molestie lorem a, congue lorem.',
  rowVersion: 16,
};
