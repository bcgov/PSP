import { screen } from '@testing-library/react';
import { Feature, Geometry } from 'geojson';
import { createMemoryHistory } from 'history';
import { noop } from 'lodash';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';

import {
  IMapStateMachineContext,
  useMapStateMachine,
} from '@/components/common/mapFSM/MapStateMachineContext';
import { mapMachineBaseMock } from '@/mocks/mapFSM.mock';
import { PMBC_FullyAttributed_Feature_Properties } from '@/models/layers/parcelMapBC';
import { act, render, RenderOptions, userEvent, waitFor } from '@/utils/test-utils';

import AddResearchContainer, { IAddResearchContainerProps } from './AddResearchContainer';

const mockStore = configureMockStore([thunk]);

const store = mockStore({});
const history = createMemoryHistory();
jest.mock('@react-keycloak/web');

// Need to mock this library for unit tests
jest.mock('react-visibility-sensor', () => {
  return jest.fn().mockImplementation(({ children }) => {
    if (children instanceof Function) {
      return children({ isVisible: true });
    }
    return children;
  });
});

jest.mock('@/components/common/mapFSM/MapStateMachineContext');
(useMapStateMachine as jest.Mock).mockImplementation(() => mapMachineBaseMock);

describe('AddResearchContainer component', () => {
  const setup = (
    renderOptions: RenderOptions & IAddResearchContainerProps & Partial<IMapStateMachineContext>,
  ) => {
    // render component under test
    const component = render(
      <>
        <AddResearchContainer onClose={renderOptions.onClose} />
      </>,
      {
        ...renderOptions,
        claims: [],
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
    (useMapStateMachine as unknown as jest.Mock<Partial<IMapStateMachineContext>>).mockReturnValue({
      ...mapMachineBaseMock,
      selectedFeatureDataset: {
        location: { lat: 0, lng: 0 },
        pimsFeature: null,
        parcelFeature: selectedFeature,
        regionFeature: null,
        districtFeature: null,
        municipalityFeature: null,
      },
    });

    const {
      component: { findByText },
    } = setup({ onClose: noop });
    await act(async () => {
      const pidText = await findByText('PID: 002-225-255');
      expect(pidText).toBeVisible();
    });
  });

  it('resets the list of draft properties when closed', async () => {
    const testMockMahine: IMapStateMachineContext = {
      ...mapMachineBaseMock,
    };
    (useMapStateMachine as unknown as jest.Mock<Partial<IMapStateMachineContext>>).mockReturnValue(
      testMockMahine,
    );

    const {
      component: { getByTitle },
    } = setup({
      onClose: noop,
    });

    const closeButton = getByTitle('close');

    await waitFor(async () => {
      userEvent.click(closeButton);
      expect(testMockMahine.setFilePropertyLocations).toHaveBeenCalledWith([]);
    });
  });

  it('should have the Help with choosing a name text in the component', async () => {
    setup({
      onClose: noop,
    });
    expect(screen.getByText(`Help with choosing a name`)).toBeInTheDocument();
  });
});

const selectedFeature: Feature<Geometry, PMBC_FullyAttributed_Feature_Properties> = {
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
    GLOBAL_UID: null,
    PLAN_ID: null,
    PID_FORMATTED: null,
    SOURCE_PARCEL_ID: null,
    SURVEY_DESIGNATION_1: null,
    SURVEY_DESIGNATION_2: null,
    SURVEY_DESIGNATION_3: null,
    LEGAL_DESCRIPTION: null,
    IS_REMAINDER_IND: null,
    GEOMETRY_SOURCE: null,
    POSITIONAL_ERROR: null,
    ERROR_REPORTED_BY: null,
    CAPTURE_METHOD: null,
    COMPILED_IND: null,
    STATED_AREA: null,
    WHEN_CREATED: null,
  },
};
