import { screen } from '@testing-library/react';
import { Feature, Geometry } from 'geojson';
import { createMemoryHistory } from 'history';
import noop from 'lodash/noop';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';

import {
  IMapStateMachineContext,
  useMapStateMachine,
} from '@/components/common/mapFSM/MapStateMachineContext';
import { mapMachineBaseMock } from '@/mocks/mapFSM.mock';
import { PMBC_Feature_Properties } from '@/models/layers/parcelMapBC';
import { RenderOptions, act, renderAsync, userEvent, waitFor } from '@/utils/test-utils';

import AddResearchContainer, { IAddResearchContainerProps } from './AddResearchContainer';
import { cleanup } from '@testing-library/react-hooks';

const mockStore = configureMockStore([thunk]);

const store = mockStore({});
const history = createMemoryHistory();

const onClose = vi.fn();

// Need to mock this library for unit tests
vi.mock('react-visibility-sensor', () => {
  return {
    default: vi.fn().mockImplementation(({ children }) => {
      if (children instanceof Function) {
        return children({ isVisible: true });
      }
      return children;
    }),
  };
});

const mockGetByPidWrapper = {
  error: undefined,
  response: undefined,
  execute: vi.fn(),
  loading: false,
};

const mockGetByPinWrapper = {
  error: undefined,
  response: undefined,
  execute: vi.fn(),
  loading: false,
};

vi.mock('@/hooks/repositories/usePimsPropertyRepository', () => ({
  usePimsPropertyRepository: () => {
    return {
      getPropertyByPidWrapper: mockGetByPidWrapper,
      getPropertyByPinWrapper: mockGetByPinWrapper,
    };
  },
}));

describe('AddResearchContainer component', () => {
  const setup = async (
    renderOptions: RenderOptions & IAddResearchContainerProps & Partial<IMapStateMachineContext>,
  ) => {
    // render component under test
    const utils = await renderAsync(<AddResearchContainer onClose={renderOptions.onClose} />, {
      claims: [],
      useMockAuthentication: true,
      store,
      history,
      ...renderOptions,
    });

    return {
      ...utils,
      store,
      getNameTextbox: () => utils.container.querySelector(`input[name="name"]`) as HTMLInputElement,
      getCancelButton: () => utils.getByText(/Cancel/i),
    };
  };

  beforeEach(() => {
    cleanup();
  });

  it('renders as expected', async () => {
    const { asFragment } = await setup({ onClose: noop });
    await act(async () => {});
    expect(asFragment()).toMatchSnapshot();
  });

  it('displays the currently selected property', async () => {
    const { findByText } = await setup({
      onClose: noop,
      mockMapMachine: {
        ...mapMachineBaseMock,
        selectedFeatureDataset: {
          location: { lat: 0, lng: 0 },
          pimsFeature: null,
          parcelFeature: selectedFeature,
          regionFeature: null,
          districtFeature: null,
          municipalityFeature: null,
          selectingComponentId: null,
        },
      },
    });
    await act(async () => {
      const pidText = await findByText('PID: 002-225-255');
      expect(pidText).toBeVisible();
    });
  });

  it('should confirm and close the form when Cancel button is clicked with changes', async () => {
    const { getCancelButton, getByText, getByTitle, getNameTextbox } = await setup({
      onClose: onClose,
    });

    expect(getByText(/Create Research File/i)).toBeVisible();

    await act(async () => userEvent.paste(getNameTextbox(), 'Test Value'));
    await act(async () => userEvent.click(getCancelButton()));
    await act(async () => userEvent.click(getByTitle('ok-modal')));

    expect(onClose).toBeCalled();
  });

  it('resets the list of draft properties when closed', async () => {
    const testMockMachine: IMapStateMachineContext = {
      ...mapMachineBaseMock,
    };

    const { getByTitle } = await setup({
      onClose: noop,
      mockMapMachine: testMockMachine,
    });

    const closeButton = getByTitle('close');

    await act(async () => {
      userEvent.click(closeButton);
    });
    await waitFor(() => {
      expect(testMockMachine.setFilePropertyLocations).toHaveBeenCalledWith([]);
    });
  });

  it('should have the Help with choosing a name text in the component', async () => {
    await setup({
      onClose: noop,
    });
    await act(async () => {});
    expect(screen.getByText(`Help with choosing a name`)).toBeInTheDocument();
  });
});

const selectedFeature: Feature<Geometry, PMBC_Feature_Properties> = {
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
    PID_FORMATTED: null,
  },
};
