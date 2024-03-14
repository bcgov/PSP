import { createMemoryHistory } from 'history';
import { useMap } from 'react-leaflet';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import Claims from '@/constants/claims';
import { mockLookups } from '@/mocks/lookups.mock';
import { mapMachineBaseMock } from '@/mocks/mapFSM.mock';
import { emptyPmbcParcel } from '@/models/layers/parcelMapBC';
import { EmptyPropertyLocation } from '@/models/layers/pimsPropertyLocationView';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { pidParser } from '@/utils/propertyUtils';
import { act, render, RenderOptions, userEvent } from '@/utils/test-utils';

import { ILayerPopupViewProps, LayerPopupView } from './LayerPopupView';
import { emptyPimsBoundaryFeatureCollection } from '@/components/common/mapFSM/models';

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
      expect(queryByText('View Property info')).toBeNull();
    });

    it('opens fly out when ellipsis is clicked', async () => {
      const { getByTestId, getByText } = setup({
        layerPopup: {} as any,
        featureDataset: null,
      });
      const ellipsis = getByTestId('fly-out-ellipsis');
      act(() => userEvent.click(ellipsis));
      expect(getByText('View Property info')).toBeVisible();
    });

    it('handles view property action for inventory properties', async () => {
      const pid = '123456789';
      const propertyId = 123456789;

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
          selectingComponentId: null,
        },
      });
      const ellipsis = getByTestId('fly-out-ellipsis');
      act(() => userEvent.click(ellipsis));
      const link = getByText('View Property info');
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
            properties: { ...emptyPmbcParcel, PID: pid },
            geometry: { type: 'Point', coordinates: [] },
          },
          location: { lat: 0, lng: 0 },
          pimsFeature: null,
          regionFeature: null,
          districtFeature: null,
          municipalityFeature: null,
          selectingComponentId: null,
        },
      });
      const ellipsis = getByTestId('fly-out-ellipsis');
      act(() => userEvent.click(ellipsis));
      const link = getByText('View Property info');
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
      const link = getByText('Research File');
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
      const link = getByText('Acquisition File');
      act(() => userEvent.click(link));
      expect(history.location.pathname).toBe('/mapview/sidebar/acquisition/new');
    });

    it('Hides subdivision and consolidation if not in the pims system', async () => {
      const { getByTestId, getByText, queryByText } = setup({
        layerPopup: { data: {} } as any,
        featureDataset: {
          pimsFeature: null,
          location: { lat: 0, lng: 0 },
          parcelFeature: null,
          regionFeature: null,
          districtFeature: null,
          municipalityFeature: null,
          selectingComponentId: null,
        },
        claims: [Claims.PROPERTY_ADD],
      });
      const ellipsis = getByTestId('fly-out-ellipsis');
      act(() => userEvent.click(ellipsis));
      const subdivisionLink = queryByText('Create Subdivision');
      expect(subdivisionLink).not.toBeInTheDocument();
      const consolidationLink = queryByText('Create Consolidation');
      expect(consolidationLink).not.toBeInTheDocument();
    });

    it('handles create create subdivision action', async () => {
      const propertyId = 1;

      const { getByTestId, getByText } = setup({
        layerPopup: { data: {} } as any,
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
          selectingComponentId: null,
        },
        claims: [Claims.PROPERTY_ADD],
      });
      const ellipsis = getByTestId('fly-out-ellipsis');
      act(() => userEvent.click(ellipsis));
      const link = getByText('Create Subdivision');
      act(() => userEvent.click(link));
      expect(history.location.pathname).toBe('/mapview/sidebar/subdivision/new');
    });

    it('handles create create consolidation action', async () => {
      const propertyId = 1;

      const { getByTestId, getByText } = setup({
        layerPopup: { data: {} } as any,
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
          selectingComponentId: null,
        },
        claims: [Claims.PROPERTY_ADD],
      });
      const ellipsis = getByTestId('fly-out-ellipsis');
      act(() => userEvent.click(ellipsis));
      const link = getByText('Create Consolidation');
      act(() => userEvent.click(link));
      expect(history.location.pathname).toBe('/mapview/sidebar/consolidation/new');
    });
  });
});
