import { first, orderBy } from 'lodash';
import moment from 'moment';

import { getCalculatedExpiry } from '@/features/leases/leaseUtils';
import { ApiGen_CodeTypes_LeaseInsuranceTypes } from '@/models/api/generated/ApiGen_CodeTypes_LeaseInsuranceTypes';
import { ApiGen_CodeTypes_LeaseSecurityDepositTypes } from '@/models/api/generated/ApiGen_CodeTypes_LeaseSecurityDepositTypes';
import { ApiGen_CodeTypes_LeaseStakeholderTypes } from '@/models/api/generated/ApiGen_CodeTypes_LeaseStakeholderTypes';
import { ApiGen_Concepts_Insurance } from '@/models/api/generated/ApiGen_Concepts_Insurance';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { ApiGen_Concepts_LeasePeriod } from '@/models/api/generated/ApiGen_Concepts_LeasePeriod';
import { ApiGen_Concepts_LeaseRenewal } from '@/models/api/generated/ApiGen_Concepts_LeaseRenewal';
import { ApiGen_Concepts_LeaseStakeholder } from '@/models/api/generated/ApiGen_Concepts_LeaseStakeholder';
import { ApiGen_Concepts_PropertyLease } from '@/models/api/generated/ApiGen_Concepts_PropertyLease';
import { ApiGen_Concepts_SecurityDeposit } from '@/models/api/generated/ApiGen_Concepts_SecurityDeposit';
import { formatMoney, isValidIsoDateTime, pidFormatter } from '@/utils';

import { Api_GenerateLeaseProperty } from './GenerateLeaseProperty';
import { Api_GenerateSecurityDeposit } from './GenerateSecurityDeposit';
import { Api_GenerateTenant } from './GenerateTenant';

export class Api_GenerateLease {
  file_number: string;
  commencement_date: string;
  expiry_date: string;
  land_string: string;
  intended_use: string;
  term_end_date: string;
  payment_amount: string;
  payment_due_date: string;
  security_amount: string;
  primary_arbitration_city: string;
  cgl_limit: string;
  marine_liability_limit: string;
  vehicle_liability_limit: string;
  aircraft_liability_limit: string;
  uav_liability_limit: string;
  deposits: Api_GenerateSecurityDeposit[];
  tenants: Api_GenerateTenant[];
  person_stakeholders: Api_GenerateTenant[];
  organization_stakeholders: Api_GenerateTenant[];
  lease_properties: Api_GenerateLeaseProperty[];

  constructor(
    lease: ApiGen_Concepts_Lease,
    insurances: ApiGen_Concepts_Insurance[],
    stakeholders: ApiGen_Concepts_LeaseStakeholder[],
    renewals: ApiGen_Concepts_LeaseRenewal[],
    securityDeposits: ApiGen_Concepts_SecurityDeposit[],
    propertyLeases: ApiGen_Concepts_PropertyLease[],
    periods: ApiGen_Concepts_LeasePeriod[],
  ) {
    const firstPeriod = first(orderBy(periods, (t: ApiGen_Concepts_LeasePeriod) => t.id));

    this.file_number = lease.lFileNo ?? '';
    this.commencement_date = isValidIsoDateTime(lease?.startDate)
      ? moment.utc(lease.startDate).format('MMMM DD, YYYY')
      : '';
    this.expiry_date = getCalculatedExpiry(lease, renewals);
    this.term_end_date = isValidIsoDateTime(this.expiry_date)
      ? moment.utc(this.expiry_date).format('MMMM DD, YYYY') ?? ''
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
    this.payment_amount = formatMoney(firstPeriod?.paymentAmount ?? 0);
    this.payment_due_date = firstPeriod?.paymentDueDateStr ?? '';
    this.security_amount =
      formatMoney(
        securityDeposits
          .filter(
            deposit =>
              deposit.depositType.id === ApiGen_CodeTypes_LeaseSecurityDepositTypes.SECURITY,
          )
          .map(d => d.amountPaid ?? 0)
          .reduce((prev, next) => prev + next, 0),
      ) ?? '$0.00';
    this.primary_arbitration_city = lease.primaryArbitrationCity ?? '';

    this.cgl_limit =
      formatMoney(
        insurances.find(i => i?.insuranceType?.id === ApiGen_CodeTypes_LeaseInsuranceTypes.GENERAL)
          ?.coverageLimit,
      ) ?? '$0.00';
    this.marine_liability_limit =
      formatMoney(
        insurances.find(i => i?.insuranceType?.id === ApiGen_CodeTypes_LeaseInsuranceTypes.MARINE)
          ?.coverageLimit,
      ) ?? '$0.00';
    this.vehicle_liability_limit =
      formatMoney(
        insurances.find(i => i?.insuranceType?.id === ApiGen_CodeTypes_LeaseInsuranceTypes.VEHICLE)
          ?.coverageLimit,
      ) ?? '$0.00';
    this.aircraft_liability_limit =
      formatMoney(
        insurances.find(i => i?.insuranceType?.id === ApiGen_CodeTypes_LeaseInsuranceTypes.AIRCRAFT)
          ?.coverageLimit,
      ) ?? '$0.00';
    this.uav_liability_limit =
      formatMoney(
        insurances.find(i => i?.insuranceType?.id === ApiGen_CodeTypes_LeaseInsuranceTypes.UAVDRONE)
          ?.coverageLimit,
      ) ?? '$0.00';

    this.deposits = securityDeposits?.map(d => new Api_GenerateSecurityDeposit(d)) ?? [];
    this.tenants = stakeholders
      .filter(t => t.stakeholderTypeCode?.id === ApiGen_CodeTypes_LeaseStakeholderTypes.TEN)
      .map(t => new Api_GenerateTenant(t));
    this.organization_stakeholders = stakeholders
      .filter(
        t =>
          t.stakeholderTypeCode?.id === ApiGen_CodeTypes_LeaseStakeholderTypes.TEN &&
          t.lessorType?.id === 'ORG',
      )
      .map(t => new Api_GenerateTenant(t));
    this.person_stakeholders = stakeholders
      .filter(
        t =>
          t.stakeholderTypeCode?.id === ApiGen_CodeTypes_LeaseStakeholderTypes.TEN &&
          t.lessorType?.id === 'PER',
      )
      .map(t => new Api_GenerateTenant(t));
    this.lease_properties = propertyLeases?.map(p => new Api_GenerateLeaseProperty(p)) ?? [];
  }
}
