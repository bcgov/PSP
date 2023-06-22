import * as Yup from 'yup';

import { FinancialCodeTypes } from '@/constants/index';
import { mockFinancialCode } from '@/mocks/index.mock';
import { Api_FinancialCode } from '@/models/api/FinancialCode';
import {
  act,
  createAxiosError,
  render,
  RenderOptions,
  screen,
  userEvent,
} from '@/utils/test-utils';

import { IAddFinancialCodeFormProps } from './AddFinancialCodeContainer';
import AddFinancialCodeForm from './AddFinancialCodeForm';
import { AddFinancialCodeYupSchema } from './AddFinancialCodeYupSchema';

const mockProps: IAddFinancialCodeFormProps = {
  validationSchema: Yup.object().shape({}),
  onSave: jest.fn(),
  onCancel: jest.fn(),
  onError: jest.fn(),
  onSuccess: jest.fn(),
};

describe('AddFinancialCode form', () => {
  const setup = (renderOptions: RenderOptions = {}) => {
    const utils = render(
      <AddFinancialCodeForm
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

  beforeAll(() => {
    // Lock the current datetime as our snapshot has date-dependent fields
    jest.useFakeTimers('modern');
    jest.setSystemTime(new Date('04 Dec 2015 10:15:00 GMT').getTime());
  });

  beforeEach(() => {
    // reset mock yup validation between tests
    mockProps.validationSchema = Yup.object().shape({});
    jest.resetAllMocks();
  });

  afterAll(() => {
    // back to reality...
    jest.useRealTimers();
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
    await act(async () => userEvent.paste(description, `another description`));
    await act(async () => userEvent.click(cancelButton));

    expect(screen.getByText('Unsaved Changes')).toBeVisible();
    expect(await screen.findByDisplayValue('another description')).toBeVisible();
  });

  it('calls onCancel when form is dirty, cancel is clicked and modal is confirmed', async () => {
    setup();

    const description = document.querySelector(`input[name="description"]`) as HTMLInputElement;
    const cancelButton = screen.getByText('Cancel');
    await act(async () => userEvent.paste(description, `another description`));
    await act(async () => userEvent.click(cancelButton));
    expect(screen.getByText('Unsaved Changes')).toBeVisible();
    const confirmButton = screen.getByText('Confirm');
    await act(async () => userEvent.click(confirmButton));

    expect(mockProps.onCancel).toHaveBeenCalled();
    expect(description).toHaveTextContent('');
  });

  it('does not call onCancel when form is dirty, cancel is clicked but modal is not confirmed', async () => {
    setup();

    const textbox = document.querySelector(`input[name="description"]`) as HTMLInputElement;
    const cancelButton = screen.getByText('Cancel');
    await act(async () => userEvent.paste(textbox, `another description`));
    await act(async () => userEvent.click(cancelButton));
    expect(screen.getByText('Unsaved Changes')).toBeVisible();
    const noButton = screen.getByText('No');
    await act(async () => userEvent.click(noButton));

    expect(mockProps.onCancel).not.toHaveBeenCalled();
    expect(await screen.findByDisplayValue('another description')).toBeVisible();
  });

  it('calls onSave and saves form data as expected', async () => {
    (mockProps.onSave as jest.Mock).mockResolvedValue(mockFinancialCode());
    setup();

    const codeType = document.querySelector(`select[name="type"]`) as HTMLSelectElement;
    const description = document.querySelector(`input[name="description"]`) as HTMLInputElement;
    const saveButton = screen.getByText('Save');
    await act(async () => userEvent.selectOptions(codeType, FinancialCodeTypes.BusinessFunction));
    await act(async () => userEvent.paste(description, `another description`));
    await act(async () => userEvent.click(saveButton));

    expect(mockProps.onSave).toHaveBeenCalledWith(
      expect.objectContaining<Partial<Api_FinancialCode>>({
        type: FinancialCodeTypes.BusinessFunction,
        description: `another description`,
      }),
    );
    expect(mockProps.onSuccess).toHaveBeenCalledWith({ ...mockFinancialCode() });
  });

  it('calls onError when it cannot save the form', async () => {
    (mockProps.onSave as jest.Mock).mockRejectedValue(createAxiosError(500));
    setup();

    const codeType = document.querySelector(`select[name="type"]`) as HTMLSelectElement;
    const description = document.querySelector(`input[name="description"]`) as HTMLInputElement;
    const saveButton = screen.getByText('Save');
    await act(async () => userEvent.selectOptions(codeType, FinancialCodeTypes.BusinessFunction));
    await act(async () => userEvent.paste(description, `another description`));
    await act(async () => userEvent.click(saveButton));

    expect(mockProps.onSave).toHaveBeenCalled();
    expect(mockProps.onError).toHaveBeenCalled();
    expect(mockProps.onSuccess).not.toHaveBeenCalled();
  });

  it('displays validation errors and does not save when given a validation schema', async () => {
    mockProps.validationSchema = AddFinancialCodeYupSchema;
    setup();

    const saveButton = screen.getByText('Save');
    await act(async () => userEvent.click(saveButton));

    expect(mockProps.onSave).not.toHaveBeenCalled();
    expect(await screen.findByText('Code value is required')).toBeVisible();
  });
});
