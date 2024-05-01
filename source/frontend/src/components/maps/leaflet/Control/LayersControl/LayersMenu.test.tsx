import noop from 'lodash/noop';

import { TenantProvider } from '@/tenants';
import { cleanup, render } from '@/utils/test-utils';
import { createMapContainer, deferred } from '@/utils/test-utils';

import { LayersMenu } from './LayersMenu';
import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { mapMachineBaseMock } from '@/mocks/mapFSM.mock';
import { vi } from 'vitest';

vi.mock('@/components/common/mapFSM/MapStateMachineContext');

describe('LayersMenu View', () => {
  afterEach(cleanup);

  beforeEach(() => {
    vi.mocked(useMapStateMachine).mockImplementation(() => mapMachineBaseMock);
  });

  const setup = (setMap = noop) => {
    // render component under test
    const { promise, resolve } = deferred();
    const Wrapper = createMapContainer(resolve, setMap);
    const result = render(
      <Wrapper>
        <TenantProvider>
          <LayersMenu />
        </TenantProvider>
      </Wrapper>,
    );
    return {
      ...result,
      ready: promise,
      findLayerTreeItem: (id: string) => document.querySelector(`#${id}`) as HTMLElement,
    };
  };

  it('renders as expected', () => {
    const result = setup();
    expect(result.asFragment()).toMatchSnapshot();
  });
});
