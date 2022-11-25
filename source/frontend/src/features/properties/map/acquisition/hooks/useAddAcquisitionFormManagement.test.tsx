import { act, renderHook } from '@testing-library/react-hooks';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { FormikHelpers } from 'formik';
import { createMemoryHistory } from 'history';
import { mockAcquisitionFileResponse } from 'mocks/mockAcquisitionFiles';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';
import { fakeText } from 'utils/test-utils';
import TestCommonWrapper from 'utils/TestCommonWrapper';
import { ValidationError } from 'yup';

import { AcquisitionForm } from '../add/models';
import {
  IUseAddAcquisitionFormManagementProps,
  useAddAcquisitionFormManagement,
} from './useAddAcquisitionFormManagement';

const history = createMemoryHistory();
const mockAxios = new MockAdapter(axios);
const mockStore = configureMockStore([thunk]);

const onSuccess = jest.fn();

describe('useAddAcquisitionFormManagement hook', () => {
  const Wrapper: React.FC<React.PropsWithChildren<unknown>> = props => (
    <TestCommonWrapper store={mockStore} history={history}>
      {props.children}
    </TestCommonWrapper>
  );

  const setup = (hookProps: IUseAddAcquisitionFormManagementProps) => {
    const { result } = renderHook(
      () => useAddAcquisitionFormManagement({ onSuccess: hookProps.onSuccess }),
      {
        wrapper: Wrapper,
      },
    );

    return result.current;
  };

  beforeEach(() => {
    mockAxios.reset();
    mockAxios.onPost('/acquisitionfiles').reply(200, mockAcquisitionFileResponse(1));
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  it('should return valid initial values', async () => {
    const { initialValues } = setup({ onSuccess });
    expect(initialValues).toEqual(new AcquisitionForm());
  });

  it('should provide form validation schema', async () => {
    expect.assertions(3);
    const { validationSchema } = setup({ onSuccess });

    const validForm = new AcquisitionForm();
    validForm.fileName = 'Lorem ipsum';
    validForm.region = '1';
    validForm.acquisitionType = 'CONSEN';

    const invalidForm = new AcquisitionForm();
    invalidForm.fileName = fakeText(550);

    expect(validationSchema).toBeDefined();
    await expect(validationSchema.validate(validForm)).resolves.toEqual(validForm);
    await expect(validationSchema.validate(invalidForm)).rejects.toBeInstanceOf(ValidationError);
  });

  it('should provide form submission handler', async () => {
    const { handleSubmit } = setup({ onSuccess });

    const formValues = new AcquisitionForm();
    formValues.fileName = 'Test Note';

    const formikHelpers: Partial<FormikHelpers<AcquisitionForm>> = {
      setSubmitting: jest.fn(),
      resetForm: jest.fn(),
    };

    await act(() => handleSubmit(formValues, formikHelpers as any));

    expect(formikHelpers.setSubmitting).toBeCalledWith(false);
    expect(formikHelpers.resetForm).toBeCalled();
    expect(onSuccess).toBeCalled();
  });
});
