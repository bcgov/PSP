import { FormikProps } from 'formik';
import { createMemoryHistory } from 'history';

import { mockLookups } from '@/mocks/lookups.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, fakeText, render, RenderOptions, userEvent } from '@/utils/test-utils';

import { AddNotesFormModal } from './AddNotesFormModal';
import { AddNotesYupSchema } from './AddNotesYupSchema';
import { EntityNoteForm } from './models';

const history = createMemoryHistory();

const handleSubmit = jest.fn();
const validationSchema = jest.fn().mockReturnValue(AddNotesYupSchema);
const handleSaveClick = jest.fn();
const handleCancelClick = jest.fn();

describe('AddNotesFormModal component', () => {
  // render component under test
  const setup = (renderOptions: RenderOptions & { initialValues: EntityNoteForm }) => {
    const utils = render(
      <AddNotesFormModal
        isOpened={true}
        initialValues={renderOptions.initialValues}
        validationSchema={validationSchema}
        onSubmit={handleSubmit}
        onSaveClick={handleSaveClick}
        onCancelClick={handleCancelClick}
      />,
      {
        ...renderOptions,
        store: {
          [lookupCodesSlice.name]: { lookupCodes: mockLookups },
        },
        history,
      },
    );

    return {
      ...utils,
      getSaveButton: () => utils.getByText(/Save/i),
      getCancelButton: () => utils.getByText(/Cancel/i),
    };
  };

  let initialValues: EntityNoteForm;

  beforeEach(() => {
    initialValues = new EntityNoteForm();
    initialValues.parentId = 1;
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  it('renders as expected', () => {
    setup({ initialValues });
    expect(document.body).toMatchSnapshot();
  });

  it('renders the Note field', () => {
    const { getByLabelText } = setup({ initialValues });
    const textarea = getByLabelText(/Type a note/i);

    expect(textarea).toBeVisible();
    expect(textarea.tagName).toBe('TEXTAREA');
  });

  it('displays existing values if they exist', async () => {
    initialValues.note.note = 'foo bar baz';
    const { findByLabelText } = setup({ initialValues });
    const textarea = await findByLabelText(/Type a note/i);

    expect(textarea).toBeVisible();
    expect(textarea).toHaveValue('foo bar baz');
  });

  it('should validate character limits', async () => {
    const { getSaveButton, findByText, findByLabelText } = setup({ initialValues });

    // note cannot exceed 4000 characters
    const textarea = await findByLabelText(/Type a note/i);
    userEvent.paste(textarea, fakeText(4001));
    await act(async () => userEvent.click(getSaveButton()));

    expect(validationSchema).toBeCalled();
    expect(await findByText(/Notes must be at most 4000 characters/i)).toBeVisible();
  });

  it('should cancel form when Cancel button is clicked', async () => {
    const { getCancelButton } = setup({ initialValues });

    await act(async () => userEvent.click(getCancelButton()));

    expect(handleCancelClick).toBeCalled();
    expect(validationSchema).not.toBeCalled();
    expect(handleSubmit).not.toBeCalled();
  });

  it('should submit form when Submit button is clicked', async () => {
    // submit form upon save click
    handleSaveClick.mockImplementation(
      (values: EntityNoteForm, formikProps: FormikProps<EntityNoteForm>) =>
        formikProps?.submitForm(),
    );

    initialValues.note.note = 'foo bar baz';
    const { getSaveButton } = setup({ initialValues });

    await act(async () => userEvent.click(getSaveButton()));

    expect(validationSchema).toBeCalled();
    expect(handleSubmit).toBeCalledWith(initialValues, expect.anything());
    expect(handleCancelClick).not.toBeCalled();
  });
});
