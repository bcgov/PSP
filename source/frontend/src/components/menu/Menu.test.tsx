import { fireEvent, render } from '@testing-library/react';

import { IMenuItemProps, Menu } from './Menu';

const mockFunction = vi.fn();

const mockOptions: IMenuItemProps[] = [
  {
    label: 'Option 1',
    onClick: mockFunction,
  },
  { label: 'Option 2' },
];
it('renders correctly', () => {
  const { asFragment } = render(<Menu options={mockOptions} />);
  expect(asFragment()).toMatchSnapshot();
});

it('calls appropriate function on click', () => {
  const { getByText } = render(<Menu options={mockOptions} />);
  fireEvent.click(getByText('Option 1'));
  expect(mockFunction).toHaveBeenCalledTimes(1);
});

it('renders label correctly', () => {
  const { getByText } = render(<Menu options={mockOptions} label="Test Label" />);
  expect(getByText('Test Label')).toBeInTheDocument();
});

it('renders all options correctly', () => {
  const { getByText } = render(<Menu options={mockOptions} />);
  expect(getByText('Option 1')).toBeInTheDocument();
  expect(getByText('Option 2')).toBeInTheDocument();
});
