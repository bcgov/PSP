import { FormikProps } from 'formik';
import { createMemoryHistory } from 'history';
import { forwardRef } from 'react';

import { mockLookups } from '@/mocks/lookups.mock';
import { getMockApiPropertyFiles } from '@/mocks/properties.mock';
import { getMockApiTakes } from '@/mocks/takes.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { render, RenderOptions, waitFor } from '@/utils/test-utils';

import { useTakesRepository } from '../repositories/useTakesRepository';
import { ITakesDetailContainerProps } from '../update/TakesUpdateContainer';
import TakesDetailContainer from './TakesDetailContainer';
import { ITakesDetailViewProps } from './TakesDetailView';

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

const mockGetApi = {
  error: undefined,
  response: undefined,
  execute: jest.fn(),
  loading: false,
};

const mockCountsApi = {
  error: undefined,
  response: undefined,
  execute: jest.fn(),
  loading: false,
};

jest.mock('../repositories/useTakesRepository');

describe('TakesDetailContainer component', () => {
  // render component under test

  let viewProps: ITakesDetailViewProps;
  const View = forwardRef<FormikProps<any>, ITakesDetailViewProps>((props, ref) => {
    viewProps = props;
    return <></>;
  });

  const onEdit = jest.fn();

  const setup = (
    renderOptions: RenderOptions & { props?: Partial<ITakesDetailContainerProps> },
  ) => {
    const utils = render(
      <TakesDetailContainer
        {...renderOptions.props}
        fileProperty={renderOptions.props?.fileProperty ?? getMockApiPropertyFiles()[0]}
        View={View}
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

  beforeEach(() => {
    (useTakesRepository as jest.Mock).mockReturnValue({
      getTakesByPropertyId: mockGetApi,
      getTakesCountByPropertyId: mockCountsApi,
    });
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  it('renders as expected', () => {
    const { asFragment } = setup({});
    expect(asFragment()).toMatchSnapshot();
  });

  it('calls getTakes and getTakesCount', async () => {
    setup({});

    await waitFor(() => {
      expect(mockGetApi.execute).toHaveBeenCalledTimes(1);
      expect(mockCountsApi.execute).toHaveBeenCalledTimes(1);
    });
  });

  it('returns the takes sorted by the created date', async () => {
    (useTakesRepository as jest.Mock).mockReturnValue({
      getTakesByPropertyId: {
        ...mockGetApi,
        response: [
          { ...getMockApiTakes(), appCreateTimestamp: '2020-01-01' },
          { ...getMockApiTakes(), appCreateTimestamp: '2020-01-02' },
        ],
      },
      getTakesCountByPropertyId: mockCountsApi,
    });
    setup({});

    await waitFor(() => {
      expect(viewProps.takes[0].appCreateTimestamp).toBe('2020-01-02');
    });
  });

  it('returns empty takes array by default', async () => {
    mockGetApi.execute.mockResolvedValue(undefined);
    setup({});

    await waitFor(() => {
      expect(viewProps.takes).toStrictEqual([]);
    });
  });

  it('returns a take count of 0 by default', async () => {
    setup({});

    expect(viewProps.allTakesCount).toBe(0);
  });
});
