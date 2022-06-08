import { createMemoryHistory } from 'history';
import { useQueryMapLayersByLocation } from 'hooks/useQueryMapLayersByLocation';
import { mockLookups } from 'mocks/mockLookups';
import { Api_Property } from 'models/api/Property';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { fillInput, render, RenderOptions, userEvent, waitFor } from 'utils/test-utils';

import { useGetProperty, useUpdateProperty } from '../hooks';
import { UpdatePropertyDetailsFormModel } from './models';
import {
  IUpdatePropertyDetailsContainerProps,
  UpdatePropertyDetailsContainer,
} from './UpdatePropertyDetailsContainer';

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

// Mock API service calls
jest.mock('../hooks');
jest.mock('hooks/useQueryMapLayersByLocation');

const mockUseGetProperty: jest.Mocked<ReturnType<typeof useGetProperty>> = {
  retrieveProperty: jest.fn(),
  retrievePropertyLoading: false,
};
(useGetProperty as jest.Mock).mockReturnValue(mockUseGetProperty);

const mockUseUpdateProperty: jest.Mocked<ReturnType<typeof useUpdateProperty>> = {
  updateProperty: jest.fn(),
  updatePropertyLoading: false,
};
(useUpdateProperty as jest.Mock).mockReturnValue(mockUseUpdateProperty);

const mockUseQueryMapLayers: jest.Mocked<ReturnType<typeof useQueryMapLayersByLocation>> = {
  queryAll: jest.fn(),
};
(useQueryMapLayersByLocation as jest.Mock).mockReturnValue(mockUseQueryMapLayers);

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
        description: 'Highway/Road',
        isDisabled: false,
      },
      rowVersion: 16,
    },
    {
      id: 454,
      propertyId: 205,
      propertyTenureTypeCode: {
        id: 'ADJLAND',
        description: 'Adjacent Land',
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
  pid: 1234,
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

describe('UpdatePropertyDetailsContainer component', () => {
  // render component under test
  const setup = (renderOptions: RenderOptions & IUpdatePropertyDetailsContainerProps = {}) => {
    const utils = render(<UpdatePropertyDetailsContainer pid={renderOptions.pid || '1234'} />, {
      ...renderOptions,
      store: storeState,
      history,
    });

    return {
      ...utils,
      getSaveButton: () => utils.getByText(/Save/i),
      getCancelButton: () => utils.getByText(/Cancel/i),
    };
  };

  let initialValues: UpdatePropertyDetailsFormModel;

  beforeEach(() => {
    history.push('/mapview/property/1234/edit');
    initialValues = UpdatePropertyDetailsFormModel.fromApi(fakeProperty);
    mockUseGetProperty.retrieveProperty.mockResolvedValue(fakeProperty);
    mockUseUpdateProperty.updateProperty.mockResolvedValue(fakeProperty);
    mockUseQueryMapLayers.queryAll.mockResolvedValue({
      isALR: false,
      motiRegion: {},
      highwaysDistrict: {},
      electoralDistrict: {},
      firstNations: {},
    });
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  it('renders as expected', async () => {
    const { asFragment, findByText } = setup();
    expect(await findByText(/Titled/i)).toBeInTheDocument();
    expect(asFragment()).toMatchSnapshot();
  });

  it('cancels the form when Cancel is clicked', async () => {
    const { getCancelButton, findByText } = setup();
    expect(await findByText(/Titled/i)).toBeInTheDocument();
    userEvent.click(getCancelButton());

    await waitFor(() => expect(history.location.pathname).toBe('/mapview/property/1234'));
  });

  it('saves the form with minimal data when Save is clicked', async () => {
    const expected = initialValues.toApi();
    const { getSaveButton, findByText } = setup();
    expect(await findByText(/Titled/i)).toBeInTheDocument();
    userEvent.click(getSaveButton());

    await waitFor(() => expect(mockUseUpdateProperty.updateProperty).toBeCalledWith(expected));
  });

  it('saves the form with updated values when Save is clicked', async () => {
    const expected = Object.assign(new UpdatePropertyDetailsFormModel(), initialValues);
    expected.municipalZoning = 'Lorem ipsum';
    expected.notes = 'Lorem ipsum dolor sit amet, consectetur adipiscing elit.';

    const { getSaveButton, findByText, container } = setup();
    expect(await findByText(/Titled/i)).toBeInTheDocument();

    // modify form values
    await fillInput(container, 'municipalZoning', expected.municipalZoning);
    await fillInput(container, 'notes', expected.notes, 'textarea');
    userEvent.click(getSaveButton());

    await waitFor(() =>
      expect(mockUseUpdateProperty.updateProperty).toBeCalledWith(expected.toApi()),
    );
  });
});
