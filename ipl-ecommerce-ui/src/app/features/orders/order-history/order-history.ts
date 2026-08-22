import {
  Component,
  OnInit,
  inject,
  signal
} from '@angular/core';

import { CommonModule } from '@angular/common';

import {
  RouterLink
} from '@angular/router';

import {
  OrderSummary
} from '../../../models/order';

import {
  OrderService
} from '../../../core/services/order.service';


@Component({
  selector: 'app-order-history',

  standalone: true,

  imports: [
    CommonModule,
    RouterLink
  ],

  templateUrl: './order-history.html'
})
export class OrderHistoryComponent
  implements OnInit {


  // =====================================
  // Service
  // =====================================

  private readonly orderService =
    inject(OrderService);


  // =====================================
  // State
  // =====================================

  orders =
    signal<OrderSummary[]>([]);


  isLoading =
    signal(false);


  errorMessage =
    signal('');


  // =====================================
  // Initialization
  // =====================================

  ngOnInit(): void {

    console.log(
      'OrderHistoryComponent initialized'
    );

    this.loadOrders();
  }


  // =====================================
  // Load Orders
  // =====================================

  loadOrders(): void {

    console.log(
      'GET /api/orders'
    );


    this.isLoading.set(true);

    this.errorMessage.set('');


    this.orderService
      .getOrders()
      .subscribe({

        next: orders => {

          console.log(
            'ORDERS RESPONSE:',
            orders
          );


          this.orders.set(
            orders
          );


          this.isLoading.set(false);
        },


        error: error => {

          console.error(
            'ORDERS ERROR:',
            error
          );


          this.isLoading.set(false);


          if (error.status === 401) {

            this.errorMessage.set(
              'Please login to view your orders.'
            );

            return;
          }


          this.errorMessage.set(
            error?.error?.detail ??
            error?.error?.message ??
            'Unable to load orders.'
          );
        }

      });
  }
}