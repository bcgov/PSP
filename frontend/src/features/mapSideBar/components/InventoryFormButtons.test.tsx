import { fireEvent, render, waitFor } from '@testing-library/react';

import InventoryFormButtons from './InventoryFormButtons';

const mockCancel = jest.fn();
const mockSubmit = jest.fn();
it('renders correctly...', () => {
  const { asFragment } = render(
    <InventoryFormButtons onCancel={mockCancel} onSubmit={mockSubmit} />,
  );
  expect(asFragment()).toMatchSnapshot();
});

it('disable prop works as intended', () => {
  const { getByText } = render(
    <InventoryFormButtons disabled onCancel={mockCancel} onSubmit={mockSubmit} />,
  );
  const submitButton = getByText('Save').closest('button');
  const cancelButton = getByText('Cancel').closest('button');
  expect(submitButton).toHaveAttribute('disabled');
  expect(cancelButton).toHaveAttribute('disabled');
});

it('calls appropriate function for saving', async () => {
  const { getByText } = render(
    <InventoryFormButtons onCancel={mockCancel} onSubmit={mockSubmit} />,
  );
  const submitButton = getByText('Save');
  await waitFor(() => {
    fireEvent.click(submitButton);
  });
  expect(mockSubmit).toHaveBeenCalledTimes(1);
});

it('calls appropriate function for cancelling', async () => {
  const { getByText } = render(
    <InventoryFormButtons onCancel={mockCancel} onSubmit={mockSubmit} />,
  );
  const cancelButton = getByText('Cancel');
  await waitFor(() => {
    fireEvent.click(cancelButton);
  });
  expect(mockCancel).toHaveBeenCalledTimes(1);
});
