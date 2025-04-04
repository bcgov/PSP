import { render, fireEvent, screen, act } from '@/utils/test-utils';
import {
  UpdateBcftaOwnershipView,
  IUpdateBcftaOwnershipViewProps,
} from './UpdateBctfaOwnershipView';

describe('UpdateBcftaOwnershipView', () => {
  const setup = (props: Partial<IUpdateBcftaOwnershipViewProps> = {}) => {
    const defaultProps: IUpdateBcftaOwnershipViewProps = {
      onSubmit: vi.fn(),
      isLoading: false,
    };

    return render(<UpdateBcftaOwnershipView {...defaultProps} {...props} />);
  };

  it('renders the component', () => {
    setup();
    expect(screen.getByText(/Update BCTFA Ownership/i)).toBeVisible();
    expect(
      screen.getByText(
        /Uploading this file here will update the BCTFA ownership layer within PIMS to reflect the PIDS listed in the uploaded file./i,
      ),
    ).toBeVisible();
  });

  it('displays a loading backdrop when isLoading is true', () => {
    setup({ isLoading: true });
    expect(screen.getByTestId('filter-backdrop-loading')).toBeVisible();
  });

  it('allows a valid file to be selected', async () => {
    const onSubmit = vi.fn();
    setup({ onSubmit });

    const file = new File(['test content'], 'test.csv', { type: 'text/csv' });
    const fileInput = screen.getByTestId('upload-input');

    await act(async () => {
      fireEvent.change(fileInput, { target: { files: [file] } });
    });

    expect(screen.getByText(/test.csv/i)).toBeVisible();
    expect(screen.getByTestId('file-check-icon')).toBeVisible();
  });

  it('calls onSubmit when the Save button is clicked', async () => {
    const onSubmit = vi.fn();
    setup({ onSubmit });

    const file = new File(['test content'], 'test.csv', { type: 'text/csv' });
    const fileInput = screen.getByTestId('upload-input');

    await act(async () => {
      fireEvent.change(fileInput, { target: { files: [file] } });
    });

    const saveButton = screen.getByText(/Save/i);
    await act(async () => {
      fireEvent.click(saveButton);
    });

    expect(onSubmit).toHaveBeenCalledWith(file);
  });

  it('resets the form when the Cancel button is clicked', async () => {
    setup();

    const file = new File(['test content'], 'test.csv', { type: 'text/csv' });
    const fileInput = screen.getByTestId('upload-input');

    await act(async () => {
      fireEvent.change(fileInput, { target: { files: [file] } });
    });

    expect(screen.getByText(/test.csv/i)).toBeVisible();

    const cancelButton = screen.getByText(/Cancel/i);
    await act(async () => {
      fireEvent.click(cancelButton);
    });

    expect(screen.queryByText(/test.csv/i)).toBeNull();
  });

  it('does not allow multiple files to be selected', async () => {
    setup();

    const file1 = new File(['test content'], 'test1.csv', { type: 'text/csv' });
    const file2 = new File(['test content'], 'test2.csv', { type: 'text/csv' });
    const fileInput = screen.getByTestId('upload-input');

    await act(async () => {
      fireEvent.change(fileInput, { target: { files: [file1, file2] } });
    });

    expect(screen.queryByText(/test1.csv/i)).toBeNull();
    expect(screen.queryByText(/test2.csv/i)).toBeNull();
  });
});
