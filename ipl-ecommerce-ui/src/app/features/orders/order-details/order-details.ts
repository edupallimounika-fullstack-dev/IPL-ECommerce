import {
  Component,
  OnInit,
  inject,
  signal
} from '@angular/core';

import { CommonModule } from '@angular/common';

import {
  ActivatedRoute,
  RouterLink
} from '@angular/router';

import {
  Order
} from '../../../models/order';

import {
  OrderService
} from '../../../core/services/order.service';


@Component({
  selector: 'app-order-details',

  standalone: true,

  imports: [
    CommonModule,
    RouterLink
  ],

  templateUrl: './order-details.html'
})
export class OrderDetailsComponent
  implements OnInit {

  private readonly route =
    inject(ActivatedRoute);

  private readonly orderService =
    inject(OrderService);


  order =
    signal<Order | null>(null);

  isLoading =
    signal(false);

  errorMessage =
    signal('');


  ngOnInit(): void {

    console.log(
      'OrderDetailsComponent initialized'
    );

    const id = Number(
      this.route.snapshot
        .paramMap
        .get('id')
    );


    console.log(
      'Order ID:',
      id
    );


    if (!id || Number.isNaN(id)) {

      this.errorMessage.set(
        'Invalid order ID.'
      );

      return;
    }


    this.loadOrder(id);
  }


  loadOrder(id: number): void {

    console.log(
      'Calling GET /api/orders/',
      id
    );

    this.isLoading.set(true);

    this.errorMessage.set('');


    this.orderService
      .getOrder(id)
      .subscribe({

        next: response => {

          console.log(
            'ORDER DETAILS RESPONSE:',
            response
          );


          this.order.set(response);

          this.isLoading.set(false);
        },


        error: error => {

          console.error(
            'ORDER DETAILS ERROR:',
            error
          );


          this.isLoading.set(false);

          this.errorMessage.set(
            error?.error?.detail ??
            'Unable to load order details.'
          );
        }

      });
  }
}