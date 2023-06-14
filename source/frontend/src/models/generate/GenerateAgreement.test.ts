import { mockAgreementsResponse } from '@/mocks/agreements.mock';

import { Api_GenerateAgreement } from './GenerateAgreement';

describe('GenerateAgreement tests', () => {
  it('Can generate an empty agreement without throwing an error', () => {
    const agreement = new Api_GenerateAgreement(null, null);
    expect(agreement.status).toBe('');
    expect(agreement.date).toBe('');
  });

  it('Can generate an agreement with dates in the expected format if no dates provided', () => {
    const agreement = new Api_GenerateAgreement(mockAgreementsResponse()[0], null);
    expect(agreement.commencement_date).toBe('');
    expect(agreement.completion_date).toBe('');
    expect(agreement.termination_date).toBe('');
    expect(agreement.date).toBe('');
  });

  it('Can generate an agreement with dates in the expected format valid', () => {
    const agreement = new Api_GenerateAgreement(mockAgreementsResponse()[1], null);
    expect(agreement.commencement_date).toBe('Apr 05, 2023');
    expect(agreement.completion_date).toBe('Apr 05, 2023');
    expect(agreement.termination_date).toBe('Apr 05, 2023');
    expect(agreement.date).toBe('Apr 05, 2023');
  });

  it('Can generate agreements with money in the expected format if not specified', () => {
    const agreement = new Api_GenerateAgreement(mockAgreementsResponse()[0], null);
    expect(agreement.deposit_amount).toBe('');
    expect(agreement.purchase_price).toBe('');
  });

  it('Can generate agreements with money in the expected format if specified', () => {
    const agreement = new Api_GenerateAgreement(mockAgreementsResponse()[1], null);
    expect(agreement.deposit_amount).toBe('$1,000.00');
    expect(agreement.purchase_price).toBe('$2,000.00');
  });
});
