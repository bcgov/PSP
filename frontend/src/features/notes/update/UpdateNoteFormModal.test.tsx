import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { NoteTypes } from 'constants/index';
import { FormikProps } from 'formik';
import { createMemoryHistory } from 'history';
import { mockLookups } from 'mocks/mockLookups';
import { mockNoteResponse } from 'mocks/mockNoteResponses';
import { Api_Note } from 'models/api/Note';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { fakeText, render, RenderOptions, userEvent, waitFor } from 'utils/test-utils';

import { NoteForm } from '../models';
import { IUpdateNoteFormModalProps, UpdateNoteFormModal } from './UpdateNoteFormModal';
import { UpdateNoteYupSchema } from './UpdateNoteYupSchema';

const history = createMemoryHistory();
const mockAxios = new MockAdapter(axios);

const validationSchema = jest.fn().mockReturnValue(UpdateNoteYupSchema);
const onSaveClick = jest.fn();
const onCancelClick = jest.fn();
const onSubmit = jest.fn();

type TestProps = Pick<IUpdateNoteFormModalProps, 'initialValues' | 'loading'>;

describe('UpdateNoteFormModal component', () => {
  // render component under test
  const setup = (props: TestProps, renderOptions: RenderOptions = {}) => {
    const utils = render(
      <UpdateNoteFormModal
        {...props}
        isOpened={true}
        validationSchema={validationSchema}
        onSubmit={onSubmit}
        onSaveClick={onSaveClick}
        onCancelClick={onCancelClick}
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

  let initialValues: NoteForm;

  beforeEach(() => {
    mockAxios.onGet(new RegExp('users/info/*')).reply(200, {});
    initialValues = NoteForm.fromApi(mockNoteResponse(1));
  });

  afterEach(() => {
    mockAxios.reset();
    jest.clearAllMocks();
  });

  it('renders as expected', () => {
    setup({ initialValues });
    expect(document.body).toMatchSnapshot();
  });

  it('renders the form fields', () => {
    const { getByLabelText, getByText } = setup({ initialValues });

    const modalTitle = getByText(/Notes/i);
    const textarea = getByLabelText(/Type a note/i);

    expect(modalTitle).toBeVisible();
    expect(textarea).toBeVisible();
    expect(textarea.tagName).toBe('TEXTAREA');
    expect(textarea).not.toHaveAttribute('readonly');
  });

  it('displays existing values if they exist', async () => {
    initialValues.note = 'foo bar baz';
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
    userEvent.click(getSaveButton());

    expect(validationSchema).toBeCalled();
    expect(await findByText(/Notes must be at most 4000 characters/i)).toBeVisible();
  });

  it('should cancel form when Cancel button is clicked', () => {
    const { getCancelButton, getByText } = setup({ initialValues });

    expect(getByText(/Notes/i)).toBeVisible();
    userEvent.click(getCancelButton());

    expect(onCancelClick).toBeCalled();
    expect(validationSchema).not.toBeCalled();
    expect(onSubmit).not.toBeCalled();
  });

  it('should save the form when Submit button is clicked', async () => {
    // submit form upon save click
    onSaveClick.mockImplementation((values: NoteForm, formikProps: FormikProps<NoteForm>) =>
      formikProps?.submitForm(),
    );

    initialValues.note = 'foo bar baz';
    const { getSaveButton } = setup({ initialValues });

    await waitFor(() => userEvent.click(getSaveButton()));

    expect(onSaveClick).toBeCalled();
    expect(validationSchema).toBeCalled();
    expect(onSubmit).toBeCalledWith(initialValues, expect.anything());
    expect(onCancelClick).not.toBeCalled();
  });
});
