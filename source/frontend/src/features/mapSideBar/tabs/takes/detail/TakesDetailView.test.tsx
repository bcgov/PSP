import { Claims } from 'constants/claims';
import { createMemoryHistory } from 'history';
import { mockLookups } from 'mocks/mockLookups';
import { getMockApiPropertyFiles } from 'mocks/mockProperties';
import { getMockApiTakes } from 'mocks/mockTakes';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { act, render, RenderOptions, screen, userEvent, within } from 'utils/test-utils';

import TakesDetailView, { ITakesDetailViewProps } from './TakesDetailView';

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

const onEdit = jest.fn();
jest.mock('@react-keycloak/web');

describe('TakesDetailView component', () => {
  // render component under test
  const setup = (renderOptions: RenderOptions & { props?: Partial<ITakesDetailViewProps> }) => {
    const utils = render(
      <TakesDetailView
        {...renderOptions.props}
        takes={renderOptions.props?.takes ?? []}
        allTakesCount={renderOptions.props?.allTakesCount ?? 0}
        loading={renderOptions.props?.loading ?? false}
        fileProperty={renderOptions.props?.fileProperty ?? getMockApiPropertyFiles()[0]}
        onEdit={onEdit}
      />,
      {
        ...renderOptions,
        claims: renderOptions.claims ?? [],
        store: storeState,
        history,
      },
    );

    return {
      ...utils,
    };
  };

  afterEach(() => {
    jest.clearAllMocks();
  });

  it('renders as expected', () => {
    const { asFragment } = setup({});
    expect(asFragment()).toMatchSnapshot();
  });

  it('displays a loading spinner when loading', () => {
    const { getByTestId } = setup({ props: { loading: true } });
    const spinner = getByTestId('filter-backdrop-loading');
    expect(spinner).toBeVisible();
  });

  it('clicking the edit button fires the edit event', () => {
    const { getByTitle } = setup({ props: { loading: true }, claims: [Claims.PROPERTY_EDIT] });
    const editButton = getByTitle('Edit takes');
    act(() => userEvent.click(editButton));
    expect(onEdit).toHaveBeenCalled();
  });

  it('displays all non-cancelled takes and then cancelled takes', () => {
    setup({
      props: {
        loading: false,
        takes: [
          { ...getMockApiTakes()[0], takeStatusTypeCode: 'CANCELLED', id: 1 },
          { ...getMockApiTakes()[0], takeStatusTypeCode: 'INPROGRESS' },
        ],
      },
    });
    const { getByText } = within(screen.getByTestId('take-0'));
    expect(getByText('In-progress'));
  });

  it('does not display an area fields if all is radio buttons are false', () => {
    const { queryAllByText } = setup({
      props: {
        loading: false,
        takes: [
          {
            ...getMockApiTakes()[0],
            isLicenseToConstruct: false,
            isNewRightOfWay: false,
            isSection16: false,
            isStatutoryRightOfWay: false,
            isSurplusSeverance: false,
          },
        ],
      },
    });
    expect(queryAllByText('Area:')).toHaveLength(0);
  });
  it('displays all area fields if all is radio buttons are true', () => {
    const { getAllByText } = setup({
      props: {
        loading: false,
        takes: [
          {
            ...getMockApiTakes()[0],
            isLicenseToConstruct: true,
            isNewRightOfWay: true,
            isSection16: true,
            isStatutoryRightOfWay: true,
            isSurplusSeverance: true,
          },
        ],
      },
    });
    expect(getAllByText('Area:')).toHaveLength(5);
  });

  it('displays ltcEndDt if specified', async () => {
    const { findByText } = setup({
      props: {
        loading: false,
        takes: [
          {
            ...getMockApiTakes()[0],
            isLicenseToConstruct: true,
            ltcEndDt: '2020-01-01',
          },
        ],
      },
    });
    const date = await findByText('LTC end date:');
    expect(date).toBeVisible();
  });

  it('displays section16EndDt if specified', async () => {
    const { findByText } = setup({
      props: {
        loading: false,
        takes: [
          {
            ...getMockApiTakes()[0],
            isSection16: true,
            section16EndDt: '2020-01-01',
          },
        ],
      },
    });
    const date = await findByText('Section 16 end date:');
    expect(date).toBeVisible();
  });

  it('displays message when there are no takes', async () => {
    const { findByText } = setup({
      props: {
        loading: false,
        takes: [],
      },
    });
    const date = await findByText('There are no takes recorded for this property in this file');
    expect(date).toBeVisible();
  });

  it('displays count of takes on file', async () => {
    const { findByText } = setup({
      props: {
        loading: false,
        takes: [{} as any],
      },
    });
    const date = await findByText('1 take(s)', {
      exact: false,
    });
    expect(date).toBeVisible();
  });
});
