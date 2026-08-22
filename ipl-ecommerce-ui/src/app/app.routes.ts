import { Routes } from '@angular/router';

import {
  ProductList
} from './features/products/product-list/product-list';

import {
  ProductDetails
} from './features/products/product-details/product-details';

import {
  LoginComponent
} from './features/auth/login/login';

import {
  CartComponent
} from './features/cart/cart';

import {
  CheckoutComponent
} from './features/orders/checkout/checkout';

import {
  OrderHistoryComponent
} from './features/orders/order-history/order-history';

import {
  OrderDetailsComponent
} from './features/orders/order-details/order-details';

import {
  authGuard
} from './core/guards/auth-guard';
import {
  RegisterComponent
} from './features/auth/register/register';

export const routes: Routes = [

  {
    path: '',
    redirectTo: 'products',
    pathMatch: 'full'
  },

  {
    path: 'products',
    component: ProductList
  },

  {
    path: 'products/:id',
    component: ProductDetails
  },

  {
    path: 'login',
    component: LoginComponent
  },

  {
    path: 'cart',
    component: CartComponent,
    canActivate: [authGuard]
  },

  {
    path: 'checkout',
    component: CheckoutComponent,
    canActivate: [authGuard]
  },

  {
    path: 'orders',
    component: OrderHistoryComponent,
    canActivate: [authGuard]
  },

  {
    path: 'orders/:id',
    component: OrderDetailsComponent,
    canActivate: [authGuard]
  },
  {
  path: 'register',
  component: RegisterComponent
}

];