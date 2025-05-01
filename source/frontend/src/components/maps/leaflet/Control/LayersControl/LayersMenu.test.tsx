import { mapMachineBaseMock } from '@/mocks/mapFSM.mock';
import { act, cleanup, render, RenderOptions, screen, userEvent } from '@/utils/test-utils';

import { LayersMenu } from './LayersMenu';
import { layersTree } from './DefaultLayers';

describe('LayersMenu View', () => {
  afterEach(cleanup);

  // render component under test
  const setup = async (renderOptions: RenderOptions = {}) => {
    const utils = render(<LayersMenu />, { ...renderOptions });

    // wait for useEffects
    await act(async () => {});

    return {
      ...utils,
    };
  };

  it('renders as expected', async () => {
    const { asFragment } = await setup();
    expect(asFragment()).toMatchSnapshot();
  });

  it('renders the list of active layers', async () => {
    await setup();

    expect(await screen.findByText(/Administrative Boundaries/i)).toBeVisible();
    expect(await screen.findByText(/Zoning/i)).toBeVisible();
    expect(await screen.findByText(/Pims/i)).toBeVisible();
  });

  it(`doesn't call 'setMapLayers' when the layer list is hidden`, async () => {
    await setup({
      mockMapMachine: {
        ...mapMachineBaseMock,
        activeLayers: layersTree,
        isShowingMapLayers: false,
      },
    });

    expect(mapMachineBaseMock.setMapLayers).not.toHaveBeenCalled();
  });

  it(`calls 'setMapLayers' when the layer list is shown`, async () => {
    await setup({
      mockMapMachine: {
        ...mapMachineBaseMock,
        activeLayers: layersTree,
        isShowingMapLayers: true,
      },
    });

    expect(mapMachineBaseMock.setMapLayers).toHaveBeenCalled();
  });

  it('triggers a change in the map state machine when layer checkbox is changed', async () => {
    await setup({
      mockMapMachine: {
        ...mapMachineBaseMock,
        activeLayers: layersTree,
        isShowingMapLayers: true,
      },
    });

    const allCheckboxes = await screen.findAllByRole('checkbox');
    expect(allCheckboxes.length).toBeGreaterThan(0);

    await act(async () => userEvent.click(allCheckboxes[0]));

    expect(mapMachineBaseMock.setMapLayers).toHaveBeenCalledWith(layersTree);
  });
});
