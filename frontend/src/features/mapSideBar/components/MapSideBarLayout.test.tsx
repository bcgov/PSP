import { fireEvent, render, waitFor } from '@testing-library/react';
import { ReactElement } from 'react';
import VisibilitySensor from 'react-visibility-sensor';
import TestCommonWrapper from 'utils/TestCommonWrapper';

import MapSideBarLayout from './MapSideBarLayout';

const mockSetShowSideBar = jest.fn();
jest.mock(
  'react-visibility-sensor',
  (): typeof VisibilitySensor => ({ children, partialVisibility, ...rest }: any) => (
    <div {...rest}>{typeof children === 'function' ? children({ isVisible: true }) : children}</div>
  ),
);

const component = (show: boolean, title: string, children?: ReactElement) => (
  <TestCommonWrapper>
    <MapSideBarLayout
      show={show}
      setShowSideBar={mockSetShowSideBar}
      title={title}
      children={children}
    />
  </TestCommonWrapper>
);

it('renders correctly...', () => {
  const { asFragment } = render(component(true, 'Snapshot Test'));
  expect(asFragment()).toMatchSnapshot();
});

it('renders title as expected...', () => {
  const { getByText } = render(component(true, 'Test Title'));
  expect(getByText('Test Title')).toBeInTheDocument();
});

it('calls appropriate function on close', async () => {
  const { getByText } = render(component(true, 'Test Title'));
  const close = getByText('close');
  await waitFor(() => {
    fireEvent.click(close);
  });
  expect(mockSetShowSideBar).toHaveBeenCalledTimes(1);
});

it('renders children correctly based on props', () => {
  const { getByText } = render(component(true, 'Test Title', <h1>Test Children</h1>));
  expect(getByText('Test Children')).toBeInTheDocument();
});
