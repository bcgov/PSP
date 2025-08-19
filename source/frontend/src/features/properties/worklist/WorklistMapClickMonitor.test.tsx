import * as turf from '@turf/turf';

import { IMapStateMachineContext } from '@/components/common/mapFSM/MapStateMachineContext';
import { WorklistLocationFeatureDataset } from '@/components/common/mapFSM/useLocationFeatureLoader';
import { mapMachineBaseMock } from '@/mocks/mapFSM.mock';
import { emptyPmbcParcel } from '@/models/layers/parcelMapBC';
import { render, RenderOptions } from '@/utils/test-utils';

import { WorklistMapClickMonitor } from './WorklistMapClickMonitor';
import { useWorklistContext } from './context/WorklistContext';

vi.mock('./context/WorklistContext');

vi.mock('@/hooks/usePrevious', () => ({
  usePrevious: () => mockPrevious,
}));

vi.mock('@/hooks/util/useDeepCompareEffect', () => ({
  __esModule: true,
  default: (cb: () => void) => cb(), // run effect immediately
}));

const add = vi.fn();
const addRange = vi.fn();
vi.mocked(useWorklistContext, { partial: true }).mockReturnValue({
  add,
  addRange,
});

// Shared mock state
let mockPrevious: WorklistLocationFeatureDataset = undefined;

describe('WorklistMapClickMonitor', () => {
  const setup = (renderOptions: RenderOptions = {}) => {
    return render(<WorklistMapClickMonitor />, {
      ...renderOptions,
      mockMapMachine: renderOptions.mockMapMachine,
    });
  };

  beforeEach(() => {
    vi.clearAllMocks();
    mockPrevious = undefined;
  });

  it('does nothing when first render (previous is undefined)', () => {
    const testMockMachine: IMapStateMachineContext = {
      ...mapMachineBaseMock,
      worklistLocationFeatureDataset: {
        location: { lat: 49, lng: -123 },
        districtFeature: null,
        regionFeature: null,
        fullyAttributedFeatures: null,
        pimsFeature: null,
      },
    };

    setup({ mockMapMachine: testMockMachine });
    expect(add).not.toHaveBeenCalled();
    expect(addRange).not.toHaveBeenCalled();
  });

  it('adds range when new dataset has valid features', () => {
    mockPrevious = {
      location: { lat: 49, lng: -123 },
      districtFeature: null,
      regionFeature: null,
      fullyAttributedFeatures: null,
      pimsFeature: null,
    };

    const testMockMachine: IMapStateMachineContext = {
      ...mapMachineBaseMock,
      worklistLocationFeatureDataset: {
        location: { lat: 49.2, lng: -123.1 },
        districtFeature: null,
        regionFeature: null,
        fullyAttributedFeatures: turf.featureCollection([
          turf.point([-123.1, 49.2], { ...emptyPmbcParcel }),
          turf.point([-75.3, 39.9], { ...emptyPmbcParcel }),
        ]),
        pimsFeature: null,
      },
    };

    setup({ mockMapMachine: testMockMachine });
    expect(addRange).toHaveBeenCalled();
    expect(add).not.toHaveBeenCalled();
  });

  it('adds single parcel by lat/lng when no features exist', () => {
    mockPrevious = {
      location: { lat: 49, lng: -123 },
      districtFeature: null,
      regionFeature: null,
      fullyAttributedFeatures: null,
      pimsFeature: null,
    };

    const testMockMachine: IMapStateMachineContext = {
      ...mapMachineBaseMock,
      worklistLocationFeatureDataset: {
        location: { lat: 50, lng: -123 },
        districtFeature: null,
        regionFeature: null,
        fullyAttributedFeatures: turf.featureCollection([]),
        pimsFeature: null,
      },
    };

    setup({ mockMapMachine: testMockMachine });
    expect(add).toHaveBeenCalledWith(
      expect.objectContaining<Partial<WorklistLocationFeatureDataset>>({
        location: { lat: 50, lng: -123 },
      }),
    );
    expect(addRange).not.toHaveBeenCalled();
  });

  it('does not re-add if same dataset is passed again', () => {
    const dataset: WorklistLocationFeatureDataset = {
      location: { lat: 49, lng: -123 },
      districtFeature: null,
      regionFeature: null,
      fullyAttributedFeatures: turf.featureCollection([
        turf.point([-123, 49], { ...emptyPmbcParcel }),
      ]),
      pimsFeature: null,
    };

    mockPrevious = dataset;
    const testMockMachine: IMapStateMachineContext = {
      ...mapMachineBaseMock,
      worklistLocationFeatureDataset: dataset,
    };

    setup({ mockMapMachine: testMockMachine });
    expect(add).not.toHaveBeenCalled();
    expect(addRange).not.toHaveBeenCalled();
  });
});
