import { act } from 'react-test-renderer';

import Claims from '@/constants/claims';
import { getMockResearchFile } from '@/mocks/researchFile.mock';
import { render, RenderOptions, userEvent, waitFor } from '@/utils/test-utils';

import ResearchTabsContainer, { IResearchTabsContainerProps } from './ResearchTabsContainer';

// mock auth library
jest.mock('@react-keycloak/web');

const setEditKey = jest.fn();
const setEditMode = jest.fn();

describe('ResearchFileTabs component', () => {
  // render component under test
  const setup = (props: IResearchTabsContainerProps, renderOptions: RenderOptions = {}) => {
    const utils = render(
      <ResearchTabsContainer
        researchFile={props.researchFile}
        setEditKey={setEditKey}
        setEditMode={setEditMode}
      />,
      {
        useMockAuthentication: true,
        ...renderOptions,
      },
    );

    return { ...utils };
  };

  afterEach(() => {
    jest.resetAllMocks();
  });

  it('matches snapshot', () => {
    const { asFragment } = setup(
      {
        researchFile: getMockResearchFile(),
        setEditKey,
        setEditMode,
      },
      { claims: [Claims.DOCUMENT_VIEW] },
    );
    expect(asFragment()).toMatchSnapshot();
  });

  it('has a documents tab', () => {
    const { getByText } = setup(
      {
        researchFile: getMockResearchFile(),
        setEditKey,
        setEditMode,
      },
      { claims: [Claims.DOCUMENT_VIEW] },
    );

    const editButton = getByText('Documents');
    expect(editButton).toBeVisible();
  });

  it('documents tab can be changed to', async () => {
    const { getByText } = setup(
      {
        researchFile: getMockResearchFile(),
        setEditKey,
        setEditMode,
      },
      { claims: [Claims.DOCUMENT_VIEW] },
    );

    const editButton = getByText('Documents');
    act(() => {
      userEvent.click(editButton);
    });
    await waitFor(() => {
      expect(getByText('Documents')).toHaveClass('active');
    });
  });

  it('notes tab can be changed to', async () => {
    const { getAllByText } = setup(
      {
        researchFile: getMockResearchFile(),
        setEditKey,
        setEditMode,
      },
      { claims: [Claims.NOTE_VIEW] },
    );

    const editButton = getAllByText('Notes')[0];
    act(() => {
      userEvent.click(editButton);
    });
    await waitFor(() => {
      expect(getAllByText('Notes')[0]).toHaveClass('active');
    });
  });
});
