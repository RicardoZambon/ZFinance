export interface IActionsInsert {
  code?: string;
  description?: string;
  entity?: string;
  name?: string;
}

export interface IActionsList {
  code: string;
  description: string;
  entity: string;
  id: number;
  name: string;
}

export interface IActionsUpdate extends IActionsInsert {
  id: number;
}