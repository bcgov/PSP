import {
  mockAcquisitionFileOwnersResponse,
  mockApiAcquisitionFileTeamOrganization,
  mockApiAcquisitionFileTeamPerson,
} from '@/mocks/acquisitionFiles.mock';
import { getMockApiDefaultCompensation } from '@/mocks/compensations.mock';
import { emptyApiInterestHolder } from '@/mocks/interestHolder.mock';
import { mockCompReqH120s } from '@/mocks/mockCompReqH120s.mock';
import { ApiGen_Concepts_AcquisitionFileOwner } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFileOwner';
import { ApiGen_Concepts_AcquisitionFileTeam } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFileTeam';
import { ApiGen_Concepts_CompensationRequisition } from '@/models/api/generated/ApiGen_Concepts_CompensationRequisition';
import { ApiGen_Concepts_InterestHolder } from '@/models/api/generated/ApiGen_Concepts_InterestHolder';
import { formatMoney } from '@/utils';

import { Api_GenerateCompensationPayee } from './GenerateCompensationPayee';

describe('GenerateCompensationPayee tests', () => {
  it('can generate an empty payee without throwing an error', () => {
    const payee = new Api_GenerateCompensationPayee(null, []);
    expect(payee.name).toBe('');
    expect(payee.gst_number).toBe('');
    expect(payee.pre_tax_amount).toBe(formatMoney(0));
    expect(payee.tax_amount).toBe(formatMoney(0));
    expect(payee.total_amount).toBe(formatMoney(0));
    expect(payee.payment_in_trust).toBe(false);
  });

  it('adds h120 financial totals', () => {
    const payee = new Api_GenerateCompensationPayee(null, mockCompReqH120s());
    expect(payee.pre_tax_amount).toBe(formatMoney(1099));
    expect(payee.tax_amount).toBe(formatMoney(101));
    expect(payee.total_amount).toBe(formatMoney(1200));
  });

  it.each([
    ['JOHH DOE', mockAcquisitionFileOwnersResponse()[0]],
    ['FORTIS BC', mockAcquisitionFileOwnersResponse()[1]],
  ])(
    'can generate with owner payee: %s',
    (expectedName: string, owner: ApiGen_Concepts_AcquisitionFileOwner) => {
      const compensation: ApiGen_Concepts_CompensationRequisition = {
        ...getMockApiDefaultCompensation(),
        acquisitionOwner: owner,
      };
      const payee = new Api_GenerateCompensationPayee(compensation, []);
      expect(payee.name).toBe(expectedName);
    },
  );

  it.each([
    [
      'first last',
      {
        ...emptyApiInterestHolder,
        interestHolderId: 1,
        acquisitionFileId: 2,
        personId: 1,
        person: { id: 1, firstName: 'first', surname: 'last' },
      } as ApiGen_Concepts_InterestHolder,
    ],
    [
      'ABC Inc',
      {
        ...emptyApiInterestHolder,
        interestHolderId: 1,
        acquisitionFileId: 2,
        organizationId: 100,
        organization: { id: 100, name: 'ABC Inc' },
      } as ApiGen_Concepts_InterestHolder,
    ],
  ])(
    'can generate with interest holder payee: %s',
    (expectedName: string, ih: ApiGen_Concepts_InterestHolder) => {
      const compensation: ApiGen_Concepts_CompensationRequisition = {
        ...getMockApiDefaultCompensation(),
        interestHolder: ih,
      };
      const payee = new Api_GenerateCompensationPayee(compensation, []);
      expect(payee.name).toBe(expectedName);
    },
  );

  it.each([
    ['first last', mockApiAcquisitionFileTeamPerson()],
    ['ABC Inc', mockApiAcquisitionFileTeamOrganization()],
  ])(
    'can generate with acquisition team payee: %s',
    (expectedName: string, teamMember: ApiGen_Concepts_AcquisitionFileTeam) => {
      const compensation: ApiGen_Concepts_CompensationRequisition = {
        ...getMockApiDefaultCompensation(),
        acquisitionFileTeam: teamMember,
      };
      const payee = new Api_GenerateCompensationPayee(compensation, []);
      expect(payee.name).toBe(expectedName);
    },
  );

  it('can generate with legacy payee', () => {
    const compensation: ApiGen_Concepts_CompensationRequisition = {
      ...getMockApiDefaultCompensation(),
      legacyPayee: 'Chester Tester',
    };
    const payee = new Api_GenerateCompensationPayee(compensation, []);
    expect(payee.name).toBe('Chester Tester');
  });
});
