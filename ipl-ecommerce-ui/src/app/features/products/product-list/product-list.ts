import {
  Component,
  OnInit,
  OnDestroy,
  inject,
  signal
} from '@angular/core';

import { CommonModule } from '@angular/common';

import { FormsModule } from '@angular/forms';

import { RouterLink } from '@angular/router';

import {
  Subject,
  debounceTime,
  distinctUntilChanged,
  takeUntil
} from 'rxjs';

import {
  ProductService
} from '../../../core/services/product';

import {
  Product
} from '../../../models/product';


@Component({
  selector: 'app-product-list',

  standalone: true,

  imports: [
    CommonModule,
    FormsModule,
    RouterLink
  ],

  templateUrl: './product-list.html',

  styleUrl: './product-list.css'
})
export class ProductList
  implements OnInit, OnDestroy {


   readonly productService =
    inject(ProductService);


  // ==========================================
  // Destroy Subject
  // ==========================================

  private readonly destroy$ =
    new Subject<void>();


  // ==========================================
  // Search Subject
  // ==========================================

  private readonly searchSubject =
    new Subject<string>();


  // ==========================================
  // Product State
  // ==========================================

  products =
    signal<Product[]>([]);

  isLoading =
    signal(false);

  errorMessage =
    signal('');

  totalCount =
    signal(0);

  pageNumber =
    signal(1);

  pageSize =
    signal(10);

  totalPages =
    signal(0);


  // ==========================================
  // Search / Filters
  // ==========================================

  search = '';

  selectedType = '';

  selectedFranchiseId?: number;


  // ==========================================
  // Product Types
  // ==========================================

  productTypes = [
    'Jersey',
    'Cap',
    'Flag',
    'AutographedPhoto'
  ];


  // ==========================================
  // Franchises
  // ==========================================

  franchises = [

    {
      id: 1,
      name: 'Chennai Super Kings'
    },

    {
      id: 2,
      name: 'Mumbai Indians'
    },

    {
      id: 3,
      name: 'Royal Challengers Bengaluru'
    },

    {
      id: 4,
      name: 'Kolkata Knight Riders'
    },

    {
      id: 5,
      name: 'Sunrisers Hyderabad'
    }

  ];


  // ==========================================
  // Initialization
  // ==========================================

  ngOnInit(): void {

    console.log(
      'ProductList initialized'
    );


    // Initial product load

    this.loadProducts();


    // Search with debounce

    this.searchSubject
      .pipe(

        debounceTime(1000),

        distinctUntilChanged(),

        takeUntil(this.destroy$)

      )
      .subscribe(searchTerm => {

        console.log(
          'Searching for:',
          searchTerm
        );


        this.search =
          searchTerm;


        this.pageNumber.set(1);

        this.loadProducts();

      });
  }


  // ==========================================
  // Search Input Changed
  // ==========================================

  onSearchChange(
    value: string
  ): void {

    this.searchSubject.next(
      value
    );
  }


  // ==========================================
  // Load Products
  // ==========================================

  loadProducts(): void {

    this.isLoading.set(true);

    this.errorMessage.set('');


    this.productService
      .getProducts(

        this.search,

        this.selectedType,

        this.selectedFranchiseId,

        this.pageNumber(),

        this.pageSize()

      )
      .subscribe({

        next: response => {

          console.log(
            'PRODUCT LIST RESPONSE:',
            response
          );


          this.products.set(
            response.items
          );


          this.pageNumber.set(
            response.pageNumber
          );


          this.pageSize.set(
            response.pageSize
          );


          this.totalPages.set(
            response.totalPages
          );


          this.totalCount.set(
            response.totalCount
          );


          this.isLoading.set(false);
        },


        error: error => {

          console.error(
            'PRODUCT LIST ERROR:',
            error
          );


          this.products.set([]);

          this.isLoading.set(false);


          this.errorMessage.set(
            error?.error?.detail ??
            'Unable to load products.'
          );
        }

      });
  }


  // ==========================================
  // Clear Filters
  // ==========================================

  clearFilters(): void {

    this.search = '';

    this.selectedType = '';

    this.selectedFranchiseId =
      undefined;


    this.pageNumber.set(1);


    // Don't wait for debounce when
    // explicitly clearing filters.

    this.loadProducts();
  }


  // ==========================================
  // Type Filter
  // ==========================================

  onTypeChange(): void {

    this.pageNumber.set(1);

    this.loadProducts();
  }


  // ==========================================
  // Franchise Filter
  // ==========================================

  onFranchiseChange(): void {

    this.pageNumber.set(1);

    this.loadProducts();
  }


  // ==========================================
  // Previous Page
  // ==========================================

  previousPage(): void {

    if (
      this.pageNumber() <= 1
    ) {

      return;
    }


    this.pageNumber.update(
      page => page - 1
    );


    this.loadProducts();
  }


  // ==========================================
  // Next Page
  // ==========================================

  nextPage(): void {

    if (
      this.pageNumber() >=
      this.totalPages()
    ) {

      return;
    }


    this.pageNumber.update(
      page => page + 1
    );


    this.loadProducts();
  }


  // ==========================================
  // Destroy
  // ==========================================

  ngOnDestroy(): void {

    this.destroy$.next();

    this.destroy$.complete();
  }
}