import { getMockWorklistParcel } from '@/mocks/worklistParcel.mock';
import {
  act,
  cleanup,
  render,
  renderAsync,
  RenderOptions,
  screen,
  userEvent,
  waitForEffects,
} from '@/utils/test-utils';

import { IWorklistViewProps, WorklistView } from './WorklistView';

describe('WorklistView', () => {
  const onRemove = vi.fn();
  const onClearAll = vi.fn();
  const onCreateAcquisitionFile = vi.fn();
  const onCreateResearchFile = vi.fn();
  const onCreateDispositionFile = vi.fn();
  const onCreateLeaseFile = vi.fn();
  const onCreateManagementFile = vi.fn();
  const onAddToOpenFile = vi.fn();

  beforeEach(() => {
    cleanup();
    vi.clearAllMocks();
  });

  const setup = async (
    renderOptions: RenderOptions & { props?: Partial<IWorklistViewProps> } = {},
  ) => {
    const utils = await renderAsync(
      <WorklistView
        parcels={
          renderOptions.props?.parcels ?? [getMockWorklistParcel('parcel-1', { PID: '123456789' })]
        }
        onRemove={onRemove}
        onClearAll={onClearAll}
        onCreateAcquisitionFile={onCreateAcquisitionFile}
        onCreateResearchFile={onCreateResearchFile}
        onCreateDispositionFile={onCreateDispositionFile}
        onCreateLeaseFile={onCreateLeaseFile}
        onCreateManagementFile={onCreateManagementFile}
        onAddToOpenFile={onAddToOpenFile}
      />,
      {
        useMockAuthentication: true,
        ...renderOptions,
      },
    );

    return {
      ...utils,
      getWorklistItemParcelIdentifier: (index: number) =>
        utils.container.querySelector(
          `div[data-testid='worklist-item[${index}].parcel.identifier']`,
        ) as HTMLElement,
      getWorklistItemCollapseBtn: (index: number) =>
        utils.container.querySelector(
          `div[data-testid='worklist-item[${index}].collapse-btn']`,
        ) as HTMLElement,
      getWorklistItemChildIdentifier: (index: number, childIndex: number) =>
        utils.container.querySelector(
          `div[data-testid='worklist-item[${index}].child[${childIndex}].identifier']`,
        ) as HTMLElement,
    };
  };

  it('matches snapshot with no parcels (empty state)', async () => {
    const { asFragment } = await setup({ props: { parcels: [] } });
    expect(asFragment()).toMatchSnapshot();
  });

  it('matches snapshot with multiple parcels', async () => {
    const { asFragment } = await setup({
      props: {
        parcels: [
          getMockWorklistParcel('parcel-1', { PID: '123456789' }),
          getMockWorklistParcel('parcel-2', { PIN: 99999999 }),
        ],
      },
    });
    expect(asFragment()).toMatchSnapshot();
  });

  it('calls onClearAll from MoreOptionsDropdown', async () => {
    await setup();

    const toggle = screen.getByRole('button', { name: /worklist more options/i });
    await act(async () => userEvent.click(toggle));

    const clearAllItem = await screen.findByRole('button', { name: /clear list/i });
    await act(async () => userEvent.click(clearAllItem));

    expect(onClearAll).toHaveBeenCalled();
  });

  it('displays the common property identifier', async () => {
    const { getWorklistItemParcelIdentifier, getWorklistItemCollapseBtn } = await setup({
      props: {
        parcels: [getMockWorklistParcel('parcel-1', { PLAN_NUMBER: 'VIS1234' })],
      },
    });

    await waitForEffects();

    expect(getWorklistItemParcelIdentifier(0)).toHaveTextContent('Common Property: VIS1234');
    expect(getWorklistItemCollapseBtn(0)).not.toBeInTheDocument();
  });

  it('displays the common property identifier and groups the child under', async () => {
    const {
      getWorklistItemParcelIdentifier,
      getWorklistItemCollapseBtn,
      getWorklistItemChildIdentifier,
    } = await setup({
      props: {
        parcels: [
          getMockWorklistParcel('parcel-1', { PLAN_NUMBER: 'VIS1234' }),
          getMockWorklistParcel('parcel-2', { PID: '000402117', PLAN_NUMBER: 'VIS1234' }),
        ],
      },
    });

    await waitForEffects();

    expect(getWorklistItemParcelIdentifier(0)).toHaveTextContent('Common Property: VIS1234');
    expect(getWorklistItemCollapseBtn(0)).toBeInTheDocument();
    expect(getWorklistItemChildIdentifier(0, 0)).toHaveTextContent('PID: 000-402-117');
  });

  it('displays the common property identifier and groups more than one child', async () => {
    const {
      getWorklistItemParcelIdentifier,
      getWorklistItemCollapseBtn,
      getWorklistItemChildIdentifier,
    } = await setup({
      props: {
        parcels: [
          getMockWorklistParcel('parcel-1', { PLAN_NUMBER: 'VIS1234' }),
          getMockWorklistParcel('parcel-2', { PID: '000402117', PLAN_NUMBER: 'VIS1234' }),
          getMockWorklistParcel('parcel-3', { PID: '000402118', PLAN_NUMBER: 'VIS1234' }),
        ],
      },
    });

    await waitForEffects();

    expect(getWorklistItemParcelIdentifier(0)).toHaveTextContent('Common Property: VIS1234');
    expect(getWorklistItemCollapseBtn(0)).toBeInTheDocument();
    expect(getWorklistItemChildIdentifier(0, 0)).toHaveTextContent('PID: 000-402-117');
    expect(getWorklistItemChildIdentifier(0, 1)).toHaveTextContent('PID: 000-402-118');
  });

  it('displays the common property identifier and orphan child parcels', async () => {
    const {
      getWorklistItemParcelIdentifier,
      getWorklistItemCollapseBtn,
      getWorklistItemChildIdentifier,
    } = await setup({
      props: {
        parcels: [
          getMockWorklistParcel('parcel-1', { PLAN_NUMBER: 'VIS1234' }),
          getMockWorklistParcel('parcel-2', { PID: '000402117', PLAN_NUMBER: 'VIS1234' }),
          getMockWorklistParcel('parcel-3', { PID: '000402118', PLAN_NUMBER: 'VIS1235' }),
        ],
      },
    });

    await waitForEffects();

    expect(getWorklistItemParcelIdentifier(0)).toHaveTextContent('Common Property: VIS1234');
    expect(getWorklistItemCollapseBtn(0)).toBeInTheDocument();
    expect(getWorklistItemChildIdentifier(0, 0)).toHaveTextContent('PID: 000-402-117');
    expect(getWorklistItemChildIdentifier(0, 1)).not.toBeInTheDocument();

    expect(getWorklistItemParcelIdentifier(1)).toBeInTheDocument();
    expect(getWorklistItemParcelIdentifier(1)).toHaveTextContent('PID: 000-402-118');
  });
});
