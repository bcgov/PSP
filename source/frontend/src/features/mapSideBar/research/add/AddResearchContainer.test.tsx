import { Feature, Geometry } from 'geojson';
import { createMemoryHistory } from 'history';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';

import { IMapStateMachineContext } from '@/components/common/mapFSM/MapStateMachineContext';
import { usePimsPropertyRepository } from '@/hooks/repositories/usePimsPropertyRepository';
import { mapMachineBaseMock } from '@/mocks/mapFSM.mock';
import { PMBC_FullyAttributed_Feature_Properties } from '@/models/layers/parcelMapBC';
import {
  RenderOptions,
  act,
  cleanup,
  getMockRepositoryObj,
  render,
  screen,
  userEvent,
} from '@/utils/test-utils';

import { getMockResearchFile } from '@/mocks/researchFile.mock';
import { SideBarContextProvider } from '../../context/sidebarContext';
import { useAddResearch } from '../hooks/useAddResearch';
import AddResearchContainer, { IAddResearchContainerProps } from './AddResearchContainer';
import AddResearchForm from './AddResearchForm';

const mockStore = configureMockStore([thunk]);

const store = mockStore({});
const history = createMemoryHistory();

const onClose = vi.fn();
const onSuccess = vi.fn();

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

const mockGetByPidWrapper = getMockRepositoryObj();
const mockGetByPinWrapper = getMockRepositoryObj();

vi.mock('@/hooks/repositories/usePimsPropertyRepository');
vi.mocked(usePimsPropertyRepository, { partial: true }).mockReturnValue({
  getPropertyByPidWrapper: mockGetByPidWrapper,
  getPropertyByPinWrapper: mockGetByPinWrapper,
});

const mockAddResearchFile = vi.fn();
vi.mock('../hooks/useAddResearch');
vi.mocked(useAddResearch, { partial: true }).mockReturnValue({
  addResearchFile: mockAddResearchFile,
});

describe('AddResearchContainer component', () => {
  const setup = async (
    renderOptions: RenderOptions & { props?: Partial<IAddResearchContainerProps> } = {},
  ) => {
    // render component under test
    const utils = render(
      <SideBarContextProvider>
        <AddResearchContainer
          onClose={renderOptions.props?.onClose ?? onClose}
          onSuccess={renderOptions.props?.onSuccess ?? onSuccess}
          View={AddResearchForm}
        />
      </SideBarContextProvider>,
      {
        claims: [],
        useMockAuthentication: true,
        store,
        history,
        ...renderOptions,
      },
    );

    // wait for the component to finish loading
    await act(async () => {});

    return {
      ...utils,
      store,
      getNameTextbox: () => utils.container.querySelector(`input[name="name"]`) as HTMLInputElement,
      getCancelButton: () => utils.getByText(/Cancel/i),
      getSaveButton: () => utils.getByRole('button', { name: /Save/i }),
    };
  };

  beforeEach(() => {
    cleanup();
    mockAddResearchFile.mockResolvedValue(getMockResearchFile());
  });

  it('renders as expected', async () => {
    const { asFragment } = await setup();
    await act(async () => {});
    expect(asFragment()).toMatchSnapshot();
  });

  it('displays the currently selected property', async () => {
    const { findByText } = await setup({
      mockMapMachine: {
        ...mapMachineBaseMock,
        selectedFeatures: [
          {
            location: { lat: 0, lng: 0 },
            fileLocation: null,
            pimsFeature: null,
            parcelFeature: selectedFeature,
            regionFeature: null,
            districtFeature: null,
            selectingComponentId: null,
            municipalityFeature: undefined,
          },
        ],
      },
    });

    const pidText = await findByText('PID: 002-225-255');
    expect(pidText).toBeVisible();
  });

  it('should confirm and close the form when navigating away', async () => {
    const { getByTitle, getNameTextbox } = await setup();

    await act(async () => userEvent.paste(getNameTextbox(), 'Test Value'));

    await act(async () => history.push('/'));

    await act(async () => userEvent.click(getByTitle('ok-modal')));
    expect(history.location.pathname).toBe('/');
  });

  it('should call onClose Cancel button is clicked with changes', async () => {
    const { getCancelButton, getByText, getNameTextbox } = await setup();

    expect(getByText(/Create Research File/i)).toBeVisible();

    await act(async () => userEvent.paste(getNameTextbox(), 'Test Value'));
    await act(async () => userEvent.click(getCancelButton()));

    expect(onClose).toHaveBeenCalled();
  });

  it('should save the form and navigate to details view when Save button is clicked', async () => {
    const testMockMachine: IMapStateMachineContext = {
      ...mapMachineBaseMock,
      processCreation: vi.fn(),
      refreshMapProperties: vi.fn(),
    };

    const { getSaveButton, getByText, getNameTextbox } = await setup({
      mockMapMachine: testMockMachine,
    });

    expect(getByText(/Create Research File/i)).toBeVisible();

    await act(async () => userEvent.paste(getNameTextbox(), 'Test Value'));
    await act(async () => userEvent.click(getSaveButton()));

    expect(onSuccess).toHaveBeenCalled();
    expect(testMockMachine.processCreation).toHaveBeenCalled();
    expect(testMockMachine.refreshMapProperties).toHaveBeenCalled();
  });

  it('resets the "draft" markers when the file is opened', async () => {
    const testMockMachine: IMapStateMachineContext = {
      ...mapMachineBaseMock,
    };
    await setup({ mockMapMachine: testMockMachine });
    expect(testMockMachine.setFilePropertyLocations).toHaveBeenCalledWith([]);
  });

  it('should have the Help with choosing a name text in the component', async () => {
    await setup();
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
    SHAPE: null,
  },
};
