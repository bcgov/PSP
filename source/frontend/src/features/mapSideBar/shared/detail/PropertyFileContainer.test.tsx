import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import noop from 'lodash/noop';
import { toast } from 'react-toastify';

import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import {
  IInventoryTabsProps,
  InventoryTabNames,
} from '@/features/mapSideBar/property/InventoryTabs';
import { mockLtsaResponse, mockWfsGetPropertyById } from '@/mocks/index.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { getMockResearchFile } from '@/mocks/researchFile.mock';
import { getEmptyProperty } from '@/models/defaultInitializers';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, render, RenderOptions } from '@/utils/test-utils';

import PropertyFileContainer, { IPropertyFileContainerProps } from './PropertyFileContainer';

const mockAxios = new MockAdapter(axios);

// mock auth library

// Need to mock this library for unit tests
vi.mock('react-visibility-sensor', () => {
  return {
    default: vi.fn().mockImplementation(({ children }) => {
      if (children instanceof Function) {
        return children({ isVisible: true });
      }
      return children;
    }),
  };
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
  setEditing: noop,
  customTabs: [],
  defaultTab: InventoryTabNames.property,
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
    vi.clearAllMocks();
  });

  it('renders as expected', async () => {
    // Need to mock toasts or snapshots will change with each test run
    vi.spyOn(toast, 'success').mockReturnValue(1);
    const { asFragment } = await setup();
    expect(asFragment()).toMatchSnapshot();
    vi.restoreAllMocks();
  });

  it('sets the default tab using the prop value', async () => {
    await setup();
    expect(viewProps?.defaultTabKey).toBe(InventoryTabNames.property);
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
      fileProperty: { ...DEFAULT_PROPS.fileProperty, property: getEmptyProperty() },
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
