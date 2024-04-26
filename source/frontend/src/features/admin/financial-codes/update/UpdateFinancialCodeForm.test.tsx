import * as Yup from 'yup';

import { mockFinancialCode } from '@/mocks/index.mock';
import { ApiGen_Concepts_FinancialCode } from '@/models/api/generated/ApiGen_Concepts_FinancialCode';
import { ApiGen_Concepts_FinancialCodeTypes } from '@/models/api/generated/ApiGen_Concepts_FinancialCodeTypes';
import {
  act,
  createAxiosError,
  render,
  RenderOptions,
  screen,
  userEvent,
} from '@/utils/test-utils';

import { IUpdateFinancialCodeFormProps } from './UpdateFinancialCodeContainer';
import UpdateFinancialCodeForm from './UpdateFinancialCodeForm';
import { UpdateFinancialCodeYupSchema } from './UpdateFinancialCodeYupSchema';

const mockProps: IUpdateFinancialCodeFormProps = {
  financialCode: undefined,
  validationSchema: Yup.object().shape({}),
  onSave: vi.fn(),
  onCancel: vi.fn(),
  onError: vi.fn(),
  onSuccess: vi.fn(),
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
    vi.resetAllMocks();
  });

  it('renders as expected', () => {
    const { asFragment } = setup();
    expect(asFragment()).toMatchSnapshot();
  });

  it('calls onCancel when form is not dirty and cancel is clicked', async () => {
    setup();

    const cancelButton = screen.getByText('Cancel');
    await act(async () => userEvent.click(cancelButton));

    expect(mockProps.onCancel).toHaveBeenCalled();
  });

  it('displays modal when form is dirty and cancel is clicked', async () => {
    setup();

    const description = document.querySelector(`input[name="description"]`) as HTMLInputElement;
    const cancelButton = screen.getByText('Cancel');
    await act(async () => userEvent.clear(description));
    await act(async () => userEvent.paste(description, `another description`));
    await act(async () => userEvent.click(cancelButton));

    expect(screen.getByText(/Confirm Changes/i)).toBeVisible();
    expect(await screen.findByDisplayValue('another description')).toBeVisible();
  });

  it('calls onCancel when form is dirty, cancel is clicked and modal is confirmed', async () => {
    setup();

    const description = document.querySelector(`input[name="description"]`) as HTMLInputElement;
    const cancelButton = screen.getByText('Cancel');
    await act(async () => userEvent.clear(description));
    await act(async () => userEvent.paste(description, `another description`));
    await act(async () => userEvent.click(cancelButton));
    expect(screen.getByText(/Confirm Changes/i)).toBeVisible();
    const confirmButton = screen.getByText('Yes');
    await act(async () => userEvent.click(confirmButton));

    expect(mockProps.onCancel).toHaveBeenCalled();
    expect(description).toHaveTextContent('');
  });

  it('does not call onCancel when form is dirty, cancel is clicked but modal is not confirmed', async () => {
    setup();

    const description = document.querySelector(`input[name="description"]`) as HTMLInputElement;
    const cancelButton = screen.getByText('Cancel');
    await act(async () => userEvent.clear(description));
    await act(async () => userEvent.paste(description, `another description`));
    await act(async () => userEvent.click(cancelButton));
    expect(screen.getByText(/Confirm Changes/i)).toBeVisible();
    const noButton = screen.getByText('No');
    await act(async () => userEvent.click(noButton));

    expect(mockProps.onCancel).not.toHaveBeenCalled();
    expect(await screen.findByDisplayValue('another description')).toBeVisible();
  });

  it('calls onSave and saves form data as expected', async () => {
    vi.mocked(mockProps.onSave).mockResolvedValue(mockFinancialCode());
    setup();

    const codeValue = document.querySelector(`input[name="code"]`) as HTMLSelectElement;
    const description = document.querySelector(`input[name="description"]`) as HTMLInputElement;
    const saveButton = screen.getByText('Save');
    await act(async () => userEvent.clear(codeValue));
    await act(async () => userEvent.paste(codeValue, 'FOO'));
    await act(async () => userEvent.clear(description));
    await act(async () => userEvent.paste(description, `another description`));
    await act(async () => userEvent.click(saveButton));

    expect(mockProps.onSave).toHaveBeenCalledWith(
      expect.objectContaining<Partial<ApiGen_Concepts_FinancialCode>>({
        type: ApiGen_Concepts_FinancialCodeTypes.BusinessFunction,
        code: 'FOO',
        description: `another description`,
      }),
    );
    expect(mockProps.onSuccess).toHaveBeenCalledWith({ ...mockFinancialCode() });
  });

  it('calls onError when it cannot save the form', async () => {
    vi.mocked(mockProps.onSave).mockRejectedValue(createAxiosError(500));
    setup();

    const description = document.querySelector(`input[name="description"]`) as HTMLInputElement;
    const saveButton = screen.getByText('Save');
    await act(async () => userEvent.clear(description));
    await act(async () => userEvent.paste(description, `another description`));
    await act(async () => userEvent.click(saveButton));

    expect(mockProps.onSave).toHaveBeenCalled();
    expect(mockProps.onError).toHaveBeenCalled();
    expect(mockProps.onSuccess).not.toHaveBeenCalled();
  });

  it('displays validation errors and does not save when given a validation schema', async () => {
    mockProps.validationSchema = UpdateFinancialCodeYupSchema;
    setup();

    const description = document.querySelector(`input[name="description"]`) as HTMLInputElement;
    const saveButton = screen.getByText('Save');
    await act(async () => userEvent.clear(description));
    await act(async () => userEvent.click(saveButton));

    expect(mockProps.onSave).not.toHaveBeenCalled();
    expect(await screen.findByText('Code description is required')).toBeVisible();
  });
});
