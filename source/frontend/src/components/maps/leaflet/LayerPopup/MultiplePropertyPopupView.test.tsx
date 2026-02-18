import { Feature, Geometry } from 'geojson';

import { getMockFullyAttributedParcel } from '@/mocks/faParcelLayerResponse.mock';
import { getMockLocationFeatureDataset } from '@/mocks/featureset.mock';
import { mapMachineBaseMock } from '@/mocks/mapFSM.mock';
import { PMBC_FullyAttributed_Feature_Properties } from '@/models/layers/parcelMapBC';
import { act, render, RenderOptions, screen, userEvent } from '@/utils/test-utils';

import { IMultiplePropertyPopupView, MultiplePropertyPopupView } from './MultiplePropertyPopupView';

describe('MultiplePropertyPopupView', () => {
  const setup = (
    renderOptions: RenderOptions & { props?: Partial<IMultiplePropertyPopupView> } = {},
  ) => {
    const rendered = render(
      <MultiplePropertyPopupView
        featureDataset={renderOptions.props?.featureDataset ?? null}
        onSelectProperty={renderOptions.props?.onSelectProperty ?? vi.fn()}
        onAddPropertyToWorklist={renderOptions.props?.onAddPropertyToWorklist ?? vi.fn()}
        onAddAllToWorklist={renderOptions.props?.onAddAllToWorklist ?? vi.fn()}
        onClose={renderOptions.props?.onClose ?? vi.fn()}
      />,
      {
        ...renderOptions,
        mockMapMachine: renderOptions?.mockMapMachine ?? mapMachineBaseMock,
      },
    );

    return {
      ...rendered,
    };
  };

  it('renders title when featureDataset is provided', () => {
    const featureDataset = {
      ...getMockLocationFeatureDataset(),
      parcelFeatures: [
        getMockFullyAttributedParcel(null, null, 'VIS547', 'Unclassified'),
        getMockFullyAttributedParcel('000-709-280', null, 'VIS547'),
        getMockFullyAttributedParcel('000-709-239', null, 'VIS547'),
      ],
    };

    setup({ props: { featureDataset } });

    expect(screen.getByText('Multiple properties found')).toBeInTheDocument();
  });

  it('renders close button and calls onClose when clicked', async () => {
    const onClose = vi.fn();
    const featureDataset = {
      ...getMockLocationFeatureDataset(),
      parcelFeatures: [
        getMockFullyAttributedParcel(null, null, 'VIS547', 'Unclassified'),
        getMockFullyAttributedParcel('000-709-280', null, 'VIS547'),
        getMockFullyAttributedParcel('000-709-239', null, 'VIS547'),
      ],
    };

    setup({ props: { featureDataset, onClose } });

    const closeButton = screen.getByTitle('close');
    await act(async () => userEvent.click(closeButton));

    expect(onClose).toHaveBeenCalledTimes(1);
  });

  it('renders property list with PID and PIN', () => {
    const featureDataset = {
      ...getMockLocationFeatureDataset(),
      parcelFeatures: [
        getMockFullyAttributedParcel(null, null, 'VIS547', 'Unclassified'),
        getMockFullyAttributedParcel('000-709-280', null, 'VIS547'),
        getMockFullyAttributedParcel(null, 55599, 'VIS547'),
      ],
    };

    setup({ props: { featureDataset } });

    expect(screen.getByText(/PID:/)).toBeInTheDocument();
    expect(screen.getByText(/PIN:/)).toBeInTheDocument();
  });

  it('calls onSelectProperty when property row is clicked', async () => {
    const onSelectProperty = vi.fn();
    const featureDataset = {
      ...getMockLocationFeatureDataset(),
      parcelFeatures: [
        getMockFullyAttributedParcel(null, null, 'VIS547', 'Unclassified'),
        getMockFullyAttributedParcel('000-709-280', null, 'VIS547'),
        getMockFullyAttributedParcel('000-709-239', null, 'VIS547'),
      ],
    };

    setup({ props: { featureDataset, onSelectProperty } });

    const propertyRow = screen.getByText(/PID: 000-709-280/).closest('div');
    expect(propertyRow).toBeInTheDocument();
    await act(async () => userEvent.click(propertyRow));

    expect(onSelectProperty).toHaveBeenCalledTimes(1);
    expect(onSelectProperty).toHaveBeenCalledWith(
      expect.objectContaining<Partial<Feature<Geometry, PMBC_FullyAttributed_Feature_Properties>>>({
        properties: expect.any(Object),
      }),
    );
  });

  it.each([
    ['Normal Property', getMockFullyAttributedParcel('000-709-280', null, 'VIS547')],
    ['Common Property', getMockFullyAttributedParcel(null, null, 'VIS547', 'Unclassified')],
  ])(
    'calls onAddPropertyToWorklist when add to worklist button is clicked - %s',
    async (_, fullyAttributedParcel) => {
      const onAddPropertyToWorklist = vi.fn();
      const featureDataset = {
        ...getMockLocationFeatureDataset(),
        parcelFeatures: [fullyAttributedParcel],
      };

      setup({ props: { featureDataset, onAddPropertyToWorklist } });

      const addButton = screen.getByTitle('Add to working list');
      await act(async () => userEvent.click(addButton));

      expect(onAddPropertyToWorklist).toHaveBeenCalledTimes(1);
      expect(onAddPropertyToWorklist).toHaveBeenCalledWith(
        expect.objectContaining<
          Partial<Feature<Geometry, PMBC_FullyAttributed_Feature_Properties>>
        >({
          properties: expect.any(Object),
        }),
        featureDataset,
      );
    },
  );

  it('calls onAddAllToWorklist when add all button is clicked', async () => {
    const onAddAllToWorklist = vi.fn();
    const featureDataset = {
      ...getMockLocationFeatureDataset(),
      parcelFeatures: [
        getMockFullyAttributedParcel(null, null, 'VIS547', 'Unclassified'),
        getMockFullyAttributedParcel('000-709-280', null, 'VIS547'),
        getMockFullyAttributedParcel('000-709-239', null, 'VIS547'),
      ],
    };

    setup({ props: { featureDataset, onAddAllToWorklist } });

    const addAllButton = screen.getByText('Add all to working list');
    await act(async () => userEvent.click(addAllButton));

    expect(onAddAllToWorklist).toHaveBeenCalledTimes(1);
    expect(onAddAllToWorklist).toHaveBeenCalledWith(featureDataset);
  });

  it('groups properties by PLAN NUMBER', () => {
    const featureDataset = {
      ...getMockLocationFeatureDataset(),
      parcelFeatures: [
        getMockFullyAttributedParcel(null, null, 'VIS547', 'Unclassified'),
        getMockFullyAttributedParcel('000-709-280', null, 'VIS547'),
        getMockFullyAttributedParcel('000-709-239', null, 'VIS547'),
      ],
    };

    setup({ props: { featureDataset } });

    const pidElements = screen.getAllByText(/PID:/);
    expect(pidElements).toHaveLength(2); // Two properties with PLAN NUMBER 'VIS547'
  });

  it('handles null featureDataset gracefully', () => {
    setup({ props: { featureDataset: null } });
    expect(screen.getByText('Multiple properties found')).toBeInTheDocument();
  });
});
