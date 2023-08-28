import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { noop } from 'lodash';
import { toast } from 'react-toastify';

import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import {
  IInventoryTabsProps,
  InventoryTabNames,
} from '@/features/mapSideBar/property/InventoryTabs';
import { mockLtsaResponse, mockWfsGetPropertyById } from '@/mocks/index.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { getMockResearchFile } from '@/mocks/researchFile.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, render, RenderOptions } from '@/utils/test-utils';

import PropertyFileContainer, { IPropertyFileContainerProps } from './PropertyFileContainer';

const mockAxios = new MockAdapter(axios);

// mock auth library
jest.mock('@react-keycloak/web');

// Need to mock this library for unit tests
jest.mock('react-visibility-sensor', () => {
  return jest.fn().mockImplementation(({ children }) => {
    if (children instanceof Function) {
      return children({ isVisible: true });
    }
    return children;
  });
});

let viewProps: IInventoryTabsProps | undefined;

const ActivityView = (props: IInventoryTabsProps) => {
  viewProps = props;
  return (
    <>
      <LoadingBackdrop show={props.loading} />
    </>
  );
};

const DEFAULT_PROPS: IPropertyFileContainerProps = {
  View: ActivityView,
  fileProperty: (getMockResearchFile().fileProperties ?? [])[0],
  setEditFileProperty: noop,
  customTabs: [],
  defaultTab: InventoryTabNames.property,
  setEditTakes: noop,
};

