import { Formik } from 'formik';
import { createMemoryHistory } from 'history';
import noop from 'lodash/noop';

import { PropertyForm } from '@/features/mapSideBar/shared/models';
import { getMockSelectedFeatureDataset } from '@/mocks/featureset.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { mapMachineBaseMock } from '@/mocks/mapFSM.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { exists } from '@/utils';
import { act, render, RenderOptions, screen, userEvent } from '@/utils/test-utils';

import SelectedPropertyRow, { ISelectedPropertyRowProps } from './SelectedPropertyRow';

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

const onRemove = vi.fn();

describe('SelectedPropertyRow component', () => {
  const setup = async (
    renderOptions: RenderOptions & { props?: Partial<ISelectedPropertyRowProps> } = {},
  ) => {
    // render component under test
    const utils = render(
      <Formik
        onSubmit={noop}
        initialValues={{
          properties: [
            exists(renderOptions.props?.property)
              ? PropertyForm.fromFeatureDataset(renderOptions.props?.property)
              : new PropertyForm(),
          ],
        }}
      >
        {() => (
          <SelectedPropertyRow
            property={renderOptions.props?.property ?? new PropertyForm().toFeatureDataset()}
            index={renderOptions.props?.index ?? 0}
            onRemove={onRemove}
            showDisable={renderOptions.props?.showDisable ?? false}
            nameSpace="properties.0"
          />
        )}
      </Formik>,
      {
        ...renderOptions,
        store: storeState,
        history,
        mockMapMachine: renderOptions.mockMapMachine ?? mapMachineBaseMock,
      },
    );

    await act(async () => {});

    return { ...utils };
  };

  it('renders as expected', async () => {
    const { asFragment } = await setup();
    expect(asFragment()).toMatchSnapshot();
  });

  it('fires onRemove when remove button is clicked', async () => {
    await setup();
    const removeButton = screen.getByTitle('remove');
    await act(async () => userEvent.click(removeButton));
    expect(onRemove).toHaveBeenCalled();
  });

  it('calls map machine when reposition button is clicked', async () => {
    await setup();
    const moveButton = screen.getByTitle('move-pin-location');
    await act(async () => userEvent.click(moveButton));
    expect(mapMachineBaseMock.startReposition).toHaveBeenCalled();
  });

  it('calls map machine when Zoom button is clicked', async () => {
    await setup({ props: { property: getMockSelectedFeatureDataset() } });
    const zoomButton = screen.getByTestId('zoom-to-property-0');
    await act(async () => userEvent.click(zoomButton));
    expect(mapMachineBaseMock.requestFlyToBounds).toHaveBeenCalled();
  });

  it('displays pid', async () => {
    const mockFeatureSet = getMockSelectedFeatureDataset();
    mockFeatureSet.parcelFeature = {} as any;
    mockFeatureSet.pimsFeature = {
      ...mockFeatureSet.pimsFeature,
      properties: {
        ...mockFeatureSet.pimsFeature?.properties,
        PID_PADDED: '111-111-111',
      },
    };
    await setup({ props: { property: mockFeatureSet } });
    expect(screen.getByText('PID: 111-111-111')).toBeVisible();
  });

  it('falls back to pin', async () => {
    const mockFeatureSet = getMockSelectedFeatureDataset();
    mockFeatureSet.parcelFeature = {} as any;
    mockFeatureSet.pimsFeature = {
      ...mockFeatureSet.pimsFeature,
      properties: {
        ...mockFeatureSet.pimsFeature?.properties,
        PID_PADDED: undefined,
        PIN: 1234,
      },
    };
    await setup({ props: { property: mockFeatureSet } });
    expect(screen.getByText('PIN: 1234')).toBeVisible();
  });

  it('falls back to plan number', async () => {
    const mockFeatureSet = getMockSelectedFeatureDataset();
    mockFeatureSet.parcelFeature = {} as any;
    mockFeatureSet.pimsFeature = {
      ...mockFeatureSet.pimsFeature,
      properties: {
        ...mockFeatureSet.pimsFeature?.properties,
        SURVEY_PLAN_NUMBER: 'VIP123',
        PID_PADDED: undefined,
        PIN: undefined,
      },
    };
    await setup({ props: { property: mockFeatureSet } });
    expect(screen.getByText('Plan #: VIP123')).toBeVisible();
  });

  it('falls back to lat/lng', async () => {
    const mockFeatureSet = getMockSelectedFeatureDataset();
    mockFeatureSet.pimsFeature = {} as any;
    mockFeatureSet.parcelFeature = {} as any;
    mockFeatureSet.location = { lat: 4, lng: 5 };

    await setup({ props: { property: mockFeatureSet } });
    expect(screen.getByText('5.000000, 4.000000')).toBeVisible();
  });

  it('falls back to address', async () => {
    const mockFeatureSet = getMockSelectedFeatureDataset();
    mockFeatureSet.location = undefined;
    mockFeatureSet.parcelFeature = {} as any;
    mockFeatureSet.pimsFeature = {
      ...mockFeatureSet.pimsFeature,
      properties: {
        ...mockFeatureSet.pimsFeature?.properties,
        PID_PADDED: undefined,
        PIN: undefined,
        SURVEY_PLAN_NUMBER: undefined,
        STREET_ADDRESS_1: 'a test address',
      },
    };
    await setup({ props: { property: mockFeatureSet } });
    expect(screen.getByText('Address: a test address')).toBeVisible();
  });

  it('shows Inactive as selected when isActive is false', async () => {
    const mockFeatureSet = getMockSelectedFeatureDataset();
    mockFeatureSet.pimsFeature = {} as any;
    mockFeatureSet.parcelFeature = {} as any;
    mockFeatureSet.isActive = false;

    await setup({ props: { property: mockFeatureSet, showDisable: true } });

    expect(screen.getByDisplayValue('Inactive')).toBeInTheDocument();
  });

  it('shows Active as selected when isActive is true', async () => {
    const mockFeatureSet = getMockSelectedFeatureDataset();
    mockFeatureSet.pimsFeature = {} as any;
    mockFeatureSet.parcelFeature = {} as any;
    mockFeatureSet.isActive = true;

    await setup({ props: { property: mockFeatureSet, showDisable: true } });

    expect(screen.getByDisplayValue('Active')).toBeInTheDocument();
  });
});
