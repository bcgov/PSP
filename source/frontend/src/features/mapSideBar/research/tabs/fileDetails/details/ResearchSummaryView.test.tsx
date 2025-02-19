import Claims from '@/constants/claims';
import { getMockResearchFile } from '@/mocks/researchFile.mock';
import { act, render, RenderOptions, userEvent } from '@/utils/test-utils';

import ResearchSummaryView, { IResearchSummaryViewProps } from './ResearchSummaryView';
import Roles from '@/constants/roles';

const setEditMode = vi.fn();

describe('ResearchSummaryView component', () => {
  const setup = (renderOptions: RenderOptions & IResearchSummaryViewProps) => {
    // render component under test
    const utils = render(
      <ResearchSummaryView
        researchFile={renderOptions.researchFile}
        setEditMode={renderOptions.setEditMode}
        isFileFinalStatus={renderOptions.isFileFinalStatus}
      />,
      {
        ...renderOptions,
        useMockAuthentication: true,
      },
    );

    return { ...utils };
  };

  afterEach(() => {
    vi.resetAllMocks();
  });

  it('renders as expected when research file is provided', () => {
    const { asFragment } = setup({
      researchFile: getMockResearchFile(),
      setEditMode,
      isFileFinalStatus: false,
    });
    expect(asFragment()).toMatchSnapshot();
  });

  it('does not render the edit button if the user does not have research edit permissions', () => {
    const { queryByTitle } = setup({
      researchFile: getMockResearchFile(),
      setEditMode,
      claims: [],
      isFileFinalStatus: false,
    });
    const editResearchFile = queryByTitle('Edit research file');
    expect(editResearchFile).toBeNull();
  });

  it('renders the edit button if the user has research edit permissions', () => {
    const { getByTitle } = setup({
      researchFile: getMockResearchFile(),
      setEditMode,
      claims: [Claims.RESEARCH_EDIT],
      isFileFinalStatus: false,
    });
    const editResearchFile = getByTitle('Edit research file');
    expect(editResearchFile).toBeVisible();
  });

  it('switches to Edit mode when edit button is clicked', async () => {
    const { getByTitle } = setup({
      researchFile: getMockResearchFile(),
      setEditMode,
      claims: [Claims.RESEARCH_EDIT],
      isFileFinalStatus: false,
    });
    const editResearchFile = getByTitle('Edit research file');
    await act(async () => userEvent.click(editResearchFile));
    expect(setEditMode).toHaveBeenCalledWith(true);
  });

  it('displays a warning tooltip when the user cannot edit', async () => {
    const { getByTestId } = setup({
      researchFile: getMockResearchFile(),
      setEditMode,
      claims: [Claims.RESEARCH_EDIT],
      isFileFinalStatus: true,
    });
    const editResearchFile = getByTestId('tooltip-icon-research-cannot-edit-tooltip');
    expect(editResearchFile).toBeVisible();
  });

  it('ignores final file status and is editable when user is an admin', async () => {
    const { queryByTestId } = setup({
      researchFile: getMockResearchFile(),
      setEditMode,
      claims: [Claims.RESEARCH_EDIT],
      roles: [Roles.SYSTEM_ADMINISTRATOR],
      isFileFinalStatus: true,
    });
    const editResearchFile = queryByTestId('tooltip-icon-research-cannot-edit-tooltip');
    expect(editResearchFile).toBeNull();
  });
});
