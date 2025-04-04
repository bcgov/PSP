import { IUserInfo } from '@/hooks/useKeycloakWrapper';
import { exists } from '@/utils';

export interface TraceUser {
  displayName: string;
  idir: string;
}

export interface UserManager {
  getUser: () => TraceUser | null;
  setUser: (user: TraceUser) => void;
  clearUser: () => void;
}

export class UserAPI {
  private static _instance?: UserAPI;
  private readonly _userManager;

  private constructor(userManager: UserManager) {
    this._userManager = userManager;
  }

  public static getInstance(): UserAPI {
    if (!this._instance) {
      this._instance = new UserAPI(new TelemetryUserManager());
    }

    return this._instance;
  }

  public getUserManager(): UserManager {
    return this._userManager;
  }
}

export class TelemetryUserManager implements UserManager {
  private _activeUser: TraceUser | null = null;

  public clearUser() {
    this._activeUser = null;
  }

  public getUser(): TraceUser | null {
    return this._activeUser;
  }

  public setUser(user: TraceUser) {
    this._activeUser = user;
  }
}

export function getUserDetailsFromKeycloak(userInfo: IUserInfo): TraceUser {
  const displayName = userInfo?.name ?? userInfo?.preferred_businessIdentifier;
  const fullName =
    exists(userInfo.firstName) && exists(userInfo.surname)
      ? `${userInfo.firstName} ${userInfo.surname}`
      : 'default';

  return {
    displayName: displayName ?? fullName,
    idir: userInfo.businessIdentifierValue,
  };
}
