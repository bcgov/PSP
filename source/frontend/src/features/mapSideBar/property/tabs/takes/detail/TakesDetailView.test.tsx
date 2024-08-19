import { createMemoryHistory } from 'history';

import { Claims } from '@/constants/claims';
import { mockLookups } from '@/mocks/lookups.mock';
import { getMockApiPropertyFiles } from '@/mocks/properties.mock';
import { getMockApiTakes } from '@/mocks/takes.mock';
import { ApiGen_Concepts_File } from '@/models/api/generated/ApiGen_Concepts_File';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { toTypeCodeNullable } from '@/utils/formUtils';
import { act, render, RenderOptions, screen, userEvent, within } from '@/utils/test-utils';

import TakesDetailView, { ITakesDetailViewProps } from './TakesDetailView';
import { ApiGen_CodeTypes_AcquisitionStatusTypes } from '@/models/api/generated/ApiGen_CodeTypes_AcquisitionStatusTypes';
import { ApiGen_CodeTypes_AcquisitionTakeStatusTypes } from '@/models/api/generated/ApiGen_CodeTypes_AcquisitionTakeStatusTypes';
import Roles from '@/constants/roles';

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

const onEdit = vi.fn();
const onAdd = vi.fn();
const onDelete = vi.fn();

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
        onAdd={onAdd}
        onDelete={onDelete}
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
    vi.clearAllMocks();
  });

  it('renders as expected', () => {
    const { asFragment } = setup({ props: { takes: getMockApiTakes() } });
    expect(asFragment()).toMatchSnapshot();
  });

  it('displays a loading spinner when loading', () => {
    const { getByTestId } = setup({ props: { loading: true } });
    const spinner = getByTestId('filter-backdrop-loading');
    expect(spinner).toBeVisible();
  });

  it('clicking the edit button fires the edit event', async () => {
    const { getByTitle } = setup({
      props: { takes: getMockApiTakes() },
      claims: [Claims.PROPERTY_EDIT, Claims.ACQUISITION_EDIT],
    });
    const editButton = getByTitle('Edit take');
    await act(async () => userEvent.click(editButton));
    expect(onEdit).toHaveBeenCalled();
  });

  it('hides the add button when the file has been completed', () => {
    const fileProperty = getMockApiPropertyFiles()[0];
    const file: ApiGen_Concepts_File = fileProperty.file as ApiGen_Concepts_File;
    const { queryByTitle, getByTestId } = setup({
      props: {
        fileProperty: {
          ...fileProperty,
          file: {
            ...file,
            fileStatusTypeCode: toTypeCodeNullable(ApiGen_CodeTypes_AcquisitionStatusTypes.COMPLT),
          },
        },
        takes: getMockApiTakes(),
      },
      claims: [Claims.PROPERTY_EDIT],
    });
    const addButton = queryByTitle('Add take');
    expect(addButton).toBeNull();
  });

  it('hides the edit button when the file has been completed', () => {
    const fileProperty = getMockApiPropertyFiles()[0];
    const file: ApiGen_Concepts_File = fileProperty.file as ApiGen_Concepts_File;
    const { queryByTitle, getByTestId } = setup({
      props: {
        fileProperty: {
          ...fileProperty,
          file: {
            ...file,
            fileStatusTypeCode: toTypeCodeNullable(ApiGen_CodeTypes_AcquisitionStatusTypes.COMPLT),
          },
        },
        takes: getMockApiTakes(),
      },
      claims: [Claims.PROPERTY_EDIT],
    });
    const editButton = queryByTitle('Edit take');
    expect(editButton).toBeNull();
    const tooltip = getByTestId('tooltip-icon-1-summary-cannot-edit-tooltip');
    expect(tooltip).toBeVisible();
  });

  it('hides the edit button when the file has been completed for admins', () => {
    const fileProperty = getMockApiPropertyFiles()[0];
    const file: ApiGen_Concepts_File = fileProperty.file as ApiGen_Concepts_File;
    const { queryByTitle, getByTestId } = setup({
      props: {
        fileProperty: {
          ...fileProperty,
          file: {
            ...file,
            fileStatusTypeCode: toTypeCodeNullable(ApiGen_CodeTypes_AcquisitionStatusTypes.COMPLT),
          },
        },
        takes: getMockApiTakes(),
      },
      claims: [Claims.PROPERTY_EDIT],
      roles: [Roles.SYSTEM_ADMINISTRATOR],
    });
    const editButton = queryByTitle('Edit take');
    expect(editButton).toBeNull();
    const tooltip = getByTestId('tooltip-icon-1-summary-cannot-edit-tooltip');
    expect(tooltip).toBeVisible();
  });

  it('hides the edit button when the take has been completed', () => {
    const { queryByTitle, getByTestId } = setup({
      props: {
        takes: [
          {
            ...getMockApiTakes()[0],
            takeStatusTypeCode: toTypeCodeNullable(
              ApiGen_CodeTypes_AcquisitionTakeStatusTypes.COMPLETE.toString(),
            ),
          },
        ],
      },
      claims: [Claims.PROPERTY_EDIT, Claims.ACQUISITION_EDIT],
    });
    const editButton = queryByTitle('Edit take');
    expect(editButton).toBeNull();
    const tooltip = getByTestId('tooltip-icon-1-summary-cannot-edit-tooltip');
    expect(tooltip).toBeVisible();
  });

  it('does not hide the edit button when the user is an admin even if the take is complete', async () => {
    const { getByTitle } = setup({
      props: {
        takes: [
          {
            ...getMockApiTakes()[0],
            takeStatusTypeCode: toTypeCodeNullable(
              ApiGen_CodeTypes_AcquisitionTakeStatusTypes.COMPLETE.toString(),
            ),
          },
        ],
      },
      claims: [Claims.PROPERTY_EDIT],
      roles: [Roles.SYSTEM_ADMINISTRATOR],
    });
    const editButton = getByTitle('Edit take');
    await act(async () => userEvent.click(editButton));
    expect(onEdit).toHaveBeenCalled();
  });

  it('clicking the delete button fires the edit event', async () => {
    const { getByTitle } = setup({
      props: { takes: getMockApiTakes() },
      claims: [Claims.PROPERTY_EDIT, Claims.ACQUISITION_EDIT],
    });
    const removeButton = getByTitle('Remove take');
    await act(async () => userEvent.click(removeButton));
    const yesButton = screen.getByTestId('ok-modal-button');
    await act(async () => userEvent.click(yesButton));
    expect(onDelete).toHaveBeenCalled();
  });

  it('hides the delete button when the file has been completed', () => {
    const fileProperty = getMockApiPropertyFiles()[0];
    const file: ApiGen_Concepts_File = fileProperty.file as ApiGen_Concepts_File;
    const { queryByTitle } = setup({
      props: {
        fileProperty: {
          ...fileProperty,
          file: {
            ...file,
            fileStatusTypeCode: toTypeCodeNullable(ApiGen_CodeTypes_AcquisitionStatusTypes.COMPLT),
          },
        },
        takes: getMockApiTakes(),
      },
      claims: [Claims.PROPERTY_EDIT],
    });
    const removeButton = queryByTitle('Remove take');
    expect(removeButton).toBeNull();
  });

  it('hides the delete button when the take has been completed', () => {
    const { queryByTitle } = setup({
      props: {
        takes: [
          {
            ...getMockApiTakes()[0],
            takeStatusTypeCode: toTypeCodeNullable(
              ApiGen_CodeTypes_AcquisitionTakeStatusTypes.COMPLETE.toString(),
            ),
          },
        ],
      },
      claims: [Claims.PROPERTY_EDIT],
    });
    const removeButton = queryByTitle('Remove take');
    expect(removeButton).toBeNull();
  });

  it('does not hide delete button when the take has been completed and user is an admin', () => {
    const { queryByTitle } = setup({
      props: {
        takes: [
          {
            ...getMockApiTakes()[0],
            takeStatusTypeCode: toTypeCodeNullable(
              ApiGen_CodeTypes_AcquisitionTakeStatusTypes.COMPLETE.toString(),
            ),
          },
        ],
      },
      claims: [Claims.PROPERTY_EDIT],
      roles: [Roles.SYSTEM_ADMINISTRATOR],
    });
    const removeButton = queryByTitle('Remove take');
    expect(removeButton).toBeVisible();
  });

  it('displays the number of takes in other files', () => {
    setup({
      props: {
        takes: [
          {
            ...getMockApiTakes()[0],
            takeStatusTypeCode: toTypeCodeNullable(
              ApiGen_CodeTypes_AcquisitionTakeStatusTypes.CANCELLED.toString(),
            ),
            id: 1,
          },
          {
            ...getMockApiTakes()[0],
            takeStatusTypeCode: toTypeCodeNullable(
              ApiGen_CodeTypes_AcquisitionTakeStatusTypes.INPROGRESS.toString(),
            ),
          },
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
        takes: [
          {
            ...getMockApiTakes()[0],
            takeStatusTypeCode: toTypeCodeNullable(
              ApiGen_CodeTypes_AcquisitionTakeStatusTypes.CANCELLED.toString(),
            ),
            id: 1,
          },
          {
            ...getMockApiTakes()[0],
            takeStatusTypeCode: toTypeCodeNullable(
              ApiGen_CodeTypes_AcquisitionTakeStatusTypes.INPROGRESS.toString(),
            ),
          },
        ],
      },
    });
    const { getByText } = within(screen.getByTestId('take-0'));
    expect(getByText('In-progress')).toBeVisible();
  });

  it('does not display an area fields if all is radio buttons are false', () => {
    const { queryAllByText } = setup({
      props: {
        takes: [
          {
            ...getMockApiTakes()[0],
            isNewLicenseToConstruct: false,
            isNewHighwayDedication: false,
            isNewLandAct: false,
            isNewInterestInSrw: false,
            isThereSurplus: false,
            isLeasePayable: false,
          },
        ],
      },
    });
    expect(queryAllByText('Area:')).toHaveLength(0);
  });

  it('displays all area fields if all is radio buttons are true', () => {
    const { getAllByText } = setup({
      props: {
        takes: [
          {
            ...getMockApiTakes()[0],
            isNewLicenseToConstruct: true,
            isNewHighwayDedication: true,
            isNewLandAct: true,
            isNewInterestInSrw: true,
            isThereSurplus: true,
            isLeasePayable: true,
          },
        ],
      },
    });
    expect(getAllByText('Area:')).toHaveLength(6);
  });

  it('displays srwEndDt if specified', async () => {
    const { findByText } = setup({
      props: {
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
        takes: [
          {
            ...getMockApiTakes()[0],
            isNewLandAct: true,
            landActEndDt: '2020-01-01',
          },
        ],
      },
    });
    const date = await findByText('Is there a new Land Act tenure', {
      exact: false,
    });
    expect(date).toBeVisible();
  });

  it('displays leasePayableEndDt if specified', async () => {
    const { findByText } = setup({
      props: {
        takes: [
          {
            ...getMockApiTakes()[0],
            isNewLandAct: true,
            leasePayableEndDt: '2022-11-21',
          },
        ],
      },
    });
    const date = await findByText('Is there a Lease (Payable)', {
      exact: false,
    });
    expect(date).toBeVisible();
  });

  it('displays completionDt if specified', async () => {
    const { findByText } = setup({
      props: {
        takes: [
          {
            ...getMockApiTakes()[0],
            completionDt: '2022-11-21',
          },
        ],
      },
    });
    const date = await findByText('Completion date *:');
    expect(date).toBeVisible();
  });

  it('displays the land act code', async () => {
    const { findByText } = setup({
      props: {
        takes: [
          {
            ...getMockApiTakes()[0],
            isNewLandAct: true,
            landActTypeCode: {
              id: 'Section 15',
              description: 'Reserve',
              displayOrder: null,
              isDisabled: false,
            },
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
