
import Claims from '@/constants/claims';

import { cleanup, getMockRepositoryObj, render, RenderOptions, waitForEffects } from '@/utils/test-utils';
import ActivitiesTab, { IActivitiesTabProps } from './ActivitiesTab';
import { mockManagementFileResponse } from '@/mocks/managementFiles.mock';
import { ApiGen_CodeTypes_ManagementFileStatusTypes } from '@/models/api/generated/ApiGen_CodeTypes_ManagementFileStatusTypes';
import { toTypeCode } from '@/utils/formUtils';

const mockManagementFileApi = mockManagementFileResponse();

const mockGetManagementActivitiesApi = {
  error: undefined,
  response: undefined,
  execute: vi.fn(),
  loading: false,
};

vi.mock('@/hooks/repositories/useManagementActivityRepository', () => ({
  useManagementActivityRepository: () => {
    return {
      getManagementFileActivities: mockGetManagementActivitiesApi,
      getManagementActivities: mockGetManagementActivitiesApi,
      deleteManagementActivity: getMockRepositoryObj(),
    };
  },
}));


describe('ManagementSummaryView component', () => {
  // render component under test
  const setup = async (
    renderOptions: RenderOptions & { props?: Partial<IActivitiesTabProps> },
  ) => {
    const utils = render(
      <ActivitiesTab managementFile={renderOptions?.props?.managementFile ?? mockManagementFileApi}      />,
      {
        ...renderOptions,
        useMockAuthentication: true,
      },
    );

    return { ...utils };
  };

  beforeEach(() => {
    mockGetManagementActivitiesApi.execute.mockResolvedValue([]);
  });

  afterEach(() => {
    cleanup();
    vi.clearAllMocks();
  });

  it('matches snapshot', async () => {
    const { asFragment } = await setup({});
    await waitForEffects();
    expect(asFragment()).toMatchSnapshot();
  });

  it('renders the edit button for users with management edit permissions', async () => {
    const { queryByTestId } = await setup({ claims: [Claims.MANAGEMENT_EDIT] });
    await waitForEffects();

    const editButton = queryByTestId('add-activity-button');
    expect(editButton).toBeInTheDocument();
  });

  it('does not render the edit button for users that do not have management edit permissions', async () => {
    const { queryByTestId } = await setup({
      claims: [],
    });
    await waitForEffects();

    const editButton = queryByTestId('add-activity-button');
    expect(editButton).not.toBeInTheDocument();
  });

  it.each([
    ['Management File Status is "Completed"', ApiGen_CodeTypes_ManagementFileStatusTypes.COMPLETE],
    ['Management File Status is "Archived"', ApiGen_CodeTypes_ManagementFileStatusTypes.ARCHIVED],
    ['Management File Status is "Hold"', ApiGen_CodeTypes_ManagementFileStatusTypes.HOLD],
    ['Management File Status is "Cancelled"', ApiGen_CodeTypes_ManagementFileStatusTypes.CANCELLED],
  ])(
    'renders the warning icon for management files in non-editable status - %s',
    async (_: string, fileStatus: ApiGen_CodeTypes_ManagementFileStatusTypes) => {
      const mockManagementFile = {
        ...mockManagementFileResponse(),
        fileStatusTypeCode: toTypeCode(fileStatus),
      };

      const { queryByTestId } = await setup({
        props: {
          managementFile: mockManagementFile,
        },
        claims: [Claims.MANAGEMENT_EDIT],
      });
      await waitForEffects();

      const icon = queryByTestId('tooltip-icon-1-summary-cannot-edit-tooltip');
      expect(icon).toBeInTheDocument();
    },
  );
});
