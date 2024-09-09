import { ApiGen_Concepts_Product } from '../api/generated/ApiGen_Concepts_Product';

export class Api_GenerateProduct {
  readonly number: string;
  readonly name: string;

  constructor(product: ApiGen_Concepts_Product | null) {
    this.name = product?.description ?? '';
    this.number = product?.code ?? '';
  }
}
