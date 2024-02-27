import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';

import { getMockAccessRequest } from '@/mocks/accessRequest.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import {
  act,
  render,
  RenderOptions,
  userEvent,
  waitFor,
  waitForElementToBeRemoved,
} from '@/utils/test-utils';

import AccessRequestContainer, { IAccessRequestContainerProps } from './AccessRequestContainer';
import { FormAccessRequest } from './models';

const mockAxios = new MockAdapter(axios);

const mockStore = configureMockStore([thunk]);

const store = mockStore({ [lookupCodesSlice.name]: { lookupCodes: mockLookups } });
const onSave = jest.fn();
jest.mock('@react-keycloak/web');

describe('AccessRequestContainer component', () => {
  const setup = (renderOptions: RenderOptions & Partial<IAccessRequestContainerProps>) => {
    // render component under test
    const component = render(
      <AccessRequestContainer accessRequestId={renderOptions.accessRequestId} onSave={onSave} />,
      {
        ...renderOptions,
        store: store,
        useMockAuthentication: true,
      },
    );

    return {
      store,
      component,
    };
  };

  afterEach(() => {
    jest.resetAllMocks();
    mockAxios.resetHistory();
  });

  it('makes a request when no id is provided', async () => {
    mockAxios.onGet().reply(200, getMockAccessRequest());
    const {
      component: { getByTestId, asFragment },
    } = setup({});

    await waitForElementToBeRemoved(getByTestId('filter-backdrop-loading'));

    await waitFor(() => {
      expect(mockAxios.history.get[0].url).toBe('/access/requests');
    });
    expect(asFragment()).toMatchSnapshot();
  });
  it('makes a request based on the access request id', async () => {
    mockAxios.onGet().reply(200, getMockAccessRequest());
    const {
      component: { getByTestId, asFragment },
    } = setup({ accessRequestId: 1 });

    await waitForElementToBeRemoved(getByTestId('filter-backdrop-loading'));

    await waitFor(() => {
      expect(mockAxios.history.get[0].url).toBe('/admin/access/requests/1');
    });
    expect(asFragment()).toMatchSnapshot();
  });

  it('makes a request when the form is saved', async () => {
    mockAxios.onGet().reply(200, getMockAccessRequest());
    mockAxios.onPut().reply(200);
    const {
      component: { getByTestId, getByText, container },
    } = setup({ accessRequestId: 1 });

    await waitForElementToBeRemoved(getByTestId('filter-backdrop-loading'));

    const textarea = container.querySelector(`textarea[name="note"]`) as HTMLElement;
    await act(async () => userEvent.type(textarea, 'test note'));

    const saveButton = getByText('Update');
    await act(async () => userEvent.click(saveButton));

    expect(mockAxios.history.put[0].url).toBe('/access/requests/8');
    expect({
      ...JSON.parse(mockAxios.history.put[0].data),
      accessRequestStatusTypeCode: null,
      userId: 30,
    }).toEqual({
      ...new FormAccessRequest(getMockAccessRequest()).toApi(),
      note: 'test note',
    });
  });
});
