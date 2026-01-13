

export interface IMenusDisplay extends IMenusUpdate {
  parentMenuIDLabel?: string;
}

export interface IMenusInsert {
  icon?:string;
  label?: string;
  order?: string;
  parentMenuID?: number;
  url?: string;
}

export interface IMenusList {
  childMenusCount: number;
  icon: string;
  id: number;
  label: string;
  order: number;
  url: string;
}

export interface IMenusUpdate extends IMenusInsert {
  id: number;
}