import noop from 'lodash/noop';

import { IMapStateMachineContext } from '@/components/common/mapFSM/MapStateMachineContext';
import { getMockLatLng, getMockPolygon } from '@/mocks/geometries.mock';
import { mapMachineBaseMock } from '@/mocks/mapFSM.mock';
import {
  act,
  createMapContainer,
  deferred,
  render,
  RenderOptions,
  screen,
  userEvent,
} from '@/utils/test-utils';

import {
  LocationFeatureDataset,
  WorklistLocationFeatureDataset,
} from '@/components/common/mapFSM/useLocationFeatureLoader';
import { emptyPmbcParcel } from '@/models/layers/parcelMapBC';
import { firstOrNull } from '@/utils';
import { feature, featureCollection } from '@turf/turf';
import { LocationPopupContainer } from './LocationPopupContainer';
import { IMultiplePropertyPopupView, MultiplePropertyPopupView } from './MultiplePropertyPopupView';

const mockViewProps = vi.fn();

// Mock the view component to test container logic in isolation
vi.mock('./MultiplePropertyPopupView');
vi.mocked(MultiplePropertyPopupView).mockImplementation(
  (props: React.ComponentProps<typeof MultiplePropertyPopupView>) => {
    mockViewProps(props);
    return (
      <div data-testid="mocked-view">
        MultiplePropertyPopupView
        <button
          data-testid="select-property-button"
          onClick={() => props.onSelectProperty(firstOrNull(props.featureDataset.parcelFeatures))}
        >
          Select Property
        </button>
        <button
          data-testid="add-to-worklist-button"
          onClick={() =>
            props.onAddPropertyToWorklist(
              firstOrNull(props.featureDataset.parcelFeatures),
              props.featureDataset,
            )
          }
        >
          Add Property To Worklist
        </button>
        <button
          data-testid="add-all-to-worklist-button"
          onClick={() => props.onAddAllToWorklist(props.featureDataset)}
        >
          Add All To Worklist
        </button>
        <button data-testid="close-button" onClick={props.onClose}>
          Close
        </button>
      </div>
    );
  },
);

