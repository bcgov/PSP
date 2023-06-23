import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { Formik } from 'formik';
import { noop } from 'lodash';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';

import { mockLookups } from '@/mocks/lookups.mock';
import { getUserMock } from '@/mocks/user.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import {
  act,
  render,
  RenderOptions,
  userEvent,
  waitFor,
  waitForElementToBeRemoved,
} from '@/utils/test-utils';

import { FormUser } from '../users/models';
import EditUserContainer, { IEditUserContainerProps } from './EditUserContainer';

const mockAxios = new MockAdapter(axios);

const mockStore = configureMockStore([thunk]);

const store = mockStore({ [lookupCodesSlice.name]: { lookupCodes: mockLookups } });

describe('EditUserContainer component', () => {
  const setup = (renderOptions: RenderOptions & Partial<IEditUserContainerProps>) => {
    // render component under test
    const component = render(
      <Formik initialValues={{ properties: [] }} onSubmit={noop}>
        <EditUserContainer userId={renderOptions.userId} />
      </Formik>,
      {
        ...renderOptions,
        store: {
          [lookupCodesSlice.name]: { lookupCodes: mockLookups },
        },
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
  it('makes a request based on the user id', async () => {
    mockAxios.onGet().reply(200, getUserMock());
    const {
      component: { getByTestId, asFragment },
    } = setup({ userId: '1' });

    await waitForElementToBeRemoved(getByTestId('filter-backdrop-loading'));

    await waitFor(() => {
      expect(mockAxios.history.get[0].url).toBe('/admin/users/1');
    });
    expect(asFragment()).toMatchSnapshot();
  });

  it('makes a request when the form is saved', async () => {
    mockAxios.onGet().reply(200, getUserMock());
    mockAxios.onPut().reply(200);
    const {
      component: { getByTestId, getByText, container },
    } = setup({ userId: '1' });

    await waitForElementToBeRemoved(getByTestId('filter-backdrop-loading'));

    const textarea = container.querySelector(`textarea[name="note"]`) as HTMLElement;
    await act(async () => userEvent.type(textarea, 'test note'));

    const saveButton = getByText('Save');
    await act(async () => userEvent.click(saveButton));

    expect(mockAxios.history.put[0].url).toBe(
      '/keycloak/users/e81274eb-a007-4f2e-ada3-2817efcdb0a6',
    );
    expect({
      ...JSON.parse(mockAxios.history.put[0].data),
      approvedById: undefined,
      issueDate: undefined,
    }).toEqual({
      ...new FormUser(getUserMock()).toApi(),
      note: 'test note',
    });
  });
});
