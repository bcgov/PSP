import { FormikProps } from 'formik/dist/types';
import { ReactNode, useEffect } from 'react';

import { FastDatePicker, Input, Multiselect, Select, TextArea } from '@/components/common/form';
import FormGuideContainer from '@/components/common/form/FormGuide/FormGuideContainer';
import { InlineInput } from '@/components/common/form/styles';
import { UserRegionSelectContainer } from '@/components/common/form/UserRegionSelect/UserRegionSelectContainer';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import * as API from '@/constants/API';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { ApiGen_CodeTypes_LeasePurposeTypes } from '@/models/api/generated/ApiGen_CodeTypes_LeasePurposeTypes';
import { isValidString } from '@/utils';

import { LeaseFormModel } from '../models';
import { LeasePurposeModel } from '../models/LeasePurposeModel';

export interface IAdministrationSubFormProps {
  formikProps: FormikProps<LeaseFormModel>;
}

const AdministrationSubForm: React.FunctionComponent<
  React.PropsWithChildren<IAdministrationSubFormProps>
> = ({ formikProps }) => {
  const { values, setFieldValue } = formikProps;
  const { leaseTypeCode, programTypeCode, purposes, purposeOtherDescription } = values;

  const { getByType, getOptionsByType } = useLookupCodeHelpers();
  const programTypes = getOptionsByType(API.LEASE_PROGRAM_TYPES);
  const types = getOptionsByType(API.LEASE_TYPES);
  const initiatorTypes = getOptionsByType(API.LEASE_INITIATOR_TYPES);
  const responsibilityTypes = getOptionsByType(API.LEASE_RESPONSIBILITY_TYPES);

  const leasePurposeOptions = getByType(API.LEASE_PURPOSE_TYPES).map(x =>
    LeasePurposeModel.fromLookup(x),
  );

  //clear the associated other fields if the corresponding type has its value changed from other to something else.
  useEffect(() => {
    if (isValidString(leaseTypeCode) && leaseTypeCode !== 'OTHER') {
      setFieldValue('otherLeaseTypeDescription', '');
    }

    if (isValidString(programTypeCode) && programTypeCode !== 'OTHER') {
      setFieldValue('otherProgramTypeDescription', '');
    }

    if (purposes.length > 0) {
      if (!purposes?.some(x => x.purposeTypeCode === ApiGen_CodeTypes_LeasePurposeTypes.OTHER)) {
        setFieldValue('purposeOtherDescription', null);
      } else {
        const otherIndex = purposes?.findIndex(
          x => x.purposeTypeCode === ApiGen_CodeTypes_LeasePurposeTypes.OTHER,
        );
        if (otherIndex >= 0) {
          setFieldValue(`purposes[${otherIndex}].purposeOtherDescription`, purposeOtherDescription);
        }
      }
    }
  }, [leaseTypeCode, programTypeCode, purposeOtherDescription, purposes, setFieldValue]);

  const guideBodyContent = (): ReactNode => {
    return (
      <>
        <p className="mb-4">
          Select the appropriate drop-down options based on the intended use of the property.
          Program is broad level and is searchable. Type is the type of agreement; only certain
          document generation options will exist depending on what option you select. Purpose is a
          summary of the intended use.
        </p>
        <ul>
          <li>Program</li>
          <ul>
            <li>
              “Engineering” includes geotechnical, aggregate, environmental, survey and other
              engineering type uses.
            </li>
            <li>
              “Rail Trail” excludes trails that are not a historical rail corridor being used for
              multi-use trail purposes. Choose “Other” program and “Trail” purpose for other trail
              uses.
            </li>
          </ul>
          <li>Type</li>
          <p>
            Consult Legal Services if in doubt about the agreement type or agreement conditions.
          </p>
          <ul>
            <li>
              If Amending Agreement is selected, request legal review if amending conditions other
              than expiry date and payment amounts.
            </li>
            <li>Select “Building Lease (receivable)” if leasing both land and building(s).</li>
          </ul>
          <li>Purpose</li>
          <ul>
            <li>“Camping” includes tree planter camps and campgrounds.</li>
            <li>
              “Crossing” includes oil and gas crossings or crossings other than Railway Crossing.
            </li>
            <li>
              “Emergency Services” includes fire halls, ambulance services, RCMP or police stations,
              etc.
            </li>
            <li>“Historical” is for historical files where the purpose is unknown.</li>
            <li>“Marine Facility” is for facilities other than BC Ferries i.e. dock.</li>
            <li>“Staging Area” has the same meaning as laydown area.</li>
          </ul>
        </ul>
      </>
    );
  };

  return (
    <Section header="Administration">
      <FormGuideContainer
        title="Help with choosing the agreement Program, Type and Purpose"
        guideBody={guideBodyContent()}
      ></FormGuideContainer>
      <SectionField label="MOTT contact" labelWidth={{ xs: 3 }} contentWidth={{ xs: 6 }}>
        <InlineInput field="motiName" />
      </SectionField>

      <SectionField
        label="MOTT region"
        labelWidth={{ xs: 3 }}
        contentWidth={{ xs: 'auto' }}
        required
      >
        <UserRegionSelectContainer field="regionId" placeholder="Select region" required />
      </SectionField>
      <SectionField label="Program" labelWidth={{ xs: 3 }} contentWidth={{ xs: 'auto' }} required>
        <Select
          field="programTypeCode"
          options={programTypes}
          placeholder="Select program"
          required
        />
      </SectionField>
      {values?.programTypeCode === 'OTHER' && (
        <SectionField
          label="Other Program"
          labelWidth={{ xs: 3 }}
          contentWidth={{ xs: 8 }}
          required
        >
          <Input field="otherProgramTypeDescription" required />
        </SectionField>
      )}
      <SectionField label="Type" labelWidth={{ xs: 3 }} contentWidth={{ xs: 9 }} required>
        <Select field="leaseTypeCode" options={types} placeholder="Select type" required />
      </SectionField>
      {values?.leaseTypeCode === 'OTHER' && (
        <SectionField
          label="Describe other"
          labelWidth={{ xs: 3 }}
          contentWidth={{ xs: 8 }}
          required
        >
          <Input field="otherLeaseTypeDescription" required />
        </SectionField>
      )}

      <SectionField label="Purpose" labelWidth={{ xs: 3 }} contentWidth={{ xs: 9 }} required>
        <Multiselect
          field="purposes"
          displayValue="purposeTypeCodeDescription"
          placeholder=""
          options={leasePurposeOptions}
          hidePlaceholder
        />
      </SectionField>
      {purposes?.some(x => x.purposeTypeCode === ApiGen_CodeTypes_LeasePurposeTypes.OTHER) && (
        <SectionField label="Describe other" labelWidth={{ xs: 3 }} required>
          <Input field="purposeOtherDescription" required />
        </SectionField>
      )}

      <SectionField
        label="Initiator"
        tooltip="Where did this lease/licence initiate?"
        labelWidth={{ xs: 3 }}
        contentWidth={{ xs: 4 }}
      >
        <Select field="initiatorTypeCode" placeholder="Select initiator" options={initiatorTypes} />
      </SectionField>

      <SectionField
        label="Responsibility"
        labelWidth={{ xs: 3 }}
        contentWidth={{ xs: 'auto' }}
        tooltip="Who is currently responsible?"
      >
        <Select
          field="responsibilityTypeCode"
          placeholder="Select group responsible"
          options={responsibilityTypes}
        />
      </SectionField>
      <SectionField label="Effective date" labelWidth={{ xs: 3 }}>
        <FastDatePicker formikProps={formikProps} field="responsibilityEffectiveDate" />
      </SectionField>

      <SectionField
        label="Intended use"
        labelWidth={{ xs: 12 }}
        tooltip="The purpose for which the license is issued, as per the agreement"
      >
        <TextArea field="description" />
      </SectionField>
      <SectionField label="Primary arbitration city" labelWidth={{ xs: 12 }}>
        <Input field="primaryArbitrationCity" />
      </SectionField>
    </Section>
  );
};

export default AdministrationSubForm;
