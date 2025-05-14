import { Claims } from '@/constants';
import {
  getMockApiManagementActivitySubTypes,
  getMockPropertyManagementActivity,
} from '@/mocks/PropertyManagementActivity.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { mockManagementFileResponse } from '@/mocks/managementFiles.mock';
import { getMockApiPropertyFiles } from '@/mocks/properties.mock';
import { ApiGen_Concepts_PropertyActivity } from '@/models/api/generated/ApiGen_Concepts_PropertyActivity';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, getByName, render, RenderOptions, screen, userEvent } from '@/utils/test-utils';

import {
  IManagementActivityEditFormProps,
  ManagementActivityEditForm,
} from './ManagementActivityEditForm';

// Need to mock this library for unit tests
vi.mock('react-visibility-sensor', () => {
  return {
    default: vi.fn().mockImplementation(({ children }) => {
      if (children instanceof Function) {
        return children({ isVisible: true });
      }
      return children as Function;
    }),
  };
});

const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

const onCancel = vi.fn();
const onSave = vi.fn();
const setShow = vi.fn();
const onClose = vi.fn();

describe('ManagementActivityEditForm component', () => {
  const setup = (
    renderOptions: RenderOptions & {
      props?: Partial<IManagementActivityEditFormProps>;
    } = {},
  ) => {
    const rendered = render(
      <ManagementActivityEditForm
        managementFile={renderOptions?.props?.managementFile ?? mockManagementFileResponse()}
        activity={renderOptions?.props?.activity}
        subtypes={renderOptions?.props?.subtypes ?? getMockApiManagementActivitySubTypes()}
        gstConstant={renderOptions?.props?.gstConstant ?? 0}
        pstConstant={renderOptions?.props?.pstConstant ?? 0}
        loading={renderOptions?.props?.loading ?? false}
        show={renderOptions?.props?.show ?? true}
        setShow={renderOptions?.props?.setShow ?? setShow}
        onSave={renderOptions?.props?.onSave ?? onSave}
        onCancel={renderOptions?.props?.onCancel ?? onCancel}
        onClose={renderOptions?.props?.onClose ?? onClose}
      />,
      {
        store: storeState,
        useMockAuthentication: true,
        claims: renderOptions?.claims ?? [
          Claims.MANAGEMENT_VIEW,
          Claims.MANAGEMENT_ADD,
          Claims.MANAGEMENT_EDIT,
        ],
        ...renderOptions,
      },
    );

    return { ...rendered };
  };

  let initialValues: ApiGen_Concepts_PropertyActivity;

  beforeAll(() => {
    // Lock the current datetime as our snapshot has date-dependent fields
    vi.useFakeTimers();
    vi.setSystemTime(new Date('2025-05-13T17:00:00.000Z').getTime());
  });

  beforeEach(() => {
    initialValues = getMockPropertyManagementActivity(1);
    initialValues.managementFileId = 1;
  });

  afterEach(() => {
    vi.clearAllMocks();
  });

  afterAll(() => {
    // back to reality...
    vi.useRealTimers();
  });

  it('renders as expected', async () => {
    const { asFragment } = setup();
    expect(asFragment()).toMatchSnapshot();
  });

  it('validates required fields before submitting the form', async () => {
    setup();

    await act(async () => userEvent.paste(getByName('requestedSource'), 'Lorem Ipsum'));
    const save = screen.getByText('Save');
    await act(async () => userEvent.click(save));

    expect(await screen.findByText(/Description is required/i)).toBeVisible();
  });

  it('validates that completion date is required when status is set to COMPLETED', async () => {
    setup();

    await act(async () => {
      userEvent.selectOptions(getByName('activityStatusCode'), 'COMPLETED');
    });

    const save = screen.getByText('Save');
    await act(async () => userEvent.click(save));

    expect(await screen.findByText(/Completion date is required/i)).toBeVisible();
  });

  it('validates that completion date is after commencement date', async () => {
    setup();

    await act(async () => userEvent.paste(getByName('requestedDate'), '2024-10-10'));
    await act(async () => userEvent.paste(getByName('completionDate'), '2005-03-15'));

    const save = screen.getByText('Save');
    await act(async () => userEvent.click(save));

    expect(
      await screen.findByText(/Completion date must be after Commencement date/i),
    ).toBeVisible();
  });

  it(`submits the form when Save button is clicked`, async () => {
    setup();

    await act(async () => userEvent.paste(getByName('description'), 'some description goes here'));
    await act(async () => {
      userEvent.selectOptions(getByName('activityTypeCode'), 'APPLICPERMIT');
    });
    await act(async () => {
      userEvent.selectOptions(getByName('activitySubtypeCode'), 'ACCESS');
    });
    const save = screen.getByText('Save');
    await act(async () => userEvent.click(save));

    expect(onSave).toHaveBeenCalled();
  });

  it('triggers a confirmation popup when cancelling the form', async () => {
    setup();

    await act(async () => userEvent.paste(getByName('description'), 'some description goes here'));
    const cancel = screen.getByText('Cancel');
    await act(async () => userEvent.click(cancel));

    expect(
      await screen.findByText(/If you choose to cancel now, your changes will not be saved/i),
    ).toBeVisible();
  });

  it('calls onCancel when acknowledging the confirmation popup', async () => {
    setup();

    await act(async () => userEvent.paste(getByName('description'), 'some description goes here'));
    const cancel = screen.getByText('Cancel');
    await act(async () => userEvent.click(cancel));

    expect(
      await screen.findByText(/If you choose to cancel now, your changes will not be saved/i),
    ).toBeVisible();

    const okButton = screen.getByTitle('ok-modal');
    await act(async () => userEvent.click(okButton));

    expect(onCancel).toHaveBeenCalled();
  });

  it('calls onClose when the form is closed', async () => {
    setup();

    const closeBtn = screen.getByTitle('close');
    await act(async () => userEvent.click(closeBtn));

    expect(onClose).toHaveBeenCalled();
  });

  it('shows all properties selected by default when creating management activity', async () => {
    setup({
      props: {
        managementFile: {
          ...mockManagementFileResponse(),
          fileProperties: getMockApiPropertyFiles() as any,
        },
      },
    });

    expect(screen.getByTestId('selectrow-1')).toBeChecked();
    expect(screen.getByTestId('selectrow-2')).toBeChecked();
  });

  it('can select management file properties', async () => {
    setup({
      props: {
        managementFile: {
          ...mockManagementFileResponse(),
          fileProperties: getMockApiPropertyFiles() as any,
        },
      },
    });

    await act(async () => userEvent.click(screen.getByTestId('selectrow-1')));
    expect(screen.getByTestId('selectrow-1')).not.toBeChecked();
  });
});
