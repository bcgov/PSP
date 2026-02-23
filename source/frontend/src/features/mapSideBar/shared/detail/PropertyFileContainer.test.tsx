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
import { usePimsHighwayLayer } from '@/hooks/repositories/mapLayer/useHighwayLayer';
import { getMockCrownTenuresLayerResponse } from '@/mocks/crownTenuresLayerResponse.mock';
import { getMockPimsLocationViewLayerResponse, mockLtsaResponse } from '@/mocks/index.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import getMockISSResult from '@/mocks/mockISSResult';
import { getMockResearchFile } from '@/mocks/researchFile.mock';
import { ApiGen_CodeTypes_FileTypes } from '@/models/api/generated/ApiGen_CodeTypes_FileTypes';
import { ApiGen_CodeTypes_LeaseStatusTypes } from '@/models/api/generated/ApiGen_CodeTypes_LeaseStatusTypes';
import { getEmptyProperty } from '@/models/defaultInitializers';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, render, RenderOptions } from '@/utils/test-utils';

import PropertyFileContainer, { IPropertyFileContainerProps } from './PropertyFileContainer';

const mockAxios = new MockAdapter(axios);

vi.mock('@/hooks/repositories/mapLayer/useHighwayLayer');
vi.mocked(usePimsHighwayLayer, { partial: true }).mockReturnValue({
  findMultipleHighwayBoundary: vi.fn().mockResolvedValue([]),
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
    // PIMS location view layer
    mockAxios
      .onGet(new RegExp('ogs-internal/ows.*&typeName=PIMS_PROPERTY_VW'))
      .reply(200, getMockPimsLocationViewLayerResponse());
    // FA ParcelMapBC layer
    mockAxios
      .onGet(new RegExp('https://apps.gov.bc.ca/ext/sgw/geo.allgov*'))
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
    const spy = vi.spyOn(toast, 'error').mockReturnValue(1);
    await setup();
    expect(viewProps).toBeDefined();
    spy.mockRestore();
  });

  it('sets the default tab using the prop value', async () => {
    await setup();
    expect(viewProps?.defaultTabKey).toBe(InventoryTabNames.property);
  });

  it('passes on the expected BASE tabs', async () => {
    await setup();

    expect(viewProps?.tabViews?.map(tab => tab.key)).toEqual([
      InventoryTabNames.title,
      InventoryTabNames.value,
      InventoryTabNames.property,
      InventoryTabNames.pims,
      InventoryTabNames.pmbc,
    ]);
  });

  it('skips the property tab if the property has no id', async () => {
    mockAxios.onGet('properties/').reply(404);
    await setup({
      ...DEFAULT_PROPS,
      fileProperty: { ...DEFAULT_PROPS.fileProperty, property: getEmptyProperty() },
    });

    expect(viewProps?.tabViews?.map(tab => tab.key)).toEqual([
      InventoryTabNames.title,
      InventoryTabNames.value,
    ]);
  });

  it('passes on custom tabs if provided', async () => {
    await setup({
      ...DEFAULT_PROPS,
      customTabs: [{ key: InventoryTabNames.research, name: 'research', content: <></> }],
    });

    expect(viewProps?.tabViews?.map(tab => tab.key)).toEqual([
      InventoryTabNames.title,
      InventoryTabNames.value,
      InventoryTabNames.research,
      InventoryTabNames.property,
      InventoryTabNames.pims,
      InventoryTabNames.pmbc,
    ]);
  });

  it('renders expected tabs when crown layer returns data', async () => {
    mockAxios
      .onGet(
        new RegExp('https://openmaps.gov.bc.ca/geo/pub/WHSE_TANTALIS.TA_CROWN_TENURES_SVW/wfs*'),
      )
      .reply(200, getMockCrownTenuresLayerResponse());

    await setup();
    expect(viewProps?.tabViews?.map(tab => tab.key)).toEqual([
      InventoryTabNames.title,
      InventoryTabNames.value,
      InventoryTabNames.property,
      InventoryTabNames.pims,
      InventoryTabNames.pmbc,
    ]);
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
    expect(mockAxios.history.get.map(m => m.url)).toContain('/leases/34');
  });

  it('renders PLAN tab when property has a valid plan number', async () => {
    await setup({
      ...DEFAULT_PROPS,
      fileProperty: {
        ...DEFAULT_PROPS.fileProperty,
        property: {
          ...DEFAULT_PROPS.fileProperty.property,
          planNumber: 'VIS1234',
        },
      },
    });
    expect(viewProps?.tabViews?.map(tab => tab.key)).toContain(InventoryTabNames.plan);
  });

  it.each<ApiGen_CodeTypes_FileTypes>([
    ApiGen_CodeTypes_FileTypes.Acquisition,
    ApiGen_CodeTypes_FileTypes.Disposition,
    ApiGen_CodeTypes_FileTypes.Lease,
    ApiGen_CodeTypes_FileTypes.Management,
    ApiGen_CodeTypes_FileTypes.Research,
  ])(
    'renders MANAGEMENT tab for all file types when user has management permissions',
    async (fileType: ApiGen_CodeTypes_FileTypes) => {
      await setup(
        {
          ...DEFAULT_PROPS,
          fileContext: fileType,
        },
        { claims: [Claims.MANAGEMENT_VIEW] },
      );
      expect(viewProps?.tabViews?.map(tab => tab.key)).toContain(InventoryTabNames.management);
    },
  );

  it.each<ApiGen_CodeTypes_FileTypes>([
    ApiGen_CodeTypes_FileTypes.Acquisition,
    ApiGen_CodeTypes_FileTypes.Disposition,
    ApiGen_CodeTypes_FileTypes.Lease,
    ApiGen_CodeTypes_FileTypes.Management,
    ApiGen_CodeTypes_FileTypes.Research,
  ])('renders DOCUMENTS tab for all file types', async (fileType: ApiGen_CodeTypes_FileTypes) => {
    await setup(
      {
        ...DEFAULT_PROPS,
        fileContext: fileType,
      },
      { claims: [Claims.DOCUMENT_VIEW] },
    );
    expect(viewProps?.tabViews?.map(tab => tab.key)).toContain(InventoryTabNames.documents);
  });

  it.each<ApiGen_CodeTypes_FileTypes>([
    ApiGen_CodeTypes_FileTypes.Acquisition,
    ApiGen_CodeTypes_FileTypes.Disposition,
    ApiGen_CodeTypes_FileTypes.Lease,
    ApiGen_CodeTypes_FileTypes.Management,
    ApiGen_CodeTypes_FileTypes.Research,
  ])('renders NOTES tab for all file types', async (fileType: ApiGen_CodeTypes_FileTypes) => {
    await setup(
      {
        ...DEFAULT_PROPS,
        fileContext: fileType,
      },
      { claims: [Claims.NOTE_VIEW] },
    );
    expect(viewProps?.tabViews?.map(tab => tab.key)).toContain(InventoryTabNames.notes);
  });

  it('renders expected tabs when Highways layer returns data', async () => {
    vi.mocked(usePimsHighwayLayer().findMultipleHighwayBoundary).mockResolvedValue(
      getMockISSResult().features,
    );

    await setup();
    expect(viewProps?.tabViews?.map(tab => tab.key)).toContain(InventoryTabNames.highway);
  });

  it('renders TAKES tab for acquisition files', async () => {
    await setup(
      {
        ...DEFAULT_PROPS,
        fileContext: ApiGen_CodeTypes_FileTypes.Acquisition,
      },
      { claims: [Claims.ACQUISITION_VIEW] },
    );
    expect(viewProps?.tabViews?.map(tab => tab.key)).toContain(InventoryTabNames.takes);
  });

  it('renders TAKES tab for acquisition files', async () => {
    await setup(
      {
        ...DEFAULT_PROPS,
        fileContext: ApiGen_CodeTypes_FileTypes.Acquisition,
      },
      { claims: [Claims.ACQUISITION_VIEW] },
    );
    expect(viewProps?.tabViews?.map(tab => tab.key)).toContain(InventoryTabNames.takes);
  });

  it('renders PROPERTY-RESEARCH tab for research files', async () => {
    await setup(
      {
        ...DEFAULT_PROPS,
        fileContext: ApiGen_CodeTypes_FileTypes.Research,
      },
      { claims: [Claims.RESEARCH_VIEW] },
    );
    expect(viewProps?.tabViews?.map(tab => tab.key)).toContain(InventoryTabNames.research);
  });
});
