import { Api_Product } from '@/models/api/Project';

export class Api_GenerateProduct {
  number: string;
  name: string;

  constructor(product: Api_Product | null) {
    this.name = product?.description ?? '';
    this.number = product?.code ?? '';
  }
}
