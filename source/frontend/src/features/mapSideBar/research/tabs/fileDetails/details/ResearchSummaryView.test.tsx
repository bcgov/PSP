import Claims from '@/constants/claims';
import { getMockResearchFile } from '@/mocks/researchFile.mock';
import { act, render, RenderOptions, userEvent } from '@/utils/test-utils';

import ResearchSummaryView, { IResearchSummaryViewProps } from './ResearchSummaryView';

jest.mock('@react-keycloak/web');

const setEditMode = jest.fn();

describe('ResearchSummaryView component', () => {
  const setup = (renderOptions: RenderOptions & IResearchSummaryViewProps) => {
    // render component under test
    const utils = render(
      <ResearchSummaryView
        researchFile={renderOptions.researchFile}
        setEditMode={renderOptions.setEditMode}
      />,
      {
        ...renderOptions,
        useMockAuthentication: true,
      },
    );

    return { ...utils };
  };

  afterEach(() => {
    jest.resetAllMocks();
  });

  it('renders as expected when research file is provided', () => {
    const { asFragment } = setup({ researchFile: getMockResearchFile(), setEditMode });
    expect(asFragment()).toMatchSnapshot();
  });

  it('does not render the edit button if the user does not have research edit permissions', () => {
    const { queryByTitle } = setup({
      researchFile: getMockResearchFile(),
      setEditMode,
      claims: [],
    });
    const editResearchFile = queryByTitle('Edit research file');
    expect(editResearchFile).toBeNull();
  });

  it('renders the edit button if the user has research edit permissions', () => {
    const { getByTitle } = setup({
      researchFile: getMockResearchFile(),
      setEditMode,
      claims: [Claims.RESEARCH_EDIT],
    });
    const editResearchFile = getByTitle('Edit research file');
    expect(editResearchFile).toBeVisible();
  });

  it('switches to Edit mode when edit button is clicked', async () => {
    const { getByTitle } = setup({
      researchFile: getMockResearchFile(),
      setEditMode,
      claims: [Claims.RESEARCH_EDIT],
    });
    const editResearchFile = getByTitle('Edit research file');
    await act(async () => userEvent.click(editResearchFile));
    expect(setEditMode).toHaveBeenCalledWith(true);
  });
});
