export interface IRolesActionsList {
  code: string;
  description: string;
  entity: string;
  id: number;
  name: string;
}

export interface IRolesInsert {
  name?: string;
}

export interface IRolesList {
  id: number;
  name?: string;
}

export interface IRolesMenusList {
  id: number;
  label?: string;
}

export interface IRolesUpdate extends IRolesInsert {
  id: number;
}

export interface IRolesUsersList {
  id: number;
  email?: string;
  name?: string;
}