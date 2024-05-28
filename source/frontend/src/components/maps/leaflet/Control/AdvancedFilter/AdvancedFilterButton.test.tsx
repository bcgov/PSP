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
  const setup = async (
    renderOptions: RenderOptions & { props?: IAdvanceFilterButtonProps } = {},
  ) => {
    const props: IAdvanceFilterButtonProps = renderOptions.props || {
      onToggle: toggle,
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
    const { asFragment } = await setup();
    expect(asFragment()).toMatchSnapshot();
  });

  it(`renders the advanced filter button in the 'closed' state by default`, async () => {
    const { mapReady, getAdvancedFilterButton } = await setup();
    await waitFor(() => mapReady);

    const button = getAdvancedFilterButton();
    expect(button).toBeInTheDocument();
    expect(button.className).not.toContain('open');
  });

  it(`when filter bar is closed, clicking the button calls 'toggle' callback`, async () => {
    const { mapReady, getAdvancedFilterButton } = await setup({
      props: { onToggle: toggle },
    });
    await waitFor(() => mapReady);

    const button = getAdvancedFilterButton();
    expect(button).toBeInTheDocument();
    await act(async () => userEvent.click(button));
    expect(toggle).toHaveBeenCalled();
  });

  it(`when filter bar is open, clicking the button calls 'toggle' callback`, async () => {
    const { mapReady, getAdvancedFilterButton } = await setup({
      props: { onToggle: toggle },
    });
    await waitFor(() => mapReady);

    const button = getAdvancedFilterButton();
    expect(button).toBeInTheDocument();
    await act(async () => userEvent.click(button));
    expect(toggle).toHaveBeenCalled();
  });
});
