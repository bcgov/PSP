import { screen } from '@testing-library/react';
import { describe, it, expect, vi } from 'vitest';
import { LayerPopupContainer } from './LayerPopupContainer';
import { render } from '@/utils/test-utils';
import { getMockLocationFeatureDataset } from '@/mocks/featureset.mock';
import getMockISSResult from '@/mocks/mockISSResult';
import { SinglePropertyFeatureDataSet } from './LocationPopupContainer';
import { firstOrNull } from '@/utils';

vi.mock('react-leaflet/Popup', () => ({
  Popup: ({ children }: any) => <>{children}</>,
}));

describe('LayerPopupContainer', () => {
  beforeEach(() => {
    vi.resetAllMocks();
  });

  const setup = (featureDataset?: SinglePropertyFeatureDataSet) => {
    return render(<LayerPopupContainer featureDataset={featureDataset} />, {});
  };

  it('matches snapshot', () => {
    const { asFragment } = setup({
      ...getMockLocationFeatureDataset(),
      parcelFeature: getMockLocationFeatureDataset().parcelFeatures[0],
      pimsFeature: firstOrNull(getMockLocationFeatureDataset().pimsFeatures),
    });
    expect(asFragment()).toMatchSnapshot();
  });

  it('displays the correct title for parcel data', () => {
    setup({
      ...getMockLocationFeatureDataset(),
      parcelFeature: getMockLocationFeatureDataset().parcelFeatures[0],
      pimsFeature: firstOrNull(getMockLocationFeatureDataset().pimsFeatures),
    });
    expect(screen.getByText('LTSA ParcelMap data')).toBeInTheDocument();
  });

  it('displays multiple highway layers correctly', () => {
    setup({
      ...getMockLocationFeatureDataset(),
      pimsFeature: null,
      parcelFeature: null,
      municipalityFeatures: null,
      highwayFeatures: getMockISSResult().features,
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
