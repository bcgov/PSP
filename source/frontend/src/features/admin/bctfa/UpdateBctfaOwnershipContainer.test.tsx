import { render, screen, fireEvent, act } from '@/utils/test-utils';
import { UpdateBctfaOwnershipContainer } from './UpdateBctfaOwnershipContainer';
import { useBctfaOwnershipRepository } from '@/hooks/repositories/useBctfaOwnershipRepository';
import { IUpdateBcftaOwnershipViewProps } from './UpdateBctfaOwnershipView';

vi.mock('@/hooks/repositories/useBctfaOwnershipRepository');

describe('UpdateBctfaOwnershipContainer', () => {
  const ViewMock = vi.fn((props: IUpdateBcftaOwnershipViewProps) => (
    <div>
      <button
        data-testid="submit-button"
        onClick={() => props.onSubmit(new File(['test content'], 'test.csv', { type: 'text/csv' }))}
      >
        Submit
      </button>
      {props.isLoading && <div data-testid="loading-indicator">Loading...</div>}
    </div>
  ));

  const mockExecute = vi.fn();
  const mockLoading = false;

  beforeEach(() => {
    vi.mocked(useBctfaOwnershipRepository).mockReturnValue({
      updateBctfaOwnershipApi: {
        execute: mockExecute,
        loading: mockLoading,
        error: null,
        response: null,
        status: null,
      },
    });
  });

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('renders the View component', () => {
    render(<UpdateBctfaOwnershipContainer View={ViewMock} />);

    expect(ViewMock).toHaveBeenCalledWith(
      expect.objectContaining({
        onSubmit: expect.any(Function),
        isLoading: mockLoading,
      }),
      {},
    );
  });

  it('calls the API when onSubmit is triggered', async () => {
    render(<UpdateBctfaOwnershipContainer View={ViewMock} />);

    const submitButton = screen.getByTestId('submit-button');

    await act(async () => {
      fireEvent.click(submitButton);
    });

    expect(mockExecute).toHaveBeenCalledWith(expect.any(File));
  });

  it('passes the loading state to the View component', () => {
    vi.mocked(useBctfaOwnershipRepository).mockReturnValueOnce({
      updateBctfaOwnershipApi: {
        execute: mockExecute,
        loading: true,
        error: null,
        response: null,
        status: null,
      },
    });

    render(<UpdateBctfaOwnershipContainer View={ViewMock} />);

    expect(screen.getByTestId('loading-indicator')).toBeVisible();
  });
});
