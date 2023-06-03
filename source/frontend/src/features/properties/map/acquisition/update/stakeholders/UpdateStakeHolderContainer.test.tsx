import { FormikProps } from 'formik';
import { createMemoryHistory } from 'history';
import { mockAcquisitionFileResponse } from 'mocks/acquisitionFiles.mock';
import { getMockApiInterestHolders } from 'mocks/interestHolders.mock';
import { mockLookups } from 'mocks/lookups.mock';
import { forwardRef } from 'react';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { render, RenderOptions, waitFor } from 'utils/test-utils';

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

jest.mock('hooks/repositories/useInterestHolderRepository', () => ({
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
    const utils = render(
      <UpdateStakeHolderContainer
        {...renderOptions.props}
        formikRef={{} as any}
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
    viewProps.onSubmit(StakeHolderForm.fromApi(getMockApiInterestHolders()));

    expect(mockUpdateApi.execute).toHaveBeenCalled();
    await waitFor(async () => expect(onSuccess).toHaveBeenCalled());
  });

  it('returns an empty takes array if no stakeholders are returned from the api', () => {
    setup({});
    viewProps.onSubmit(StakeHolderForm.fromApi([]));

    expect(viewProps.interestHolders.interestHolders).toHaveLength(0);
    expect(viewProps.interestHolders.nonInterestPayees).toHaveLength(0);
  });
});