describe('PropertyFileContainer component', () => {
  // render component under test
  const setup = async (
    props: IPropertyFileContainerProps = { ...DEFAULT_PROPS },
    renderOptions: RenderOptions = {},
  ) => {
    const utils = render(<PropertyFileContainer {...props} />, {
      store: {
        [lookupCodesSlice.name]: { lookupCodes: mockLookups },
      },
      useMockAuthentication: true,
      claims: renderOptions?.claims ?? [],
      ...renderOptions,
    });

    await act(async () => {}); // Wait for async mount actions to settle

    return {
      ...utils,
    };
  };

  beforeEach(() => {
    mockAxios
      .onGet('properties/495')
      .reply(200, (getMockResearchFile().fileProperties ?? [])[0].property);
    mockAxios.onGet(new RegExp('ogs-internal/ows.*')).reply(200, mockWfsGetPropertyById);
    mockAxios
      .onGet(new RegExp('https://openmaps.gov.bc.ca.*'))
      .reply(200, { features: [{ properties: { PROPERTY_ID: 200 } }] });
    mockAxios
      .onGet(new RegExp('https://delivery.apps.gov.bc.ca/ext/sgw/geo.bca*'))
      .reply(200, { features: [{ properties: { FOLIO_ID: 1, ROLL_NUMBER: 1 } }] });

    mockAxios.onPost(new RegExp('tools/ltsa/*')).reply(200, mockLtsaResponse);
    mockAxios.onGet(new RegExp('properties/495/associations')).reply(200, {
      id: 1,
      leaseAssociations: [],
      researchAssociations: [],
      acquisitionAssociations: [],
      dispositionAssociations: [],
    });
  });

  afterEach(() => {
    mockAxios.resetHistory();
    jest.clearAllMocks();
  });

  it('renders as expected', async () => {
    // Need to mock toasts or snapshots will change with each test run
    jest.spyOn(toast, 'success').mockReturnValue(1);
    const { asFragment } = await setup();
    expect(asFragment()).toMatchSnapshot();
    jest.restoreAllMocks();
  });

  it('sets the default tab using the prop value', async () => {
    await setup();
    expect(viewProps?.defaultTabKey).toBe(InventoryTabNames.property);
  });

  it('loads from the expected sources', async () => {
    await setup();

    expect(mockAxios.history.get).toContainEqual(
      expect.objectContaining({ url: '/properties/495' }),
    );
    expect(mockAxios.history.get).toContainEqual(
      expect.objectContaining({ url: '/properties/495/associations' }),
    );
    expect(mockAxios.history.get).toContainEqual(
      expect.objectContaining({
        url: 'https://delivery.apps.gov.bc.ca/ext/sgw/geo.bca?service=WFS&version=2.0.0&outputFormat=json&typeNames=geo.bca%3AWHSE_HUMAN_CULTURAL_ECONOMIC.BCA_FOLIO_LEGAL_DESCRIPTS_SV&srsName=EPSG%3A4326&request=GetFeature&cql_filter=PID_NUMBER+%3D+%27123456789%27',
      }),
    );
    expect(mockAxios.history.get).toContainEqual(
      expect.objectContaining({
        url: 'https://openmaps.gov.bc.ca/geo/pub/WHSE_ADMIN_BOUNDARIES.EBC_ELECTORAL_DISTS_BS10_SVW/wfs?SERVICE=WFS&REQUEST=GetFeature&VERSION=1.3.0&outputFormat=application/json&typeNames=pub:WHSE_ADMIN_BOUNDARIES.EBC_ELECTORAL_DISTS_BS10_SVW&srsName=EPSG:4326&count=1&cql_filter=CONTAINS(SHAPE,SRID=4326;POINT ( -123.128633565 49.27720127104871))',
      }),
    );
    expect(mockAxios.history.get).toContainEqual(
      expect.objectContaining({
        url: `https://openmaps.gov.bc.ca/geo/pub/WHSE_LEGAL_ADMIN_BOUNDARIES.OATS_ALR_POLYS/wfs?SERVICE=WFS&REQUEST=GetFeature&VERSION=1.3.0&outputFormat=application/json&typeNames=pub:WHSE_LEGAL_ADMIN_BOUNDARIES.OATS_ALR_POLYS&srsName=EPSG:4326&count=1&cql_filter=CONTAINS(GEOMETRY,SRID=4326;POINT ( -123.128633565 49.27720127104871))`,
      }),
    );
    expect(mockAxios.history.get).toContainEqual(
      expect.objectContaining({
        url: `https://openmaps.gov.bc.ca/geo/pub/WHSE_ADMIN_BOUNDARIES.ADM_INDIAN_RESERVES_BANDS_SP/wfs?SERVICE=WFS&REQUEST=GetFeature&VERSION=1.3.0&outputFormat=application/json&typeNames=pub:WHSE_ADMIN_BOUNDARIES.ADM_INDIAN_RESERVES_BANDS_SP&srsName=EPSG:4326&count=1&cql_filter=CONTAINS(GEOMETRY,SRID=4326;POINT ( -123.128633565 49.27720127104871))`,
      }),
    );
    expect(mockAxios.history.post).toContainEqual(
      expect.objectContaining({ url: '/tools/ltsa/all?pid=123-456-789' }),
    );
  });

  it('passes on the expected BASE tabs', async () => {
    await setup();

    expect(viewProps?.tabViews).toHaveLength(4);
    expect(viewProps?.tabViews[0].key).toBe(InventoryTabNames.title);
    expect(viewProps?.tabViews[1].key).toBe(InventoryTabNames.value);
    expect(viewProps?.tabViews[2].key).toBe(InventoryTabNames.property);
    expect(viewProps?.tabViews[3].key).toBe(InventoryTabNames.pims);
  });

  it('skips the property tab if the property has no id', async () => {
    mockAxios.onGet('properties/').reply(404);
    await setup({
      ...DEFAULT_PROPS,
      fileProperty: { ...DEFAULT_PROPS.fileProperty, property: { id: undefined } },
    });

    expect(viewProps?.tabViews).toHaveLength(2);
    expect(viewProps?.tabViews[0].key).toBe(InventoryTabNames.title);
    expect(viewProps?.tabViews[1].key).toBe(InventoryTabNames.value);
  });

  it('passes on custom tabs if provided', async () => {
    await setup({
      ...DEFAULT_PROPS,
      customTabs: [{ key: InventoryTabNames.research, name: 'research', content: <></> }],
    });

    expect(viewProps?.tabViews).toHaveLength(5);
    expect(viewProps?.tabViews[0].key).toBe(InventoryTabNames.title);
    expect(viewProps?.tabViews[1].key).toBe(InventoryTabNames.value);
    expect(viewProps?.tabViews[2].key).toBe(InventoryTabNames.research);
    expect(viewProps?.tabViews[3].key).toBe(InventoryTabNames.property);
    expect(viewProps?.tabViews[4].key).toBe(InventoryTabNames.pims);
  });
});
