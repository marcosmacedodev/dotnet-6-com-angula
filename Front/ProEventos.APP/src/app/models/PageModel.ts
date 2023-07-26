import { Evento } from "./Evento";

export interface PageModel<T> {
  pageNumber: number;
  items: T[];
  pageSize: number;
  totalCount: number;
  totalPages: number;
}
