import { first, orderBy } from 'lodash';
import moment from 'moment';

import { ApiGen_Concepts_Insurance } from '@/models/api/generated/ApiGen_Concepts_Insurance';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { ApiGen_Concepts_LeaseTenant } from '@/models/api/generated/ApiGen_Concepts_LeaseTenant';
import { ApiGen_Concepts_LeaseTerm } from '@/models/api/generated/ApiGen_Concepts_LeaseTerm';
import { ApiGen_Concepts_PropertyLease } from '@/models/api/generated/ApiGen_Concepts_PropertyLease';
import { ApiGen_Concepts_SecurityDeposit } from '@/models/api/generated/ApiGen_Concepts_SecurityDeposit';
import { formatMoney, pidFormatter } from '@/utils';

import { Api_GenerateLeaseProperty } from './GenerateLeaseProperty';
import { Api_GenerateSecurityDeposit } from './GenerateSecurityDeposit';
import { Api_GenerateTenant } from './GenerateTenant';

export class Api_GenerateLease {
  file_number: string;
  commencement_date: string;
  land_string: string;
  intended_use: string;
  term_end_date: string;
  payment_amount: string;
  payment_due_date: string;
  security_amount: string;
  cgl_limit: string;
  marine_liability_limit: string;
  vehicle_liability_limit: string;
  aircraft_liability_limit: string;
  deposits: Api_GenerateSecurityDeposit[];
  tenants: Api_GenerateTenant[];
  person_tenants: Api_GenerateTenant[];
  organization_tenants: Api_GenerateTenant[];
  lease_properties: Api_GenerateLeaseProperty[];

  constructor(
    lease: ApiGen_Concepts_Lease,
    insurances: ApiGen_Concepts_Insurance[],
    tenants: ApiGen_Concepts_LeaseTenant[],
    securityDeposits: ApiGen_Concepts_SecurityDeposit[],
    propertyLeases: ApiGen_Concepts_PropertyLease[],
    terms: ApiGen_Concepts_LeaseTerm[],
  ) {
    const firstTerm = first(orderBy(terms, (t: ApiGen_Concepts_LeaseTerm) => t.id));
    this.file_number = lease.lFileNo ?? '';
    this.commencement_date = firstTerm?.startDate
      ? moment.utc(firstTerm?.startDate).format('MMMM DD, YYYY')
      : '';
    this.land_string =
      propertyLeases
        ?.map(p =>
          [
            `Parcel Identifier: ${pidFormatter(p.property?.pid?.toString())}`,
            p.property?.landLegalDescription ?? '',
            `${p.leaseArea ?? 0} ${p.areaUnitType?.description ?? 'm²'}`,
          ].join('\n'),
        )
        .join('\n\n') ?? '';
    this.intended_use = lease.description ?? '';
    this.term_end_date = firstTerm?.expiryDate
      ? moment.utc(firstTerm?.expiryDate).format('MMMM DD, YYYY') ?? ''
      : '';
    this.payment_amount = formatMoney(firstTerm?.paymentAmount ?? 0);
    this.payment_due_date = firstTerm?.paymentDueDateStr ?? '';
    this.security_amount =
      formatMoney(
        securityDeposits
          .filter(deposit => deposit.depositType.id === 'SECURITY')
          .map(d => d.amountPaid ?? 0)
          .reduce((prev, next) => prev + next, 0),
      ) ?? '$0.00';

    this.cgl_limit =
      formatMoney(insurances.find(i => i?.insuranceType?.id === 'GENERAL')?.coverageLimit) ??
      '$0.00';
    this.marine_liability_limit =
      formatMoney(insurances.find(i => i?.insuranceType?.id === 'MARINE')?.coverageLimit) ??
      '$0.00';
    this.vehicle_liability_limit =
      formatMoney(insurances.find(i => i?.insuranceType?.id === 'VEHICLE')?.coverageLimit) ??
      '$0.00';
    this.aircraft_liability_limit =
      formatMoney(insurances.find(i => i?.insuranceType?.id === 'AIRCRAFT')?.coverageLimit) ??
      '$0.00';
    this.deposits = securityDeposits?.map(d => new Api_GenerateSecurityDeposit(d)) ?? [];
    this.tenants = tenants
      .filter(t => t.tenantTypeCode?.id === 'TEN')
      .map(t => new Api_GenerateTenant(t));
    this.organization_tenants = tenants
      .filter(t => t.tenantTypeCode?.id === 'TEN' && t.lessorType?.id === 'ORG')
      .map(t => new Api_GenerateTenant(t));
    this.person_tenants = tenants
      .filter(t => t.tenantTypeCode?.id === 'TEN' && t.lessorType?.id === 'PER')
      .map(t => new Api_GenerateTenant(t));
    this.lease_properties = propertyLeases?.map(p => new Api_GenerateLeaseProperty(p)) ?? [];
  }
}
