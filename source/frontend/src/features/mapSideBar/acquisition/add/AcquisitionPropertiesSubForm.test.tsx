import { Formik } from 'formik';
import { noop } from 'lodash';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import {
  emptyFullyFeaturedFeatureCollection,
  emptyPimsFeatureCollection,
} from '@/components/common/mapFSM/models';
import { render, RenderOptions, userEvent, waitFor } from '@/utils/test-utils';

import { PropertyForm } from '../../shared/models';
import { AcquisitionPropertiesSubForm } from './AcquisitionPropertiesSubForm';
import { AcquisitionForm } from './models';

const mockStore = configureMockStore([thunk]);
jest.mock('@react-keycloak/web');

const setDraftProperties = jest.fn();

jest.mock('@/components/common/mapFSM/MapStateMachineContext');
const mapMachineBaseMock = {
  requestFlyToBounds: jest.fn(),
  mapFeatureData: {
    pimsFeatures: emptyPimsFeatureCollection,
    fullyAttributedFeatures: emptyFullyFeaturedFeatureCollection,
  },

  isSidebarOpen: false,
  hasPendingFlyTo: false,
  requestedFlyTo: {
    location: null,
    bounds: null,
  },
  mapFeatureSelected: null,
  mapLocationSelected: null,
  mapLocationFeatureDataset: null,
  selectedFeatureDataset: null,
  showPopup: false,
  isLoading: false,
  mapFilter: null,

  draftLocations: [],
  pendingRefresh: false,
  iSelecting: false,
  requestFlyToLocation: jest.fn(),

  processFlyTo: jest.fn(),
  processPendingRefresh: jest.fn(),
  openSidebar: jest.fn(),
  closeSidebar: jest.fn(),
  closePopup: jest.fn(),
  mapClick: jest.fn(),
  mapMarkerClick: jest.fn(),
  setMapFilter: jest.fn(),
  refreshMapProperties: jest.fn(),
  prepareForCreation: jest.fn(),
  startSelection: jest.fn(),
  finishSelection: jest.fn(),
  setDraftLocations: setDraftProperties,
};

describe('AcquisitionProperties component', () => {
  // render component under test
  const setup = (props: { initialForm: AcquisitionForm }, renderOptions: RenderOptions = {}) => {
    const utils = render(
      <>
        <Formik initialValues={props.initialForm} onSubmit={noop}>
          {formikProps => <AcquisitionPropertiesSubForm formikProps={formikProps} />}
        </Formik>
      </>,
      {
        ...renderOptions,
        store: mockStore({}),
        claims: [],
      },
    );

    return { ...utils };
  };

  let testForm: AcquisitionForm;

  beforeEach(() => {
    testForm = new AcquisitionForm();
    testForm.fileName = 'Test name';
    testForm.properties = [
      PropertyForm.fromMapProperty({ pid: '123-456-789' }),
      PropertyForm.fromMapProperty({ pin: '1111222' }),
    ];
    (useMapStateMachine as jest.Mock).mockImplementation(() => mapMachineBaseMock);
  });

  afterEach(() => {
    jest.clearAllMocks();
    setDraftProperties.mockReset();
  });

  it('renders as expected', () => {
    const { asFragment } = setup({ initialForm: testForm });
    expect(asFragment()).toMatchSnapshot();
  });

  it('renders list of properties', async () => {
    const { getByText } = setup({ initialForm: testForm });

    await waitFor(() => {
      expect(setDraftProperties).toHaveBeenCalledWith([
        { lat: 0, lng: 0 },
        { lat: 0, lng: 0 },
      ]);
    });

    expect(getByText('PID: 123-456-789')).toBeVisible();
    expect(getByText('PIN: 1111222')).toBeVisible();
  });

  it('should remove property from list when Remove button is clicked', async () => {
    const { getAllByTitle, queryByText } = setup({ initialForm: testForm });
    const pidRow = getAllByTitle('remove')[0];
    userEvent.click(pidRow);

    await waitFor(() => {
      expect(setDraftProperties).toHaveBeenCalledWith([{ lat: 0, lng: 0 }]);
    });

    expect(queryByText('PID: 123-456-789')).toBeNull();
  });

  it('should display properties with svg prefixed with incrementing id', async () => {
    const { getByTitle } = setup({ initialForm: testForm });

    await waitFor(() => {
      expect(setDraftProperties).toHaveBeenCalledWith([
        { lat: 0, lng: 0 },
        { lat: 0, lng: 0 },
      ]);
    });

    expect(getByTitle('1')).toBeInTheDocument();
    expect(getByTitle('2')).toBeInTheDocument();
  });

  it('should synchronize a single property with provided lat/lng', async () => {
    const formWithProperties = testForm;
    formWithProperties.properties[0].latitude = 1;
    formWithProperties.properties[0].longitude = 2;
    setup({ initialForm: formWithProperties });

    await waitFor(() => {
      expect(setDraftProperties).toHaveBeenCalledWith([
        { lat: 1, lng: 2 },
        { lat: 0, lng: 0 },
      ]);
    });
  });

  it('should synchronize multiple properties with provided lat/lng', async () => {
    const formWithProperties = testForm;
    formWithProperties.properties[0].latitude = 1;
    formWithProperties.properties[0].longitude = 2;
    formWithProperties.properties[1].latitude = 3;
    formWithProperties.properties[1].longitude = 4;

    setup({ initialForm: formWithProperties });

    await waitFor(() => {
      expect(setDraftProperties).toHaveBeenCalledWith([
        { lat: 1, lng: 2 },
        { lat: 3, lng: 4 },
      ]);
    });
  });
});
