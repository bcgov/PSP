import { Feature, Geometry } from 'geojson';

import { useHistoricalNumberRepository } from '@/hooks/repositories/useHistoricalNumberRepository';
import { getMockPimsLocationViewLayerResponse } from '@/mocks/data.mock';
import { ApiGen_Concepts_Property } from '@/models/api/generated/ApiGen_Concepts_Property';
import { getEmptyProperty } from '@/models/defaultInitializers';
import { PIMS_Property_Location_View } from '@/models/layers/pimsPropertyLocationView';
import { act, render, RenderOptions, RenderResult } from '@/utils/test-utils';

import { ComposedProperty } from './ComposedProperty';
import { IMotiInventoryHeaderProps, MotiInventoryHeader } from './MotiInventoryHeader';

const defaultComposedProperty: ComposedProperty = {
  pid: undefined,
  pin: undefined,
  id: undefined,
  ltsaOrders: undefined,
  pimsProperty: undefined,
  propertyAssociations: undefined,
  parcelMapFeatureCollection: undefined,
  pimsGeoserverFeatureCollection: undefined,
  bcAssessmentSummary: undefined,
  crownTenureFeatures: undefined,
  planNumber: undefined,
  spcpOrder: undefined,
  crownInclusionFeatures: undefined,
  crownInventoryFeatures: undefined,
  crownLeaseFeatures: undefined,
  crownLicenseFeatures: undefined,
  highwayFeatures: undefined,
  municipalityFeatures: undefined,
  firstNationFeatures: undefined,
  alrFeatures: undefined,
  electoralFeatures: undefined,
};

vi.mock('@/hooks/repositories/useHistoricalNumberRepository');
vi.mocked(useHistoricalNumberRepository).mockReturnValue({
  getPropertyHistoricalNumbers: {
    error: null,
    response: [],
    execute: vi.fn().mockResolvedValue([]),
    loading: false,
    status: 200,
  },
  updatePropertyHistoricalNumbers: {
    error: null,
    response: [],
    execute: vi.fn().mockResolvedValue([]),
    loading: false,
    status: 200,
  },
});

describe('MotiInventoryHeader component', () => {
  const setup = async (
    renderOptions: RenderOptions & IMotiInventoryHeaderProps = {
      composedProperty: defaultComposedProperty,
      isLoading: false,
    },
  ): Promise<RenderResult> => {
    // render component under test
    const result = render(
      <MotiInventoryHeader
        composedProperty={renderOptions.composedProperty}
        isLoading={renderOptions.isLoading}
      />,
    );
    await act(async () => {});
    return result;
  };

  it('renders as expected', async () => {
    const result = await setup();
    expect(result.asFragment()).toMatchSnapshot();
  });

  it('renders a spinner when the data is loading', async () => {
    const { getByTestId } = await setup({
      composedProperty: { ...defaultComposedProperty },
      isLoading: true,
    });

    const spinner = getByTestId('filter-backdrop-loading');
    expect(spinner).toBeVisible();
  });

  it('displays PID', async () => {
    const testPid = '009-212-434';
    const result = await setup({
      composedProperty: {
        ...defaultComposedProperty,
        pid: testPid,
      },
      isLoading: false,
    });
    // PID is shown
    expect(result.getByText(testPid)).toBeVisible();
  });

  it('displays land parcel type', async () => {
    const testProperty: ApiGen_Concepts_Property = {
      ...getEmptyProperty(),
      propertyType: {
        description: 'A land type description',
        displayOrder: null,
        isDisabled: false,
        id: null,
      },
    };
    const result = await setup({
      composedProperty: {
        ...defaultComposedProperty,
        pimsProperty: testProperty,
      },
      isLoading: false,
    });
    // land parcel type is shown
    expect(result.getByText(testProperty?.propertyType?.description as string)).toBeVisible();
  });

  it('displays RETIRED indicator for retired properties', async () => {
    const testProperty: ApiGen_Concepts_Property = {
      ...getEmptyProperty(),
      isRetired: true,
    };
    const result = await setup({
      composedProperty: {
        ...defaultComposedProperty,
        pimsProperty: testProperty,
      },
      isLoading: false,
    });
    // RETIRED indicator is shown
    expect(result.getByText(/retired/i)).toBeVisible();
  });

  it('displays DISPOSED indicator for retired properties', async () => {
    const testProperty: Feature<Geometry, PIMS_Property_Location_View> =
      getMockPimsLocationViewLayerResponse().features[0];
    testProperty.properties = { ...testProperty.properties, IS_DISPOSED: true };

    const result = await setup({
      composedProperty: {
        ...defaultComposedProperty,
        pimsGeoserverFeatureCollection: {
          type: 'FeatureCollection',
          features: [testProperty],
        },
      },
      isLoading: false,
    });
    // DISPOSED indicator is shown
    expect(result.getByText(/disposed/i)).toBeVisible();
  });

  it('displays PIN', async () => {
    const testPin = '9212434';
    const result = await setup({
      composedProperty: {
        ...defaultComposedProperty,
        pin: testPin,
      },
      isLoading: false,
    });
    // PIN is shown
    expect(result.getByText(testPin)).toBeVisible();
  });
});
