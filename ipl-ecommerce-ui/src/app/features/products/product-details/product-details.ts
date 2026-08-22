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

import { ProductService }
  from '../../../core/services/product';

import { CartService }
  from '../../../core/services/cart.service';

import { Product }
  from '../../../models/product';


@Component({
  selector: 'app-product-details',

  imports: [
    CommonModule,
    RouterLink
  ],

  templateUrl: './product-details.html',

  styleUrl: './product-details.css'
})
export class ProductDetails
  implements OnInit {


  private readonly route =
    inject(ActivatedRoute);


  // IMPORTANT:
  // readonly instead of private because
  // the HTML accesses productService.
  readonly productService =
    inject(ProductService);


  private readonly cartService =
    inject(CartService);


  // -----------------------------------
  // Product state
  // -----------------------------------

  product =
    signal<Product | null>(null);


  isLoading =
    signal(false);


  errorMessage =
    signal('');


  // -----------------------------------
  // Cart state
  // -----------------------------------

  quantity =
    signal(1);


  isAddingToCart =
    signal(false);


  cartMessage =
    signal('');


  cartError =
    signal('');


  // -----------------------------------
  // Initialization
  // -----------------------------------

  ngOnInit(): void {

    const id = Number(
      this.route.snapshot
        .paramMap
        .get('id')
    );


    console.log(
      'Product ID:',
      id
    );


    if (!id || Number.isNaN(id)) {

      this.errorMessage.set(
        'Invalid product ID.'
      );

      return;
    }


    this.loadProduct(id);
  }


  // -----------------------------------
  // Load Product
  // -----------------------------------

  loadProduct(id: number): void {

    this.isLoading.set(true);

    this.errorMessage.set('');


    this.productService
      .getProduct(id)
      .subscribe({

        next: response => {

          console.log(
            'PRODUCT DETAILS:',
            response
          );


          this.product.set(
            response
          );


          this.isLoading.set(false);
        },


        error: error => {

          console.error(
            'PRODUCT DETAILS ERROR:',
            error
          );


          this.isLoading.set(false);


          this.errorMessage.set(
            'Unable to load product details.'
          );
        }

      });
  }


  // -----------------------------------
  // Increase quantity
  // -----------------------------------

  increaseQuantity(): void {

    const currentProduct =
      this.product();


    if (!currentProduct) {
      return;
    }


    const currentQuantity =
      this.quantity();


    if (
      currentQuantity <
      currentProduct.stockQuantity
    ) {

      this.quantity.set(
        currentQuantity + 1
      );
    }
  }


  // -----------------------------------
  // Decrease quantity
  // -----------------------------------

  decreaseQuantity(): void {

    const currentQuantity =
      this.quantity();


    if (currentQuantity > 1) {

      this.quantity.set(
        currentQuantity - 1
      );
    }
  }


  // -----------------------------------
  // Add To Cart
  // -----------------------------------

  addToCart(): void {

    const currentProduct =
      this.product();


    if (!currentProduct) {
      return;
    }


    const requestedQuantity =
      this.quantity();


    if (requestedQuantity <= 0) {

      this.cartError.set(
        'Quantity must be at least 1.'
      );

      return;
    }


    if (
      requestedQuantity >
      currentProduct.stockQuantity
    ) {

      this.cartError.set(
        'Requested quantity is not available.'
      );

      return;
    }


    this.cartMessage.set('');

    this.cartError.set('');

    this.isAddingToCart.set(true);


    console.log(
      'Adding to cart:',
      {
        productId: currentProduct.id,
        quantity: requestedQuantity
      }
    );


    this.cartService
      .addItem({

        productId:
          currentProduct.id,

        quantity:
          requestedQuantity

      })
      .subscribe({

        next: cart => {

          console.log(
            'ADD TO CART SUCCESS:',
            cart
          );


          this.isAddingToCart.set(false);


          this.cartMessage.set(
            'Product added to cart successfully.'
          );
        },


        error: error => {

          console.error(
            'ADD TO CART ERROR:',
            error
          );


          this.isAddingToCart.set(false);


          if (error.status === 401) {

            this.cartError.set(
              'Please login before adding products to cart.'
            );

            return;
          }


          if (error.status === 400) {

            this.cartError.set(
              error.error?.message ??
              error.error?.detail ??
              'Unable to add product to cart.'
            );

            return;
          }


          this.cartError.set(
            'Unable to add product to cart.'
          );
        }

      });
  }
}