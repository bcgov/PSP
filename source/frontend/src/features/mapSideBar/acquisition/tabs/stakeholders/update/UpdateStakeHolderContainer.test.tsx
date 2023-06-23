import { FormikProps } from 'formik';
import { createMemoryHistory } from 'history';
import { createRef, forwardRef } from 'react';

import { mockAcquisitionFileResponse } from '@/mocks/acquisitionFiles.mock';
import { getMockApiInterestHolders } from '@/mocks/interestHolders.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, render, RenderOptions, waitFor } from '@/utils/test-utils';

import { StakeHolderForm } from './models';
import UpdateStakeHolderContainer, {
  IUpdateStakeHolderContainerProps,
} from './UpdateStakeHolderContainer';
import { IUpdateStakeHolderFormProps } from './UpdateStakeHolderForm';

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

const mockGetApi = {
  error: undefined,
  response: undefined,
  execute: jest.fn().mockResolvedValue(getMockApiInterestHolders()),
  loading: false,
};

const mockUpdateApi = {
  error: undefined,
  response: undefined,
  execute: jest.fn().mockResolvedValue(getMockApiInterestHolders()),
  loading: false,
};

jest.mock('@/hooks/repositories/useInterestHolderRepository', () => ({
  useInterestHolderRepository: () => {
    return {
      getAcquisitionInterestHolders: mockGetApi,
      updateAcquisitionInterestHolders: mockUpdateApi,
    };
  },
}));

describe('InterestHolder component', () => {
  // render component under test

  let viewProps: IUpdateStakeHolderFormProps;
  const View = forwardRef<FormikProps<any>, IUpdateStakeHolderFormProps>((props, ref) => {
    viewProps = props;
    return <></>;
  });

  const onSuccess = jest.fn();

  const setup = (
    renderOptions: RenderOptions & { props?: Partial<IUpdateStakeHolderContainerProps> },
  ) => {
    const formikRef = createRef<FormikProps<StakeHolderForm>>();

    const utils = render(
      <UpdateStakeHolderContainer
        {...renderOptions.props}
        formikRef={formikRef}
        View={View}
        onSuccess={onSuccess}
        acquisitionFile={renderOptions.props?.acquisitionFile ?? mockAcquisitionFileResponse()}
      />,
      {
        ...renderOptions,
        store: storeState,
        history,
      },
    );

    return {
      ...utils,
      formikRef,
    };
  };

  afterEach(() => {
    jest.clearAllMocks();
  });

  it('renders as expected', () => {
    const { asFragment } = setup({});
    expect(asFragment()).toMatchSnapshot();
  });

  it('calls onSuccess when onSubmit method is called', async () => {
    setup({});
    const formikHelpers = { setSubmitting: jest.fn() };
    viewProps.onSubmit(StakeHolderForm.fromApi(getMockApiInterestHolders()), formikHelpers as any);

    expect(mockUpdateApi.execute).toHaveBeenCalled();
    await waitFor(async () => expect(onSuccess).toHaveBeenCalled());
  });

  it('returns an empty takes array if no stakeholders are returned from the api', async () => {
    const { formikRef } = setup({});
    await act(async () => formikRef.current?.submitForm());

    expect(viewProps.interestHolders.interestHolders).toHaveLength(0);
    expect(viewProps.interestHolders.nonInterestPayees).toHaveLength(0);
  });
});
