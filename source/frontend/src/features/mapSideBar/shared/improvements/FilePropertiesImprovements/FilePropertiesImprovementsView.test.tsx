import { getMockApiProperty } from '@/mocks/properties.mock';
import { getMockPropertyImprovementsApi } from '@/mocks/propertyImprovements.mock';
import { render, RenderOptions, waitFor } from '@/utils/test-utils';

import { IFilePropertyImprovements } from '../models/FilePropertyImprovements';
import {
  FilePropertiesImprovementsView,
  IFilePropertiesImprovementsViewProps,
} from './FilePropertiesImprovementsView';

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

    expect(queryByTestId('improvement[0].name')).toHaveTextContent('TEST NAME');
    expect(queryByTestId('improvement[0].type')).toHaveTextContent('Commercial Building');
    expect(queryByTestId('improvement[0].date')).toHaveTextContent('Feb 14, 2026');
    expect(queryByTestId('improvement[0].status')).toHaveTextContent('Active');
    expect(queryByTestId('improvement[0].description')).toHaveTextContent('TEST DESCRIPTION');
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

  it('displays Lat/Long for properties when no other identifier is available', async () => {
    const propertyWithoutIdentifier = {
      ...getMockApiProperty(),
      pid: null,
      pin: null,
      latitude: 48.43,
      longitude: -123.49,
    };
    const { getByText } = await setup({
      props: {
        filePropertiesImprovements: [
          { property: propertyWithoutIdentifier, improvements: getMockPropertyImprovementsApi(1) },
        ],
      },
    });
    expect(getByText(/Location: 48.430000, -123.490000/i)).toBeInTheDocument();
  });
});
