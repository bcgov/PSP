import { getMockWorklistParcel } from '@/mocks/worklistParcel.mock';
import { act, cleanup, render, RenderOptions, screen, userEvent } from '@/utils/test-utils';

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

  const setup = (renderOptions: RenderOptions & { props?: Partial<IWorklistViewProps> } = {}) => {
    return render(
      <WorklistView
        parcels={renderOptions.props?.parcels ?? [getMockWorklistParcel('parcel-1', { PID: '123456789' })]}
        onRemove={onRemove}
        onClearAll={onClearAll}
        onCreateAcquisitionFile={onCreateAcquisitionFile}
        onCreateResearchFile={onCreateResearchFile}
        onCreateDispositionFile={onCreateDispositionFile}
        onCreateLeaseFile={onCreateLeaseFile}
        onCreateManagementFile={onCreateManagementFile}
        onAddToOpenFile={onAddToOpenFile} />,
      {
        useMockAuthentication: true,
        ...renderOptions,
      },
    );
  };

  it('matches snapshot with no parcels (empty state)', () => {
    const { asFragment } = setup({ props: { parcels: [] } });
    expect(asFragment()).toMatchSnapshot();
  });

  it('matches snapshot with multiple parcels', () => {
    const { asFragment } = setup({
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
    setup();

    const toggle = screen.getByRole('button', { name: /worklist more options/i });
    await act(async () => userEvent.click(toggle));

    const clearAllItem = await screen.findByRole('button', { name: /clear list/i });
    await act(async () => userEvent.click(clearAllItem));

    expect(onClearAll).toHaveBeenCalled();
  });
});
