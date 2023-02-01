import { fireEvent, render, RenderOptions, waitFor } from 'utils/test-utils';

import ResearchMenu, { IResearchMenuProps } from './ResearchMenu';

const onChange = jest.fn();
const onEdit = jest.fn();

describe('ResearchMenu component', () => {
  const setup = (renderOptions: RenderOptions & IResearchMenuProps) => {
    // render component under test
    const component = render(
      <ResearchMenu
        selectedIndex={renderOptions.selectedIndex}
        items={renderOptions.items}
        onChange={renderOptions.onChange}
        onEdit={renderOptions.onEdit}
      />,
      {
        ...renderOptions,
      },
    );

    return {
      component,
    };
  };

  afterEach(() => {
    jest.resetAllMocks();
  });

  it('renders as expected when provided no research file', () => {
    const testItems = ['First label', 'Second', 'Third'];
    const { component } = setup({
      items: testItems,
      selectedIndex: 0,
      onChange,
      onEdit,
    });
    expect(component.asFragment()).toMatchSnapshot();
  });

  it('renders the items ', async () => {
    const testItems = ['First label', 'Second', 'Third'];
    const {
      component: { getByText },
    } = setup({
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
    const testItems = ['First label', 'Second', 'Third'];
    const {
      component: { getByText, getByTestId },
    } = setup({
      items: testItems,
      selectedIndex: 1,
      onChange,
      onEdit,
    });

    expect(getByTestId('menu-item-row-0')).not.toHaveClass('selected');
    expect(getByTestId('menu-item-row-1')).toHaveClass('selected');
    expect(getByTestId('menu-item-row-2')).not.toHaveClass('selected');

    const secondItem = getByText(testItems[2]);
    await waitFor(() => {
      fireEvent.click(secondItem);
    });
    expect(onChange).toHaveBeenCalledWith(2);
  });
});
