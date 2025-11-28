import { act, render, RenderOptions, screen, userEvent } from '@/utils/test-utils';

import { getMockApiFileProperty } from '@/mocks/fileProperty.mock';
import { getMockApiProperty } from '@/mocks/properties.mock';
import { CopyToWorklist } from './CopyToWorklist';
import { useWorklistContext } from './context/WorklistContext';

vi.mock('./context/WorklistContext');

const addRange = vi.fn();
vi.mocked(useWorklistContext, { partial: true }).mockReturnValue({
  addRange,
});

describe('CopyToWorklist component', () => {
  const setup = (
    renderOptions: RenderOptions & {
      props?: Partial<React.ComponentProps<typeof CopyToWorklist>>;
    } = {},
  ) => {
    const rendered = render(
      <CopyToWorklist
        fileProperties={renderOptions.props?.fileProperties ?? []}
        iconSize={renderOptions.props?.iconSize ?? 24}
      />,
      {
        ...renderOptions,
      },
    );

    return { ...rendered };
  };

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('renders the copy button with correct title', () => {
    setup();
    const button = screen.getByTitle('Copy to worklist');
    expect(button).toBeVisible();
  });

  it('disables button when fileProperties is empty', () => {
    setup({ props: { fileProperties: [] } });
    const button = screen.getByTitle('Copy to worklist');
    expect(button).toBeDisabled();
  });

  it('enables button when fileProperties has items', () => {
    setup({
      props: {
        fileProperties: [
          { ...getMockApiFileProperty(1, { ...getMockApiProperty(), pid: 123456789 }) },
        ],
      },
    });
    const button = screen.getByTitle('Copy to worklist');
    expect(button).not.toBeDisabled();
  });

  it('calls addRange with converted parcel items on click', async () => {
    const fileProperties = [
      getMockApiFileProperty(1),
      getMockApiFileProperty(2, { ...getMockApiProperty(), pid: 987654321 }),
    ];
    setup({ props: { fileProperties } });

    const button = screen.getByTitle('Copy to worklist');
    await act(async () => userEvent.click(button));

    expect(addRange).toHaveBeenCalledWith(expect.any(Array));
    expect(addRange).toHaveBeenCalledTimes(1);
  });

  it('converts multiple file properties to parcel items', async () => {
    const fileProperties = [
      getMockApiFileProperty(1, { ...getMockApiProperty(), pid: 111111111 }),
      getMockApiFileProperty(1, { ...getMockApiProperty(), pid: 222222222 }),
      getMockApiFileProperty(1, { ...getMockApiProperty(), pid: 333333333 }),
    ];
    setup({ props: { fileProperties } });

    const button = screen.getByTitle('Copy to worklist');
    await act(async () => userEvent.click(button));

    const callArgs: any[] = addRange.mock.calls[0][0];
    expect(callArgs).toHaveLength(3);
  });

  it('uses custom iconSize when provided', () => {
    setup({ props: { fileProperties: [getMockApiFileProperty()], iconSize: 32 } });
    const icon = screen.getByTitle('Copy to worklist').querySelector('svg');
    expect(icon).toHaveAttribute('width', '32');
    expect(icon).toHaveAttribute('height', '32');
  });

  it('uses default iconSize of 24 when not provided', () => {
    setup({ props: { fileProperties: [getMockApiFileProperty()] } });
    const icon = screen.getByTitle('Copy to worklist').querySelector('svg');
    expect(icon).toHaveAttribute('width', '24');
    expect(icon).toHaveAttribute('height', '24');
  });
});
