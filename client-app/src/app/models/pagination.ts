export interface Pagination {
  currentPage: number;
  itemsPerPage: number;
  totalItems: number;
  totalPages: number;
}

export class PaginatedResult<T> {
  data: T;
  pagination: Pagination;

  constructor(data: T, pagination: Pagination) {
    this.data = data;
    this.pagination = pagination;
  }
}

export class PagingParams {
  constructor(pageNumber: number, diaryName: string) {
    this.#pageSize = this.#DiariesPageSize.get(diaryName);
    this.#pageNumber = pageNumber;
  }

  public get pageNumber() {
    return this.#pageNumber;
  }
  public setPreviousPage() {
    if (this.#pageNumber != 1) this.#pageNumber -= 1;
  }

  public get pageSize() {
    return this.#pageSize;
  }

  #pageNumber: number;
  #pageSize: number;

  readonly #DiariesPageSize: Map<string, number> = new Map([
    ["diary1", 1],
    ["diary5", 10],
  ]);
}
