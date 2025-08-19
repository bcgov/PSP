import { vi } from 'vitest';

import { act, render, RenderOptions, screen, userEvent } from '@/utils/test-utils';

import MoreOptionsMenu, { IMoreOptionsMenuProps, MenuOption } from './MoreOptionsMenu';

describe('MoreOptionsMenu component', () => {
  const baseOptions: MenuOption[] = [
    { label: 'Edit', onClick: vi.fn() },
    {
      label: 'Delete',
      disabled: true,
      tooltip: 'Not allowed',
      separator: true,
    },
  ];

  const setup = (
    renderOptions: RenderOptions & { props?: Partial<IMoreOptionsMenuProps> } = {},
  ) => {
    return render(
      <MoreOptionsMenu
        options={renderOptions.props?.options ?? baseOptions}
        variant={renderOptions.props?.variant}
        ariaLabel={renderOptions.props?.ariaLabel}
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

  it('renders toggle with light variant by default', () => {
    setup();
    const toggle = screen.getByRole('button');
    expect(toggle).toBeInTheDocument();
    expect(toggle.querySelector('svg')).toHaveStyle('color: #1a5a96');
  });

  it('renders toggle with dark variant when specified', () => {
    setup({ props: { variant: 'dark' } });
    const toggle = screen.getByRole('button');
    expect(toggle.querySelector('svg')).toHaveStyle('color: #ffffff');
  });

  it('applies tooltip and matches structure', async () => {
    setup();
    const toggle = screen.getByLabelText(/more options/i);
    await act(async () => userEvent.hover(toggle));
    expect(screen.getByRole('tooltip')).toBeTruthy();
    expect(screen.getByText(/more options\.\.\./i)).toBeVisible();
  });

  it('calls onClick handler when enabled item is clicked', async () => {
    const onEdit = vi.fn();
    const options: MenuOption[] = [{ label: 'Edit', onClick: onEdit }];
    setup({ props: { options } });

    await act(async () => userEvent.click(screen.getByRole('button')));
    await act(async () => userEvent.click(screen.getByText('Edit')));
    expect(onEdit).toHaveBeenCalledTimes(1);
  });

  it('renders disabled item as Dropdown.ItemText with tooltip', async () => {
    setup();

    await act(async () => userEvent.click(screen.getByRole('button')));
    const disabledItem = screen.getByText('Delete');
    expect(disabledItem.tagName).toBe('SPAN');
    expect(disabledItem).toHaveClass('dropdown-item-text');

    const tooltipIcon = screen.getByTestId('tooltip-icon-tooltip-1');
    expect(tooltipIcon).toBeInTheDocument();

    // Hover to trigger tooltip
    await act(async () => userEvent.hover(tooltipIcon));

    // Tooltip text should appear in the document
    expect(await screen.findByText('Not allowed')).toBeInTheDocument();
  });

  it('renders a separator before items with separator: true (not first item)', async () => {
    setup();
    await act(async () => userEvent.click(screen.getByRole('button')));
    const dividers = screen.getAllByRole('separator');
    expect(dividers.length).toBe(1);
  });

  it('does not render caret icon in toggle', () => {
    setup();
    const toggle = screen.getByRole('button');
    // The caret from Bootstrap is a ::after style; here we just check no text-based caret
    expect(toggle).not.toHaveTextContent(/▾/);
  });

  it('renders custom icons when provided', async () => {
    const customIcon = <span data-testid="custom-icon">★</span>;
    const options: MenuOption[] = [{ label: 'Star', icon: customIcon }];

    setup({ props: { options } });

    await act(async () => userEvent.click(screen.getByRole('button')));
    expect(screen.getByTestId('custom-icon')).toBeInTheDocument();
  });
});
