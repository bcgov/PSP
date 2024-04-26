import { FormikProps } from 'formik';
import { createMemoryHistory } from 'history';
import { forwardRef } from 'react';

import { mockAcquisitionFileResponse } from '@/mocks/acquisitionFiles.mock';
import { getMockApiInterestHolders } from '@/mocks/interestHolders.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { ApiGen_Concepts_InterestHolder } from '@/models/api/generated/ApiGen_Concepts_InterestHolder';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { render, RenderOptions } from '@/utils/test-utils';

import StakeHolderContainer, { IStakeHolderContainerProps } from './StakeHolderContainer';
import { IStakeHolderViewProps } from './StakeHolderView';

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

const mockGetApi = {
  error: undefined,
  response: [] as ApiGen_Concepts_InterestHolder[],
  execute: vi.fn().mockResolvedValue(getMockApiInterestHolders()),
  loading: false,
};

const mockUpdateApi = {
  error: undefined,
  response: undefined,
  execute: vi.fn().mockResolvedValue(getMockApiInterestHolders()),
  loading: false,
};

vi.mock('@/hooks/repositories/useInterestHolderRepository', () => ({
  useInterestHolderRepository: () => {
    return {
      getAcquisitionInterestHolders: mockGetApi,
      updateAcquisitionInterestHolders: mockUpdateApi,
    };
  },
}));

describe('StakeHolderContainer component', () => {
  // render component under test
  const View = forwardRef<FormikProps<any>, IStakeHolderViewProps>((props, ref) => {
    return <></>;
  });

  const onEdit = vi.fn();

  const setup = (
    renderOptions: RenderOptions & { props?: Partial<IStakeHolderContainerProps> },
  ) => {
    const utils = render(
      <StakeHolderContainer
        {...renderOptions.props}
        View={View}
        acquisitionFile={renderOptions.props?.acquisitionFile ?? mockAcquisitionFileResponse()}
        onEdit={onEdit}
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
    vi.clearAllMocks();
  });

  it('renders as expected', () => {
    const { asFragment } = setup({});
    expect(asFragment()).toMatchSnapshot();
  });
});
