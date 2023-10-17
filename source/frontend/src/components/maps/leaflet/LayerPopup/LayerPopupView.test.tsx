import { createMemoryHistory } from 'history';
import { useMap } from 'react-leaflet';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import Claims from '@/constants/claims';
import { mockLookups } from '@/mocks/lookups.mock';
import { mapMachineBaseMock } from '@/mocks/mapFSM.mock';
import { emptyFullyAttributed } from '@/models/layers/parcelMapBC';
import { EmptyPropertyLocation } from '@/models/layers/pimsPropertyLocationView';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { pidParser } from '@/utils/propertyUtils';
import { act, render, RenderOptions, userEvent } from '@/utils/test-utils';

import { ILayerPopupViewProps, LayerPopupView } from './LayerPopupView';

jest.mock('@react-keycloak/web');
jest.mock('react-leaflet');

jest.mock('@/components/common/mapFSM/MapStateMachineContext');

const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

const history = createMemoryHistory();
(useMap as jest.Mock).mockReturnValue({});

describe('LayerPopupView component', () => {
  const setup = (renderOptions: RenderOptions & ILayerPopupViewProps) => {
    // render component under test
    const component = render(
      <LayerPopupView
        layerPopup={renderOptions.layerPopup}
        featureDataset={renderOptions.featureDataset}
      />,
      {
        ...renderOptions,
        store: storeState,
        history,
        useMockAuthentication: true,
      },
    );

    return {
      ...component,
    };
  };

  beforeEach(() => {
    jest.resetAllMocks();
    (useMapStateMachine as jest.Mock).mockImplementation(() => mapMachineBaseMock);
  });

  it('renders as expected with layer popup content', async () => {
    const { asFragment } = setup({
      layerPopup: {} as any,
      featureDataset: null,
    });
    expect(asFragment()).toMatchSnapshot();
  });
  describe('fly out behaviour', () => {
    it('fly out is hidden by default', async () => {
      const { queryByText } = setup({
        layerPopup: {} as any,
        featureDataset: null,
      });
      expect(queryByText('View more property info')).toBeNull();
    });

    it('opens fly out when ellipsis is clicked', async () => {
      const { getByTestId, getByText } = setup({
        layerPopup: {} as any,
        featureDataset: null,
      });
      const ellipsis = getByTestId('fly-out-ellipsis');
      act(() => userEvent.click(ellipsis));
      expect(getByText('View more property info')).toBeVisible();
    });

    it('handles view property action for inventory properties', async () => {
      const pid = '123456789';
      const propertyId = '123456789';

      const { getByTestId, getByText } = setup({
        layerPopup: {
          pimsProperty: { properties: { PROPERTY_ID: 1 } },
          data: { PID: pid },
        } as any,
        featureDataset: {
          pimsFeature: {
            type: 'Feature',
            properties: { ...EmptyPropertyLocation, PROPERTY_ID: propertyId },
            geometry: { type: 'Point', coordinates: [] },
          },
          location: { lat: 0, lng: 0 },
          parcelFeature: null,
          regionFeature: null,
          districtFeature: null,
          municipalityFeature: null,
        },
      });
      const ellipsis = getByTestId('fly-out-ellipsis');
      act(() => userEvent.click(ellipsis));
      const link = getByText('View more property info');
      act(() => userEvent.click(link));
      expect(history.location.pathname).toBe(`/mapview/sidebar/property/${propertyId}`);
    });

    it('handles view property action for non-inventory properties', async () => {
      const pid = '123456789';
      const parsedPid = pidParser(pid);
      const { getByTestId, getByText } = setup({
        layerPopup: { data: { PID: pid } } as any,
        featureDataset: {
          parcelFeature: {
            type: 'Feature',
            properties: { ...emptyFullyAttributed, PID: pid },
            geometry: { type: 'Point', coordinates: [] },
          },
          location: { lat: 0, lng: 0 },
          pimsFeature: null,
          regionFeature: null,
          districtFeature: null,
          municipalityFeature: null,
        },
      });
      const ellipsis = getByTestId('fly-out-ellipsis');
      act(() => userEvent.click(ellipsis));
      const link = getByText('View more property info');
      act(() => userEvent.click(link));
      expect(history.location.pathname).toBe(
        `/mapview/sidebar/non-inventory-property/${parsedPid}`,
      );
    });

    it('handles create research file action', async () => {
      const { getByTestId, getByText } = setup({
        layerPopup: {} as any,
        featureDataset: null,

        claims: [Claims.RESEARCH_ADD],
      });
      const ellipsis = getByTestId('fly-out-ellipsis');
      act(() => userEvent.click(ellipsis));
      const link = getByText('Research File - Create new');
      act(() => userEvent.click(link));
      expect(history.location.pathname).toBe('/mapview/sidebar/research/new');
    });

    it('handles create acquisition file action', async () => {
      const { getByTestId, getByText } = setup({
        layerPopup: {} as any,
        featureDataset: null,

        claims: [Claims.ACQUISITION_ADD],
      });
      const ellipsis = getByTestId('fly-out-ellipsis');
      act(() => userEvent.click(ellipsis));
      const link = getByText('Acquisition File - Create new');
      act(() => userEvent.click(link));
      expect(history.location.pathname).toBe('/mapview/sidebar/acquisition/new');
    });
  });
});
