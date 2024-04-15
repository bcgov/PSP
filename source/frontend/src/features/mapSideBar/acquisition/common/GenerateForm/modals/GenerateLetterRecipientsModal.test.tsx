import { FormikProps } from 'formik';
import { createRef } from 'react';

import { act, render, RenderOptions, userEvent } from '@/utils/test-utils';

import GenerateLetterRecipientsModal, {
  IGenerateLetterRecipientsModalProps,
} from './GenerateLetterRecipientsModal';
import { LetterRecipientsForm } from './models/LetterRecipientsForm';

const onGenerateLetterOk = vi.fn();
const onCancelClick = vi.fn();

describe('GenerateLetterRecipients modal', () => {
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
    vi.clearAllMocks();
  });

  it('renders as expected', async () => {
    await setup({});
    expect(document.body).toMatchSnapshot();
  });

  it('should validate that at least one recipient is selected', async () => {
    const { queryByTestId, getByText } = await setup({});

    const saveButton = getByText('Continue');
    await act(async () => userEvent.click(saveButton));

    expect(queryByTestId('missing-recipient-error')).toBeInTheDocument();
    expect(onGenerateLetterOk).toHaveBeenCalledTimes(0);
  });
});
