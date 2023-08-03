import { FormikProps } from 'formik';
import { createRef } from 'react';

import { act, render, RenderOptions, userEvent, waitFor } from '@/utils/test-utils';

import GenerateLetterRecipientsModal, {
  IGenerateLetterRecipientsModalProps,
} from './GenerateLetterRecipientsModal';
import { LetterRecipientsForm } from './models/LetterRecipientsForm';

const onGenerateLetterOk = jest.fn();
const onCancelClick = jest.fn();

describe('Expropriation Tab Container View', () => {
  const setup = async (
    renderOptions: RenderOptions & { props?: Partial<IGenerateLetterRecipientsModalProps> },
  ) => {
    const formikRef = createRef<FormikProps<LetterRecipientsForm>>();

    const utils = render(
      <GenerateLetterRecipientsModal
        {...renderOptions.props}
        isOpened={renderOptions.props?.isOpened ?? true}
        recipientList={renderOptions.props?.recipientList ?? []}
        onGenerateLetterOk={onGenerateLetterOk}
        onCancelClick={onCancelClick}
        formikRef={formikRef}
      />,
      {
        ...renderOptions,
      },
    );

    return {
      ...utils,
    };
  };

  afterEach(() => {
    jest.clearAllMocks();
  });

  it('renders as expected', async () => {
    const { asFragment } = await setup({});
    expect(asFragment()).toMatchSnapshot();
  });

  it('should validate that at least one recipient is selected', async () => {
    const { queryByTestId, getByText } = await setup({});
    const saveButton = getByText('Continue');

    await act(async () => {
      await waitFor(() => userEvent.click(saveButton));
    });

    expect(queryByTestId('missing-recipient-error')).toBeInTheDocument();
    expect(onGenerateLetterOk).toHaveBeenCalledTimes(0);
  });
});
