import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { Formik } from 'formik';
import noop from 'lodash/noop';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';

import { useMapProperties } from '@/hooks/repositories/useMapProperties';
import { mockFAParcelLayerResponse, mockGeocoderOptions } from '@/mocks/index.mock';
import { mapMachineBaseMock } from '@/mocks/mapFSM.mock';
import { act, fillInput, render, RenderOptions, screen, userEvent } from '@/utils/test-utils';

import { PropertyForm } from '@/features/mapSideBar/shared/models';
import { IMapStateMachineContext } from '../common/mapFSM/MapStateMachineContext';
import MapSelectorContainer, { IMapSelectorContainerProps } from './MapSelectorContainer';
import { IMapProperty } from './models';

const mockStore = configureMockStore([thunk]);

const mockAxios = new MockAdapter(axios);

const store = mockStore({});

const onSelectedProperties = vi.fn();
const onRepositionSelectedProperty = vi.fn();

const testProperty: IMapProperty = {
  propertyId: 123,
  pid: '123-456-789',
  planNumber: 'SPS22411',
  address: 'Test address 123',
  legalDescription: 'Test Legal Description',
  region: 1,
  regionName: 'South Coast',
  district: 5,
  districtName: 'Okanagan-Shuswap',
};

vi.mock('@/hooks/repositories/useMapProperties');
vi.mocked(useMapProperties).mockReturnValue({
  loadProperties: {
    error: null,
    response: { features: [] } as any,
    execute: vi.fn().mockResolvedValue({
      features: [PropertyForm.fromMapProperty(testProperty).toFeatureDataset().pimsFeature],
    }),
    loading: false,
    status: 200,
  },
});

