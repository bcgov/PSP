import noop from 'lodash/noop';

import {
  act,
  createMapContainer,
  deferred,
  render,
  RenderOptions,
  userEvent,
  waitFor,
} from '@/utils/test-utils';

import AdvancedFilterButton, { IAdvanceFilterButtonProps } from './AdvancedFilterButton';

const toggle = vi.fn();

describe('AdvancedFilterButton', () => {
  const setup = (renderOptions: RenderOptions & { props?: IAdvanceFilterButtonProps } = {}) => {
    const props: IAdvanceFilterButtonProps = renderOptions.props || {
      onToggle: toggle,
      active: false,
    };

    // create a promise to wait for the map to be ready (which happens after initial render)
    const { promise, resolve } = deferred();
    const ReactMap = createMapContainer(resolve, noop);

    const utils = render(
      <ReactMap>
        <AdvancedFilterButton {...props} />
      </ReactMap>,
      {
        ...renderOptions,
      },
    );

    return {
      ...utils,
      mapReady: promise,
      getAdvancedFilterButton: () => utils.getByTitle('advanced-filter-button'),
    };
  };

  it('renders as expected', async () => {
    const { mapReady, asFragment } = setup();
    await waitFor(() => mapReady);
    expect(asFragment()).toMatchSnapshot();
  });

  it(`renders the advanced filter button in the 'closed' state by default`, async () => {
    const { mapReady, getAdvancedFilterButton } = setup();
    await waitFor(() => mapReady);

    const button = getAdvancedFilterButton();
    expect(button).toBeVisible();
    expect(button.classList).not.toContain('open');
  });

  it(`calls 'toggle' callback when the button is clicked`, async () => {
    const { mapReady, getAdvancedFilterButton } = setup({
      props: { onToggle: toggle },
    });
    await waitFor(() => mapReady);

    const button = getAdvancedFilterButton();
    expect(button).toBeVisible();
    await act(async () => userEvent.click(button));
    expect(toggle).toHaveBeenCalled();
  });

  it.each([
    [true, '#FFFFFF', '#013366'],
    [false, '#013366', '#FFFFFF'],
  ])(
    `applies appropriate styling when 'active' prop value is '%s'`,
    async (active: boolean, expectedCssColor: string, expectedCssBgColor: string) => {
      const { mapReady, getAdvancedFilterButton } = setup({
        props: { onToggle: toggle, active },
      });
      await waitFor(() => mapReady);

      const button = getAdvancedFilterButton();
      expect(button).toBeVisible();
      expect(button).toHaveStyle({
        color: expectedCssColor,
        'background-color': expectedCssBgColor,
      });
    },
  );
});
