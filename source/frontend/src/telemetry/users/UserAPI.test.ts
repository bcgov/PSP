import { TelemetryUserManager, TraceUser, UserAPI } from './UserAPI';

describe('Telemetry', () => {
  describe('UserAPI', () => {
    let userAPI: UserAPI;

    beforeEach(() => {
      userAPI = UserAPI.getInstance();
    });

    it('should return an instance of UserAPI', () => {
      expect(userAPI).toBeInstanceOf(UserAPI);
    });

    it('should return the same instance on multiple calls', () => {
      const userAPIInstance1 = UserAPI.getInstance();
      const userAPIInstance2 = UserAPI.getInstance();
      expect(userAPIInstance1).toEqual(userAPIInstance2);
    });

    it('should return an instance of TelemetryUserManager for getUserManager', () => {
      const userManager = userAPI.getUserManager();
      expect(userManager).toBeInstanceOf(TelemetryUserManager);
    });

    it('should set the current user as expected', () => {
      const mockUser: TraceUser = { displayName: 'mocked user', idir: 'MOCKED', client_roles: [] };
      const userManager = userAPI.getUserManager();
      userManager.setUser(mockUser);
      expect(userManager.getUser()).not.toBeNull();
      expect(userManager.getUser().idir).toBe('MOCKED');
    });

    it('should clear the current user', () => {
      const mockUser: TraceUser = { displayName: 'mocked user', idir: 'MOCKED', client_roles: [] };
      const userManager = userAPI.getUserManager();
      userManager.setUser(mockUser);

      expect(userManager.getUser()).not.toBeNull();
      expect(userManager.getUser().idir).toBe('MOCKED');

      userManager.clearUser();
      expect(userManager.getUser()).toBeNull();
    });
  });
});
