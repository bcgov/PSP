import { act, renderHook } from '@testing-library/react-hooks';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { NoteTypes } from 'constants/index';
import { FormikHelpers } from 'formik';
import { createMemoryHistory } from 'history';
import { mockEntityNote } from 'mocks/mockNoteResponses';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';
import { fakeText } from 'utils/test-utils';
import TestCommonWrapper from 'utils/TestCommonWrapper';
import { ValidationError } from 'yup';

import { EntityNoteForm } from '../add/models';
import {
  IUseAddNotesFormManagementProps,
  useAddNotesFormManagement,
} from './useAddNotesFormManagement';

const history = createMemoryHistory();
const mockAxios = new MockAdapter(axios);
const mockStore = configureMockStore([thunk]);

describe('useAddNotesFormManagement hook', () => {
  const setup = (hookProps: IUseAddNotesFormManagementProps) => {
    const { result } = renderHook(
      () =>
        useAddNotesFormManagement({
          parentId: hookProps.parentId,
          type: hookProps.type,
        }),
      {
        wrapper: props => (
          <TestCommonWrapper store={mockStore} history={history}>
            {props.children}
          </TestCommonWrapper>
        ),
      },
    );

    return result.current;
  };

  beforeEach(() => {
    mockAxios.reset();
    mockAxios.onPost('/notes/activity').reply(200, mockEntityNote(1));
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  it('should return valid initial values', async () => {
    const { initialValues } = setup({ parentId: 1, type: NoteTypes.Activity });

    expect(initialValues).toEqual(
      expect.objectContaining({
        parentId: 1,
        note: { note: '' },
      }),
    );
  });

  it('should provide form validation schema', async () => {
    expect.assertions(3);
    const { validationSchema } = setup({ parentId: 1, type: NoteTypes.Activity });

    const validForm = new EntityNoteForm();
    const invalidForm = new EntityNoteForm();
    invalidForm.note.note = fakeText(4001);

    expect(validationSchema).toBeDefined();
    await expect(validationSchema.validate(validForm)).resolves.toEqual(validForm);
    await expect(validationSchema.validate(invalidForm)).rejects.toBeInstanceOf(ValidationError);
  });

  it('should provide form submission handler', async () => {
    const { handleSubmit } = setup({ parentId: 1, type: NoteTypes.Activity });

    const formValues = new EntityNoteForm();
    formValues.note.note = 'Test Note';

    const formikHelpers: Partial<FormikHelpers<EntityNoteForm>> = {
      setSubmitting: jest.fn(),
      resetForm: jest.fn(),
    };

    await act(() => handleSubmit(formValues, formikHelpers as any));

    expect(formikHelpers.setSubmitting).toBeCalledWith(false);
    expect(formikHelpers.resetForm).toBeCalled();
    // TODO: navigate to Notes LIST VIEW - route not implemented yet
    expect(history.location.pathname).toBe('/mapview');
  });
});
