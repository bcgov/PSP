import L from 'leaflet';
import { noop } from 'lodash';
import { useState } from 'react';

import { act, cleanup, render, waitFor } from '@/utils/test-utils';
import { createMapContainer, deferred, userEvent } from '@/utils/test-utils';
import LayersControl, { ILayersControl } from './LayersControl';

jest.mock('axios');

const onToggle = jest.fn();
// component under test

function setup(params: ILayersControl) {
  // create a promise to wait for the map to be ready (which happens after initial render)
  const { promise, resolve } = deferred();
  const Wrapper = createMapContainer(resolve, noop);
  const utils = render(
    <Wrapper>
      <LayersControl {...params} />
    </Wrapper>,
  );

  return {
    ...utils,
    ready: promise,
    findToggleButton: () => document.querySelector('#layersControlButton') as HTMLElement,
  };
}

describe('LayersControl View', () => {
  afterEach(cleanup);

  it('renders correctly', () => {
    const { asFragment } = setup({ onToggle: onToggle });
    expect(asFragment()).toMatchSnapshot();
  });

  it('should render the layers control button', async () => {
    const { ready, findToggleButton } = setup({ onToggle: onToggle });
    await waitFor(() => ready);
    const toggleBtn = findToggleButton();
    expect(toggleBtn).toBeInTheDocument();
  });

  it('when clicked should fire toggle param', async () => {
    const { ready, findToggleButton } = setup({ onToggle: onToggle });
    await waitFor(() => ready);
    const toggleBtn = findToggleButton();
    expect(toggleBtn).toBeInTheDocument();
  });
});
