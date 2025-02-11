import { Claims } from '@/constants/index';
import { act, render, RenderOptions, userEvent, waitFor } from '@/utils/test-utils';

import ResearchMenu, { IResearchMenuProps } from './ResearchMenu';
import { getMockResearchFile } from '@/mocks/researchFile.mock';
import { toTypeCode } from '@/utils/formUtils';
import { ApiGen_CodeTypes_ResearchFileStatusTypes } from '@/models/api/generated/ApiGen_CodeTypes_ResearchFileStatusTypes';

// mock auth library

const onChange = vi.fn();
const onEdit = vi.fn();
const testItems = ['First label', 'Second', 'Third'];

describe('ResearchMenu component', () => {
  const setup = (props: IResearchMenuProps, renderOptions: RenderOptions = {}) => {
    // render component under test
    const utils = render(
      <ResearchMenu
        selectedIndex={props.selectedIndex}
        items={props.items}
        onChange={props.onChange}
        onEdit={props.onEdit}
        researchFile={props.researchFile}
      />,
      {
        useMockAuthentication: true,
        claims: [Claims.RESEARCH_EDIT],
        ...renderOptions,
      },
    );

    return { ...utils };
  };

  afterEach(() => {
    vi.resetAllMocks();
  });

  it('renders as expected when provided no research file', () => {
    const { asFragment } = setup({
      items: testItems,
      selectedIndex: 0,
      onChange,
      onEdit,
      researchFile: getMockResearchFile(),
    });
    expect(asFragment()).toMatchSnapshot();
  });

  it('renders the items ', async () => {
    const { getByText } = setup({
      items: testItems,
      selectedIndex: 0,
      onChange,
      onEdit,
      researchFile: getMockResearchFile(),
    });

    expect(getByText(testItems[0])).toBeVisible();
    expect(getByText(testItems[1])).toBeVisible();
    expect(getByText(testItems[2])).toBeVisible();
  });

  it('selected item is rendered with different style', async () => {
    const { getByTestId } = setup({
      items: testItems,
      selectedIndex: 1,
      onChange,
      onEdit,
      researchFile: getMockResearchFile(),
    });

    expect(getByTestId('menu-item-row-0')).not.toHaveClass('selected');
    expect(getByTestId('menu-item-row-1')).toHaveClass('selected');
    expect(getByTestId('menu-item-row-2')).not.toHaveClass('selected');
  });

  it('allows the selected item to be changed', async () => {
    const { getByText } = setup({
      items: testItems,
      selectedIndex: 1,
      onChange,
      onEdit,
      researchFile: getMockResearchFile(),
    });

    const lastItem = getByText(testItems[2]);
    await act(async () => userEvent.click(lastItem));

    expect(onChange).toHaveBeenCalledWith(2);
  });

  it(`renders the edit button for users with property edit permissions`, async () => {
    const { getByTitle } = setup(
      {
        items: testItems,
        selectedIndex: 0,
        onChange,
        onEdit,
        researchFile: getMockResearchFile(),
      },
      { claims: [Claims.RESEARCH_EDIT] },
    );

    const button = getByTitle('Change properties');
    expect(button).toBeVisible();
    await act(async () => userEvent.click(button));

    expect(onEdit).toHaveBeenCalled();
  });

  it(`doesn't render the edit button for users without edit permissions`, () => {
    const { queryByTitle } = setup(
      {
        items: testItems,
        selectedIndex: 1,
        onChange,
        onEdit,
        researchFile: getMockResearchFile(),
      },
      { claims: [Claims.RESEARCH_VIEW] }, // no edit permissions, just view.
    );

    const button = queryByTitle('Change properties');
    expect(button).toBeNull();
  });

  it(`doesn't render the edit button and displays tooltip for files in final status`, () => {
    const { queryByTitle, queryByTestId } = setup(
      {
        items: testItems,
        selectedIndex: 1,
        onChange,
        onEdit,
        researchFile: {
          ...getMockResearchFile(),
          fileStatusTypeCode: toTypeCode(ApiGen_CodeTypes_ResearchFileStatusTypes.CLOSED),
        },
      },
      { claims: [Claims.RESEARCH_VIEW] }, // no edit permissions, just view.
    );

    const button = queryByTitle('Change properties');
    expect(button).toBeNull();
    expect(queryByTestId('tooltip-icon-1-summary-cannot-edit-tooltip'));
  });
});
