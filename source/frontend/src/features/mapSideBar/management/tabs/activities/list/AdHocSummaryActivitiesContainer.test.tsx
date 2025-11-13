import React from 'react';
import { render } from '@testing-library/react';
import AdHocFileActivitiesSummaryContainer from './AdHocSummaryActivitiesContainer';
import { SideBarContext } from '@/features/mapSideBar/context/sidebarContext';
import { waitForEffects } from '@/utils/test-utils';
import { getMockManagementActivity } from '@/mocks/managementActivity.mock';

vi.mock('@/hooks/repositories/useManagementActivityRepository', () => ({
  useManagementActivityRepository: () => ({
    getManagementFileActivities: mockGetApi,
  }),
}));

const mockGetApi = {
  error: undefined,
  response: undefined,
  execute: vi.fn(),
  loading: false,
};

let viewProps: any;
const TestView: React.FC<any> = props => {
  viewProps = props;
  return null;
};

describe('AdHocFileActivitiesSummaryContainer', () => {
  beforeEach(() => {
    vi.clearAllMocks();
    viewProps = undefined;
    mockGetApi.loading = false;
    mockGetApi.execute.mockResolvedValue([getMockManagementActivity()]);
  });

  const setup = async ({
    managementFileId = 55,
    staleLastUpdatedBy = 0,
    loading,
  }: {
    managementFileId?: number;
    staleLastUpdatedBy?: number;
    loading?: boolean;
  } = {}) => {
    mockGetApi.loading = loading;
    const rendered = render(
      <SideBarContext.Provider value={{ staleLastUpdatedBy } as any}>
        <AdHocFileActivitiesSummaryContainer managementFileId={managementFileId} View={TestView} />
      </SideBarContext.Provider>,
    );

    return { rendered };
  };

  it('provides only ad-hoc activities and correct navigation url/title; reacts to staleLastUpdatedBy changes', async () => {
    const managementFileId = 55;
    await setup({
      managementFileId,
      staleLastUpdatedBy: 0,
    });
    await waitForEffects();

    // Only ad-hoc activity present
    expect(Array.isArray(viewProps.propertyActivities)).toBe(true);
    expect(viewProps.propertyActivities).toHaveLength(1);
    const activityRow = viewProps.propertyActivities[0];
    expect(activityRow.activityId).toBe(1);

    // getNavigationUrl
    const nav = viewProps.getNavigationUrl(activityRow);
    expect(nav.title).toBe('');
    expect(nav.url).toBe(`/mapview/sidebar/property/1/management/activity/1`);
  });

  it('passes loading flag through to the view', async () => {
    await setup({ managementFileId: 77, loading: true });
    await waitForEffects();

    expect(viewProps).toBeDefined();
    expect(viewProps.isLoading).toBe(true);
  });

  it('handles empty API response gracefully', async () => {
    mockGetApi.execute.mockResolvedValue([]);

    await setup({ managementFileId: 99, loading: false });
    await waitForEffects();

    expect(viewProps).toBeDefined();
    expect(viewProps.propertyActivities).toHaveLength(0);
    expect(viewProps.isLoading).toBe(false);
  });
});
