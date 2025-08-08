import { act, render, RenderOptions, screen, userEvent } from '@/utils/test-utils';

import MoreOptionsDropdown, { IMoreOptionsDropdownProps } from './MoreOptionsDropdown';

describe('<MoreOptionsDropdown />', () => {
  const onClearAll = vi.fn();
  const onCreateAcquisitionFile = vi.fn();
  const onCreateResearchFile = vi.fn();
  const onCreateDispositionFile = vi.fn();
  const onCreateLeaseFile = vi.fn();
  const onCreateManagementFile = vi.fn();

  beforeEach(() => {
    vi.clearAllMocks();
  });

  const setup = (
    renderOptions: RenderOptions & { props?: Partial<IMoreOptionsDropdownProps> } = {},
  ) => {
    return render(
      <MoreOptionsDropdown
        onClearAll={onClearAll}
        canClearAll={renderOptions.props?.canClearAll}
        ariaLabel={renderOptions.props?.ariaLabel}
        onCreateAcquisitionFile={onCreateAcquisitionFile}
        onCreateResearchFile={onCreateResearchFile}
        onCreateDispositionFile={onCreateDispositionFile}
        onCreateLeaseFile={onCreateLeaseFile}
        onCreateManagementFile={onCreateManagementFile}
      />,
      { ...renderOptions },
    );
  };

  it('renders the toggle button with ARIA label', () => {
    setup();
    const toggle = screen.getByLabelText(/more options/i);
    expect(toggle).toBeInTheDocument();
  });

  it('opens the dropdown and calls onClearAll when enabled', async () => {
    setup({ props: { canClearAll: true } });

    const toggle = screen.getByLabelText(/more options/i);
    await act(async () => userEvent.click(toggle));

    const clearItem = await screen.findByText(/clear list/i);
    expect(clearItem).not.toHaveAttribute('disabled');

    await act(async () => userEvent.click(clearItem));
    expect(onClearAll).toHaveBeenCalled();
  });

  it('disables "Clear list" when canClearAll is false', async () => {
    setup({ props: { canClearAll: false } });

    const toggle = screen.getByLabelText(/more options/i);
    await act(async () => userEvent.click(toggle));

    const clearItem = await screen.findByText(/clear list/i);
    expect(clearItem).toHaveAttribute('aria-disabled', 'true');

    await act(async () => userEvent.click(clearItem));
    expect(onClearAll).not.toHaveBeenCalled();
  });

  it('applies tooltip and matches structure', async () => {
    setup();
    const toggle = screen.getByLabelText(/more options/i);
    await act(async () => userEvent.hover(toggle));
    expect(screen.getByRole('tooltip')).toBeTruthy();
    expect(screen.getByText(/more options\.\.\./i)).toBeVisible();
  });
});
