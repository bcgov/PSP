import { screen } from '@testing-library/react';
import { describe, it, expect, vi } from 'vitest';
import { LayerPopupContainer } from './LayerPopupContainer';
import { IMapStateMachineContext } from '@/components/common/mapFSM/MapStateMachineContext';
import { render } from '@/utils/test-utils';
import { mapMachineBaseMock } from '@/mocks/mapFSM.mock';
import { getMockLocationFeatureDataset } from '@/mocks/featureset.mock';
import getMockISSResult from '@/mocks/mockISSResult';
import { Popup } from 'react-leaflet/Popup';

const mockMapStateMachine = {
  ...mapMachineBaseMock,
  mapLocationFeatureDataset: getMockLocationFeatureDataset(),
};

vi.mock('react-leaflet/Popup', () => ({
  Popup: ({ children }: any) => <>{children}</>,
}));

describe('LayerPopupContainer', () => {
  beforeEach(() => {
    vi.resetAllMocks();
  });

  const setup = (context?: IMapStateMachineContext) => {
    return render(<LayerPopupContainer />, { mockMapMachine: context ?? mockMapStateMachine });
  };

  it('matches snapshot', () => {
    const { asFragment } = setup();
    expect(asFragment()).toMatchSnapshot();
  });

  it('displays the correct title for parcel data', () => {
    setup();
    expect(screen.getByText('LTSA ParcelMap data')).toBeInTheDocument();
  });

  it('displays multiple highway layers correctly', () => {
    setup({
      ...mapMachineBaseMock,
      mapLocationFeatureDataset: {
        ...getMockLocationFeatureDataset(),
        pimsFeature: null,
        parcelFeature: null,
        municipalityFeature: null,
        highwayFeatures: getMockISSResult().features,
      },
    });
    expect(screen.getByText('Highway Research (1 of 2)')).toBeInTheDocument();
    expect(screen.getByText('Hyperlink to plan documents:')).toBeInTheDocument();
    expect(
      screen.getByText(
        'https://bcgov.sharepoint.com/teams/01157/Shared Documents/General/LTSA Packages/RS 4284_VIP2490RW',
      ),
    ).toBeInTheDocument();
  });
});
