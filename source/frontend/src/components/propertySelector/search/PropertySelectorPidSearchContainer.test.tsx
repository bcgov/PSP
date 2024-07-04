import { FormikProps } from 'formik';
import { createMemoryHistory } from 'history';
import { createRef } from 'react';

import { createAxiosError, renderAsync, RenderOptions, screen } from '@/utils/test-utils';
import PropertySelectorPidSearchContainer, {
  PropertySelectorPidSearchContainerProps,
} from './PropertySelectorPidSearchContainer';

import { IPropertySearchSelectorPidFormViewProps } from './PropertySelectorPidSearchView';
import { SubdivisionFormModel } from '@/features/mapSideBar/subdivision/AddSubdivisionModel';

const history = createMemoryHistory();

const onClose = vi.fn();

let viewProps: IPropertySearchSelectorPidFormViewProps | undefined;
const TestView: React.FunctionComponent<
  React.PropsWithChildren<IPropertySearchSelectorPidFormViewProps>
> = props => {
  viewProps = props;
  return <span>Content Rendered</span>;
};

const mockGetByPidWrapper = {
  error: undefined,
  response: undefined,
  execute: vi.fn(),
  loading: false,
};

const setSelectProperty = vi.fn();

vi.mock('@/hooks/repositories/usePimsPropertyRepository', () => ({
  usePimsPropertyRepository: () => {
    return {
      getPropertyByPidWrapper: mockGetByPidWrapper,
    };
  },
}));

describe('PropertySearchPidSelector component', () => {
  const setup = async (
    renderOptions: RenderOptions & {
      props?: Partial<PropertySelectorPidSearchContainerProps>;
    } = {},
  ) => {
    const ref = createRef<FormikProps<SubdivisionFormModel>>();
    const component = await renderAsync(
      <PropertySelectorPidSearchContainer
        PropertySelectorPidSearchView={TestView}
        setSelectProperty={setSelectProperty}
      />,
      {
        history,
        useMockAuthentication: true,
        claims: [],
        ...renderOptions,
      },
    );

    return {
      ...component,
      getFormikRef: () => ref,
    };
  };

  beforeEach(() => {
    viewProps = undefined;
    history.location.pathname = '/';
    vi.resetAllMocks();
  });

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('calls setSelectedProperty when onSearch', async () => {
    await setup();

    viewProps?.onSearch({ pid: '111-111-111' });
    mockGetByPidWrapper.execute.mockResolvedValue({ id: 1 });

    expect(mockGetByPidWrapper.execute).toHaveBeenCalledWith('111-111-111');
  });

  it('displays an error modal when the property is not owned', async () => {
    mockGetByPidWrapper.execute.mockResolvedValue({ id: 1, isOwned: false });

    await setup();
    viewProps?.onSearch({ pid: '111-111-111' });

    const modal = await screen.findByText('Error');

    expect(modal).toBeVisible();
  });

  it('displays an error when the property is not found', async () => {
    mockGetByPidWrapper.execute.mockRejectedValue(createAxiosError(404, 'Some error'));

    await setup();
    viewProps?.onSearch({ pid: '111-111-111' });

    const modal = await screen.findByText('Error');

    expect(modal).toBeVisible();
  });
});
