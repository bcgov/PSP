import { render, RenderOptions } from '@testing-library/react';
import { IPropertyImprovementsListViewProps } from './PropertyImprovementsListView';
import PropertyImprovementsListContainer, {
  IPropertyImprovementsListContainerProps,
} from './PropertyImprovementsListContainer';
import { act, waitForEffects } from '@/utils/test-utils';

const mockGetImprovementsApi = {
  error: undefined,
  response: undefined,
  execute: vi.fn(),
  loading: false,
};

const mockDeleteImprovementApi = {
  error: undefined,
  response: undefined,
  execute: vi.fn(),
  loading: false,
};

vi.mock('@/hooks/repositories/usePropertyImprovementRepository', () => ({
  usePropertyImprovementRepository: () => {
    return {
      getPropertyImprovements: mockGetImprovementsApi,
      deletePropertyImprovement: mockDeleteImprovementApi,
    };
  },
}));

// eslint-disable-next-line @typescript-eslint/no-unused-vars
let viewProps: IPropertyImprovementsListViewProps | undefined;
const TestView: React.FC<IPropertyImprovementsListViewProps> = props => {
  viewProps = props;
  return <span>Content Rendered</span>;
};

describe('Property improvements list container component', () => {
  const setup = async (
    renderOptions: RenderOptions & {
      props?: Partial<IPropertyImprovementsListContainerProps>;
    } = {},
  ) => {
    const utils = render(
      <PropertyImprovementsListContainer
        propertyId={renderOptions?.props?.propertyId ?? 1}
        View={TestView}
      ></PropertyImprovementsListContainer>,
      {
        ...renderOptions,
      },
    );

    return {
      ...utils,
    };
  };

  beforeEach(() => {
    viewProps = undefined;
    mockGetImprovementsApi.execute.mockResolvedValue([]);
  });

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('renders the underlying form', async () => {
    const { getByText } = await setup();
    await waitForEffects();

    expect(getByText(/Content Rendered/)).toBeVisible();
    expect(mockGetImprovementsApi.execute).toHaveBeenCalledTimes(1);
  });

  it('handles delete', async () => {
    const {} = await setup();
    await waitForEffects();

    expect(mockGetImprovementsApi.execute).toHaveBeenCalledTimes(1);

    await act(async () => viewProps.onDelete(1));
    expect(mockDeleteImprovementApi.execute).toHaveBeenCalledTimes(1);
  });
});
