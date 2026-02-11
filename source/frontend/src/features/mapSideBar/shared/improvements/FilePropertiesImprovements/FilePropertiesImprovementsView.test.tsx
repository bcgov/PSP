import { render, RenderOptions, waitFor } from '@/utils/test-utils';
import {
  FilePropertiesImprovementsView,
  IFilePropertiesImprovementsViewProps,
} from './FilePropertiesImprovementsView';
import { IFilePropertyImprovements } from '../models/FilePropertyImprovements';
import { getMockApiProperty } from '@/mocks/properties.mock';
import { getMockPropertyImprovementsApi } from '@/mocks/propertyImprovements.mock';

const mockFilePropertiesImprovements: IFilePropertyImprovements[] = [
  { property: getMockApiProperty(), improvements: getMockPropertyImprovementsApi(1) },
];

describe('File properties improvements list view', () => {
  const setup = async (
    renderOptions: RenderOptions & { props?: Partial<IFilePropertiesImprovementsViewProps> },
  ) => {
    const utils = render(
      <FilePropertiesImprovementsView
        isLoading={renderOptions?.props?.isLoading ?? false}
        filePropertiesImprovements={
          renderOptions?.props?.filePropertiesImprovements ?? mockFilePropertiesImprovements
        }
      />,
      {
        ...renderOptions,
        useMockAuthentication: true,
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

    expect(queryByTestId('improvement[1000].name')).toHaveTextContent('TEST NAME');
    expect(queryByTestId('improvement[1000].type')).toHaveTextContent('Commercial Building');
    expect(queryByTestId('improvement[1000].date')).toHaveTextContent('Feb 14, 2026');
    expect(queryByTestId('improvement[1000].status')).toHaveTextContent('Active');
    expect(queryByTestId('improvement[1000].description')).toHaveTextContent('TEST DESCRIPTION');
  });

  it('displays message when no improvements', async () => {
    const { getByText } = await setup({
      props: {
        isLoading: false,
        filePropertiesImprovements: [{ property: getMockApiProperty(), improvements: [] }],
      },
    });

    expect(
      getByText(/There are no commercial, residential, or other improvements indicated with this/i),
    ).toBeInTheDocument();
  });
});