describe('LocationPopupContainer', () => {
  let testMockMachine: IMapStateMachineContext;

  const setup = async (renderOptions: RenderOptions = {}) => {
    // create a promise to wait for the map to be ready (which happens after initial render)
    const { promise, resolve } = deferred();
    const ReactMap = createMapContainer(resolve, noop);

    const rendered = render(
      <ReactMap>
        <LocationPopupContainer />
      </ReactMap>,
      {
        ...renderOptions,
        mockMapMachine: renderOptions?.mockMapMachine ?? mapMachineBaseMock,
      },
    );

    await act(async () => promise as any);

    return {
      ...rendered,
    };
  };

  beforeEach(() => {
    testMockMachine = {
      ...mapMachineBaseMock,
      mapLocationFeatureDataset: {
        location: getMockLatLng(48, -123),
        fileLocation: null,
        parcelFeatures: [
          // Common property
          feature(getMockPolygon(), {
            ...emptyPmbcParcel,
            PLAN_NUMBER: 'VIS547',
            OWNER_TYPE: 'Unclassified',
          }),
          // Regular properties
          feature(getMockPolygon(), {
            ...emptyPmbcParcel,
            PLAN_NUMBER: 'VIS547',
            PID_FORMATTED: '000-709-280',
            OWNER_TYPE: 'Private',
          }),
          feature(getMockPolygon(), {
            ...emptyPmbcParcel,
            PLAN_NUMBER: 'VIS547',
            PID_FORMATTED: '000-709-239',
            OWNER_TYPE: 'Private',
          }),
        ],
        pimsFeatures: [],
        regionFeature: null,
        districtFeature: null,
        municipalityFeatures: [],
        highwayFeatures: [],
        crownLandLeasesFeatures: [],
        crownLandLicensesFeatures: [],
        crownLandTenuresFeatures: [],
        crownLandInventoryFeatures: [],
        crownLandInclusionsFeatures: [],
        selectingComponentId: '',
      },
    };
  });

  it('renders the view component with expected props', async () => {
    await setup({ mockMapMachine: testMockMachine });

    expect(mockViewProps).toHaveBeenCalledWith(
      expect.objectContaining<IMultiplePropertyPopupView>({
        featureDataset: testMockMachine.mapLocationFeatureDataset,
        onSelectProperty: expect.any(Function),
        onAddPropertyToWorklist: expect.any(Function),
        onAddAllToWorklist: expect.any(Function),
        onClose: expect.any(Function),
      }),
    );
  });

  it('calls onSelectProperty with the selected property when a property is selected', async () => {
    await setup({ mockMapMachine: testMockMachine });

    const selectButton = screen.getByTestId('select-property-button');
    await act(async () => userEvent.click(selectButton));

    expect(mockViewProps).toHaveBeenCalledWith(
      expect.objectContaining<Partial<IMultiplePropertyPopupView>>({
        onSelectProperty: expect.any(Function),
      }),
    );
    expect(testMockMachine.setSelectedLocation).toHaveBeenCalledWith(
      expect.objectContaining<Partial<LocationFeatureDataset>>({
        parcelFeatures: [firstOrNull(testMockMachine.mapLocationFeatureDataset.parcelFeatures)],
      }),
    );
  });

  it('calls onAddPropertyToWorklist with the selected property when add to worklist is clicked', async () => {
    await setup({ mockMapMachine: testMockMachine });

    const addToWorklistButton = screen.getByTestId('add-to-worklist-button');
    await act(async () => userEvent.click(addToWorklistButton));

    expect(mockViewProps).toHaveBeenCalledWith(
      expect.objectContaining<Partial<IMultiplePropertyPopupView>>({
        onAddPropertyToWorklist: expect.any(Function),
      }),
    );
    expect(testMockMachine.worklistAdd).toHaveBeenCalledWith(
      expect.objectContaining<Partial<WorklistLocationFeatureDataset>>({
        fullyAttributedFeatures: featureCollection([
          firstOrNull(testMockMachine.mapLocationFeatureDataset.parcelFeatures),
        ]),
        regionFeature: testMockMachine.mapLocationFeatureDataset.regionFeature,
        districtFeature: testMockMachine.mapLocationFeatureDataset.districtFeature,
        location: testMockMachine.mapLocationFeatureDataset.location,
      }),
    );
  });

  it('calls onAddAllToWorklist with all properties when add all to worklist is clicked', async () => {
    await setup({ mockMapMachine: testMockMachine });

    const addAllToWorklistButton = screen.getByTestId('add-all-to-worklist-button');
    await act(async () => userEvent.click(addAllToWorklistButton));

    expect(mockViewProps).toHaveBeenCalledWith(
      expect.objectContaining<Partial<IMultiplePropertyPopupView>>({
        onAddAllToWorklist: expect.any(Function),
      }),
    );
    expect(testMockMachine.worklistAdd).toHaveBeenCalledWith(
      expect.objectContaining<Partial<WorklistLocationFeatureDataset>>({
        fullyAttributedFeatures: featureCollection(
          testMockMachine.mapLocationFeatureDataset.parcelFeatures,
        ),
        regionFeature: testMockMachine.mapLocationFeatureDataset.regionFeature,
        districtFeature: testMockMachine.mapLocationFeatureDataset.districtFeature,
        location: testMockMachine.mapLocationFeatureDataset.location,
      }),
    );
  });

  it('calls closePopup when the close button is clicked', async () => {
    await setup({ mockMapMachine: testMockMachine });

    const closeButton = screen.getByTestId('close-button');
    await act(async () => userEvent.click(closeButton));

    expect(testMockMachine.closePopup).toHaveBeenCalled();
  });
});
