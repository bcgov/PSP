import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { Formik } from 'formik';
import noop from 'lodash/noop';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';

import { useMapProperties } from '@/hooks/repositories/useMapProperties';
import {
  getMockLocationFeatureDataset,
  getMockSelectedFeatureDataset,
} from '@/mocks/featureset.mock';
import { mockFAParcelLayerResponse, mockGeocoderOptions } from '@/mocks/index.mock';
import { mapMachineBaseMock } from '@/mocks/mapFSM.mock';
import { act, fillInput, render, RenderOptions, screen, userEvent } from '@/utils/test-utils';

import { IMapStateMachineContext } from '../common/mapFSM/MapStateMachineContext';
import MapSelectorContainer, { IMapSelectorContainerProps } from './MapSelectorContainer';

const mockStore = configureMockStore([thunk]);

const mockAxios = new MockAdapter(axios);

const store = mockStore({});

const onSelectedProperties = vi.fn();
const onRepositionSelectedProperty = vi.fn();

vi.mock('@/hooks/repositories/useMapProperties');
vi.mocked(useMapProperties).mockReturnValue({
  loadProperties: {
    error: null,
    response: { features: [] } as any,
    execute: vi.fn().mockResolvedValue({
      features: [
        {
          ...getMockSelectedFeatureDataset().pimsFeature,
          properties: {},
        },
      ],
    }),
    loading: false,
    status: 200,
  },
});

