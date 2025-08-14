import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import noop from 'lodash/noop';
import { toast } from 'react-toastify';

import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import Claims from '@/constants/claims';
import {
  IInventoryTabsProps,
  InventoryTabNames,
} from '@/features/mapSideBar/property/InventoryTabs';
import { getMockCrownTenuresLayerResponse } from '@/mocks/crownTenuresLayerResponse.mock';
import { getMockPimsLocationViewLayerResponse, mockLtsaResponse } from '@/mocks/index.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { getMockResearchFile } from '@/mocks/researchFile.mock';
import { ApiGen_CodeTypes_LeaseStatusTypes } from '@/models/api/generated/ApiGen_CodeTypes_LeaseStatusTypes';
import { getEmptyProperty } from '@/models/defaultInitializers';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, render, RenderOptions, waitForEffects } from '@/utils/test-utils';

import PropertyFileContainer, { IPropertyFileContainerProps } from './PropertyFileContainer';

const mockAxios = new MockAdapter(axios);

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
  onChildSuccess: noop,
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
    mockAxios
      .onGet(new RegExp('ogs-internal/ows.*'))
      .reply(200, getMockPimsLocationViewLayerResponse());
    mockAxios
      .onGet(
        new RegExp(
          'https://openmaps.gov.bc.ca/geo/pub/WHSE_CADASTRE.PMBC_PARCEL_FABRIC_POLY_FA_SVW/ows*',
        ),
      )
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
    // Crown land layer
    mockAxios
      .onGet(
        new RegExp('https://openmaps.gov.bc.ca/geo/pub/WHSE_TANTALIS.TA_CROWN_TENURES_SVW/wfs*'),
      )
      .reply(200, { features: [] });
  });

  afterEach(() => {
    mockAxios.reset();
    vi.clearAllMocks();
  });

  it('renders as expected', async () => {
    // Need to mock toasts or snapshots will change with each test run
    vi.spyOn(toast, 'error').mockReturnValue(1);
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

    expect(viewProps?.tabViews).toHaveLength(5);
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

    expect(viewProps?.tabViews).toHaveLength(6);
    expect(viewProps?.tabViews[0].key).toBe(InventoryTabNames.title);
    expect(viewProps?.tabViews[1].key).toBe(InventoryTabNames.value);
    expect(viewProps?.tabViews[2].key).toBe(InventoryTabNames.research);
    expect(viewProps?.tabViews[3].key).toBe(InventoryTabNames.property);
    expect(viewProps?.tabViews[4].key).toBe(InventoryTabNames.pims);
    expect(viewProps?.tabViews[5].key).toBe(InventoryTabNames.highway);
  });

  it('shows the crown tab if the property has a TANTALIS record', async () => {
    mockAxios
      .onGet(
        new RegExp('https://openmaps.gov.bc.ca/geo/pub/WHSE_TANTALIS.TA_CROWN_TENURES_SVW/wfs*'),
      )
      .reply(200, getMockCrownTenuresLayerResponse());
    await setup();

    expect(viewProps?.tabViews).toHaveLength(6);
    expect(viewProps?.tabViews[0].key).toBe(InventoryTabNames.title);
    expect(viewProps?.tabViews[1].key).toBe(InventoryTabNames.value);
    expect(viewProps?.tabViews[2].key).toBe(InventoryTabNames.property);
    expect(viewProps?.tabViews[3].key).toBe(InventoryTabNames.pims);
    expect(viewProps?.tabViews[4].key).toBe(InventoryTabNames.crown);
    expect(viewProps?.tabViews[5].key).toBe(InventoryTabNames.highway);
  });

  it('does not call lease endpoints when user does not have lease permissions', async () => {
    mockAxios.onGet(new RegExp('properties/495/associations')).reply(200, {
      id: 1,
      leaseAssociations: [
        {
          id: 34,
          fileNumber: '951547254',
          fileName: '-',
          createdDateTime: '2022-05-13T11:51:29.23',
          createdBy: 'Lease Seed Data',
          status: 'Active',
          statusCode: ApiGen_CodeTypes_LeaseStatusTypes.ACTIVE,
          createdByGuid: null,
        },
      ],
      researchAssociations: [],
      acquisitionAssociations: [],
      dispositionAssociations: [],
    });

    await setup(undefined, { claims: [] });
    await waitForEffects();
    expect(mockAxios.history.get.map(m => m.url)).not.toContain('/leases/34');
  });

  it('calls lease endpoints when user does has lease permissions', async () => {
    mockAxios.onGet(new RegExp('properties/495/associations')).reply(200, {
      id: 1,
      leaseAssociations: [
        {
          id: 34,
          fileNumber: '951547254',
          fileName: '-',
          createdDateTime: '2022-05-13T11:51:29.23',
          createdBy: 'Lease Seed Data',
          status: 'Active',
          statusCode: ApiGen_CodeTypes_LeaseStatusTypes.ACTIVE,
          createdByGuid: null,
        },
      ],
      researchAssociations: [],
      acquisitionAssociations: [],
      dispositionAssociations: [],
    });

    await setup(undefined, { claims: [Claims.LEASE_VIEW] });
    await waitForEffects();
    const test = mockAxios.history.get;
    expect(mockAxios.history.get.map(m => m.url)).toContain('/leases/34');
  });
});
