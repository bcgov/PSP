import { getMockWorklistParcel } from '@/mocks/worklistParcel.mock';
import { NameSourceType } from '@/utils';
import { act, mockKeycloak, render, RenderOptions, screen, userEvent } from '@/utils/test-utils';

import ParcelItem, { IParcelItemProps } from './ParcelItem';

describe('ParcelItem component', () => {
  const onRemove = vi.fn();

  beforeEach(() => {
    mockKeycloak();
    vi.clearAllMocks();
  });

  const setup = (renderOptions: RenderOptions & { props?: Partial<IParcelItemProps> } = {}) => {
    return render(
      <ParcelItem
        parcel={
          renderOptions.props?.parcel ?? getMockWorklistParcel('parcel-1', { PID: '123456789' })
        }
        onRemove={onRemove}
        canAddToWorklist={renderOptions.props?.canAddToWorklist ?? true}
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

  it('calls onRemove when remove button is clicked', async () => {
    setup();
    const removeButton = screen.getByTestId('delete-list-parcel-parcel-1');
    await act(async () => userEvent.click(removeButton));
    expect(onRemove).toHaveBeenCalledWith('parcel-1');
  });

  /*
  it('calls requestFlyToLocation when parcel has no feature', async () => {
    mockParcels[0].pmbcFeature = null;

    const testMockMachine: IMapStateMachineContext = {
      ...mapMachineBaseMock,
      requestFlyToBounds,
      requestFlyToLocation,
    };

    setup({ mockMapMachine: testMockMachine });
    await act(async () => viewProps.onZoomToParcel(mockParcels[0]));

    expect(requestFlyToLocation).toHaveBeenCalledWith(mockParcels[0].location);
    expect(requestFlyToBounds).not.toHaveBeenCalled();
  });

  it('calls requestFlyToBounds when parcel has valid geometry', async () => {
    const testMockMachine: IMapStateMachineContext = {
      ...mapMachineBaseMock,
      requestFlyToBounds,
      requestFlyToLocation,
    };

    setup({ mockMapMachine: testMockMachine });
    await act(async () => viewProps.onZoomToParcel(mockParcels[0]));

    expect(requestFlyToBounds).toHaveBeenCalledWith(mockBounds);
    expect(requestFlyToLocation).not.toHaveBeenCalled();
  });

  it('falls back to requestFlyToLocation if bounds are invalid', async () => {
    mockBounds.isValid = vi.fn(() => false); // simulate invalid geometry

    const testMockMachine: IMapStateMachineContext = {
      ...mapMachineBaseMock,
      requestFlyToBounds,
      requestFlyToLocation,
    };

    setup({ mockMapMachine: testMockMachine });
    await act(async () => viewProps.onZoomToParcel(mockParcels[0]));

    expect(requestFlyToBounds).not.toHaveBeenCalled();
    expect(requestFlyToLocation).toHaveBeenCalledWith(mockParcels[0].location);
  });
  */
});
