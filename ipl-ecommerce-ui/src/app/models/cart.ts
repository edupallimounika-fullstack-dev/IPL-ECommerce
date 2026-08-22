export interface CartItem {
  id: number;
  productId: number;

  productName: string;
  productType: string;
  franchiseName: string;

  unitPrice: number;

  quantity: number;

  availableStock: number;

  imageUrl?: string;

  totalPrice: number;
}

export interface Cart {
  id: number;
  userId: number;

  items: CartItem[];

  totalAmount: number;
  totalItems: number;
}

export interface AddCartItemRequest {
  productId: number;
  quantity: number;
}

export interface UpdateCartItemRequest {
  quantity: number;
}