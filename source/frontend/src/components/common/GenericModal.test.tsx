import { fireEvent, render, waitFor } from '@/utils/test-utils';

import GenericModal from './GenericModal';

it('renders correctly', () => {
  const { asFragment } = render(<GenericModal variant="info" />);
  expect(asFragment()).toMatchSnapshot();
});

it('renders title based off of prop...', () => {
  const { getByText } = render(<GenericModal title="Test Title" variant="info" />);
  expect(getByText('Test Title')).toBeInTheDocument();
});

it('renders message based off of prop', () => {
  const { getByText } = render(<GenericModal message="Test Message" variant="info" />);
  expect(getByText('Test Message')).toBeInTheDocument();
});

it('renders button text based off of prop', () => {
  const { getByText } = render(
    <GenericModal okButtonText="Ok Text" cancelButtonText="Cancel Text" variant="info" />,
  );
  expect(getByText('Ok Text')).toBeInTheDocument();
  expect(getByText('Cancel Text')).toBeInTheDocument();
});

it('calls custom ok on click', async () => {
  const mockOk = vi.fn();
  const { getByText } = render(
    <GenericModal handleOk={mockOk} okButtonText="Ok Button" variant="info" />,
  );
  const okButton = getByText('Ok Button');
  await waitFor(() => {
    fireEvent.click(okButton);
  });
  expect(mockOk).toHaveBeenCalledTimes(1);
});

it('calls custom cancel function on click', async () => {
  const mockCancel = vi.fn();
  const { getByText } = render(
    <GenericModal handleCancel={mockCancel} cancelButtonText="Cancel Button" variant="info" />,
  );
  const cancelButton = getByText('Cancel Button');
  await waitFor(() => {
    fireEvent.click(cancelButton);
  });
  expect(mockCancel).toHaveBeenCalledTimes(1);
});
