import { Claims } from '@/constants';
import { getMockPropertyManagementActivity } from '@/mocks/PropertyManagementActivity.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, getByName, render, RenderOptions, screen, userEvent } from '@/utils/test-utils';

import {
  IPropertyActivityEditFormProps,
  PropertyActivityEditForm,
} from './PropertyActivityEditForm';

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

describe('PropertyActivityEditForm component', () => {
  const setup = (
    renderOptions: RenderOptions & {
      props?: Partial<IPropertyActivityEditFormProps>;
    } = {},
  ) => {
    const result = render(
      <PropertyActivityEditForm
        propertyId={renderOptions?.props?.propertyId ?? 1}
        activity={renderOptions?.props?.activity ?? getMockPropertyManagementActivity(1)}
        subtypes={renderOptions?.props?.subtypes ?? []}
        gstConstant={renderOptions?.props?.gstConstant ?? 0}
        pstConstant={renderOptions?.props?.pstConstant ?? 0}
        onCancel={renderOptions?.props?.onCancel ?? onCancel}
        loading={renderOptions?.props?.loading ?? false}
        show={renderOptions?.props?.show ?? true}
        setShow={renderOptions?.props?.setShow ?? setShow}
        onSave={renderOptions?.props?.onSave ?? onSave}
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

    return { ...result };
  };

  beforeEach(() => {
    vi.clearAllMocks();
  });

  it('renders as expected', async () => {
    const { asFragment } = setup();
    expect(asFragment()).toMatchSnapshot();
  });

  it('validates form values before submitting the form', async () => {
    setup();

    await act(async () => userEvent.paste(getByName('requestedSource'), 'Lorem Ipsum'));
    const save = screen.getByText('Save');
    await act(async () => userEvent.click(save));

    expect(await screen.findByText(/Description is required/i)).toBeVisible();
  });

  it(`submits the form when Save button is clicked`, async () => {
    setup();

    await act(async () => userEvent.paste(getByName('description'), 'some description goes here'));
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
});
