export interface Product {

  id: number;

  name: string;

  description: string;

  productType: string;

  price: number;

  stockQuantity: number;

  imageUrl?: string;

  franchiseId: number;

  franchiseName: string;
}

export interface PagedResult<T> {

  items: T[];

  pageNumber: number;

  pageSize: number;

  totalCount: number;

  totalPages: number;

  hasPreviousPage: boolean;

  hasNextPage: boolean;
}