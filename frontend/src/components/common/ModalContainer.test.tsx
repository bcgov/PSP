import { screen } from '@testing-library/react';
import { useModalContext } from 'hooks/useModalContext';
import { mockLookups } from 'mocks';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { render, RenderOptions, userEvent, waitFor } from 'utils/test-utils';

import { ModalProps } from './GenericModal';

const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

jest.mock('@react-keycloak/web');

describe('ModalContainer component', () => {
  const TestComponent = (props: ModalProps) => {
    useModalContext(props);
    return <></>;
  };

  const setup = (renderOptions?: RenderOptions & { modalProps: ModalProps }) => {
    // render component under test
    const component = render(
      <>
        <TestComponent {...renderOptions?.modalProps} />
      </>,
      {
        ...renderOptions,
        store: storeState,
      },
    );

    return {
      ...component,
    };
  };
  it('renders as expected', async () => {
    const { asFragment } = setup();
    const fragment = await waitFor(() => asFragment());
    expect(fragment).toMatchSnapshot();
  });

  it('displays a modal based on props', async () => {
    setup({ modalProps: { title: 'test', message: 'test 2', display: true } });

    expect(await screen.findByText('test')).toBeVisible();
    expect(await screen.findByText('test 2')).toBeVisible();
  });

  it('shows/hides modal', async () => {
    setup({ modalProps: { title: 'test', message: 'test 2', display: true, okButtonText: 'ok' } });

    const okButton = await screen.findByText('ok');
    userEvent.click(okButton);

    await (() => {
      expect(screen.queryByText('test')).toBeNull();
    });
  });
});
