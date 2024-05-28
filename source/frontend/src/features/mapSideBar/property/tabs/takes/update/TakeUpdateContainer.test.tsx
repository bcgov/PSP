import { FormikProps } from 'formik';
import { createMemoryHistory } from 'history';
import { createRef, forwardRef } from 'react';

import { mockLookups } from '@/mocks/lookups.mock';
import { getMockApiPropertyFiles } from '@/mocks/properties.mock';
import { getMockApiTakes } from '@/mocks/takes.mock';
import { ApiGen_Concepts_Take } from '@/models/api/generated/ApiGen_Concepts_Take';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, render, RenderOptions, waitFor, waitForEffects } from '@/utils/test-utils';

import { TakeModel } from '../models';
import TakesUpdateContainer, { ITakesDetailContainerProps } from './TakeUpdateContainer';
import { emptyTake, ITakesFormProps } from './TakeForm';
import { useParams } from 'react-router-dom';
import { useTakesRepository } from '../repositories/useTakesRepository';
import waitForExpect from 'wait-for-expect';

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

const mockUpdateApi = {
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
  updateTakeByAcquisitionPropertyId: mockUpdateApi,
  addTakeByAcquisitionPropertyId: mockUpdateApi,
  deleteTakeByAcquisitionPropertyId: mockUpdateApi,
}));

describe('TakeUpdateContainer component', () => {
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
      <TakesUpdateContainer
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

  it('renders as expected', async () => {
    const { asFragment } = setup({});
    await act(async () => {});
    expect(asFragment()).toMatchSnapshot();
  });

  it('throws an error if file property is invalid', async () => {
    vi.spyOn(console, 'error');
    (console.error as any).mockImplementation(() => {});
    const render = () => setup({ props: { fileProperty: {} as any } });

    expect(render).toThrow('Unable to edit take with invalid ids');
    (console.error as any).mockRestore();
  });

  it('calls onSuccess when onSubmit method is called', async () => {
    setup({});
    const formikHelpers = { setSubmitting: vi.fn() };
    await act(async () => {});
    await act(async () =>
      viewProps.onSubmit(new TakeModel(getMockApiTakes()[0]), formikHelpers as any),
    );

    expect(mockUpdateApi.execute).toHaveBeenCalled();
    expect(onSuccess).toHaveBeenCalled();
  });

  it('returns an empty takeif no take are returned from the api', async () => {
    setup({});
    await act(async () => {});

    expect(viewProps.take).toStrictEqual(new TakeModel(emptyTake));
  });

  it('returns converted take returned from the api into form models', async () => {
    const apiTake: ApiGen_Concepts_Take = { ...getMockApiTakes()[0], propertyAcquisitionFileId: 1 };
    mockGetApi.response = apiTake;
    setup({});
    await waitForExpect(() => expect(viewProps.take).toStrictEqual(new TakeModel(apiTake)));
  });
});
