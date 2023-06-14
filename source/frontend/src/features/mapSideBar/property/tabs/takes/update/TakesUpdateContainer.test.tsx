import { FormikProps } from 'formik';
import { createMemoryHistory } from 'history';
import { forwardRef } from 'react';

import { mockLookups } from '@/mocks/lookups.mock';
import { getMockApiPropertyFiles } from '@/mocks/properties.mock';
import { getMockApiTakes } from '@/mocks/takes.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { render, RenderOptions } from '@/utils/test-utils';

import { TakeModel } from './models';
import TakesUpdateContainer, { ITakesDetailContainerProps } from './TakesUpdateContainer';
import { emptyTake, ITakesUpdateFormProps } from './TakesUpdateForm';

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

const mockUpdateApi = {
  error: undefined,
  response: undefined,
  execute: jest.fn(),
  loading: false,
};

jest.mock('../repositories/useTakesRepository', () => ({
  useTakesRepository: () => {
    return {
      getTakesByFileId: mockGetApi,
      updateTakesByAcquisitionPropertyId: mockUpdateApi,
    };
  },
}));

describe('TakesUpdateContainer component', () => {
  // render component under test

  let viewProps: ITakesUpdateFormProps;
  const View = forwardRef<FormikProps<any>, ITakesUpdateFormProps>((props, ref) => {
    viewProps = props;
    return <></>;
  });

  const onSuccess = jest.fn();

  const setup = (
    renderOptions: RenderOptions & { props?: Partial<ITakesDetailContainerProps> },
  ) => {
    const utils = render(
      <TakesUpdateContainer
        {...renderOptions.props}
        fileProperty={renderOptions.props?.fileProperty ?? getMockApiPropertyFiles()[0]}
        View={View}
        onSuccess={onSuccess}
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

  it('throws an error if file property is invalid', () => {
    const render = () => setup({ props: { fileProperty: {} as any } });
    expect(render).toThrow('File property must have id');
  });

  it('calls onSuccess when onSubmit method is called', () => {
    setup({});
    const formikHelpers = { setSubmitting: jest.fn() };
    viewProps.onSubmit({ takes: [new TakeModel(getMockApiTakes()[0])] }, formikHelpers as any);

    expect(mockUpdateApi.execute).toHaveBeenCalled();
  });

  it('returns an empty takes array if no takes are returned from the api', () => {
    setup({});

    expect(viewProps.takes).toStrictEqual([new TakeModel(emptyTake)]);
  });
});
