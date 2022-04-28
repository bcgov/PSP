import { PropertyAdjacentLandTypes, PropertyTenureTypes } from 'constants/index';
import { createMemoryHistory } from 'history';
import { IPropertyApiModel } from 'interfaces/IPropertyApiModel';
import { render, RenderOptions } from 'utils/test-utils';

import { PropertyDetailsTabView } from './PropertyDetailsTabView';
import { toFormValues } from './PropertyDetailsTabView.helpers';

const history = createMemoryHistory();

describe('PropertyDetailsTabView component', () => {
  // render component under test
  const setup = (renderOptions: RenderOptions & { property?: IPropertyApiModel } = {}) => {
    const { property, ...rest } = renderOptions;
    const formValues = toFormValues(property);
    const component = render(<PropertyDetailsTabView property={formValues} />, {
      ...rest,
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
    const { getByText } = setup({ property: {} as IPropertyApiModel });
    expect(getByText('Property attributes')).toBeVisible();
  });

  it('shows highway/road multi-select when tenure status is Highway/Road', () => {
    const property: IPropertyApiModel = {
      ...mockPropertyInfo,
      tenure: [{ id: PropertyTenureTypes.HighwayRoad }],
    };

    const { getByText } = setup({ property });
    expect(getByText('Highway / Road:')).toBeVisible();
  });

  it('does not show highway/road multi-select when tenure status is not Highway/Road', () => {
    const property: IPropertyApiModel = {
      ...mockPropertyInfo,
      tenure: [{ id: PropertyTenureTypes.Unknown }],
    };

    const { queryByText } = setup({ property });
    expect(queryByText('Highway / Road:')).toBeNull();
  });

  it('shows first nations information when adjacent land is Indian Reserve', () => {
    const property: IPropertyApiModel = {
      ...mockPropertyInfo,
      tenure: [{ id: PropertyTenureTypes.AdjacentLand }],
      adjacentLand: [{ id: PropertyAdjacentLandTypes.IndianReserve }],
    };

    const { getByText } = setup({ property });
    expect(getByText('First Nations Information')).toBeVisible();
  });

  it('does not show first nations information when adjacent land is not Indian Reserve', () => {
    const property: IPropertyApiModel = {
      ...mockPropertyInfo,
      tenure: [{ id: PropertyTenureTypes.AdjacentLand }],
      adjacentLand: [{ id: PropertyAdjacentLandTypes.Crown }],
    };

    const { queryByText } = setup({ property });
    expect(queryByText('First Nations Information')).toBeNull();
  });

  it('shows additional volume measurements for volumetric parcels', () => {
    const property: IPropertyApiModel = {
      ...mockPropertyInfo,
      isVolumetricParcel: true,
    };

    const { getByText } = setup({ property });
    expect(getByText('Volumetric measurement')).toBeVisible();
  });

  it('does not show shows additional volume measurements for non-volumetric parcels', () => {
    const property: IPropertyApiModel = {
      ...mockPropertyInfo,
      isVolumetricParcel: false,
    };

    const { queryByText } = setup({ property });
    expect(queryByText('Volumetric measurement')).toBeNull();
  });
});

export const mockPropertyInfo: IPropertyApiModel = {
  id: 1,
  propertyType: {
    id: 'TITLED',
    description: 'Titled',
    isDisabled: false,
  },
  anomalies: [
    {
      id: 'BLDGLIENS',
      description: 'Building liens',
      isDisabled: false,
    },
  ],
  tenure: [
    {
      id: 'CLOSEDRD',
      description: 'Closed Road',
      isDisabled: false,
    },
  ],
  roadType: [
    {
      id: 'GAZSURVD',
      description: 'Gazetted (Surveyed)',
      isDisabled: false,
    },
  ],
  adjacentLand: [
    {
      id: 'PRIVATE',
      description: 'Private (Fee Simple)',
      isDisabled: false,
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
  pid: '007-723-385',
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
  leases: [],
  rowVersion: 16,
};
