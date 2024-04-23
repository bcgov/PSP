import { FormikProps } from 'formik';
import { createMemoryHistory } from 'history';
import { forwardRef } from 'react';

import {
  IMapStateMachineContext,
  useMapStateMachine,
} from '@/components/common/mapFSM/MapStateMachineContext';
import { mockLookups } from '@/mocks/lookups.mock';
import { mapMachineBaseMock } from '@/mocks/mapFSM.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { render, RenderOptions, waitFor } from '@/utils/test-utils';

import { FilterContentContainer, IFilterContentContainerProps } from './FilterContentContainer';
import { IFilterContentFormProps } from './FilterContentForm';
import { PropertyFilterFormModel } from './models';

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

const mockGetApi = {
  error: undefined,
  response: [1] as number[] | undefined,
  execute: jest.fn().mockResolvedValue([1]),
  loading: false,
};
jest.mock('@/components/common/mapFSM/MapStateMachineContext');
jest.mock('@/hooks/repositories/usePimsPropertyRepository', () => ({
  usePimsPropertyRepository: () => {
    return {
      getMatchingProperties: mockGetApi,
    };
  },
}));

describe('FilterContentContainer component', () => {
  let viewProps: IFilterContentFormProps;

  const View = forwardRef<FormikProps<any>, IFilterContentFormProps>((props, ref) => {
    viewProps = props;
    return <></>;
  });

  const setup = (
    renderOptions?: RenderOptions & { props?: Partial<IFilterContentContainerProps> },
  ) => {
    renderOptions = renderOptions ?? {};
    const utils = render(<FilterContentContainer {...renderOptions.props} View={View} />, {
      ...renderOptions,
      store: storeState,
      history,
    });

    return {
      ...utils,
    };
  };

  beforeEach(() => {
    jest.resetAllMocks();
    const testMockMahine: IMapStateMachineContext = {
      ...mapMachineBaseMock,
      isFiltering: true,
    };

    (useMapStateMachine as unknown as jest.Mock<Partial<IMapStateMachineContext>>).mockReturnValue(
      testMockMahine,
    );
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  it('fetches filter data from the api', async () => {
    mockGetApi.execute.mockResolvedValue([1, 2]);
    setup({});
    viewProps.onChange(new PropertyFilterFormModel());
    expect(mockGetApi.execute).toBeCalledWith(new PropertyFilterFormModel().toApi());
    await waitFor(() => expect(mapMachineBaseMock.setVisiblePimsProperties).toBeCalledWith([1, 2]));
  });
});
