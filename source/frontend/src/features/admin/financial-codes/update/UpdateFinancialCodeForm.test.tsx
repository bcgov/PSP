import { FinancialCodeTypes } from 'constants/index';
import { mockFinancialCode } from 'mocks';
import { Api_FinancialCode } from 'models/api/FinancialCode';
import { act, createAxiosError, render, RenderOptions, screen, userEvent } from 'utils/test-utils';
import * as Yup from 'yup';

import { IUpdateFinancialCodeFormProps } from './UpdateFinancialCodeContainer';
import UpdateFinancialCodeForm from './UpdateFinancialCodeForm';
import { UpdateFinancialCodeYupSchema } from './UpdateFinancialCodeYupSchema';

const mockProps: IUpdateFinancialCodeFormProps = {
  financialCode: undefined,
  validationSchema: Yup.object().shape({}),
  onSave: jest.fn(),
  onCancel: jest.fn(),
  onError: jest.fn(),
  onSuccess: jest.fn(),
};

describe('UpdateFinancialCode form', () => {
  const setup = (renderOptions: RenderOptions = {}) => {
    const utils = render(
      <UpdateFinancialCodeForm
        financialCode={mockProps.financialCode}
        validationSchema={mockProps.validationSchema}
        onSave={mockProps.onSave}
        onSuccess={mockProps.onSuccess}
        onCancel={mockProps.onCancel}
        onError={mockProps.onError}
      />,
      {
        ...renderOptions,
      },
    );
    return { ...utils };
  };

  beforeEach(() => {
    // reset mock yup validation between tests
    mockProps.validationSchema = Yup.object().shape({});
    mockProps.financialCode = mockFinancialCode();
    jest.resetAllMocks();
  });

  it('renders as expected', () => {
    const { asFragment } = setup();
    expect(asFragment()).toMatchSnapshot();
  });

  it('calls onCancel when form is not dirty and cancel is clicked', async () => {
    setup();

    const cancelButton = screen.getByText('Cancel');
    await act(() => userEvent.click(cancelButton));

    expect(mockProps.onCancel).toHaveBeenCalled();
  });

  it('displays modal when form is dirty and cancel is clicked', async () => {
    setup();

    const description = document.querySelector(`input[name="description"]`) as HTMLInputElement;
    const cancelButton = screen.getByText('Cancel');
    await act(() => userEvent.clear(description));
    await act(() => userEvent.paste(description, `another description`));
    await act(() => userEvent.click(cancelButton));

    expect(screen.getByText('Unsaved Changes')).toBeVisible();
    expect(await screen.findByDisplayValue('another description')).toBeVisible();
  });

  it('calls onCancel when form is dirty, cancel is clicked and modal is confirmed', async () => {
    setup();

    const description = document.querySelector(`input[name="description"]`) as HTMLInputElement;
    const cancelButton = screen.getByText('Cancel');
    await act(() => userEvent.clear(description));
    await act(() => userEvent.paste(description, `another description`));
    await act(() => userEvent.click(cancelButton));
    expect(screen.getByText('Unsaved Changes')).toBeVisible();
    const confirmButton = screen.getByText('Confirm');
    await act(() => userEvent.click(confirmButton));

    expect(mockProps.onCancel).toHaveBeenCalled();
    expect(description).toHaveTextContent('');
  });

  it('does not call onCancel when form is dirty, cancel is clicked but modal is not confirmed', async () => {
    setup();

    const description = document.querySelector(`input[name="description"]`) as HTMLInputElement;
    const cancelButton = screen.getByText('Cancel');
    await act(() => userEvent.clear(description));
    await act(() => userEvent.paste(description, `another description`));
    await act(() => userEvent.click(cancelButton));
    expect(screen.getByText('Unsaved Changes')).toBeVisible();
    const noButton = screen.getByText('No');
    await act(() => userEvent.click(noButton));

    expect(mockProps.onCancel).not.toHaveBeenCalled();
    expect(await screen.findByDisplayValue('another description')).toBeVisible();
  });

  it('calls onSave and saves form data as expected', async () => {
    (mockProps.onSave as jest.Mock).mockResolvedValue(mockFinancialCode());
    setup();

    const codeValue = document.querySelector(`input[name="code"]`) as HTMLSelectElement;
    const description = document.querySelector(`input[name="description"]`) as HTMLInputElement;
    const saveButton = screen.getByText('Save');
    await act(() => userEvent.clear(codeValue));
    await act(() => userEvent.paste(codeValue, 'FOO'));
    await act(() => userEvent.clear(description));
    await act(() => userEvent.paste(description, `another description`));
    await act(() => userEvent.click(saveButton));

    expect(mockProps.onSave).toHaveBeenCalledWith(
      expect.objectContaining<Partial<Api_FinancialCode>>({
        type: FinancialCodeTypes.BusinessFunction,
        code: 'FOO',
        description: `another description`,
      }),
    );
    expect(mockProps.onSuccess).toHaveBeenCalledWith({ ...mockFinancialCode() });
  });

  it('calls onError when it cannot save the form', async () => {
    (mockProps.onSave as jest.Mock).mockRejectedValue(createAxiosError(500));
    setup();

    const description = document.querySelector(`input[name="description"]`) as HTMLInputElement;
    const saveButton = screen.getByText('Save');
    await act(() => userEvent.clear(description));
    await act(() => userEvent.paste(description, `another description`));
    await act(() => userEvent.click(saveButton));

    expect(mockProps.onSave).toHaveBeenCalled();
    expect(mockProps.onError).toHaveBeenCalled();
    expect(mockProps.onSuccess).not.toHaveBeenCalled();
  });

  it('displays validation errors and does not save when given a validation schema', async () => {
    mockProps.validationSchema = UpdateFinancialCodeYupSchema;
    setup();

    const description = document.querySelector(`input[name="description"]`) as HTMLInputElement;
    const saveButton = screen.getByText('Save');
    await act(() => userEvent.clear(description));
    await act(() => userEvent.click(saveButton));

    expect(mockProps.onSave).not.toHaveBeenCalled();
    expect(await screen.findByText('Code description is required')).toBeVisible();
  });
});
