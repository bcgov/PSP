import {
  ISelectedPropertyContext,
  SelectedPropertyContextProvider,
} from 'components/maps/providers/SelectedPropertyContext';
import { mapFeatureToProperty } from 'features/properties/selector/components/MapClickMonitor';
import { Feature, GeoJsonProperties, Geometry } from 'geojson';
import { createMemoryHistory } from 'history';
import { noop } from 'lodash';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';
import { act, render, RenderOptions, userEvent, waitFor } from 'utils/test-utils';

import AddResearchContainer, { IAddResearchContainerProps } from './AddResearchContainer';

const mockStore = configureMockStore([thunk]);

const store = mockStore({});
const history = createMemoryHistory();

// Need to mock this library for unit tests
jest.mock('react-visibility-sensor', () => {
  return jest.fn().mockImplementation(({ children }) => {
    if (children instanceof Function) {
      return children({ isVisible: true });
    }
    return children;
  });
});

describe('AddResearchContainer component', () => {
  const setup = (
    renderOptions: RenderOptions & IAddResearchContainerProps & Partial<ISelectedPropertyContext>,
  ) => {
    // render component under test
    const component = render(
      <SelectedPropertyContextProvider
        values={{
          selectedResearchFeature: renderOptions.selectedResearchFeature,
          setDraftProperties: renderOptions.setDraftProperties,
          setSelectedResearchFeature: renderOptions.setSelectedResearchFeature,
        }}
      >
        <AddResearchContainer onClose={renderOptions.onClose} />
      </SelectedPropertyContextProvider>,
      {
        ...renderOptions,
        store: store,
        history: history,
      },
    );

    return {
      store,
      component,
    };
  };

  it('renders as expected', () => {
    const { component } = setup({ onClose: noop });
    expect(component.asFragment()).toMatchSnapshot();
  });

  it('displays the currently selected property', async () => {
    const {
      component: { findByText },
    } = setup({ onClose: noop, selectedResearchFeature: selectedFeature });
    await act(async () => {
      const pidText = await findByText('PID: 002-225-255');
      expect(pidText).toBeVisible();
    });
  });

  it('resets the form if selected features already contains properties', async () => {
    const {
      component: { queryByText },
    } = setup({
      onClose: noop,
      selectedFeature: null,
      draftProperties: [mapFeatureToProperty(selectedFeature) as any],
    });
    history.push('/mapview/research/new');
    const pidText = await queryByText('PID: 002-225-255');
    expect(pidText).toBeNull();
  });

  it('resets the list of draft properties when closed', async () => {
    const setDraftMarkers = jest.fn();
    const {
      component: { getByTitle },
    } = setup({
      onClose: noop,
      selectedFeature: null,
      setDraftProperties: setDraftMarkers,
    });

    const closeButton = getByTitle('close');

    await waitFor(async () => {
      userEvent.click(closeButton);
      expect(setDraftMarkers).toHaveBeenCalledWith([]);
    });
  });

  it('resets the selected research property when closed', async () => {
    const setSelectedResearchFeature = jest.fn();
    setSelectedResearchFeature.mockName('selectedResearch');
    const {
      component: { getByTitle, unmount },
    } = setup({
      onClose: noop,
      selectedFeature: null,
      setSelectedResearchFeature: setSelectedResearchFeature,
    });

    const closeButton = getByTitle('close');
    userEvent.click(closeButton);
    unmount();
    await waitFor(async () => {
      expect(setSelectedResearchFeature).toHaveBeenCalledWith(null);
    });
  });
});

const selectedFeature = {
  type: 'Feature',
  id: 'WHSE_CADASTRE.PMBC_PARCEL_FABRIC_POLY_SVW.fid-570f7aaf_180481054f6_-993',
  geometry: {
    type: 'Polygon',
    coordinates: [
      [
        [-123.32022547, 48.43351697],
        [-123.32023219, 48.43337997],
        [-123.31973714, 48.43336902],
        [-123.31973042, 48.43350602],
        [-123.32022547, 48.43351697],
      ],
    ],
  },
  geometry_name: 'SHAPE',
  properties: {
    PARCEL_FABRIC_POLY_ID: 5156389,
    PARCEL_NAME: '002225255',
    PLAN_NUMBER: 'VIP915',
    PIN: null,
    PID: '002225255',
    PID_NUMBER: 2225255,
    PARCEL_STATUS: 'Active',
    PARCEL_CLASS: 'Subdivision',
    OWNER_TYPE: 'Private',
    PARCEL_START_DATE: null,
    MUNICIPALITY: 'Oak Bay, The Corporation of the District of',
    REGIONAL_DISTRICT: 'Capital Regional District',
    WHEN_UPDATED: '2019-01-05Z',
    FEATURE_AREA_SQM: 558.6863,
    FEATURE_LENGTH_M: 103.8813,
    OBJECTID: 584001723,
    SE_ANNO_CAD_DATA: null,
    IS_SELECTED: false,
  },
} as Feature<Geometry, GeoJsonProperties>;
