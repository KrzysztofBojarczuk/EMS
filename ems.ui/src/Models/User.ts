export type UserProfileToken = {
  userName: string;
  email: string;
  token: string;
  roles: string[];
};

export type UserProfile = {
  userName: string;
  email: string;
  roles: string[];
};

export interface UserGet {
  id: string;
  userName: string;
  normalizedUserName: string;
  email: string;
  normalizedEmail: string;
  emailConfirmed: boolean;
  passwordHash: string;
  securityStamp: string;
  concurrencyStamp: string;
  phoneNumber: string;
  phoneNumberConfirmed: boolean;
  twoFactorEnabled: boolean;
  lockoutEnd: Date | null;
  lockoutEnabled: boolean;
  accessFailedCount: number;
  active: boolean;
  createdAt: string;
}

export interface PaginatedUserResponse {
  userGet: UserGet[];
  totalItems: number;
  totalPages: number;
  pageIndex: number;
}
