import { createMemoryHistory } from 'history';

import Claims from '@/constants/claims';
import { mockLookups } from '@/mocks/lookups.mock';
import { emptyPmbcParcel } from '@/models/layers/parcelMapBC';
import { EmptyPropertyLocation } from '@/models/layers/pimsPropertyLocationView';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { pidParser, pinParser } from '@/utils/propertyUtils';
import { act, render, RenderOptions, userEvent } from '@/utils/test-utils';

import { ILayerPopupViewProps, LayerPopupView } from './LayerPopupView';

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

    it('handles view property action for non-inventory properties - with PID', async () => {
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
        `/mapview/sidebar/non-inventory-property/pid/${parsedPid}`,
      );
    });

    it('handles view property action for non-inventory properties - with PIN', async () => {
      const pin = '123456789';
      const parsedPin = pinParser(pin);
      const { getByTestId, getByText } = setup({
        layerPopup: {
          layers: [
            {
              data: { PIN: pin },
              title: '',
              config: {},
            },
          ],
          latlng: undefined,
        },
        featureDataset: {
          parcelFeature: {
            type: 'Feature',
            properties: { ...emptyPmbcParcel, PIN: parsedPin },
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
        `/mapview/sidebar/non-inventory-property/pin/${parsedPin}`,
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

    it('only shows all file options if file not disposed or retired', async () => {
      const { getByTestId, getByText } = setup({
        layerPopup: {
          latlng: undefined,
          layers: [],
        },
        featureDataset: {
          pimsFeature: {
            type: 'Feature',
            properties: {
              ...EmptyPropertyLocation,
              IS_RETIRED: false,
              IS_DISPOSED: false,
              PROPERTY_ID: 1,
            },
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

        claims: [
          Claims.RESEARCH_ADD,
          Claims.ACQUISITION_ADD,
          Claims.DISPOSITION_ADD,
          Claims.LEASE_ADD,
        ],
      });
      const ellipsis = getByTestId('fly-out-ellipsis');
      await act(async () => userEvent.click(ellipsis));
      const link = getByText('Research File');
      expect(getByText('Acquisition File')).toBeVisible();
      expect(getByText('Disposition File')).toBeVisible();
      expect(getByText('Lease/Licence File')).toBeVisible();
      expect(link).toBeVisible();
    });

    it('only shows all file options if file not disposed or retired', async () => {
      const { getByTestId, getByText, queryByText } = setup({
        layerPopup: {
          latlng: undefined,
          layers: [],
        },
        featureDataset: {
          pimsFeature: {
            type: 'Feature',
            properties: {
              ...EmptyPropertyLocation,
              IS_RETIRED: true,
              IS_DISPOSED: true,
              PROPERTY_ID: 1,
            },
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

        claims: [Claims.RESEARCH_ADD],
      });
      const ellipsis = getByTestId('fly-out-ellipsis');
      await act(async () => userEvent.click(ellipsis));
      const link = getByText('Research File');
      expect(queryByText('Acquisition File')).toBeNull();
      expect(queryByText('Disposition File')).toBeNull();
      expect(queryByText('Lease/Licence File')).toBeNull();
      expect(link).toBeVisible();
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

    it('handles create lease and licence file action', async () => {
      const { getByTestId, getByText } = setup({
        layerPopup: {
          latlng: undefined,
          layers: [],
        },
        featureDataset: null,

        claims: [Claims.LEASE_ADD],
      });
      const ellipsis = getByTestId('fly-out-ellipsis');
      await act(async () => userEvent.click(ellipsis));
      const link = getByText('Lease/Licence File');
      await act(async () => userEvent.click(link));
      expect(history.location.pathname).toBe('/mapview/sidebar/lease/new');
    });

    it('handles create disposition file action', async () => {
      const { getByTestId, getByText } = setup({
        layerPopup: {
          latlng: undefined,
          layers: [],
        },
        featureDataset: null,

        claims: [Claims.DISPOSITION_ADD],
      });
      const ellipsis = getByTestId('fly-out-ellipsis');
      await act(async () => userEvent.click(ellipsis));
      const link = getByText('Disposition File');
      await act(async () => userEvent.click(link));
      expect(history.location.pathname).toBe('/mapview/sidebar/disposition/new');
    });
  });
});
