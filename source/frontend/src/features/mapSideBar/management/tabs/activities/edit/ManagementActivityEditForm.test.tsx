import { Claims } from '@/constants';
import { getMockPropertyManagementActivity } from '@/mocks/PropertyManagementActivity.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { mockManagementFileResponse } from '@/mocks/managementFiles.mock';
import { getMockApiPropertyFiles } from '@/mocks/properties.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import {
  act,
  getByName,
  render,
  RenderOptions,
  screen,
  selectOptions,
  userEvent,
  waitForEffects,
  within,
} from '@/utils/test-utils';

import {
  IManagementActivityEditFormProps,
  ManagementActivityEditForm,
} from './ManagementActivityEditForm';
import { ManagementActivityFormModel } from './models';

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

const mockManagementActivityFormValues: ManagementActivityFormModel =
  ManagementActivityFormModel.fromApi({
    ...getMockPropertyManagementActivity(1),
    activityProperties: [],
  });

const onCancel = vi.fn();
const onSave = vi.fn();
const setShow = vi.fn();
const onClose = vi.fn();

describe('ManagementActivityEditForm component', () => {
  const setup = async (
    renderOptions: RenderOptions & {
      props?: Partial<IManagementActivityEditFormProps>;
    } = {},
  ) => {
    const utils = render(
      <ManagementActivityEditForm
        managementFile={renderOptions?.props?.managementFile ?? mockManagementFileResponse()}
        initialValues={renderOptions?.props?.initialValues ?? mockManagementActivityFormValues}
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

    return {
      ...utils,
      getSubTypesMultiSelect: () =>
        utils.container.querySelector(`#multiselect-activitySubtypeCodes_input`) as HTMLElement,
    };
  };

  let initialValues: ManagementActivityFormModel;

  beforeAll(() => {
    // Lock the current datetime as our snapshot has date-dependent fields
    vi.useFakeTimers();
    vi.setSystemTime(new Date('2025-05-13T17:00:00.000Z').getTime());
  });

  beforeEach(() => {
    initialValues = ManagementActivityFormModel.fromApi(getMockPropertyManagementActivity(1));
  });

  afterEach(() => {
    vi.clearAllMocks();
  });

  afterAll(() => {
    // back to reality...
    vi.useRealTimers();
  });

  it('renders as expected', async () => {
    const { asFragment } = await setup();
    expect(asFragment()).toMatchSnapshot();
  });

  it('validates required fields before submitting the form', async () => {
    await setup();
    await waitForEffects();

    await act(async () => userEvent.paste(getByName('requestedSource'), 'Lorem Ipsum'));
    const save = screen.getByText('Save');
    await act(async () => userEvent.click(save));

    expect(await screen.findByText(/Description is required/i)).toBeVisible();
  });

  it('validates that completion date is required when status is set to COMPLETED', async () => {
    await setup();
    await waitForEffects();

    await act(async () => {
      userEvent.selectOptions(getByName('activityStatusCode'), 'COMPLETED');
    });

    const save = screen.getByText('Save');
    await act(async () => userEvent.click(save));

    expect(await screen.findByText(/Completion date is required/i)).toBeVisible();
  });

  it('validates that completion date is after commencement date', async () => {
    vi.spyOn(console, 'error').mockImplementation(() => {});
    await setup();
    await waitForEffects();

    await act(async () => {
      userEvent.type(getByName('requestedDate'), '2024-10-10', { delay: 100 });
    });
    await act(async () => {
      userEvent.type(getByName('completionDate'), '2005-03-15', { delay: 100 });
    });

    const save = screen.getByText('Save');
    await act(async () => userEvent.click(save));

    expect(
      await screen.findByText(/Completion date must be after Commencement date/i),
    ).toBeVisible();
  });

  it(`clears the activity sub-type when the main type is changed`, async () => {
    const { getSubTypesMultiSelect, container } = await setup({
      props: { initialValues: initialValues },
    });
    await waitForEffects();

    await act(async () => selectOptions('activityTypeCode', 'CONSULTATION'));
    await waitForEffects();

    // Select the Sub-Type
    const multiSelectSubTypes = getSubTypesMultiSelect();
    expect(multiSelectSubTypes).not.toBeNull();

    await act(async () => {
      userEvent.click(multiSelectSubTypes);
      userEvent.type(multiSelectSubTypes, 'Internal');
      userEvent.click(multiSelectSubTypes);

      const firstOption = container.querySelector(`div ul li.option`);
      console.log(firstOption);
      userEvent.click(firstOption);
    });
    await waitForEffects();

    // Change the Type
    await act(async () => {
      userEvent.selectOptions(getByName('activityTypeCode'), 'PROPERTYMTC');
    });
    await waitForEffects();

    expect(multiSelectSubTypes).toHaveValue('');
  });

  it(`submits the form when Save button is clicked`, async () => {
    const { getSubTypesMultiSelect, container } = await setup();
    await waitForEffects();

    await act(async () => userEvent.paste(getByName('description'), 'some description goes here'));
    await act(async () => selectOptions('activityTypeCode', 'CONSULTATION'));

    await waitForEffects();

    // Select the Sub-Type
    const multiSelectSubTypes = getSubTypesMultiSelect();
    expect(multiSelectSubTypes).not.toBeNull();

    await act(async () => {
      userEvent.click(multiSelectSubTypes);
      userEvent.type(multiSelectSubTypes, 'Internal');
      userEvent.click(multiSelectSubTypes);

      const firstOption = container.querySelector(`div ul li.option`);
      console.log(firstOption);
      userEvent.click(firstOption);
    });
    await waitForEffects();
    const save = screen.getByText('Save');
    await act(async () => userEvent.click(save));

    expect(onSave).toHaveBeenCalled();
  });

  it('triggers a confirmation popup when cancelling the form', async () => {
    await setup();
    await waitForEffects();

    await act(async () => userEvent.paste(getByName('description'), 'some description goes here'));
    const cancel = screen.getByText('Cancel');
    await act(async () => userEvent.click(cancel));

    expect(
      await screen.findByText(/If you choose to cancel now, your changes will not be saved/i),
    ).toBeVisible();
  });

  it('calls onCancel when acknowledging the confirmation popup', async () => {
    await setup();
    await waitForEffects();

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
    await setup();
    await waitForEffects();

    const closeBtn = screen.getByTitle('close');
    await act(async () => userEvent.click(closeBtn));

    expect(onClose).toHaveBeenCalled();
  });

  it('shows all properties selected by default when creating management activity', async () => {
    const mockDefaultValues = new ManagementActivityFormModel(null, 1);

    await setup({
      props: {
        initialValues: mockDefaultValues,
        managementFile: {
          ...mockManagementFileResponse(),
        },
      },
    });
    await waitForEffects();

    expect(screen.getByTestId('selectrow-10')).toBeChecked();
  });

  it('can select management file properties', async () => {
    const mockDefaultValues = new ManagementActivityFormModel(null, 1);

    await setup({
      props: {
        initialValues: mockDefaultValues,
        managementFile: {
          ...mockManagementFileResponse(),
        },
      },
    });
    await waitForEffects();

    await act(async () => userEvent.click(screen.getByTestId('selectrow-10')));
    expect(screen.getByTestId('selectrow-10')).not.toBeChecked();
  });
});
