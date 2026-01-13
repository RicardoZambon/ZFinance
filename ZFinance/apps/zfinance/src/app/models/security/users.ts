interface IUsersBase {
  email?: string;
  name?: string;
}

export interface IUsersDisplay extends IUsersUpdate {
  isActive: boolean;
}

export interface IUsersInsert extends IUsersBase {
}

export interface IUsersList {
  email?: string;
  id: number;
  isActive: boolean;
  name?: string;
}

export interface IUsersRolesList {
  id: number;
  name?: string;
}

export interface IUsersUpdate extends IUsersBase {
  id: number;
}