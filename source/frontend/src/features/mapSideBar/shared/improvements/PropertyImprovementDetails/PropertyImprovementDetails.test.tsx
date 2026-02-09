import { render, RenderOptions, waitFor } from '@/utils/test-utils';
import { getMockPropertyImprovementApi } from '@/mocks/propertyImprovements.mock';
import PropertyImprovementDetails, {
  IPropertyImprovementDetailsProps,
} from './PropertyImprovementDetails';

describe('Property Improvement Details view', () => {
  const setup = async (
    renderOptions: RenderOptions & { props?: Partial<IPropertyImprovementDetailsProps> },
  ) => {
    const utils = render(
      <PropertyImprovementDetails
        propertyImprovement={
          renderOptions?.props?.propertyImprovement ?? getMockPropertyImprovementApi()
        }
        propertyImprovementIndex={renderOptions?.props?.propertyImprovementIndex ?? 1000}
      />,
      {
        ...renderOptions,
      },
    );

    return {
      ...utils,
    };
  };

  beforeEach(() => {
    vi.restoreAllMocks();
  });

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('renders as expected', async () => {
    const { asFragment } = await setup({});
    const fragment = await waitFor(() => asFragment());
    expect(fragment).toMatchSnapshot();
  });

  it('displays values', async () => {
    const { queryByTestId } = await setup({});

    expect(queryByTestId('improvement[1000].type')).toHaveTextContent('Commercial Building');
    expect(queryByTestId('improvement[1000].description')).toHaveTextContent('TEST DESCRIPTION');
  });
});
