import { FormikProps } from 'formik';
import { createMemoryHistory } from 'history';
import { forwardRef } from 'react';

import { mockLookups } from '@/mocks/lookups.mock';
import { mapMachineBaseMock } from '@/mocks/mapFSM.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, render, RenderOptions } from '@/utils/test-utils';

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
  execute: vi.fn().mockResolvedValue([1]),
  loading: false,
};
vi.mock('@/hooks/repositories/usePimsPropertyRepository', () => ({
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
      mockMapMachine: { ...mapMachineBaseMock, isFiltering: true },
    });

    return {
      ...utils,
    };
  };

  beforeEach(() => {
    vi.resetAllMocks();
  });

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('fetches filter data from the api', async () => {
    mockGetApi.execute.mockResolvedValue([1, 2]);
    setup({});
    await act(async () => viewProps.onChange(new PropertyFilterFormModel()));
    expect(mockGetApi.execute).toHaveBeenCalledWith(new PropertyFilterFormModel().toApi());
    expect(mapMachineBaseMock.setVisiblePimsProperties).toHaveBeenCalledWith([1, 2]);
  });

  it(`resets the map filter state when "onReset" is called`, async () => {
    setup({});
    await act(async () => viewProps.onReset());
    expect(mapMachineBaseMock.resetMapFilter).toHaveBeenCalled();
  });
});
