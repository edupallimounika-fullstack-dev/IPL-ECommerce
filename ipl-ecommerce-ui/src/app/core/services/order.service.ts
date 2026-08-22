import { Injectable, inject } from '@angular/core';

import {
  HttpClient
} from '@angular/common/http';

import {
  Observable
} from 'rxjs';

import {
  CheckoutRequest,
  Order,
  OrderSummary
} from '../../models/order';


@Injectable({
  providedIn: 'root'
})
export class OrderService {

  private readonly http =
    inject(HttpClient);

  private readonly apiUrl =
    'http://localhost:5221/api/orders';


  checkout(
    request: CheckoutRequest
  ): Observable<Order> {

    return this.http.post<Order>(
      `${this.apiUrl}/checkout`,
      request
    );
  }


  getOrders(): Observable<OrderSummary[]> {

    return this.http.get<OrderSummary[]>(
      this.apiUrl
    );
  }


  getOrder(
    orderId: number
  ): Observable<Order> {

    return this.http.get<Order>(
      `${this.apiUrl}/${orderId}`
    );
  }
}