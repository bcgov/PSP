import { IUpdateCompensationStrategy } from './UpdateCompensationStrategy';

export class UpdateCompensationContext {
  private strategy: IUpdateCompensationStrategy;

  constructor(strategy: IUpdateCompensationStrategy) {
    this.strategy = strategy;
  }

  public canEditCompensations(isDraftCompensation: boolean): boolean {
    return this.strategy.canEditOrDeleteCompensation(isDraftCompensation);
  }
}
