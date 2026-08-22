import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

import {
  Product,
  PagedResult
} from '../../models/product';

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  private readonly http = inject(HttpClient);
  private readonly apiUrl =
  `${environment.apiUrl}/api/products`;

  getProducts(
  search: string = '',
  type: string = '',
  franchiseId?: number,
  pageNumber: number = 1,
  pageSize: number = 10
): Observable<PagedResult<Product>> {

  let params = new HttpParams()
    .set(
      'pageNumber',
      pageNumber.toString()
    )
    .set(
      'pageSize',
      pageSize.toString()
    );

  if (search.trim()) {

    params = params.set(
      'search',
      search.trim()
    );
  }

  if (type) {

    params = params.set(
      'type',
      type
    );
  }

  if (franchiseId) {

    params = params.set(
      'franchiseId',
      franchiseId.toString()
    );
  }

  return this.http.get<PagedResult<Product>>(
    this.apiUrl,
    { params }
  );
}

  getProduct(id: number): Observable<Product> {

    return this.http.get<Product>(
      `${this.apiUrl}/${id}`
    );
  }
  getImageUrl(imageUrl?: string): string {

  if (!imageUrl) {
    return '';
  }

  return `${environment.apiUrl}${imageUrl}`;
}
}