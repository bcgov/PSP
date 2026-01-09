import { render, RenderOptions, waitFor } from '@/utils/test-utils';
import { IFilePropertyImprovements } from '../models/FilePropertyImprovements';
import { getMockApiProperty } from '@/mocks/properties.mock';
import { getMockPropertyImprovementApi, getMockPropertyImprovementsApi } from '@/mocks/propertyImprovements.mock';
import PropertyImprovementDetails, {
  IPropertyImprovementDetailsProps,
} from './PropertyImprovementDetails';

const mockFilePropertiesImprovements: IFilePropertyImprovements[] = [
  { property: getMockApiProperty(), improvements: getMockPropertyImprovementsApi(1) },
];

describe('Property Improvement Details view', () => {
  const setup = async (
    renderOptions: RenderOptions & { props?: Partial<IPropertyImprovementDetailsProps> },
  ) => {
    const utils = render(<PropertyImprovementDetails propertyImprovement={
      renderOptions?.props?.propertyImprovement ?? getMockPropertyImprovementApi()
    } />, {
      ...renderOptions,
    });

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
