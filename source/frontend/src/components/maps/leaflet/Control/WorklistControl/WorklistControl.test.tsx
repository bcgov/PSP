import { act, render, RenderOptions, screen } from '@/utils/test-utils';
import userEvent from '@testing-library/user-event';
import { vi } from 'vitest';

import WorklistControl, { IWorklistControl } from './WorklistControl';

describe('WorklistControl component', () => {
  const setup = (renderOptions: RenderOptions & { props?: Partial<IWorklistControl> } = {}) => {
    const onToggle = vi.fn();
    render(
      <WorklistControl
        itemCount={renderOptions.props?.itemCount ?? 0}
        active={renderOptions.props?.active}
        onToggle={onToggle}
      />,
      {
        useMockAuthentication: true,
        claims: [],
        ...renderOptions,
      },
    );
    return { onToggle };
  };

  it('renders the worklist button and tooltip', () => {
    setup();
    const button = screen.getByRole('button');
    expect(button).toBeInTheDocument();
    expect(button).toHaveAttribute('id', 'worklistControlButton');
  });

  it('shows no badge when itemCount is 0', () => {
    setup({ props: { itemCount: 0 } });
    const badge = screen.queryByText(/^\d+$/);
    expect(badge).not.toBeInTheDocument();
  });

  it('displays correct count in badge when items exist', () => {
    setup({ props: { itemCount: 5 } });
    const badge = screen.getByText('5');
    expect(badge).toBeInTheDocument();
    expect(badge).toHaveStyle('background-color: #CE3E39'); // danger color
  });

  it('caps badge display at 99+', () => {
    setup({ props: { itemCount: 125 } });
    expect(screen.getByText('99+')).toBeInTheDocument();
  });

  it('triggers onToggle when clicked', async () => {
    const { onToggle } = setup({ props: { itemCount: 10 } });
    const button = screen.getByRole('button');
    await act(async () => userEvent.click(button));
    expect(onToggle).toHaveBeenCalledTimes(1);
  });

  it('renders with active styling when active is true', () => {
    setup({ props: { active: true } });
    const button = screen.getByRole('button');
    expect(button).toHaveStyle('background-color: #013366'); // active background color
  });

  it('renders with inactive styling when active is false', () => {
    setup({ props: { active: false } });
    const button = screen.getByRole('button');
    expect(button).toHaveStyle('background-color: #FFFFFF'); // inactive background color
  });
});
