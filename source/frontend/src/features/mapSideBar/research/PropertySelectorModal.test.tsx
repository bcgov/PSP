import { act, render, RenderOptions, userEvent } from '@/utils/test-utils';
import PropertySelectorModal, { IPropertySelectorModalProps } from './PropertySelectorModal';

const onSelectOkHandle = vi.fn();
const onCancelClick = vi.fn();

describe('PropertySelectorModal modal', () => {
  const setup = async (
    renderOptions: RenderOptions & { props?: Partial<IPropertySelectorModalProps> },
  ) => {
    const utils = render(
      <PropertySelectorModal
        {...renderOptions.props}
        isOpened={renderOptions.props?.isOpened ?? true}
        availiableProperties={renderOptions.props?.availiableProperties ?? []}
        onSelectOk={onSelectOkHandle}
        onCancelClick={onCancelClick}
      />,
      {
        ...renderOptions,
      },
    );

    return {
      ...utils,
    };
  };

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('renders as expected', async () => {
    await setup({});
    expect(document.body).toMatchSnapshot();
  });
});
