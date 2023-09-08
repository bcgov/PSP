import { createMemoryHistory } from 'history';

import { Claims } from '@/constants';
import { mockGetExpropriationPaymentApi } from '@/mocks/ExpropriationPayment.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { systemConstantsSlice } from '@/store/slices/systemConstants/systemConstantsSlice';
import { act, render, RenderOptions } from '@/utils/test-utils';

import { IForm8FormProps } from '../UpdateForm8Form';
import UpdateForm8Container, { IUpdateForm8ContainerProps } from './UpdateForm8Container';

const history = createMemoryHistory();
const mockForm8 = mockGetExpropriationPaymentApi(1, 1);

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

let viewProps: IForm8FormProps | undefined;
const TestView: React.FC<IForm8FormProps> = props => {
  viewProps = props;
  return <span>Content Rendered</span>;
};

jest.mock('@/hooks/repositories/useForm8Repository', () => ({
  useForm8Repository: () => {
    return {
      getForm8: mockGetApi,
      updateForm8: mockUpdateApi,
    };
  },
}));

describe('Update Form8 Container component', () => {
  const setup = async (
    renderOptions: RenderOptions & {
      props?: Partial<IUpdateForm8ContainerProps>;
    } = {},
  ) => {
    const component = render(
      <UpdateForm8Container
        form8Id={renderOptions?.props?.form8Id ?? mockForm8.id!}
        View={TestView}
      />,
      {
        history,
        store: {
          [lookupCodesSlice.name]: { lookupCodes: mockLookups },
          [systemConstantsSlice.name]: { systemConstants: [{ name: 'GST', value: '5.0' }] },
        },
        useMockAuthentication: true,
        claims: renderOptions?.claims ?? [Claims.ACQUISITION_EDIT],
        ...renderOptions,
      },
    );

    return {
      ...component,
    };
  };

  beforeEach(() => {
    viewProps = undefined;
    jest.resetAllMocks();
  });

  it('Renders the underlying form', async () => {
    const { getByText } = await setup();
    expect(getByText(/Content Rendered/)).toBeVisible();
    expect(mockGetApi.execute).toHaveBeenCalled();
  });

  it('navigates back to expropriation tab when form is cancelled', async () => {
    await setup();
    expect(mockGetApi.execute).toHaveBeenCalled();

    await act(async () => {
      viewProps?.onCancel();
    });
    expect(history.location.pathname).toBe('//expropriation');
  });
});
