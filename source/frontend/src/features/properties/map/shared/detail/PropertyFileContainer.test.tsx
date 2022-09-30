import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import LoadingBackdrop from 'components/maps/leaflet/LoadingBackdrop/LoadingBackdrop';
import { IInventoryTabsProps, InventoryTabNames } from 'features/mapSideBar/tabs/InventoryTabs';
import { noop } from 'lodash';
import { mockLtsaResponse, mockWfsGetPropertyById } from 'mocks';
import { mockLookups } from 'mocks/mockLookups';
import { getMockResearchFile } from 'mocks/mockResearchFile';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { render, RenderOptions, waitForElementToBeRemoved } from 'utils/test-utils';

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
};

describe('PropertyFileContainer component', () => {
  // render component under test
  const setup = (
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
    const { asFragment, getByTestId } = setup();
    await waitForElementToBeRemoved(getByTestId('filter-backdrop-loading'));
    expect(asFragment()).toMatchSnapshot();
  });

  it('sets the default tab using the prop value', async () => {
    const { getByTestId } = setup();

    await waitForElementToBeRemoved(getByTestId('filter-backdrop-loading'));
    expect(viewProps?.defaultTabKey).toBe(InventoryTabNames.property);
  });

  it('loads from the expected sources', async () => {
    const { getByTestId } = setup();

    await waitForElementToBeRemoved(getByTestId('filter-backdrop-loading'));
    expect(mockAxios.history.get[0].url).toEqual('/properties/495');
    expect(mockAxios.history.get[1].url).toEqual(
      `ogs-internal/ows?service=wfs&request=GetFeature&typeName=PIMS_PROPERTY_LOCATION_VW&outputformat=json&srsName=EPSG:4326&version=2.0.0&cql_filter=PROPERTY_ID%20ilike%20'%25495%25'`,
    );
    expect(mockAxios.history.get[2].url).toEqual('/properties/495/associations');
    expect(mockAxios.history.post[0].url).toEqual('/tools/ltsa/all?pid=123-456-789');
  });

  it('passes on the expected BASE tabs', async () => {
    const { getByTestId } = setup();

    await waitForElementToBeRemoved(getByTestId('filter-backdrop-loading'));
    expect(viewProps?.tabViews).toHaveLength(4);
    expect(viewProps?.tabViews[0].key).toBe(InventoryTabNames.title);
    expect(viewProps?.tabViews[1].key).toBe(InventoryTabNames.value);
    expect(viewProps?.tabViews[2].key).toBe(InventoryTabNames.property);
    expect(viewProps?.tabViews[3].key).toBe(InventoryTabNames.pims);
  });

  it('skips the property tab if no property is found', async () => {
    mockAxios.onGet('properties/495').reply(404);
    const { getByTestId } = setup();

    await waitForElementToBeRemoved(getByTestId('filter-backdrop-loading'));
    expect(viewProps?.tabViews).toHaveLength(3);
    expect(viewProps?.tabViews[0].key).toBe(InventoryTabNames.title);
    expect(viewProps?.tabViews[1].key).toBe(InventoryTabNames.value);
    expect(viewProps?.tabViews[2].key).toBe(InventoryTabNames.pims);
  });

  it('skips the property associations tab if no property associations are found', async () => {
    mockAxios.onGet(new RegExp('properties/495/associations')).reply(200, {});
    const { getByTestId } = setup();

    await waitForElementToBeRemoved(getByTestId('filter-backdrop-loading'));
    expect(viewProps?.tabViews).toHaveLength(3);
    expect(viewProps?.tabViews[0].key).toBe(InventoryTabNames.title);
    expect(viewProps?.tabViews[1].key).toBe(InventoryTabNames.value);
    expect(viewProps?.tabViews[2].key).toBe(InventoryTabNames.property);
  });

  it('passes on custom tabs if provided', async () => {
    const { getByTestId } = setup({
      ...DEFAULT_PROPS,
      customTabs: [{ key: InventoryTabNames.research, name: 'research', content: <></> }],
    });

    await waitForElementToBeRemoved(getByTestId('filter-backdrop-loading'));
    expect(viewProps?.tabViews).toHaveLength(5);
    expect(viewProps?.tabViews[0].key).toBe(InventoryTabNames.title);
    expect(viewProps?.tabViews[1].key).toBe(InventoryTabNames.value);
    expect(viewProps?.tabViews[2].key).toBe(InventoryTabNames.research);
    expect(viewProps?.tabViews[3].key).toBe(InventoryTabNames.property);
    expect(viewProps?.tabViews[4].key).toBe(InventoryTabNames.pims);
  });
});
