import { act, render, RenderOptions, userEvent } from '@/utils/test-utils';

import AdvancedFilterBar, { IAdvancedFilterBarProps } from './AdvancedFilterBar';

const toggle = jest.fn();

describe('AdvancedFilterBar', () => {
  const setup = async (renderOptions: RenderOptions & { props?: IAdvancedFilterBarProps } = {}) => {
    const props: IAdvancedFilterBarProps = {
      toggle: renderOptions?.props?.toggle ? renderOptions.props.toggle : toggle,
      isOpen: renderOptions?.props?.isOpen ? renderOptions.props.isOpen : false,
    };

    const utils = render(<AdvancedFilterBar {...props} />, {
      ...renderOptions,
    });

    return { ...utils };
  };

  it('renders as expected', async () => {
    const { asFragment } = await setup();
    expect(asFragment()).toMatchSnapshot();
  });

  it('renders the sidebar collapsed by default', async () => {
    const { getByTestId } = await setup();
    const sidebar = getByTestId('advanced-filter-sidebar');
    expect(sidebar).not.toBeVisible();
  });

  it(`expands the sidebar based on 'isOpen' prop`, async () => {
    const { getByTestId } = await setup({ props: { isOpen: true, toggle } });
    const sidebar = getByTestId('advanced-filter-sidebar');
    expect(sidebar).toBeVisible();
  });

  it(`calls 'toggle' callback when close button is clicked`, async () => {
    const { getByTitle } = await setup({ props: { isOpen: true, toggle } });
    await act(async () => userEvent.click(getByTitle('close')));
    expect(toggle).toHaveBeenCalled();
  });
});
