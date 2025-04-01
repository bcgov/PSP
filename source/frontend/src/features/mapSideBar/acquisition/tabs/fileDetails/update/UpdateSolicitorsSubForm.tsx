import { FieldArray, useFormikContext } from 'formik';
import React from 'react';

import { ContactInputContainer } from '@/components/common/form/ContactInput/ContactInputContainer';
import ContactInputView from '@/components/common/form/ContactInput/ContactInputView';
import { PrimaryContactSelector } from '@/components/common/form/PrimaryContactSelector/PrimaryContactSelector';
import { SectionField } from '@/components/common/Section/SectionField';

import { InterestHolderForm } from '../../stakeholders/update/models';
import { UpdateAcquisitionSummaryFormModel } from './models';

/** This sub form is only used for etl data, where multiple AOSLCTR may exist. In that case, allow the user to interact with those items. */
export const UpdateSolicitorsSubForm: React.FunctionComponent<React.PropsWithChildren> = () => {
  const { values } = useFormikContext<UpdateAcquisitionSummaryFormModel>();

  return (
    <FieldArray
      name="ownerSolicitors"
      render={() => (
        <>
          {values.ownerSolicitors.map((ownerSolicitor: InterestHolderForm, index: number) => (
            <React.Fragment key={`owner-solicitor-${ownerSolicitor?.contact?.id ?? index}`}>
              <SectionField
                label="Owner solicitor"
                className="mt-4"
                labelWidth={{ xs: 4 }}
                contentWidth={{ xs: 8 }}
              >
                <ContactInputContainer
                  field={`ownerSolicitors.${index}.contact`}
                  View={ContactInputView}
                  displayErrorAsTooltip={false}
                ></ContactInputContainer>
              </SectionField>

              {ownerSolicitor.contact?.organizationId && !ownerSolicitor.contact?.personId && (
                <SectionField
                  label="Primary contact"
                  labelWidth={{ xs: 5 }}
                  contentWidth={{ xs: 7 }}
                >
                  <PrimaryContactSelector
                    field={`ownerSolicitors.${index}.primaryContactId`}
                    contactInfo={ownerSolicitor?.contact}
                  ></PrimaryContactSelector>
                </SectionField>
              )}
            </React.Fragment>
          ))}
        </>
      )}
    />
  );
};
