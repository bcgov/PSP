import { act, renderHook } from '@testing-library/react-hooks';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { FormikHelpers } from 'formik';
import { createMemoryHistory } from 'history';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';
import { ValidationError } from 'yup';

import { NoteTypes } from '@/constants/index';
import { mockEntityNote } from '@/mocks/noteResponses.mock';
import { fakeText } from '@/utils/test-utils';
import TestCommonWrapper from '@/utils/TestCommonWrapper';

import { EntityNoteForm } from '../add/models';
import {
  IUseAddNotesFormManagementProps,
  useAddNotesFormManagement,
} from './useAddNotesFormManagement';

const history = createMemoryHistory();
const mockAxios = new MockAdapter(axios);
const mockStore = configureMockStore([thunk]);

const onSuccess = jest.fn();

describe('useAddNotesFormManagement hook', () => {
  const setup = (hookProps: IUseAddNotesFormManagementProps) => {
    const { result } = renderHook(
      () =>
        useAddNotesFormManagement({
          parentId: hookProps.parentId,
          type: hookProps.type,
          onSuccess,
        }),
      {
        wrapper: (props: React.PropsWithChildren) => (
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
    expect.assertions(4);
    const { validationSchema } = setup({ parentId: 1, type: NoteTypes.Activity });

    const validForm = new EntityNoteForm();
    validForm.note.note = 'Lorem ipsum';

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

    await act(async () => handleSubmit(formValues, formikHelpers as any));

    expect(formikHelpers.setSubmitting).toBeCalledWith(false);
    expect(formikHelpers.resetForm).toBeCalled();
    expect(onSuccess).toBeCalled();
  });
});
