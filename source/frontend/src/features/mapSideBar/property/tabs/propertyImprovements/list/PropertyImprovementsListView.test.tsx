import { act, render, RenderOptions, userEvent, waitFor } from '@/utils/test-utils';
import PropertyImprovementsListView, {
  IPropertyImprovementsListViewProps,
} from './PropertyImprovementsListView';
import { getMockPropertyImprovementsApi } from '@/mocks/propertyImprovements.mock';
import Claims from '@/constants/claims';

const onDelete = vi.fn();
const mockPropertyImprovements = getMockPropertyImprovementsApi();

describe('File properties improvements list view', () => {
  const setup = async (
    renderOptions: RenderOptions & { props?: Partial<IPropertyImprovementsListViewProps> },
  ) => {
    const utils = render(
      <PropertyImprovementsListView
        loading={renderOptions?.props?.loading ?? false}
        propertyImprovements={
          renderOptions?.props?.propertyImprovements ?? mockPropertyImprovements
        }
        onDelete={onDelete}
      />,
      {
        ...renderOptions,
        claims: renderOptions?.claims ?? [Claims.PROPERTY_VIEW],
      },
    );

    return {
      ...utils,
      useMockAuthentication: true,
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
        loading: false,
        propertyImprovements: [],
      },
    });

    expect(
      getByText(/There are no commercial, residential, or other improvements indicated with this/i),
    ).toBeInTheDocument();
  });

  it(`renders the 'add improvement' button when user has permissions`, async () => {
    const { getByText, findByText } = await setup({
      props: {
        loading: false,
        propertyImprovements: [],
      },
      claims: [Claims.PROPERTY_VIEW, Claims.PROPERTY_EDIT],
    });

    const button = await findByText(/Add Improvement/i);
    expect(button).toBeVisible();

    expect(
      getByText(/There are no commercial, residential, or other improvements indicated with this/i),
    ).toBeInTheDocument();
  });

  it('call on delete improvement when clicked and confirm', async () => {
    const { getByTitle, findByText } = await setup({
      props: {},
      claims: [Claims.PROPERTY_VIEW, Claims.PROPERTY_EDIT],
    });

    const deleteButton = await getByTitle(/Delete Improvement/i);
    expect(deleteButton).toBeVisible();

    await act(async () => userEvent.click(deleteButton));

    expect(
      await findByText(/You have selected to delete this Improvement/i),
    ).toBeVisible();

    const okButton = getByTitle('ok-modal');
    await act(async () => userEvent.click(okButton));

    expect(onDelete).toHaveBeenCalledWith(1000);
  });
});
