import L from 'leaflet';
import noop from 'lodash/noop';
import { useState } from 'react';

import { act, cleanup, render, waitFor } from '@/utils/test-utils';
import { createMapContainer, deferred, userEvent } from '@/utils/test-utils';

import LayersControl from './LayersControl';

vi.mock('axios');

// component under test
function Template({ openByDefault = false }) {
  const [open, setOpen] = useState(openByDefault);
  const toggle = () => {
    setOpen(!open);
  };
  return <LayersControl onToggle={toggle} />;
}

function setup(ui = <Template />, setMap = noop) {
  // create a promise to wait for the map to be ready (which happens after initial render)
  const { promise, resolve } = deferred();
  const Wrapper = createMapContainer(resolve, setMap);
  const utils = render(<Wrapper>{ui}</Wrapper>);
  return {
    ...utils,
    ready: promise,
    findToggleButton: () => document.querySelector('#layersControlButton') as HTMLElement,
  };
}

describe('LayersControl View', () => {
  afterEach(cleanup);

  it('renders correctly', () => {
    const { asFragment } = setup();
    expect(asFragment()).toMatchSnapshot();
  });

  it('should render the layers control button', async () => {
    const { ready, findToggleButton } = setup();
    await waitFor(() => ready);
    const toggleBtn = findToggleButton();
    expect(toggleBtn).toBeInTheDocument();
  });
});
