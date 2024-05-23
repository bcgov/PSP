import { FormikProps } from 'formik';
import { createMemoryHistory } from 'history';
import { forwardRef } from 'react';

import { mockLookups } from '@/mocks/lookups.mock';
import { getMockApiPropertyFiles } from '@/mocks/properties.mock';
import { getMockApiTakes } from '@/mocks/takes.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { render, RenderOptions, waitFor } from '@/utils/test-utils';

import { useTakesRepository } from '../repositories/useTakesRepository';
import { ITakesDetailContainerProps } from '../update/TakeUpdateContainer';
import TakesDetailContainer from './TakesDetailContainer';
import { ITakesDetailViewProps } from './TakesDetailView';
import { IResponseWrapper } from '@/hooks/util/useApiRequestWrapper';
import { ApiGen_Concepts_Take } from '@/models/api/generated/ApiGen_Concepts_Take';
import { AxiosResponse } from 'axios';
import { vi } from 'vitest';

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

const mockCountsApi = {
  error: undefined,
  response: undefined,
  execute: vi.fn(),
  loading: false,
  status: 200,
};

const mockUpdateApi = {
  error: undefined,
  response: undefined,
  execute: vi.fn(),
  loading: false,
  status: 201,
};

vi.mock('../repositories/useTakesRepository');
vi.mocked(useTakesRepository).mockImplementation(() => ({
  getTakesByFileId: mockGetApi,
  getTakesByPropertyId: mockGetApi,
  getTakesCountByPropertyId: mockCountsApi,
  getTakeById: mockGetApi,
  updateTakeByAcquisitionPropertyId: mockUpdateApi,
  addTakeByAcquisitionPropertyId: mockUpdateApi,
  deleteTakeByAcquisitionPropertyId: mockUpdateApi,
}));

describe('TakesDetailContainer component', () => {
  // render component under test

  let viewProps: ITakesDetailViewProps;
  const View = forwardRef<FormikProps<any>, ITakesDetailViewProps>((props, ref) => {
    viewProps = props;
    return <></>;
  });

  const setup = (
    renderOptions: RenderOptions & { props?: Partial<ITakesDetailContainerProps> },
  ) => {
    const utils = render(
      <TakesDetailContainer
        {...renderOptions.props}
        fileProperty={renderOptions.props?.fileProperty ?? getMockApiPropertyFiles()[0]}
        View={View}
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
    mockGetApi.response = [
      { ...getMockApiTakes(), id: 1 } as unknown as ApiGen_Concepts_Take,
      { ...getMockApiTakes(), id: 2 } as unknown as ApiGen_Concepts_Take,
    ];
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

  it('returns the takes sorted by the id', async () => {
    mockGetApi.response = [
      { ...getMockApiTakes(), id: 1 } as unknown as ApiGen_Concepts_Take,
      { ...getMockApiTakes(), id: 2 } as unknown as ApiGen_Concepts_Take,
    ];
    setup({});

    await waitFor(() => {
      expect(viewProps.takes[0].id).toBe(2);
    });
  });

  it('returns empty takes array by default', async () => {
    mockGetApi.response = [];
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
