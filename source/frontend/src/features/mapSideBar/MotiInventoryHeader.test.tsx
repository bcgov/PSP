import { Api_Property } from 'models/api/Property';
import { render, RenderOptions, RenderResult, userEvent } from 'utils/test-utils';

import { IMotiInventoryHeaderProps, MotiInventoryHeader } from './MotiInventoryHeader';

const defaultComposedProperty = {
  composedLoading: false,
  apiPropertyLoading: false,
  propertyAssociationsLoading: false,
};

const onZoom = jest.fn();
describe('MotiInventoryHeader component', () => {
  const setup = (
    renderOptions: RenderOptions & IMotiInventoryHeaderProps = {
      composedProperty: defaultComposedProperty,
    },
  ): RenderResult => {
    // render component under test
    const result = render(
      <MotiInventoryHeader composedProperty={renderOptions.composedProperty} onZoom={onZoom} />,
    );
    return result;
  };

  afterEach(() => {
    onZoom.mockClear();
  });

  it('renders as expected', () => {
    const result = setup();
    expect(result.asFragment()).toMatchSnapshot();
  });

  it('renders a spinner when the data is loading', () => {
    const { getByTestId } = setup({
      composedProperty: { ...defaultComposedProperty, apiWrapper: { loading: true } as any },
    });

    const spinner = getByTestId('filter-backdrop-loading');
    expect(spinner).toBeVisible();
  });

  it('displays PID', async () => {
    const testPid = '009-212-434';
    const result = setup({
      composedProperty: {
        ...defaultComposedProperty,
        pid: testPid,
      },
    });
    // PID is shown
    expect(result.getByText(testPid)).toBeVisible();
  });

  it('displays land parcel type', async () => {
    const testProperty: Api_Property = {
      propertyType: { description: 'A land type description' },
    };
    const result = setup({
      composedProperty: {
        ...defaultComposedProperty,
        apiWrapper: { response: testProperty, loading: false } as any,
      },
    });
    // PID is shown
    expect(result.getByText(testProperty?.propertyType?.description as string)).toBeVisible();
  });

  it('allows the active property to be zoomed in', async () => {
    const testProperty: Api_Property = {} as any;

    const { getByTitle } = setup({
      composedProperty: {
        ...defaultComposedProperty,
        apiWrapper: { response: testProperty, loading: false } as any,
      },
    });
    const zoomButton = getByTitle('Zoom Map');
    userEvent.click(zoomButton);
    expect(onZoom).toHaveBeenCalled();
  });

  it('does not allow property zooming if no property is visible', async () => {
    const { getByTitle } = setup({
      composedProperty: {
        ...defaultComposedProperty,
        apiWrapper: { response: undefined, loading: false } as any,
      },
    });

    const zoomButton = getByTitle('Zoom Map');
    userEvent.click(zoomButton);
    expect(onZoom).toHaveBeenCalled();
  });
});
