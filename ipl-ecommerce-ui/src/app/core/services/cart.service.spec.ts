import { TestBed } from '@angular/core/testing';
import { CartService } from './cart.service.js';

describe('CartServiceTs', () => {
  let service: CartService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CartService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
