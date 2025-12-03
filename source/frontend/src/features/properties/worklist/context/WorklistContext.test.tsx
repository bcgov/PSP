import { renderHook } from '@testing-library/react-hooks';

import { act } from '@/utils/test-utils';

import { getMockWorklistParcel } from '@/mocks/worklistParcel.mock';
import { IWorklistNotifier, useWorklistContext, WorklistContextProvider } from './WorklistContext';
import { ParcelDataset } from '../../parcelList/models';

//  Mock notification service
const mockNotifier: IWorklistNotifier = {
  error: vi.fn(),
  success: vi.fn(),
  warn: vi.fn(),
};

describe('WorklistContextProvider', () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  const renderWorklistHook = (initial: ParcelDataset[] = []) =>
    renderHook(() => useWorklistContext(), {
      wrapper: ({ children }) => (
        <WorklistContextProvider parcels={initial} notifier={mockNotifier}>
          {children}
        </WorklistContextProvider>
      ),
    });

  it('throws if useWorklistContext is used without provider', () => {
    expect.hasAssertions();
    // Suppress console.error to avoid noisy test output
    const originalError = console.error;
    console.error = vi.fn();

    const { result } = renderHook(() => useWorklistContext());

    expect(result.error).toBeDefined();
    expect(result.error?.message).toBe('useWorklistContext must be used within WorklistProvider');

    console.error = originalError; // restore original
  });

  it('starts with empty state', () => {
    const { result } = renderWorklistHook();
    expect(result.current.parcels).toEqual([]);
    expect(result.current.selectedId).toBeNull();
  });

  it('respects initial parcels', () => {
    const a = getMockWorklistParcel('a');
    const b = getMockWorklistParcel('b');
    const { result } = renderWorklistHook([a, b]);
    expect(result.current.parcels.map(p => p.id)).toEqual(['a', 'b']);
  });

  it('select() sets selectedId', () => {
    const { result } = renderWorklistHook();
    act(() => result.current.select('123'));
    expect(result.current.selectedId).toBe('123');
  });

  it('add() adds a unique parcel', () => {
    const parcel = getMockWorklistParcel('1', { PID: '123456789' });
    const { result } = renderWorklistHook();
    act(() => result.current.add(parcel));

    expect(result.current.parcels).toHaveLength(1);
    expect(result.current.parcels[0].id).toBe('1');
    expect(mockNotifier.error).not.toHaveBeenCalled();
  });

  it('add() prevents duplicate by internal ID and calls notifier.error', () => {
    const parcel = getMockWorklistParcel('1');
    const duplicate = getMockWorklistParcel('1');

    const { result } = renderWorklistHook();

    act(() => result.current.add(parcel));
    act(() => result.current.add(duplicate));

    expect(result.current.parcels).toHaveLength(1);
    expect(mockNotifier.error).toHaveBeenCalledWith(
      'Duplicate parcel detected. Add to worklist skipped.',
    );
  });

  it('add() prevents duplicate by PID and calls notifier.error', () => {
    const parcel = getMockWorklistParcel('1', { PID: '123456789' });
    const duplicate = getMockWorklistParcel('2', { PID: '123456789' });

    const { result } = renderWorklistHook();

    act(() => result.current.add(parcel));
    act(() => result.current.add(duplicate));

    expect(result.current.parcels).toHaveLength(1);
    expect(mockNotifier.error).toHaveBeenCalledWith(
      'Duplicate parcel detected. Add to worklist skipped.',
    );
  });

  it('add() prevents duplicate by PIN and calls notifier.error', () => {
    const parcel = getMockWorklistParcel('p1', { PIN: 999999999 });
    const duplicate = getMockWorklistParcel('p2', { PIN: 999999999 });

    const { result } = renderWorklistHook();

    act(() => result.current.add(parcel));
    act(() => result.current.add(duplicate));

    expect(result.current.parcels).toHaveLength(1);
    expect(mockNotifier.error).toHaveBeenCalledWith(
      'Duplicate parcel detected. Add to worklist skipped.',
    );
  });

  it('add() prevents duplicate by PLAN_NUMBER (fallback) and calls notifier.error', () => {
    const parcel = getMockWorklistParcel('plan1', { PLAN_NUMBER: 'SP-12345' });
    const duplicate = getMockWorklistParcel('plan2', { PLAN_NUMBER: 'SP-12345' });

    const { result } = renderWorklistHook();

    act(() => result.current.add(parcel));
    act(() => result.current.add(duplicate));

    expect(result.current.parcels).toHaveLength(1);
    expect(mockNotifier.error).toHaveBeenCalledWith(
      'Duplicate parcel detected. Add to worklist skipped.',
    );
  });

  it('add() prevents duplicate by lat/lng (no PID/PIN)', () => {
    const parcel = getMockWorklistParcel('loc1', {}, { lat: 49.1, lng: -123.1 });
    const duplicate = getMockWorklistParcel('loc2', {}, { lat: 49.1, lng: -123.1 });

    const { result } = renderWorklistHook();

    act(() => result.current.add(parcel));
    act(() => result.current.add(duplicate));

    expect(result.current.parcels).toHaveLength(1);
    expect(mockNotifier.error).toHaveBeenCalledWith(
      'Duplicate parcel detected. Add to worklist skipped.',
    );
  });

  it('addRange() adds only unique parcels and warns on duplicates', () => {
    const existing = getMockWorklistParcel('base', { PID: '111' });
    const p1 = getMockWorklistParcel('a', { PID: '222' });
    const p2 = getMockWorklistParcel('b', { PID: '333' });
    const dupe = getMockWorklistParcel('dupe', { PID: '111' });

    const { result } = renderWorklistHook([existing]);

    act(() => result.current.addRange([p1, dupe, p2]));

    expect(result.current.parcels.map(p => p.pmbcFeature?.properties?.PID)).toEqual([
      '111',
      '222',
      '333',
    ]);
    expect(mockNotifier.success).toHaveBeenCalledWith('Added 2 new parcel(s).');
    expect(mockNotifier.warn).toHaveBeenCalledWith('1 duplicate parcel(s) were skipped.');
  });

  it('addRange() notifies only success when no duplicates', () => {
    const p1 = getMockWorklistParcel('a', { PID: '123' });
    const p2 = getMockWorklistParcel('b', { PID: '456' });

    const { result } = renderWorklistHook();

    act(() => result.current.addRange([p1, p2]));

    expect(result.current.parcels).toHaveLength(2);
    expect(mockNotifier.success).toHaveBeenCalledWith('Added 2 new parcel(s).');
    expect(mockNotifier.warn).not.toHaveBeenCalled();
  });

  it('addRange() notifies only warn when all are duplicates', () => {
    const existing = getMockWorklistParcel('a', { PID: '123' });
    const dupes = [
      getMockWorklistParcel('b', { PID: '123' }),
      getMockWorklistParcel('c', { PID: '123' }),
    ];

    const { result } = renderWorklistHook([existing]);

    act(() => result.current.addRange(dupes));

    expect(result.current.parcels).toHaveLength(1);
    expect(mockNotifier.success).not.toHaveBeenCalled();
    expect(mockNotifier.warn).toHaveBeenCalledWith('2 duplicate parcel(s) were skipped.');
  });

  it('remove() deletes parcel by ID', () => {
    const a = getMockWorklistParcel('a', { PID: '111' });
    const b = getMockWorklistParcel('b', { PID: '222' });
    const { result } = renderWorklistHook([a, b]);

    act(() => result.current.remove('a'));
    expect(result.current.parcels.map(p => p.id)).toEqual(['b']);
  });

  it('clearAll() removes all parcels', () => {
    const a = getMockWorklistParcel('a');
    const b = getMockWorklistParcel('b');
    const { result } = renderWorklistHook([a, b]);

    act(() => result.current.clearAll());
    expect(result.current.parcels).toEqual([]);
  });
});
