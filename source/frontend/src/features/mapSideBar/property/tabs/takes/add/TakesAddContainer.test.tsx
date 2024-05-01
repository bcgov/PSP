import { FormikProps } from 'formik';
import { createMemoryHistory } from 'history';
import { createRef, forwardRef } from 'react';

import { mockLookups } from '@/mocks/lookups.mock';
import { getMockApiPropertyFiles } from '@/mocks/properties.mock';
import { getMockApiTakes } from '@/mocks/takes.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, render, RenderOptions, screen } from '@/utils/test-utils';

import { TakeModel } from '../models';
import { useParams } from 'react-router-dom';
import { useTakesRepository } from '../repositories/useTakesRepository';
import TakesAddContainer, { ITakesDetailContainerProps } from './TakesAddContainer';
import { ITakesFormProps, emptyTake } from '../update/TakeForm';

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

const mockAddApi = {
  error: undefined,
  response: undefined,
  execute: vi.fn(),
  loading: false,
  status: 200,
};
vi.mock('react-router-dom');
vi.mocked(useParams).mockReturnValue({ takeId: '1' });

vi.mock('../repositories/useTakesRepository');
vi.mocked(useTakesRepository).mockImplementation(() => ({
  getTakesByFileId: mockGetApi,
  getTakesByPropertyId: mockGetApi,
  getTakesCountByPropertyId: mockGetApi,
  getTakeById: mockGetApi,
  updateTakeByAcquisitionPropertyId: mockAddApi,
  addTakeByAcquisitionPropertyId: mockAddApi,
  deleteTakeByAcquisitionPropertyId: mockAddApi,
}));

describe('TakeAddContainer component', () => {
  // render component under test

  let viewProps: ITakesFormProps;
  const View = forwardRef<FormikProps<any>, ITakesFormProps>((props, ref) => {
    viewProps = props;
    return <></>;
  });

  const onSuccess = vi.fn();

  const setup = (
    renderOptions: RenderOptions & { props?: Partial<ITakesDetailContainerProps> },
  ) => {
    const utils = render(
      <TakesAddContainer
        {...renderOptions.props}
        fileProperty={renderOptions.props?.fileProperty ?? getMockApiPropertyFiles()[0]}
        View={View}
        onSuccess={onSuccess}
        ref={createRef<FormikProps<any>>()}
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

  beforeEach(() => {
    history.push('takes/1');
  });

  it('calls onSuccess when onSubmit method is called', async () => {
    setup({});
    const formikHelpers = { setSubmitting: vi.fn() };
    await act(async () =>
      viewProps.onSubmit(new TakeModel(getMockApiTakes()[0]), formikHelpers as any),
    );

    expect(mockAddApi.execute).toHaveBeenCalled();
    expect(onSuccess).toHaveBeenCalled();
  });

  it('returns an empty take', async () => {
    setup({});

    expect(viewProps.take).toStrictEqual(new TakeModel(emptyTake));
  });
});
