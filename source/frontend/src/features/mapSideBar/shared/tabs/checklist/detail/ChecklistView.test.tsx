import * as API from '@/constants/API';
import { Claims } from '@/constants/index';
import { useApiUsers } from '@/hooks/pims-api/useApiUsers';
import { mockFileChecklistResponse } from '@/mocks/acquisitionFiles.mock';
import { mockDispositionFileResponse } from '@/mocks/dispositionFiles.mock';
import { mockLookups } from '@/mocks/index.mock';
import { ApiGen_Concepts_FileWithChecklist } from '@/models/api/generated/ApiGen_Concepts_FileWithChecklist';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, render, RenderOptions, userEvent } from '@/utils/test-utils';

import { ChecklistView, IChecklistViewProps } from './ChecklistView';

// mock auth library

// mock API service calls
vi.mock('@/hooks/pims-api/useApiUsers');

vi.mocked(useApiUsers).mockReturnValue({
  getUserInfo: vi.fn().mockResolvedValue({}),
} as any);

const mockViewProps: IChecklistViewProps = {
  apiFile: undefined,
  showEditButton: true,
  onEdit: vi.fn(),
  sectionTypeName: API.ACQUISITION_CHECKLIST_SECTION_TYPES,
  editClaim: Claims.ACQUISITION_EDIT,
};

describe('ChecklistView component', () => {
  const setup = (renderOptions: RenderOptions & { props?: Partial<IChecklistViewProps> } = {}) => {
    const utils = render(
      <ChecklistView
        apiFile={mockViewProps.apiFile}
        showEditButton={mockViewProps.showEditButton}
        onEdit={mockViewProps.onEdit}
        sectionTypeName={API.ACQUISITION_CHECKLIST_SECTION_TYPES}
        editClaim={Claims.ACQUISITION_EDIT}
        isFileFinalStatus={renderOptions?.props?.isFileFinalStatus ?? false}
      />,
      {
        store: {
          [lookupCodesSlice.name]: { lookupCodes: mockLookups },
        },
        useMockAuthentication: true,
        claims: renderOptions?.claims ?? [],
        ...renderOptions,
      },
    );

    return {
      ...utils,
    };
  };

  beforeEach(() => {
    mockViewProps.apiFile =
      mockDispositionFileResponse() as unknown as ApiGen_Concepts_FileWithChecklist;
    mockViewProps.apiFile.fileChecklistItems = mockFileChecklistResponse();
  });

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('renders as expected', async () => {
    const { asFragment } = setup();
    expect(asFragment()).toMatchSnapshot();
  });

  it('renders the edit button for users with acquisition edit permissions', async () => {
    const { getByTitle } = setup({ claims: [Claims.ACQUISITION_EDIT] });
    const editButton = getByTitle('Edit checklist');
    expect(editButton).toBeVisible();
    await act(async () => userEvent.click(editButton));
    expect(mockViewProps.onEdit).toHaveBeenCalled();
  });

  it('does not render the edit button for users that do not have acquisition edit permissions', () => {
    const { queryByTitle } = setup({ claims: [] });
    const editResearchFile = queryByTitle('Edit checklist');
    expect(editResearchFile).toBeNull();
  });

  it('does not render the edit button when file in final status', () => {
    const { queryByTitle, getByTestId } = setup({ props: { isFileFinalStatus: true }, claims: [] });
    const editResearchFile = queryByTitle('Edit checklist');
    expect(editResearchFile).toBeNull();
    expect(getByTestId('tooltip-icon-lease-checklist-cannot-edit-tooltip')).toBeVisible();
  });

  it('renders last updated by and last updated on for the overall checklist', () => {
    const { getByText } = setup();
    expect(getByText(/This checklist was last updated Mar 17, 2023 by/i)).toBeVisible();
  });
});
