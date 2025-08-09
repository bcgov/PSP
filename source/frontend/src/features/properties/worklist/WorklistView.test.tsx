import { getMockWorklistParcel } from '@/mocks/worklistParcel.mock';
import { act, cleanup, render, RenderOptions, screen, userEvent } from '@/utils/test-utils';

import { IWorklistViewProps, WorklistView } from './WorklistView';

describe('WorklistView', () => {
  const onSelect = vi.fn();
  const onRemove = vi.fn();
  const onClearAll = vi.fn();
  const onZoomToParcel = vi.fn();
  const onCreateAcquisitionFile = vi.fn();
  const onCreateResearchFile = vi.fn();
  const onCreateDispositionFile = vi.fn();
  const onCreateLeaseFile = vi.fn();
  const onCreateManagementFile = vi.fn();

  beforeEach(() => {
    cleanup();
    vi.clearAllMocks();
  });

  const setup = (renderOptions: RenderOptions & { props?: Partial<IWorklistViewProps> } = {}) => {
    return render(
      <WorklistView
        parcels={
          renderOptions.props?.parcels ?? [getMockWorklistParcel('parcel-1', { PID: '123456789' })]
        }
        selectedId={renderOptions.props?.selectedId ?? null}
        onSelect={onSelect}
        onRemove={onRemove}
        onClearAll={onClearAll}
        onZoomToParcel={onZoomToParcel}
        onCreateAcquisitionFile={onCreateAcquisitionFile}
        onCreateResearchFile={onCreateResearchFile}
        onCreateDispositionFile={onCreateDispositionFile}
        onCreateLeaseFile={onCreateLeaseFile}
        onCreateManagementFile={onCreateManagementFile}
      />,
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

  it('selects parcel when item is clicked', async () => {
    setup();
    const item = screen.getByText('PID: 123-456-789');
    await act(async () => userEvent.click(item));

    expect(onSelect).toHaveBeenCalledWith('parcel-1');
  });

  it('calls zoom and remove buttons', async () => {
    const mockParcel = getMockWorklistParcel('parcel-2', { PIN: 99999999 });
    setup({ props: { parcels: [mockParcel] } });

    // Hover to show buttons if needed
    const item = await screen.findByText('PIN: 99999999');
    await act(async () => userEvent.hover(item));

    const zoomButton = screen.getByTitle('Zoom to worklist parcel');
    const deleteButton = screen.getByTitle('Delete worklist parcel');

    await act(async () => userEvent.click(zoomButton));
    expect(onZoomToParcel).toHaveBeenCalledWith(mockParcel);

    await act(async () => userEvent.click(deleteButton));
    expect(onRemove).toHaveBeenCalledWith(mockParcel.id);
  });

  it('calls onClearAll from MoreOptionsDropdown', async () => {
    setup();

    const toggle = screen.getByRole('button', { name: /more options/i });
    await act(async () => userEvent.click(toggle));

    const clearAllItem = await screen.findByRole('button', { name: /clear list/i });
    await act(async () => userEvent.click(clearAllItem));

    expect(onClearAll).toHaveBeenCalled();
  });
});
