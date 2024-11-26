import { FormikProps } from 'formik';
import { createMemoryHistory } from 'history';
import { forwardRef } from 'react';

import { mockLookups } from '@/mocks/lookups.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { render, RenderOptions, waitFor } from '@/utils/test-utils';

import { vi } from 'vitest';
import HistoricalNumbersContainer, {
  IHistoricalNumbersContainerProps,
} from './HistoricalNumberContainer';
import { IHistoricalNumbersViewProps } from './HistoricalNumberFieldView';
import { useHistoricalNumberRepository } from '@/hooks/repositories/useHistoricalNumberRepository';

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

const mockGetApi = {
  error: undefined,
  response: undefined,
  execute: vi.fn(),
  loading: false,
  status: 200,
};

vi.mock('@/hooks/repositories/useHistoricalNumberRepository');
vi.mocked(useHistoricalNumberRepository).mockImplementation(() => ({
  getPropertyHistoricalNumbers: mockGetApi,
  updatePropertyHistoricalNumbers: {} as any,
}));

const onSuccess = vi.fn();

describe('HistoricalNumberContainer component', () => {
  // render component under test

  let viewProps: IHistoricalNumbersViewProps;
  const View = forwardRef<FormikProps<any>, IHistoricalNumbersViewProps>((props, ref) => {
    viewProps = props;
    return <></>;
  });

  const setup = (
    renderOptions: RenderOptions & { props?: Partial<IHistoricalNumbersContainerProps> },
  ) => {
    const utils = render(
      <HistoricalNumbersContainer
        {...renderOptions.props}
        View={View}
        propertyIds={renderOptions?.props?.propertyIds}
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

  it('calls hist file number api for every propertyId', async () => {
    setup({ props: { propertyIds: [1, 2, 3] } });

    await waitFor(async () => {
      expect(mockGetApi.execute).toHaveBeenCalledWith(1);
      expect(mockGetApi.execute).toHaveBeenCalledWith(2);
      expect(mockGetApi.execute).toHaveBeenCalledWith(3);
    });
  });

  it('passes the list of historical file numbers to the view', async () => {
    mockGetApi.execute.mockResolvedValue(['hist1', 'hist2']);
    setup({ props: { propertyIds: [1] } });

    await waitFor(() => expect(viewProps.historicalNumbers).toEqual(['hist1', 'hist2']));
  });
});
