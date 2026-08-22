import {
  Component,
  OnInit,
  inject,
  signal
} from '@angular/core';

import { CommonModule } from '@angular/common';

import { RouterLink } from '@angular/router';

import {
  Cart,
  CartItem
} from '../../models/cart';

import {
  CheckoutRequest
} from '../../models/order';

import {
  CartService
} from '../../core/services/cart.service';

import {
  ProductService
} from '../../core/services/product';

import {
  OrderService
} from '../../core/services/order.service';


@Component({
  selector: 'app-cart',

  standalone: true,

  imports: [
    CommonModule,
    RouterLink
  ],

  templateUrl: './cart.html'
})
export class CartComponent
  implements OnInit {


  // =====================================
  // Services
  // =====================================

  private readonly cartService =
    inject(CartService);

  readonly productService =
    inject(ProductService);

  private readonly orderService =
    inject(OrderService);


  // =====================================
  // Cart state
  // =====================================

  cart =
    signal<Cart | null>(null);

  isLoading =
    signal(false);

  errorMessage =
    signal('');


  // =====================================
  // Checkout state
  // =====================================

  isCheckingOut =
    signal(false);

  checkoutMessage =
    signal('');

  checkoutError =
    signal('');


  // =====================================
  // Initialization
  // =====================================

  ngOnInit(): void {

    console.log(
      'CartComponent initialized'
    );

    this.loadCart();
  }


  // =====================================
  // Load Cart
  // =====================================

  loadCart(): void {

    console.log(
      'GET /api/cart called'
    );

    this.isLoading.set(true);

    this.errorMessage.set('');


    this.cartService
      .getCart()
      .subscribe({

        next: cart => {

          console.log(
            'CART RESPONSE:',
            cart
          );


          if (cart) {

            this.cart.set(cart);
          }


          this.isLoading.set(false);
        },


        error: error => {

          console.error(
            'Failed to load cart',
            error
          );


          this.isLoading.set(false);


          this.errorMessage.set(
            error?.error?.detail ??
            'Unable to load cart.'
          );
        }

      });
  }


  // =====================================
  // Increase Quantity
  // =====================================

  increaseQuantity(
    item: CartItem
  ): void {

    if (
      item.quantity >=
      item.availableStock
    ) {

      return;
    }


    this.updateQuantity(
      item.productId,
      item.quantity + 1
    );
  }


  // =====================================
  // Decrease Quantity
  // =====================================

  decreaseQuantity(
    item: CartItem
  ): void {

    if (
      item.quantity <= 1
    ) {

      return;
    }


    this.updateQuantity(
      item.productId,
      item.quantity - 1
    );
  }


  // =====================================
  // Update Quantity
  // =====================================

  updateQuantity(
    productId: number,
    quantity: number
  ): void {

    if (quantity <= 0) {

      return;
    }


    console.log(
      'Updating cart quantity:',
      {
        productId,
        quantity
      }
    );


    this.cartService
      .updateItem(
        productId,
        {
          quantity
        }
      )
      .subscribe({

        next: cart => {

          console.log(
            'CART UPDATED:',
            cart
          );


          if (cart) {

            this.cart.set(cart);
          }


          this.errorMessage.set('');
        },


        error: error => {

          console.error(
            'Failed to update cart',
            error
          );


          this.errorMessage.set(
            error?.error?.detail ??
            'Unable to update cart.'
          );
        }

      });
  }


  // =====================================
  // Remove Item
  // =====================================

  removeItem(
    productId: number
  ): void {

    console.log(
      'Removing cart item:',
      productId
    );


    this.cartService
      .removeItem(productId)
      .subscribe({

        next: cart => {

          console.log(
            'ITEM REMOVED:',
            cart
          );


          if (cart) {

            this.cart.set(cart);

          } else {

            this.cart.set(null);
          }


          this.errorMessage.set('');
        },


        error: error => {

          console.error(
            'Failed to remove item',
            error
          );


          this.errorMessage.set(
            error?.error?.detail ??
            'Unable to remove item.'
          );
        }

      });
  }


  // =====================================
  // Clear Cart
  // =====================================

  clearCart(): void {

    console.log(
      'Clearing cart'
    );


    this.cartService
      .clearCart()
      .subscribe({

        next: () => {

          console.log(
            'CART CLEARED'
          );


          // Do not call GET /api/cart again.
          this.cart.set(null);

          this.errorMessage.set('');
        },


        error: error => {

          console.error(
            'Failed to clear cart',
            error
          );


          this.errorMessage.set(
            error?.error?.detail ??
            'Unable to clear cart.'
          );
        }

      });
  }


  // =====================================
  // Checkout
  // =====================================

  checkout(
    shippingAddress: string
  ): void {

    const currentCart =
      this.cart();


    // Validate cart

    if (
      !currentCart ||
      currentCart.items.length === 0
    ) {

      this.checkoutError.set(
        'Your cart is empty.'
      );

      return;
    }


    // Validate address

    if (
      !shippingAddress ||
      !shippingAddress.trim()
    ) {

      this.checkoutError.set(
        'Shipping address is required.'
      );

      return;
    }


    // Clear previous messages

    this.checkoutMessage.set('');

    this.checkoutError.set('');


    // Show processing

    this.isCheckingOut.set(true);


    // Create request

    const request: CheckoutRequest = {

      shippingAddress:
        shippingAddress.trim()

    };


    console.log(
      'CHECKOUT REQUEST:',
      request
    );


    // Call API

    this.orderService
      .checkout(request)
      .subscribe({

        next: order => {

          console.log(
            'CHECKOUT SUCCESS:',
            order
          );


          this.isCheckingOut.set(false);


          this.checkoutMessage.set(
            'Order placed successfully.'
          );


          // Backend should have
          // cleared the cart.

          this.cart.set(null);
        },


        error: error => {

          console.error(
            'CHECKOUT ERROR:',
            error
          );


          this.isCheckingOut.set(false);


          if (
            error.status === 401
          ) {

            this.checkoutError.set(
              'Please login before checkout.'
            );

            return;
          }


          if (
            error.status === 400
          ) {

            this.checkoutError.set(
              error.error?.detail ??
              error.error?.message ??
              'Invalid checkout request.'
            );

            return;
          }


          this.checkoutError.set(
            error.error?.detail ??
            error.error?.message ??
            'Checkout failed.'
          );
        }

      });
  }
}