describe('MapSelectorContainer component', () => {
  const setup = async (renderOptions: RenderOptions & Partial<IMapSelectorContainerProps>) => {
    // render component under test
    const utils = render(
      <Formik initialValues={{ properties: [] }} onSubmit={noop}>
        <MapSelectorContainer
          addSelectedProperties={onSelectedProperties}
          repositionSelectedProperty={onRepositionSelectedProperty}
          modifiedProperties={
            renderOptions.modifiedProperties ?? [{ ...getMockSelectedFeatureDataset() }]
          }
        />
      </Formik>,
      {
        ...renderOptions,
        store: store,
        mockMapMachine: renderOptions.mockMapMachine ?? mapMachineBaseMock,
      },
    );

    await act(async () => {});

    return {
      store,
      ...utils,
    };
  };

  beforeEach(() => {
    mockAxios
      .onGet(new RegExp('WHSE_CADASTRE.PMBC_PARCEL_FABRIC_POLY_SVW'))
      .reply(200, mockFAParcelLayerResponse)
      .onGet(new RegExp('tools/geocoder/nearest*'))
      .reply(200, mockGeocoderOptions[0]);
  });

  it('renders as expected when provided no properties', async () => {
    const { asFragment } = await setup({});
    await act(async () => {
      expect(asFragment()).toMatchSnapshot();
    });
  });

  it('displays two tabs', async () => {
    const { getByText } = await setup({});
    await act(async () => {
      expect(getByText('Locate on Map')).toBeVisible();
    });
  });

  it('displays locate on Map by default', async () => {
    const { getByText } = await setup({});
    await act(async () => {
      expect(getByText('Locate on Map')).toHaveClass('active');
    });
  });

  it('allows the search tab to be selected', async () => {
    const { getByText } = await setup({});
    const searchTab = getByText('Search');
    await act(async () => userEvent.click(searchTab));
    expect(searchTab).toHaveClass('active');
  });

  it('displays all selected property attributes', async () => {
    const mockFeatureSet = getMockSelectedFeatureDataset();
    const { getByText } = await setup({
      modifiedProperties: [
        {
          ...mockFeatureSet,
          pimsFeature: {
            ...mockFeatureSet.pimsFeature,
            properties: {
              ...mockFeatureSet.pimsFeature?.properties,
              PID: 123456789,
              SURVEY_PLAN_NUMBER: 'SPS22411',
              LAND_LEGAL_DESCRIPTION: 'Test Legal Description',
              STREET_ADDRESS_1: 'Test address 123',
              REGION_CODE: 1,
              DISTRICT_CODE: 5,
            },
          },
          regionFeature: {
            ...mockFeatureSet.regionFeature,
            properties: {
              ...mockFeatureSet.regionFeature?.properties,
              REGION_NUMBER: 1,
              REGION_NAME: 'South Coast',
            },
          },
          districtFeature: {
            ...mockFeatureSet.districtFeature,
            properties: {
              ...mockFeatureSet.districtFeature?.properties,
              DISTRICT_NUMBER: 5,
              DISTRICT_NAME: 'Okanagan-Shuswap',
            },
          },
        },
      ],
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
    const mockFeatureSet = getMockSelectedFeatureDataset();
    const { getByText, getByTitle, findByTestId, container } = await setup({
      modifiedProperties: [
        {
          ...mockFeatureSet,
          pimsFeature: {
            ...mockFeatureSet.pimsFeature,
            properties: {
              ...mockFeatureSet.pimsFeature?.properties,
              PID: 123456789,
              SURVEY_PLAN_NUMBER: 'SPS22411',
              LAND_LEGAL_DESCRIPTION: 'Test Legal Description',
              STREET_ADDRESS_1: 'Test address 123',
              REGION_CODE: 1,
              DISTRICT_CODE: 5,
            },
          },
        },
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

    expect(onSelectedProperties).toHaveBeenCalled();
  });

  it('selected properties display a warning if added multiple times', async () => {
    const mockFeatureSet = getMockSelectedFeatureDataset();
    const { getByText, getByTitle, findByTestId, container } = await setup({
      modifiedProperties: [
        {
          ...mockFeatureSet,
          pimsFeature: {
            ...mockFeatureSet.pimsFeature,
            properties: {
              ...mockFeatureSet.pimsFeature?.properties,
              PID_PADDED: '009-727-493',
              SURVEY_PLAN_NUMBER: 'SPS22411',
              LAND_LEGAL_DESCRIPTION: 'Test Legal Description',
              STREET_ADDRESS_1: 'Test address 123',
              REGION_CODE: 1,
              DISTRICT_CODE: 5,
            },
          },
        },
      ],
    });

    const searchTab = getByText('Search');
    await act(async () => userEvent.click(searchTab));
    await act(async () => {
      fillInput(container, 'searchBy', 'pid', 'select');
    });
    await fillInput(container, 'pid', '009-727-493');
    const searchButton = getByTitle('search');

    await act(async () => userEvent.click(searchButton));

    const checkbox = await findByTestId(
      'selectrow-PID-009-727-493-48.76613749999999--123.46163749999998',
    );
    expect(checkbox).toBeVisible();

    await act(async () => userEvent.click(checkbox));
    const addButton = getByText('Add to selection');
    await act(async () => userEvent.click(addButton));

    const toast = await screen.findAllByText(
      'A property that the user is trying to select has already been added to the selected properties list',
    );
    expect(toast[0]).toBeVisible();
  });

  it(`calls "repositionSelectedProperty" callback when file marker has been repositioned`, async () => {
    const mockFeatureSet = getMockSelectedFeatureDataset();

    // simulate file marker repositioning via the map state machine
    const testMapMock: IMapStateMachineContext = {
      ...mapMachineBaseMock,
      isRepositioning: true,
      repositioningFeatureDataset: {} as any,
      mapLocationFeatureDataset: {} as any,
    };
    const mapProperties = [
      {
        ...mockFeatureSet,
        pimsFeature: {
          ...mockFeatureSet.pimsFeature,
          properties: {
            ...mockFeatureSet.pimsFeature?.properties,
            PID_PADDED: '009-727-493',
            SURVEY_PLAN_NUMBER: 'SPS22411',
            LAND_LEGAL_DESCRIPTION: 'Test Legal Description',
            STREET_ADDRESS_1: 'Test address 123',
            REGION_CODE: 1,
            DISTRICT_CODE: 5,
          },
        },
      },
    ];

    const { rerender } = await setup({
      modifiedProperties: mapProperties,
      mockMapMachine: testMapMock,
    });

    // simulate file marker repositioning via the map state machine
    await act(async () => {
      testMapMock.isRepositioning = true;
      testMapMock.repositioningFeatureDataset = getMockSelectedFeatureDataset();
      testMapMock.mapLocationFeatureDataset = getMockLocationFeatureDataset();
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
