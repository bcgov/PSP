import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { createMemoryHistory } from 'history';
import { cleanup, render, RenderOptions, waitFor } from 'utils/test-utils';

import MotiInventoryContainer, { IMotiInventoryContainerProps } from './MotiInventoryContainer';

const setShowSideBar = jest.fn();

describe('MotiInventoryContainer component', () => {
  const mockAxios = new MockAdapter(axios);
  const history = createMemoryHistory();
  const setup = (renderOptions: RenderOptions & IMotiInventoryContainerProps) => {
    // render component under test
    const component = render(
      <MotiInventoryContainer
        showSideBar={renderOptions.showSideBar}
        setShowSideBar={renderOptions.setShowSideBar}
        pid={renderOptions.pid}
      />,
      {
        ...renderOptions,
        history,
      },
    );

    return {
      component,
    };
  };

  it('requests ltsa data by pid', async () => {
    mockAxios.onPost().reply(200, {});
    mockAxios.onGet().reply(200, {});
    setup({
      pid: '9212434',
      showSideBar: false,
      setShowSideBar,
    });
    await waitFor(() => {
      expect(mockAxios.history.post).toHaveLength(1);
      expect(mockAxios.history.post[0].url).toBe(`/tools/ltsa/all?pid=009-212-434`);
    });
  });
  afterEach(() => {
    mockAxios.reset();
    jest.restoreAllMocks();
    cleanup();
    history.push('');
  });
});
