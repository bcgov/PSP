import { createMemoryHistory } from 'history';

import { Claims } from '@/constants';
import { mockGetForm8Api } from '@/mocks/form8.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { systemConstantsSlice } from '@/store/slices/systemConstants/systemConstantsSlice';
import { act, render, RenderOptions } from '@/utils/test-utils';

import { IForm8FormProps } from '../UpdateForm8Form';
import UpdateForm8Container, { IUpdateForm8ContainerProps } from './UpdateForm8Container';

const history = createMemoryHistory();
const mockForm8Api = mockGetForm8Api(1, 1);

let viewProps: IForm8FormProps | null;
const TestView: React.FC<IForm8FormProps> = props => {
  viewProps = props;
  return <span>Content Rendered</span>;
};

describe('Add Form8 Container component', () => {
  const setup = async (
    renderOptions: RenderOptions & {
      props?: Partial<IUpdateForm8ContainerProps>;
    } = {},
  ) => {
    const component = render(
      <UpdateForm8Container
        form8Id={renderOptions?.props?.form8Id ?? mockForm8Api.id!}
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
    viewProps = null;
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  it('Renders the underlying form', async () => {
    const { getByText } = await setup();
    expect(getByText(/Content Rendered/)).toBeVisible();
  });

  it('navigates back to expropriation tab when form is cancelled', async () => {
    await setup();
    await act(async () => {
      viewProps?.onCancel();
    });

    expect(history.location.pathname).toBe('/expropriation');
  });
});
