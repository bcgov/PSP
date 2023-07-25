import { screen } from '@testing-library/react';

import { useModalContext } from '@/hooks/useModalContext';
import { mockLookups } from '@/mocks/index.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, render, RenderOptions, userEvent, waitFor } from '@/utils/test-utils';

import { ModalContent } from './GenericModal';

const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

jest.mock('@react-keycloak/web');

describe('ModalContainer component', () => {
  const TestComponent = (props: ModalContent, isVisible: boolean) => {
    useModalContext(props, isVisible);
    return <></>;
  };

  const setup = (
    renderOptions?: RenderOptions & { modalProps: ModalContent; isVisible: boolean },
  ) => {
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
    setup({ modalProps: { title: 'test', message: 'test 2' }, isVisible: true });

    expect(await screen.findByText('test')).toBeVisible();
    expect(await screen.findByText('test 2')).toBeVisible();
  });

  it('shows/hides modal', async () => {
    setup({
      modalProps: { title: 'test', message: 'test 2', okButtonText: 'ok' },
      isVisible: true,
    });

    const okButton = await screen.findByText('ok');
    act(() => userEvent.click(okButton));

    await (() => {
      expect(screen.queryByText('test')).toBeNull();
    });
  });
});
