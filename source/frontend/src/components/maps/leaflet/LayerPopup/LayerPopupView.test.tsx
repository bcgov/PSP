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
import { useKeycloak } from '@react-keycloak/web';

vi.mock('react-leaflet');

const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

const history = createMemoryHistory();

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

  it('renders as expected with layer popup content', async () => {
    const { asFragment } = setup({
      layerPopup: {
        latlng: undefined,
        layers: [],
      },
      featureDataset: null,
    });
    expect(asFragment()).toMatchSnapshot();
  });
  describe('fly out behaviour', () => {
    it('fly out is hidden by default', async () => {
      const { queryByText } = setup({
        layerPopup: {
          latlng: undefined,
          layers: [],
        },
        featureDataset: null,
      });
      expect(queryByText('View Property Info')).toBeNull();
    });

    it('opens fly out when ellipsis is clicked', async () => {
      const { getByTestId, getByText } = setup({
        layerPopup: {
          latlng: undefined,
          layers: [],
        },
        featureDataset: null,
      });
      const ellipsis = getByTestId('fly-out-ellipsis');
      await act(async () => userEvent.click(ellipsis));
      expect(getByText('View Property Info')).toBeVisible();
    });

    it('handles view property action for inventory properties', async () => {
      const pid = '123456789';
      const propertyId = 123456789;

      const { getByTestId, getByText } = setup({
        layerPopup: {
          latlng: undefined,
          layers: [
            {
              title: '',
              data: { PID: pid },
              config: {},
            },
          ],
        },
        featureDataset: {
          pimsFeature: {
            type: 'Feature',
            properties: { ...EmptyPropertyLocation, PROPERTY_ID: propertyId },
            geometry: { type: 'Point', coordinates: [] },
          },
          location: { lat: 0, lng: 0 },
          fileLocation: null,
          parcelFeature: null,
          regionFeature: null,
          districtFeature: null,
          municipalityFeature: null,
          highwayFeature: null,
          selectingComponentId: null,
          crownLandLeasesFeature: null,
          crownLandLicensesFeature: null,
          crownLandTenuresFeature: null,
          crownLandInventoryFeature: null,
          crownLandInclusionsFeature: null,
        },
      });
      const ellipsis = getByTestId('fly-out-ellipsis');
      await act(async () => userEvent.click(ellipsis));
      const link = getByText('View Property Info');
      await act(async () => userEvent.click(link));
      expect(history.location.pathname).toBe(`/mapview/sidebar/property/${propertyId}`);
    });

    it('handles view property action for non-inventory properties', async () => {
      const pid = '123456789';
      const parsedPid = pidParser(pid);
      const { getByTestId, getByText } = setup({
        layerPopup: {
          layers: [
            {
              data: { PID: pid },
              title: '',
              config: {},
            },
          ],
          latlng: undefined,
        },
        featureDataset: {
          parcelFeature: {
            type: 'Feature',
            properties: { ...emptyPmbcParcel, PID: pid },
            geometry: { type: 'Point', coordinates: [] },
          },
          location: { lat: 0, lng: 0 },
          fileLocation: null,
          pimsFeature: null,
          regionFeature: null,
          districtFeature: null,
          municipalityFeature: null,
          highwayFeature: null,
          selectingComponentId: null,
          crownLandLeasesFeature: null,
          crownLandLicensesFeature: null,
          crownLandTenuresFeature: null,
          crownLandInventoryFeature: null,
          crownLandInclusionsFeature: null,
        },
      });
      const ellipsis = getByTestId('fly-out-ellipsis');
      await act(async () => userEvent.click(ellipsis));
      const link = getByText('View Property Info');
      await act(async () => userEvent.click(link));
      expect(history.location.pathname).toBe(
        `/mapview/sidebar/non-inventory-property/${parsedPid}`,
      );
    });

    it('handles view property action for non-inventory properties where the properties object is null', async () => {
      const pid = '123456789';
      const { getByTestId, getByText } = setup({
        layerPopup: {
          layers: [
            {
              data: { PID: pid },
              title: '',
              config: {},
            },
          ],
          latlng: undefined,
        },
        featureDataset: {
          parcelFeature: {
            type: 'Feature',
            properties: { ...emptyPmbcParcel, PID: pid },
            geometry: { type: 'Point', coordinates: [] },
          },
          location: { lat: 0, lng: 0 },
          fileLocation: null,
          pimsFeature: {
            type: 'Feature',
            properties: null as any,
            geometry: { type: 'Point', coordinates: [] },
          },
          regionFeature: null,
          districtFeature: null,
          municipalityFeature: null,
          highwayFeature: null,
          selectingComponentId: null,
          crownLandLeasesFeature: null,
          crownLandLicensesFeature: null,
          crownLandTenuresFeature: null,
          crownLandInventoryFeature: null,
          crownLandInclusionsFeature: null,
        },
      });
      const ellipsis = getByTestId('fly-out-ellipsis');
      await act(async () => userEvent.click(ellipsis));
      const link = getByText('View Property Info');
      await act(async () => userEvent.click(link));
    });

    it('handles create research file action', async () => {
      const { getByTestId, getByText } = setup({
        layerPopup: {
          latlng: undefined,
          layers: [],
        },
        featureDataset: null,

        claims: [Claims.RESEARCH_ADD],
      });
      const ellipsis = getByTestId('fly-out-ellipsis');
      await act(async () => userEvent.click(ellipsis));
      const link = getByText('Research File');
      await act(async () => userEvent.click(link));
      expect(history.location.pathname).toBe('/mapview/sidebar/research/new');
    });

    it('handles create acquisition file action', async () => {
      const { getByTestId, getByText } = setup({
        layerPopup: {
          latlng: undefined,
          layers: [],
        },
        featureDataset: null,

        claims: [Claims.ACQUISITION_ADD],
      });
      const ellipsis = getByTestId('fly-out-ellipsis');
      await act(async () => userEvent.click(ellipsis));
      const link = getByText('Acquisition File');
      await act(async () => userEvent.click(link));
      expect(history.location.pathname).toBe('/mapview/sidebar/acquisition/new');
    });

    it('hides subdivision and consolidation if not in the pims system', async () => {
      const { getByTestId, getByText, queryByText } = setup({
        layerPopup: {
          latlng: undefined,
          layers: [],
        },
        featureDataset: {
          pimsFeature: null,
          location: { lat: 0, lng: 0 },
          fileLocation: null,
          parcelFeature: null,
          regionFeature: null,
          districtFeature: null,
          municipalityFeature: null,
          highwayFeature: null,
          selectingComponentId: null,
          crownLandLeasesFeature: null,
          crownLandLicensesFeature: null,
          crownLandTenuresFeature: null,
          crownLandInventoryFeature: null,
          crownLandInclusionsFeature: null,
        },
        claims: [Claims.PROPERTY_ADD],
      });
      const ellipsis = getByTestId('fly-out-ellipsis');
      await act(async () => userEvent.click(ellipsis));
      const subdivisionLink = queryByText('Create Subdivision');
      expect(subdivisionLink).not.toBeInTheDocument();
      const consolidationLink = queryByText('Create Consolidation');
      expect(consolidationLink).not.toBeInTheDocument();
    });

    it('handles create subdivision action', async () => {
      const propertyId = 1;

      const { getByTestId, getByText } = setup({
        layerPopup: {
          latlng: undefined,
          layers: [],
        },
        featureDataset: {
          pimsFeature: {
            type: 'Feature',
            properties: { ...EmptyPropertyLocation, PROPERTY_ID: propertyId },
            geometry: { type: 'Point', coordinates: [] },
          },
          location: { lat: 0, lng: 0 },
          fileLocation: null,
          parcelFeature: null,
          regionFeature: null,
          districtFeature: null,
          municipalityFeature: null,
          highwayFeature: null,
          selectingComponentId: null,
          crownLandLeasesFeature: null,
          crownLandLicensesFeature: null,
          crownLandTenuresFeature: null,
          crownLandInventoryFeature: null,
          crownLandInclusionsFeature: null,
        },
        claims: [Claims.PROPERTY_ADD],
      });
      const ellipsis = getByTestId('fly-out-ellipsis');
      await act(async () => userEvent.click(ellipsis));
      const link = getByText('Create Subdivision');
      await act(async () => userEvent.click(link));
      expect(history.location.pathname).toBe('/mapview/sidebar/subdivision/new');
    });

    it('handles create create consolidation action', async () => {
      const propertyId = 1;

      const { getByTestId, getByText } = setup({
        layerPopup: {
          latlng: undefined,
          layers: [],
        },
        featureDataset: {
          pimsFeature: {
            type: 'Feature',
            properties: { ...EmptyPropertyLocation, PROPERTY_ID: propertyId },
            geometry: { type: 'Point', coordinates: [] },
          },
          location: { lat: 0, lng: 0 },
          fileLocation: null,
          parcelFeature: null,
          regionFeature: null,
          districtFeature: null,
          municipalityFeature: null,
          highwayFeature: null,
          selectingComponentId: null,
          crownLandLeasesFeature: null,
          crownLandLicensesFeature: null,
          crownLandTenuresFeature: null,
          crownLandInventoryFeature: null,
          crownLandInclusionsFeature: null,
        },
        claims: [Claims.PROPERTY_ADD],
      });
      const ellipsis = getByTestId('fly-out-ellipsis');
      await act(async () => userEvent.click(ellipsis));
      const link = getByText('Create Consolidation');
      await act(async () => userEvent.click(link));
      expect(history.location.pathname).toBe('/mapview/sidebar/consolidation/new');
    });
  });
});
