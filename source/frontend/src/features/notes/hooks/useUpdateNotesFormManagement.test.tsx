import { act, renderHook } from '@testing-library/react-hooks';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { NoteTypes } from 'constants/index';
import { FormikHelpers } from 'formik';
import { createMemoryHistory } from 'history';
import { mockNoteResponse } from 'mocks/mockNoteResponses';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';
import { fakeText } from 'utils/test-utils';
import TestCommonWrapper from 'utils/TestCommonWrapper';
import { ValidationError } from 'yup';

import { NoteForm } from '../models';
import {
  IUseUpdateNotesFormManagementProps,
  useUpdateNotesFormManagement,
} from './useUpdateNotesFormManagement';

const history = createMemoryHistory();
const mockAxios = new MockAdapter(axios);
const mockStore = configureMockStore([thunk]);

const onSuccess = jest.fn();

const BASIC_PROPS: IUseUpdateNotesFormManagementProps = {
  type: NoteTypes.Activity,
  note: mockNoteResponse(1),
  onSuccess,
};

describe('useUpdateNotesFormManagement hook', () => {
  const setup = (hookProps: IUseUpdateNotesFormManagementProps = { ...BASIC_PROPS }) => {
    const { result } = renderHook(() => useUpdateNotesFormManagement({ ...hookProps }), {
      wrapper: (props: React.PropsWithChildren) => (
        <TestCommonWrapper store={mockStore} history={history}>
          {props.children}
        </TestCommonWrapper>
      ),
    });

    return result.current;
  };

  beforeEach(() => {
    mockAxios.reset();
    mockAxios.onPut('/notes/activity/1').reply(200, mockNoteResponse(1));
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  it('should return valid initial values', async () => {
    const { initialValues } = setup();
    expect(initialValues).toEqual({ ...mockNoteResponse(1) });
  });

  it('should provide form validation schema', async () => {
    expect.assertions(3);
    const { validationSchema } = setup();

    const validForm = new NoteForm();
    validForm.note = 'Lorem ipsum';

    const invalidForm = new NoteForm();
    invalidForm.note = fakeText(4001);

    expect(validationSchema).toBeDefined();
    await expect(validationSchema.validate(validForm)).resolves.toEqual(validForm);
    await expect(validationSchema.validate(invalidForm)).rejects.toBeInstanceOf(ValidationError);
  });

  it('should provide form submission handler', async () => {
    const { handleSubmit } = setup();

    const formValues = NoteForm.fromApi(mockNoteResponse(1));

    const formikHelpers: Partial<FormikHelpers<NoteForm>> = {
      setSubmitting: jest.fn(),
      resetForm: jest.fn(),
    };

    await act(() => handleSubmit(formValues, formikHelpers as any));

    expect(formikHelpers.setSubmitting).toBeCalledWith(false);
    expect(formikHelpers.resetForm).toBeCalled();
    expect(onSuccess).toBeCalled();
  });
});
