import { Claims } from '@/constants/index';
import { render, RenderOptions, userEvent, waitFor } from '@/utils/test-utils';

import ResearchMenu, { IResearchMenuProps } from './ResearchMenu';

// mock auth library
jest.mock('@react-keycloak/web');

const onChange = jest.fn();
const onEdit = jest.fn();
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
    jest.resetAllMocks();
  });

  it('renders as expected when provided no research file', () => {
    const { asFragment } = setup({
      items: testItems,
      selectedIndex: 0,
      onChange,
      onEdit,
    });
    expect(asFragment()).toMatchSnapshot();
  });

  it('renders the items ', async () => {
    const { getByText } = setup({
      items: testItems,
      selectedIndex: 0,
      onChange,
      onEdit,
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
    });

    const lastItem = getByText(testItems[2]);
    await waitFor(() => userEvent.click(lastItem));

    expect(onChange).toHaveBeenCalledWith(2);
  });

  it(`renders the edit button for users with property edit permissions`, async () => {
    const { getByTitle } = setup(
      {
        items: testItems,
        selectedIndex: 0,
        onChange,
        onEdit,
      },
      { claims: [Claims.RESEARCH_EDIT] },
    );

    const button = getByTitle('Change properties');
    expect(button).toBeVisible();
    await waitFor(() => userEvent.click(button));

    expect(onEdit).toHaveBeenCalledWith();
  });

  it(`doesn't render the edit button for users without edit permissions`, () => {
    const { queryByTitle } = setup(
      {
        items: testItems,
        selectedIndex: 1,
        onChange,
        onEdit,
      },
      { claims: [Claims.RESEARCH_VIEW] }, // no edit permissions, just view.
    );

    const button = queryByTitle('Change properties');
    expect(button).toBeNull();
  });
});
