import { createMemoryHistory } from 'history';

import { AcquisitionStatus } from '@/constants/acquisitionFileStatus';
import { Claims } from '@/constants/claims';
import { mockLookups } from '@/mocks/lookups.mock';
import { getMockApiPropertyFiles } from '@/mocks/properties.mock';
import { getMockApiTakes } from '@/mocks/takes.mock';
import { ApiGen_Concepts_File } from '@/models/api/generated/ApiGen_Concepts_File';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { toTypeCodeNullable } from '@/utils/formUtils';
import { act, render, RenderOptions, screen, userEvent, within } from '@/utils/test-utils';

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

  it('hides the edit button when the file has been completed', () => {
    const fileProperty = getMockApiPropertyFiles()[0];
    const file: ApiGen_Concepts_File = fileProperty!.file as ApiGen_Concepts_File;
    const { queryByTitle, getByTestId } = setup({
      props: {
        loading: true,
        fileProperty: {
          ...fileProperty,
          file: {
            ...file,
            fileStatusTypeCode: toTypeCodeNullable(AcquisitionStatus.Complete),
          },
        },
      },
      claims: [Claims.PROPERTY_EDIT],
    });
    const editButton = queryByTitle('Edit takes');
    expect(editButton).toBeNull();
    const tooltip = getByTestId('tooltip-icon-1-summary-cannot-edit-tooltip');
    expect(tooltip).toBeVisible();
  });

  it('displays the number of takes in other files', () => {
    setup({
      props: {
        loading: false,
        takes: [
          {
            ...getMockApiTakes()[0],
            takeStatusTypeCode: toTypeCodeNullable('CANCELLED'),
            id: 1,
          },
          { ...getMockApiTakes()[0], takeStatusTypeCode: toTypeCodeNullable('INPROGRESS') },
        ],
        allTakesCount: 10,
      },
    });
    const takesNotInFile = screen.getByTestId('takes-in-other-files');
    expect(takesNotInFile).toHaveTextContent('8');
  });

  it('displays all non-cancelled takes and then cancelled takes', () => {
    setup({
      props: {
        loading: false,
        takes: [
          {
            ...getMockApiTakes()[0],
            takeStatusTypeCode: toTypeCodeNullable('CANCELLED'),
            id: 1,
          },
          { ...getMockApiTakes()[0], takeStatusTypeCode: toTypeCodeNullable('INPROGRESS') },
        ],
      },
    });
    const { getByText } = within(screen.getByTestId('take-0'));
    expect(getByText('In-progress')).toBeVisible();
  });

  it('does not display an area fields if all is radio buttons are false', () => {
    const { queryAllByText } = setup({
      props: {
        loading: false,
        takes: [
          {
            ...getMockApiTakes()[0],
            isNewLicenseToConstruct: false,
            isNewHighwayDedication: false,
            isNewLandAct: false,
            isNewInterestInSrw: false,
            isThereSurplus: false,
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
            isNewLicenseToConstruct: true,
            isNewHighwayDedication: true,
            isNewLandAct: true,
            isNewInterestInSrw: true,
            isThereSurplus: true,
          },
        ],
      },
    });
    expect(getAllByText('Area:')).toHaveLength(5);
  });

  it('displays srwEndDt if specified', async () => {
    const { findByText } = setup({
      props: {
        loading: false,
        takes: [
          {
            ...getMockApiTakes()[0],
            isNewInterestInSrw: true,
            srwEndDt: '2020-01-01',
          },
        ],
      },
    });
    const date = await findByText('SRW end date:');
    expect(date).toBeVisible();
  });

  it('displays ltcEndDt if specified', async () => {
    const { findByText } = setup({
      props: {
        loading: false,
        takes: [
          {
            ...getMockApiTakes()[0],
            isNewLicenseToConstruct: true,
            ltcEndDt: '2020-01-01',
          },
        ],
      },
    });
    const date = await findByText('LTC end date:');
    expect(date).toBeVisible();
  });

  it('displays landActEndDt if specified', async () => {
    const { findByText } = setup({
      props: {
        loading: false,
        takes: [
          {
            ...getMockApiTakes()[0],
            isNewLandAct: true,
            landActEndDt: '2020-01-01',
          },
        ],
      },
    });
    const date = await findByText('Is a there a new Land Act tenure', {
      exact: false,
    });
    expect(date).toBeVisible();
  });

  it('displays the land act code', async () => {
    const { findByText } = setup({
      props: {
        loading: false,
        takes: [
          {
            ...getMockApiTakes()[0],
            isNewLicenseToConstruct: true,
            ltcEndDt: '2020-01-01',
          },
        ],
      },
    });
    const code = await findByText('Section 15', {
      exact: false,
    });
    expect(code).toBeVisible();
  });
});
