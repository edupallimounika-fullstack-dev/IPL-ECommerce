import {
  Component,
  inject,
  signal
} from '@angular/core';

import {
  FormBuilder,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';

import {
  Router
} from '@angular/router';

import {
  OrderService
} from '../../../core/services/order.service';

import {
  CheckoutRequest
} from '../../../models/order';


@Component({
  selector: 'app-checkout',

  standalone: true,

  imports: [
    ReactiveFormsModule
  ],

  templateUrl: './checkout.html'
})
export class CheckoutComponent {

  private readonly fb =
    inject(FormBuilder);

  private readonly orderService =
    inject(OrderService);

  private readonly router =
    inject(Router);


  form =
    this.fb.nonNullable.group({

      shippingAddress: [
        '',
        [
          Validators.required,
          Validators.maxLength(1000)
        ]
      ]

    });


  isSubmitting =
    signal(false);


  errorMessage =
    signal('');


  checkout(): void {

    if (this.form.invalid) {

      this.form.markAllAsTouched();

      return;
    }


    this.isSubmitting.set(true);

    this.errorMessage.set('');


    const request: CheckoutRequest = {

      shippingAddress:
        this.form
          .getRawValue()
          .shippingAddress
          .trim()

    };


    console.log(
      'PLACE ORDER REQUEST:',
      request
    );


    this.orderService
      .checkout(request)
      .subscribe({

        next: order => {

          console.log(
            'ORDER CREATED:',
            order
          );


          this.isSubmitting.set(false);


          // Go directly to order details
          this.router.navigate([
            '/orders',
            order.id
          ]);
        },


        error: error => {

          console.error(
            'CHECKOUT ERROR:',
            error
          );


          this.isSubmitting.set(false);


          this.errorMessage.set(
            error?.error?.detail ??
            error?.error?.message ??
            'Unable to place order.'
          );
        }

      });
  }
}