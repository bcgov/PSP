import { render, RenderOptions } from '@testing-library/react';
import { IFilePropertiesImprovementsViewProps } from './FilePropertiesImprovementsView';
import FilePropertiesImprovementsContainer, {
  IFilePropertiesImprovementsContainerProps,
} from './FilePropertiesImprovementsContainer';
import { getMockApiProperty } from '@/mocks/properties.mock';
import { ApiGen_Concepts_Property } from '@/models/api/generated/ApiGen_Concepts_Property';
import { waitForEffects } from '@/utils/test-utils';

const mockProperty: ApiGen_Concepts_Property = getMockApiProperty();

const mockGetImprovementsApi = {
  error: undefined,
  response: undefined,
  execute: vi.fn(),
  loading: false,
};

vi.mock('@/hooks/repositories/usePropertyImprovementRepository', () => ({
  usePropertyImprovementRepository: () => {
    return {
      getPropertyImprovements: mockGetImprovementsApi,
    };
  },
}));

// eslint-disable-next-line @typescript-eslint/no-unused-vars
let viewProps: IFilePropertiesImprovementsViewProps | undefined;
const TestView: React.FC<IFilePropertiesImprovementsViewProps> = props => {
  viewProps = props;
  return <span>Content Rendered</span>;
};

describe('UpdateCompensationRequisition Container component', () => {
  const setup = async (
    renderOptions: RenderOptions & {
      props?: Partial<IFilePropertiesImprovementsContainerProps>;
    } = {},
  ) => {
    const utils = render(
      <FilePropertiesImprovementsContainer
        fileProperties={renderOptions?.props?.fileProperties ?? [mockProperty]}
        View={TestView}
      ></FilePropertiesImprovementsContainer>,
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
});
