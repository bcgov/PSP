import { IMapStateMachineContext } from '@/components/common/mapFSM/MapStateMachineContext';
import { mapMachineBaseMock } from '@/mocks/mapFSM.mock';
import { getMockWorklistParcel } from '@/mocks/worklistParcel.mock';
import { act, render, RenderOptions, screen } from '@/utils/test-utils';

import { useWorklistContext } from './context/WorklistContext';
import { ParcelDataset } from './models';
import { WorklistContainer } from './WorklistContainer';
import { IWorklistViewProps } from './WorklistView';

vi.mock('./context/WorklistContext');

// Mock Leaflet geoJSON().getBounds().isValid()
const mockBounds = {
  isValid: vi.fn(() => true),
};
vi.mock('leaflet', async () => {
  const actual = await vi.importActual<any>('leaflet');
  return {
    ...actual,
    geoJSON: vi.fn(() => ({
      getBounds: () => mockBounds,
    })),
  };
});

// Parcel list mock
let mockParcels: ParcelDataset[] = [];

// Mocks
const select = vi.fn();
const remove = vi.fn();
const clearAll = vi.fn();
const requestFlyToBounds = vi.fn();
const requestFlyToLocation = vi.fn();

vi.mocked(useWorklistContext, { partial: true }).mockReturnValue({
  select,
  remove,
  clearAll,
  parcels: mockParcels,
});

let viewProps: IWorklistViewProps = undefined;

const ViewMock = vi.fn((props: IWorklistViewProps) => {
  viewProps = props;
  return <div data-testid="view" />;
});

describe('WorklistContainer', () => {
  beforeEach(() => {
    vi.clearAllMocks();
    viewProps = undefined;
    // clear the array in place (without assigning a new empty array instance)
    mockParcels.length = 0;
    mockParcels.push(
      getMockWorklistParcel('parcel-1', {}, { lat: 49.0, lng: -123.0 }),
      getMockWorklistParcel('parcel-2', {}, { lat: 50.0, lng: -122.0 }),
    );
  });

  const setup = (renderOptions: RenderOptions = {}) => {
    return render(<WorklistContainer View={ViewMock} />, {
      ...renderOptions,
      mockMapMachine: renderOptions.mockMapMachine,
    });
  };

  it('renders the given View with correct props', () => {
    setup();
    expect(screen.getByTestId('view')).toBeInTheDocument();
    expect(ViewMock).toHaveBeenCalledWith(
      expect.objectContaining({
        parcels: mockParcels,
        onSelect: select,
        onRemove: remove,
        onClearAll: clearAll,
        onZoomToParcel: expect.any(Function),
      }),
      {},
    );
  });

  it('calls requestFlyToBounds when parcel has valid geometry', async () => {
    const testMockMachine: IMapStateMachineContext = {
      ...mapMachineBaseMock,
      requestFlyToBounds,
      requestFlyToLocation,
    };

    setup({ mockMapMachine: testMockMachine });
    await act(async () => viewProps.onZoomToParcel(mockParcels[0]));

    expect(requestFlyToBounds).toHaveBeenCalledWith(mockBounds);
    expect(requestFlyToLocation).not.toHaveBeenCalled();
  });

  it('falls back to requestFlyToLocation if bounds are invalid', async () => {
    mockBounds.isValid = vi.fn(() => false); // simulate invalid geometry

    const testMockMachine: IMapStateMachineContext = {
      ...mapMachineBaseMock,
      requestFlyToBounds,
      requestFlyToLocation,
    };

    setup({ mockMapMachine: testMockMachine });
    await act(async () => viewProps.onZoomToParcel(mockParcels[0]));

    expect(requestFlyToBounds).not.toHaveBeenCalled();
    expect(requestFlyToLocation).toHaveBeenCalledWith(mockParcels[0].location);
  });

  it('calls requestFlyToLocation when parcel has no feature', async () => {
    mockParcels[0].pmbcFeature = null;

    const testMockMachine: IMapStateMachineContext = {
      ...mapMachineBaseMock,
      requestFlyToBounds,
      requestFlyToLocation,
    };

    setup({ mockMapMachine: testMockMachine });
    await act(async () => viewProps.onZoomToParcel(mockParcels[0]));

    expect(requestFlyToLocation).toHaveBeenCalledWith(mockParcels[0].location);
    expect(requestFlyToBounds).not.toHaveBeenCalled();
  });
});
