import { renderHook } from '@testing-library/react-hooks';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { IPagedItems } from 'interfaces';
import { mockAccessRequest } from 'mocks/filterDataMock';

import { mockUser } from './../../mocks/filterDataMock';
import { useApiUsers } from './useApiUsers';

const mockAxios = new MockAdapter(axios);

describe('useApiUsers api hook', () => {
  beforeEach(() => {
    mockAxios.reset();
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  it('Gets paged users', () => {
    renderHook(async () => {
      mockAxios.onPost(`/admin/users/my/agency`).reply(200, {
        items: [mockAccessRequest],
        pageIndex: 1,
        page: 1,
        quantity: 5,
        total: 10,
      } as IPagedItems);

      const api = useApiUsers();
      const response = await api.getUsersPaged({ page: 1 });

      expect(response.status).toBe(200);
      expect(response.data).toStrictEqual({
        items: [mockAccessRequest],
        pageIndex: 1,
        page: 1,
        quantity: 5,
        total: 10,
      });
    });
  });
  it('Get detailed user', () => {
    renderHook(async () => {
      mockAxios.onGet(`/admin/users/14c9a273-6f4a-4859-8d59-9264d3cee53f`).reply(200, mockUser);

      const api = useApiUsers();
      const response = await api.getUser(mockUser.id!);

      expect(response.status).toBe(200);
      expect(response.data).toStrictEqual(mockUser);
    });
  });

  it('Puts an updated user', () => {
    const newUser = { ...mockUser, sendEmail: true, addressTo: '' };
    renderHook(async () => {
      mockAxios.onPut('/keycloak/users/14c9a273-6f4a-4859-8d59-9264d3cee53f').reply(200, newUser);
      const api = useApiUsers();
      const response = await api.putUser(newUser);

      expect(response.status).toBe(200);
      expect(response.data).toStrictEqual(newUser);
    });
  });

  it('activates a user', () => {
    const newUser = { ...mockUser, sendEmail: true, addressTo: '' };
    renderHook(async () => {
      mockAxios.onPost('/auth/activate').reply(200, newUser);
      const api = useApiUsers();
      const response = await api.activateUser();

      expect(response.status).toBe(200);
      expect(response.data).toStrictEqual(newUser);
    });
  });
});
