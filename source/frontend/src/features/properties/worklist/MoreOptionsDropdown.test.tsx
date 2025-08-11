import { act, render, RenderOptions, screen, userEvent } from '@/utils/test-utils';

import { Claims } from '@/constants';
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
      {
        useMockAuthentication: true,
        claims: [],
        ...renderOptions,
      },
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

  it('renders all file creation options in dropdown', async () => {
    setup({
      claims: [
        Claims.RESEARCH_ADD,
        Claims.ACQUISITION_ADD,
        Claims.DISPOSITION_ADD,
        Claims.LEASE_ADD,
        Claims.MANAGEMENT_ADD,
      ],
    });

    const toggle = screen.getByLabelText(/more options/i);
    await act(async () => userEvent.click(toggle));

    expect(screen.getByText(/create acquisition file/i)).toBeInTheDocument();
    expect(screen.getByText(/create research file/i)).toBeInTheDocument();
    expect(screen.getByText(/create disposition file/i)).toBeInTheDocument();
    expect(screen.getByText(/create lease\/licence file/i)).toBeInTheDocument();
    expect(screen.getByText(/create management file/i)).toBeInTheDocument();
  });

  it('calls correct callback when "Create Acquisition File" is clicked', async () => {
    setup({ claims: [Claims.ACQUISITION_ADD] });

    const toggle = screen.getByLabelText(/more options/i);
    await act(async () => userEvent.click(toggle));

    const item = screen.getByText(/create acquisition file/i);
    await act(async () => userEvent.click(item));
    expect(onCreateAcquisitionFile).toHaveBeenCalled();
  });

  it('calls correct callback when "Create Research File" is clicked', async () => {
    setup({ claims: [Claims.RESEARCH_ADD] });

    const toggle = screen.getByLabelText(/more options/i);
    await act(async () => userEvent.click(toggle));

    const item = screen.getByText(/create research file/i);
    await act(async () => userEvent.click(item));
    expect(onCreateResearchFile).toHaveBeenCalled();
  });

  it('calls correct callback when "Create Disposition File" is clicked', async () => {
    setup({ claims: [Claims.DISPOSITION_ADD] });

    const toggle = screen.getByLabelText(/more options/i);
    await act(async () => userEvent.click(toggle));

    const item = screen.getByText(/create disposition file/i);
    await act(async () => userEvent.click(item));
    expect(onCreateDispositionFile).toHaveBeenCalled();
  });

  it('calls correct callback when "Create Lease File" is clicked', async () => {
    setup({ claims: [Claims.LEASE_ADD] });

    const toggle = screen.getByLabelText(/more options/i);
    await act(async () => userEvent.click(toggle));

    const item = screen.getByText(/create lease\/licence file/i);
    await act(async () => userEvent.click(item));
    expect(onCreateLeaseFile).toHaveBeenCalled();
  });

  it('calls correct callback when "Create Management File" is clicked', async () => {
    setup({ claims: [Claims.MANAGEMENT_ADD] });

    const toggle = screen.getByLabelText(/more options/i);
    await act(async () => userEvent.click(toggle));

    const item = screen.getByText(/create management file/i);
    await act(async () => userEvent.click(item));
    expect(onCreateManagementFile).toHaveBeenCalled();
  });
});
