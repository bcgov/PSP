import Claims from '@/constants/claims';
import { getMockResearchFile } from '@/mocks/researchFile.mock';
import { act, render, RenderOptions, userEvent, waitFor, waitForEffects } from '@/utils/test-utils';

import ResearchSummaryView, { IResearchSummaryViewProps } from './ResearchSummaryView';
import Roles from '@/constants/roles';

const setEditMode = vi.fn();
const mockResearchFile = getMockResearchFile();

describe('ResearchSummaryView component', () => {
  const setup = async (
    renderOptions: RenderOptions & { props?: Partial<IResearchSummaryViewProps> },
  ) => {
    // render component under test
    const utils = render(
      <ResearchSummaryView
        researchFile={renderOptions.props?.researchFile ?? mockResearchFile}
        setEditMode={renderOptions.props?.setEditMode ?? setEditMode}
        isFileFinalStatus={renderOptions.props?.isFileFinalStatus ?? false}
      />,
      {
        ...renderOptions,
        useMockAuthentication: true,
      },
    );

    return { ...utils, useMockAuthentication: true };
  };

  beforeEach(() => {
    vi.resetAllMocks();
  });

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('renders as expected', () => {
    setup({});
    expect(document.body).toMatchSnapshot();
  });

  it('does not render the edit button if the user does not have research edit permissions', async () => {
    const { queryByTitle } = await setup(
      {
        props: {
        researchFile: getMockResearchFile(),
        setEditMode,
        isFileFinalStatus: false,
        },
        claims: [],
      },
    );
    await waitForEffects();
    const editResearchFile = queryByTitle('Edit research file');
    expect(editResearchFile).toBeNull();
  });

  it('renders the edit button if the user has research edit permissions', async () => {
    const { getByTitle } = await setup({
      props: {
        researchFile: getMockResearchFile(),
        setEditMode,
        isFileFinalStatus: false,
      },
      claims: [Claims.RESEARCH_EDIT],
    });
    const editResearchFile = getByTitle('Edit research file');
    expect(editResearchFile).toBeVisible();
  });

  it('switches to Edit mode when edit button is clicked', async () => {
    const { getByTitle } = await setup({
      props: {
        researchFile: getMockResearchFile(),
        setEditMode,
        isFileFinalStatus: false,
      },
      claims: [Claims.RESEARCH_EDIT],
    });
    const editResearchFile = getByTitle('Edit research file');
    await act(async () => userEvent.click(editResearchFile));
    expect(setEditMode).toHaveBeenCalledWith(true);
  });

  it('displays a warning tooltip when the user cannot edit', async () => {
    const { getByTestId } = await setup({
      props: {
        researchFile: getMockResearchFile(),
        setEditMode,
        isFileFinalStatus: true,
      },
      claims: [Claims.RESEARCH_EDIT],
    });
    const editResearchFile = getByTestId('tooltip-icon-research-cannot-edit-tooltip');
    expect(editResearchFile).toBeVisible();
  });

  it('ignores final file status and is editable when user is an admin', async () => {
    const { queryByTestId } = await setup({
      props: {
        researchFile: getMockResearchFile(),
        setEditMode,
        isFileFinalStatus: true,
      },
      claims: [Claims.RESEARCH_EDIT],
      roles: [Roles.SYSTEM_ADMINISTRATOR],
    });
    const editResearchFile = queryByTestId('tooltip-icon-research-cannot-edit-tooltip');
    expect(editResearchFile).toBeNull();
  });
});
