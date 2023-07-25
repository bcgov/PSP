import { renderHook } from '@testing-library/react-hooks';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';

import { IPagedItems } from '@/interfaces';
import { mockAccessRequest } from '@/mocks/filterData.mock';

import { mockUser } from '../../mocks/filterData.mock';
import { useApiUsers } from './useApiUsers';

const mockAxios = new MockAdapter(axios);

describe('useApiUsers api hook', () => {
  beforeEach(() => {
    mockAxios.reset();
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  const setup = () => {
    const { result } = renderHook(useApiUsers);
    return result.current;
  };

  it('Gets paged users', async () => {
    mockAxios.onPost(`/admin/users/filter`).reply(200, {
      items: [mockAccessRequest],
      pageIndex: 1,
      page: 1,
      quantity: 5,
      total: 10,
    } as IPagedItems);

    const { getUsersPaged } = setup();
    const response = await getUsersPaged({ page: 1 });

    expect(response.status).toBe(200);
    expect(response.data).toStrictEqual({
      items: [mockAccessRequest],
      pageIndex: 1,
      page: 1,
      quantity: 5,
      total: 10,
    });
  });
  it('Get detailed user', async () => {
    mockAxios.onGet(`/admin/users/14c9a273-6f4a-4859-8d59-9264d3cee53f`).reply(200, mockUser);

    const { getUser } = setup();
    const response = await getUser(mockUser.guidIdentifierValue!);

    expect(response.status).toBe(200);
    expect(response.data).toStrictEqual(mockUser);
  });

  it('Puts an updated user', async () => {
    const newUser = { ...mockUser, sendEmail: true, addressTo: '' };
    mockAxios.onPut('/keycloak/users/14c9a273-6f4a-4859-8d59-9264d3cee53f').reply(200, newUser);
    const { putUser } = setup();
    const response = await putUser(newUser);

    expect(response.status).toBe(200);
    expect(response.data).toStrictEqual(newUser);
  });

  it('activates a user', async () => {
    const newUser = { ...mockUser, sendEmail: true, addressTo: '' };
    mockAxios.onPost('/auth/activate').reply(200, newUser);
    const { activateUser } = setup();
    const response = await activateUser();

    expect(response.status).toBe(200);
    expect(response.data).toStrictEqual(newUser);
  });
});
