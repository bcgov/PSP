import { useKeycloak } from '@react-keycloak/web';

export const mockKeycloak = (claims: string[], organizations: number[]) => {
  (useKeycloak as jest.Mock).mockReturnValue({
    keycloak: {
      userInfo: {
        organizations: organizations,
        roles: claims,
      },
      subject: 'test',
    },
  });
};
