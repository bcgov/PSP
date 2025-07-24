import { getMockWorklistParcel } from '@/mocks/worklistParcel.mock';
import { NameSourceType } from '@/utils';
import { act, render, RenderOptions, screen, userEvent } from '@/utils/test-utils';

import ParcelItem, { IParcelItemProps } from './ParcelItem';

describe('ParcelItem component', () => {
  const onSelect = vi.fn();
  const onRemove = vi.fn();
  const onZoomToParcel = vi.fn();

  beforeEach(() => {
    vi.clearAllMocks();
  });

  const setup = (renderOptions: RenderOptions & { props?: Partial<IParcelItemProps> } = {}) => {
    return render(
      <ParcelItem
        parcel={
          renderOptions.props?.parcel ?? getMockWorklistParcel('parcel-1', { PID: '123456789' })
        }
        isSelected={renderOptions.props?.isSelected ?? false}
        onSelect={onSelect}
        onRemove={onRemove}
        onZoomToParcel={onZoomToParcel}
      />,
      { ...renderOptions },
    );
  };

  it('renders parcel identifier text', () => {
    setup();
    expect(screen.getByText('PID: 123-456-789')).toBeInTheDocument();
  });

  it.each([
    [
      NameSourceType.PID,
      '123-456-789',
      getMockWorklistParcel('parcel-1', { PID: '123456789' }),
      'PID: 123-456-789',
    ],
    [
      NameSourceType.PIN,
      '99999999',
      getMockWorklistParcel('parcel-1', { PIN: 99999999 }),
      'PIN: 99999999',
    ],
    [
      NameSourceType.PLAN,
      'SP-54321',
      getMockWorklistParcel('parcel-1', { PLAN_NUMBER: 'SP-54321' }),
      'Plan #: SP-54321',
    ],
    [
      NameSourceType.LOCATION,
      '-123.100000, 49.250000',
      getMockWorklistParcel('parcel-1', {}, { lat: 49.25, lng: -123.1 }),
      '-123.100000, 49.250000',
    ], // no prefix
  ])('renders %s as "%s"', (_, __, mockParcel, expected) => {
    setup({ props: { parcel: mockParcel } });
    expect(screen.getByText(expected)).toBeInTheDocument();
  });

  it('calls onSelect when row is clicked', async () => {
    setup();
    await act(async () => userEvent.click(screen.getByText('PID: 123-456-789')));
    expect(onSelect).toHaveBeenCalledWith('parcel-1');
  });

  it('calls onZoomToParcel when zoom button is clicked', async () => {
    const mockParcel = getMockWorklistParcel('parcel-1', { PIN: 99999999 });
    setup({ props: { parcel: mockParcel } });
    const zoomButton = screen.getByTitle('Zoom to worklist parcel');
    await act(async () => userEvent.click(zoomButton));
    expect(onZoomToParcel).toHaveBeenCalledWith(mockParcel);
  });

  it('calls onRemove when remove button is clicked', async () => {
    setup();
    const removeButton = screen.getByTestId('delete-worklist-parcel-parcel-1');
    await act(async () => userEvent.click(removeButton));
    expect(onRemove).toHaveBeenCalledWith('parcel-1');
  });
});