describe('MapSelectorContainer component', () => {
  const setup = (renderOptions: RenderOptions & Partial<IMapSelectorContainerProps>) => {
    // render component under test
    const utils = render(
      <Formik initialValues={{ properties: [] }} onSubmit={noop}>
        <MapSelectorContainer
          addSelectedProperties={onSelectedProperties}
          repositionSelectedProperty={onRepositionSelectedProperty}
          modifiedProperties={renderOptions.modifiedProperties ?? []}
        />
      </Formik>,
      {
        ...renderOptions,
        store: store,
        mockMapMachine: renderOptions.mockMapMachine ?? mapMachineBaseMock,
      },
    );

    return {
      store,
      ...utils,
    };
  };

  beforeEach(() => {
    mockAxios
      .onGet(new RegExp('typeNames=WHSE_CADASTRE.PMBC_PARCEL_FABRIC_POLY_FA_SVW'))
      .reply(200, mockFAParcelLayerResponse)
      .onGet(new RegExp('tools/geocoder/nearest*'))
      .reply(200, mockGeocoderOptions[0]);
  });

  it('renders as expected when provided no properties', async () => {
    const { asFragment } = setup({});
    await act(async () => {
      expect(asFragment()).toMatchSnapshot();
    });
  });

  it('displays two tabs', async () => {
    const { getByText } = setup({});
    await act(async () => {
      expect(getByText('Locate on Map')).toBeVisible();
    });
  });

  it('displays locate on Map by default', async () => {
    const { getByText } = setup({});
    await act(async () => {
      expect(getByText('Locate on Map')).toHaveClass('active');
    });
  });

  it('allows the search tab to be selected', async () => {
    const { getByText } = setup({});
    const searchTab = getByText('Search');
    await act(async () => userEvent.click(searchTab));
    expect(searchTab).toHaveClass('active');
  });

  it('displays all selected property attributes', async () => {
    const { getByText } = setup({
      modifiedProperties: [PropertyForm.fromMapProperty(testProperty).toFeatureDataset()],
    });
    await act(async () => {
      expect(getByText(/SPS22411/i, { exact: false })).toBeVisible();
      expect(getByText(/Test address 123/i, { exact: false })).toBeVisible();
      expect(getByText(/1 - South Coast/i, { exact: false })).toBeVisible();
      expect(getByText(/5 - Okanagan-Shuswap/i, { exact: false })).toBeVisible();
      expect(getByText(/Test Legal Description/i, { exact: false })).toBeVisible();
    });
  });

  it('selected properties display a warning if added', async () => {
    const { getByText, getByTitle, findByTestId, container } = setup({
      modifiedProperties: [],
    });

    const searchTab = getByText('Search');
    await act(async () => userEvent.click(searchTab));
    await act(async () => {
      fillInput(container, 'searchBy', 'pid', 'select');
    });
    await fillInput(container, 'pid', '123-456-789');
    const searchButton = getByTitle('search');
    await act(async () => userEvent.click(searchButton));
    const checkbox = await findByTestId(
      'selectrow-PID-009-727-493-48.76613749999999--123.46163749999998',
    );
    await act(async () => userEvent.click(checkbox));
    const addButton = getByText('Add to selection');
    await act(async () => userEvent.click(addButton));

    expect(onSelectedProperties).toHaveBeenCalledWith([
      {
        parcelFeature: {
          type: 'Feature',
          id: 'PMBC_PARCEL_POLYGON_FABRIC.551',
          geometry: {
            type: 'Polygon',
            coordinates: [
              [
                [-123.46, 48.767],
                [-123.4601, 48.7668],
                [-123.461, 48.7654],
                [-123.4623, 48.7652],
                [-123.4627, 48.7669],
                [-123.4602, 48.7672],
                [-123.4601, 48.7672],
                [-123.4601, 48.7672],
                [-123.46, 48.767],
              ],
            ],
          },
          properties: {
            GLOBAL_UID: '{4C4D758A-1261-44C1-8835-0FEB93150495}',
            PARCEL_NAME: '009727493',
            PLAN_ID: 3,
            PLAN_NUMBER: 'NO_PLAN',
            PIN: null,
            PID: '9727493',
            PID_FORMATTED: '009-727-493',
            PID_NUMBER: 9727493,
            SOURCE_PARCEL_ID: null,
            PARCEL_STATUS: 'ACTIVE',
            PARCEL_CLASS: 'SUBDIVISION',
            PARCEL_FABRIC_POLY_ID: 5156389,
            OWNER_TYPE: 'CROWN PROVINCIAL',
            PARCEL_START_DATE: null,
            SURVEY_DESIGNATION_1: 'PART 13',
            SURVEY_DESIGNATION_2: 'SOUTH SALT SPRING,1',
            SURVEY_DESIGNATION_3: '',
            LEGAL_DESCRIPTION:
              'THAT PART OF SECTION 13, RANGE 1, SOUTH SALT SPRING ISLAND, COWICHAN DISTRICT',
            MUNICIPALITY: '0163',
            REGIONAL_DISTRICT: '0005',
            IS_REMAINDER_IND: 'NO',
            GEOMETRY_SOURCE: 'MODIFIED-ICIS',
            POSITIONAL_ERROR: 2,
            ERROR_REPORTED_BY: 'DATACOMPILATION',
            CAPTURE_METHOD: 'UNKNOWN',
            COMPILED_IND: '1',
            STATED_AREA: null,
            WHEN_CREATED: '2016-01-06T17:44:42Z',
            WHEN_UPDATED: '2019-01-05T10:21:32Z',
            FEATURE_AREA_SQM: 29217,
            FEATURE_LENGTH_M: 702,
            OBJECTID: 551,
            SE_ANNO_CAD_DATA: null,
            SHAPE: null,
          },
          bbox: [-123.4627, 48.7652, -123.46, 48.7672],
        },
        selectingComponentId: null,
        pimsFeature: {
          geometry: null,
          properties: {
            STREET_ADDRESS_1: 'Test address 123',
            ADDRESS_ID: null,
            COUNTRY_CODE: null,
            COUNTRY_NAME: null,
            DISTRICT_CODE: 5,
            HAS_ACTIVE_ACQUISITION_FILE: null,
            HAS_ACTIVE_RESEARCH_FILE: null,
            HISTORICAL_FILE_NUMBER_STR: null,
            IS_ACTIVE_PAYABLE_LEASE: null,
            IS_ACTIVE_RECEIVABLE_LEASE: null,
            IS_DISPOSED: null,
            IS_OTHER_INTEREST: null,
            IS_OWNED: null,
            IS_PAYABLE_LEASE: null,
            IS_RECEIVABLE_LEASE: null,
            IS_RETIRED: undefined,
            LAND_AREA: undefined,
            LAND_LEGAL_DESCRIPTION: 'Test Legal Description',
            MUNICIPALITY_NAME: undefined,
            PID: 123456789,
            PID_PADDED: '123-456-789',
            PIN: null,
            POSTAL_CODE: undefined,
            PROPERTY_AREA_UNIT_TYPE_CODE: undefined,
            PROPERTY_DATA_SOURCE_EFFECTIVE_DATE: null,
            PROPERTY_DATA_SOURCE_TYPE_CODE: null,
            PROPERTY_ID: 123,
            PROPERTY_STATUS_TYPE_CODE: null,
            PROPERTY_TENURE_TYPE_CODE: null,
            PROPERTY_TYPE_CODE: null,
            PROVINCE_NAME: null,
            PROVINCE_STATE_CODE: null,
            REGION_CODE: 1,
            STREET_ADDRESS_2: undefined,
            STREET_ADDRESS_3: undefined,
            SURVEY_PLAN_NUMBER: 'SPS22411',
          },
          type: 'Feature',
        },

        location: {
          lat: 48.76613749999999,
          lng: -123.46163749999998,
        },
        regionFeature: null,
        districtFeature: null,
        municipalityFeature: null,
        fileLocation: null,
        id: 'PID-009-727-493-48.76613749999999--123.46163749999998',
      },
    ]);
  });

  it('selected properties display a warning if added multiple times', async () => {
    const { getByText, getByTitle, findByTestId, container } = setup({
      modifiedProperties: [
        PropertyForm.fromMapProperty({ ...testProperty, pid: '009-727-493' }).toFeatureDataset(),
      ],
    });

    const searchTab = getByText('Search');
    await act(async () => userEvent.click(searchTab));
    await act(async () => {
      fillInput(container, 'searchBy', 'pid', 'select');
    });
    await fillInput(container, 'pid', '123-456-789');
    const searchButton = getByTitle('search');

    await act(async () => userEvent.click(searchButton));

    const checkbox = await findByTestId(
      'selectrow-PID-009-727-493-48.76613749999999--123.46163749999998',
    );
    await act(async () => userEvent.click(checkbox));
    const addButton = getByText('Add to selection');
    await act(async () => userEvent.click(addButton));

    const toast = await screen.findAllByText(
      'A property that the user is trying to select has already been added to the selected properties list',
    );
    expect(toast[0]).toBeVisible();
  });

  it(`calls "repositionSelectedProperty" callback when file marker has been repositioned`, async () => {
    const testMapMock: IMapStateMachineContext = { ...mapMachineBaseMock };
    const mapProperties = [
      PropertyForm.fromMapProperty({ ...testProperty, pid: '009-727-493' }).toFeatureDataset(),
    ];

    const { rerender } = setup({
      modifiedProperties: mapProperties,
      mockMapMachine: testMapMock,
    });

    // simulate file marker repositioning via the map state machine
    await act(async () => {
      testMapMock.isRepositioning = true;
      testMapMock.repositioningFeatureDataset = {} as any;
      testMapMock.mapLocationFeatureDataset = {} as any;
    });

    rerender(
      <Formik initialValues={{ properties: [] }} onSubmit={noop}>
        <MapSelectorContainer
          addSelectedProperties={onSelectedProperties}
          repositionSelectedProperty={onRepositionSelectedProperty}
          modifiedProperties={mapProperties}
        />
      </Formik>,
    );

    expect(onRepositionSelectedProperty).toHaveBeenCalled();
  });
});
