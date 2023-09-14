import { renderHook } from '@testing-library/react-hooks';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { createMemoryHistory } from 'history';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';
import { ValidationError } from 'yup';

import { mockAcquisitionFileResponse } from '@/mocks/acquisitionFiles.mock';
import { fakeText } from '@/utils/test-utils';
import TestCommonWrapper from '@/utils/TestCommonWrapper';

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
      () =>
        useAddAcquisitionFormManagement({
          onSuccess: hookProps.onSuccess,
          selectedFeature: null,
          formikRef: {} as any,
        }),
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
    const { initialValues } = setup({ onSuccess, selectedFeature: null, formikRef: {} as any });
    expect(initialValues).toEqual(new AcquisitionForm());
  });

  it('should provide form validation schema', async () => {
    expect.assertions(4);
    const { validationSchema } = setup({ onSuccess, selectedFeature: null, formikRef: {} as any });

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
});
