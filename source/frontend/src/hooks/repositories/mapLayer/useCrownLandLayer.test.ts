import { describe, it, expect, vi, beforeEach } from 'vitest';
import { renderHook, act } from '@testing-library/react';
import { useCrownLandLayer } from './useCrownLandLayer';

vi.mock('@/tenants', () => ({
  useTenant: () => ({
    crownLandLeasesUrl: 'leases-url',
    crownLandLicensesUrl: 'licenses-url',
    crownLandTenuresUrl: 'tenures-url',
    crownLandInventoryUrl: 'inventory-url',
    crownLandInclusionsUrl: 'inclusions-url',
    crownLandSurveyedParcelsUrl: 'surveyed-url',
  }),
}));

const mockExecuteRaw = vi.fn();
const mockLoading = false;

vi.mock('@/hooks/layer-api/useLayerQuery', async () => {
  return {
    useLayerQuery: () => {
      return {
        findMultipleWhereContainsWrapped: {
          execute: vi.fn(),
          loading: false,
        },
        findMultipleWhereContainsBoundaryWrapped: {
          execute: vi.fn(),
          loading: false,
        },
        findMultipleRawWrapped: {
          execute: mockExecuteRaw,
          loading: false,
        },
      };
    },
  };
});

describe('useCrownLandLayer - findMultipleSurveyParcel', () => {
  beforeEach(() => {
    mockExecuteRaw.mockReset();
  });

  function setup() {
    return renderHook(() => useCrownLandLayer());
  }

  it('calls execute with correct params for all fields', async () => {
    const fakeFeatureCollection = { type: 'FeatureCollection', features: [] };
    mockExecuteRaw.mockResolvedValue(fakeFeatureCollection);

    const { result } = setup();
    await act(async () => {
      await result.current.findMultipleSurveyParcel('1', '2', '3', 'KAMLOOPS DISTRICT');
    });

    expect(mockExecuteRaw).toHaveBeenCalledTimes(1);
    const params = mockExecuteRaw.mock.calls[0][0];
    expect(params.get('request')).toBe('GetFeature');
    expect(params.get('cql_filter')).toContain('SECTION 1');
    expect(params.get('cql_filter')).toContain('TOWNSHIP 2');
    expect(params.get('cql_filter')).toContain('RANGE 3');
    expect(params.get('cql_filter')).toContain('KAMLOOPS DIST');
  });

  it('omits empty fields from query', async () => {
    const fakeFeatureCollection = { type: 'FeatureCollection', features: [] };
    mockExecuteRaw.mockResolvedValue(fakeFeatureCollection);

    const { result } = setup();
    await act(async () => {
      await result.current.findMultipleSurveyParcel(undefined, '2', undefined, undefined);
    });

    expect(mockExecuteRaw).toHaveBeenCalledTimes(1);
    const params = mockExecuteRaw.mock.calls[0][0];
    expect(params.get('cql_filter')).toContain('TOWNSHIP 2');
    expect(params.get('cql_filter')).not.toContain('SECTION');
    expect(params.get('cql_filter')).not.toContain('KAMLOOPS');
  });

  it('handles empty query gracefully', async () => {
    const fakeFeatureCollection = { type: 'FeatureCollection', features: [] };
    mockExecuteRaw.mockResolvedValue(fakeFeatureCollection);

    const { result } = setup();
    await act(async () => {
      await result.current.findMultipleSurveyParcel();
    });

    expect(mockExecuteRaw).toHaveBeenCalledTimes(1);
    const params = mockExecuteRaw.mock.calls[0][0];
    expect(params.get('cql_filter')).toBeFalsy();
    expect(params.get('request')).toBe('GetFeature');
  });

  it('returns the feature collection as expected', async () => {
    const fakeFeatureCollection = { type: 'FeatureCollection', features: [{ id: 1 }] };
    mockExecuteRaw.mockResolvedValue(fakeFeatureCollection);

    const { result } = setup();
    let output;
    await act(async () => {
      output = await result.current.findMultipleSurveyParcel('1', '2', '3', 'DISTRICT');
    });

    expect(output).toEqual(fakeFeatureCollection);
  });

  it('handles section and range logic for query', async () => {
    const fakeFeatureCollection = { type: 'FeatureCollection', features: [] };
    mockExecuteRaw.mockResolvedValue(fakeFeatureCollection);

    const { result } = setup();
    await act(async () => {
      await result.current.findMultipleSurveyParcel('1', undefined, '3', undefined);
    });

    expect(mockExecuteRaw).toHaveBeenCalledTimes(1);
    const params = mockExecuteRaw.mock.calls[0][0];
    expect(params.get('cql_filter')).toContain('SECTIONS');
    expect(params.get('cql_filter')).toContain('1');
    expect(params.get('cql_filter')).toContain('3');
  });
});
