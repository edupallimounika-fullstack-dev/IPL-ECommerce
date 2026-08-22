export interface OrderItem {
  id: number;
  productId: number;
  productName: string;
  quantity: number;
  unitPrice: number;
  totalPrice: number;
}

export interface Order {
  id: number;
  userId: number;
  orderDate: string;
  totalAmount: number;
  status: string;
  shippingAddress: string;
  items: OrderItem[];
}

export interface OrderSummary {
  id: number;
  orderDate: string;
  totalAmount: number;
  status: string;
  totalItems: number;
}

export interface CheckoutRequest {
  shippingAddress: string;
}

export interface CheckoutResponse {
  id: number;
}