import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import {
  Cart,
  AddCartItemRequest,
  UpdateCartItemRequest
} from '../../models/cart';

@Injectable({
  providedIn: 'root'
})
export class CartService {

  private readonly http =
    inject(HttpClient);

  private readonly apiUrl =
    'http://localhost:5221/api/cart';


  // -----------------------------------
  // Get current user's cart
  // -----------------------------------

  getCart(): Observable<Cart> {

    return this.http.get<Cart>(
      this.apiUrl
    );
  }


  // -----------------------------------
  // Add product to cart
  // -----------------------------------

  addItem(
    request: AddCartItemRequest
  ): Observable<Cart> {

    return this.http.post<Cart>(
      `${this.apiUrl}/items`,
      request
    );
  }


  // -----------------------------------
  // Update cart item
  // -----------------------------------

  updateItem(
    productId: number,
    request: UpdateCartItemRequest
  ): Observable<Cart> {

    return this.http.put<Cart>(
      `${this.apiUrl}/items/${productId}`,
      request
    );
  }


  // -----------------------------------
  // Remove cart item
  // -----------------------------------

  removeItem(
    productId: number
  ): Observable<Cart> {

    return this.http.delete<Cart>(
      `${this.apiUrl}/items/${productId}`
    );
  }


  // -----------------------------------
  // Clear cart
  // -----------------------------------

  clearCart(): Observable<void> {

    return this.http.delete<void>(
      this.apiUrl
    );
  }
}