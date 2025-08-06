import { getIn, useFormikContext } from 'formik';
import { useEffect } from 'react';

import { FastDatePicker, Select, TextArea } from '@/components/common/form';
import { RadioGroup, yesNoRadioGroupValues } from '@/components/common/form/RadioGroup';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import AreaContainer from '@/components/measurements/AreaContainer';
import { Roles } from '@/constants';
import * as API from '@/constants/API';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { useModalContext } from '@/hooks/useModalContext';
import { ApiGen_CodeTypes_AcquisitionTakeStatusTypes } from '@/models/api/generated/ApiGen_CodeTypes_AcquisitionTakeStatusTypes';
import { ApiGen_CodeTypes_LandActTypes } from '@/models/api/generated/ApiGen_CodeTypes_LandActTypes';

import { TakeModel } from '../models';
import { StyledBorderSection, StyledNoTabSection } from '../styles';

interface ITakeSubFormProps {
  take: TakeModel;
}

const TakeSubForm: React.FunctionComponent<ITakeSubFormProps> = ({ take }) => {
  const formikProps = useFormikContext();
  const { values, setFieldValue, handleChange } = formikProps;
  const { getOptionsByType } = useLookupCodeHelpers();
  const { setModalContent, setDisplayModal } = useModalContext();
  const { hasRole } = useKeycloakWrapper();

  const takeTypeOptions = getOptionsByType(API.TAKE_TYPES);
  const takeStatusTypeOptions = getOptionsByType(API.TAKE_STATUS_TYPES);
  const takeSiteContamTypeOptions = getOptionsByType(API.TAKE_SITE_CONTAM_TYPES);
  const takeLandActTypeOptions = getOptionsByType(API.TAKE_LAND_ACT_TYPES).map(landAct => ({
    ...landAct,
    label: landAct.value + ' ' + landAct.label,
  }));

  useEffect(() => {
    if (
      take.completionDt &&
      take.takeStatusTypeCode !== ApiGen_CodeTypes_AcquisitionTakeStatusTypes.COMPLETE
    ) {
      setFieldValue('completionDt', '');
    }
  }, [take.completionDt, take.takeStatusTypeCode, setFieldValue]);

  const getModalWarning = (onOk: () => void, isLeasePayable = false) => {
    return (e: React.ChangeEvent<any>) => {
      if (e.target.value === 'false') {
        setModalContent({
          variant: 'info',
          title: 'Confirm change',
          message: 'The area, if provided, will be cleared. Do you wish to proceed?',
          okButtonText: 'Confirm',
          cancelButtonText: 'Cancel',
          handleOk: () => {
            onOk();
            setDisplayModal(false);
          },
        });
        setDisplayModal(true);
      } else if (isLeasePayable) {
        setModalContent({
          variant: 'warning',
          title: 'Acknowledgement',
          message:
            'You have created a Lease (Payable) Take. You also need to create a Lease/Licence File.',
          okButtonText: 'Close',
          cancelButtonText: null,
          handleOk: () => {
            handleChange(e);
            setDisplayModal(false);
          },
        });
        setDisplayModal(true);
      } else {
        handleChange(e);
      }
    };
  };

  const canEditTake =
    take?.id === 0 ||
    take.takeStatusTypeCode !== ApiGen_CodeTypes_AcquisitionTakeStatusTypes.COMPLETE ||
    hasRole(Roles.SYSTEM_ADMINISTRATOR);

  return (
    <Section
      className="position-relative"
      header={take?.id ? `Update Take` : 'New Take'}
      isCollapsable={true}
      initiallyExpanded={true}
    >
      <SectionField label="Take type" required labelWidth={{ xs: 4 }} contentWidth={{ xs: 5 }}>
        <Select
          field="takeTypeCode"
          options={takeTypeOptions}
          placeholder="Select take type"
          disabled={!canEditTake}
        />
      </SectionField>
      <SectionField label="Take status" required labelWidth={{ xs: 4 }} contentWidth={{ xs: 5 }}>
        <Select
          field="takeStatusTypeCode"
          options={takeStatusTypeOptions}
          disabled={!canEditTake}
        />
      </SectionField>
      <SectionField
        label="Completion date"
        required={take.takeStatusTypeCode === ApiGen_CodeTypes_AcquisitionTakeStatusTypes.COMPLETE}
        tooltip={`This will be enabled when the take status is set to "Completed"`}
        labelWidth={{ xs: 4 }}
        contentWidth={{ xs: 5 }}
      >
        <FastDatePicker
          formikProps={formikProps}
          field="completionDt"
          maxDate={new Date()}
          disabled={
            take.takeStatusTypeCode !== ApiGen_CodeTypes_AcquisitionTakeStatusTypes.COMPLETE
          }
        />
      </SectionField>
      <SectionField label="Site contamination" labelWidth={{ xs: 4 }} contentWidth={{ xs: 5 }}>
        <Select
          field="takeSiteContamTypeCode"
          options={takeSiteContamTypeOptions}
          disabled={!canEditTake}
        />
      </SectionField>
      <SectionField label="Description of this Take" labelWidth={{ xs: 12 }}>
        <TextArea field="description" disabled={!canEditTake} />
      </SectionField>
      <StyledNoTabSection header="Area">
        <StyledBorderSection>
          <SectionField
            label="Is there a new highway dedication? *"
            labelWidth={{ xs: 8 }}
            tooltip="The term new highway dedication includes municipal road or provincial public highway"
          >
            <RadioGroup
              field="isNewHighwayDedication"
              radioValues={yesNoRadioGroupValues}
              flexDirection="row"
              handleChange={getModalWarning(() => {
                setFieldValue('isNewHighwayDedication', 'false');
                setFieldValue('newHighwayDedicationArea', 0);
              })}
              disabled={!canEditTake}
            />
          </SectionField>
          {take.isNewHighwayDedication === 'true' && (
            <>
              <SectionField label="Area" labelWidth={{ xs: 12 }}>
                <AreaContainer
                  onChange={(landArea, areaUnitTypeCode) => {
                    formikProps.setFieldValue('newHighwayDedicationArea', landArea);
                    formikProps.setFieldValue(
                      'newHighwayDedicationAreaUnitTypeCode',
                      areaUnitTypeCode,
                    );
                  }}
                  isEditable
                  unitCode={getIn(values, 'newHighwayDedicationAreaUnitTypeCode')}
                  landArea={take.newHighwayDedicationArea}
                  field="newHighwayDedicationArea"
                />
              </SectionField>
            </>
          )}
          <SectionField
            label="Is this being acquired for MOTT inventory? *"
            labelWidth={{ xs: 8 }}
            tooltip="Selecting Yes for this option will result in the property being added to inventory"
            className="pt-4"
          >
            <RadioGroup
              field="isAcquiredForInventory"
              radioValues={yesNoRadioGroupValues}
              flexDirection="row"
              disabled={!canEditTake}
            />
          </SectionField>
        </StyledBorderSection>
        <StyledBorderSection>
          <SectionField
            label="Is there a new registered interest in land (SRW, Easement or Covenant)? *"
            labelWidth={{ xs: 8 }}
          >
            <RadioGroup
              radioValues={yesNoRadioGroupValues}
              flexDirection="row"
              field="isNewInterestInSrw"
              handleChange={getModalWarning(() => {
                setFieldValue('isNewInterestInSrw', 'false');
                setFieldValue('statutoryRightOfWayArea', 0);
                setFieldValue('srwEndDt', '');
              })}
              disabled={!canEditTake}
            />
          </SectionField>
          {take.isNewInterestInSrw === 'true' && (
            <>
              <SectionField label="Area" labelWidth={{ xs: 12 }}>
                <AreaContainer
                  onChange={(landArea, areaUnitTypeCode) => {
                    formikProps.setFieldValue('statutoryRightOfWayArea', landArea);
                    formikProps.setFieldValue(
                      'statutoryRightOfWayAreaUnitTypeCode',
                      areaUnitTypeCode,
                    );
                  }}
                  isEditable={canEditTake}
                  unitCode={getIn(values, 'statutoryRightOfWayAreaUnitTypeCode')}
                  landArea={take.statutoryRightOfWayArea}
                  field="statutoryRightOfWayArea"
                />
              </SectionField>
              <SectionField label="SRW end date" labelWidth={{ xs: 3 }} className="mt-4">
                <FastDatePicker field="srwEndDt" formikProps={formikProps} />
              </SectionField>
            </>
          )}
        </StyledBorderSection>
        <StyledBorderSection>
          <SectionField label="Is there a new Land Act tenure? *" labelWidth={{ xs: 8 }}>
            <RadioGroup
              radioValues={yesNoRadioGroupValues}
              flexDirection="row"
              field="isNewLandAct"
              handleChange={getModalWarning(() => {
                setFieldValue('isNewLandAct', 'false');
                setFieldValue('landActArea', 0);
                setFieldValue('landActEndDt', '');
                setFieldValue('landActTypeCode', '');
              })}
              disabled={!canEditTake}
            />
          </SectionField>
          {take.isNewLandAct === 'true' && (
            <>
              <SectionField label="Land Act" required contentWidth={{ xs: 7 }}>
                <Select
                  field="landActTypeCode"
                  placeholder="Select Land Act"
                  options={takeLandActTypeOptions}
                  disabled={!canEditTake}
                  onChange={(e: React.ChangeEvent<HTMLSelectElement>) => {
                    if (
                      [
                        ApiGen_CodeTypes_LandActTypes.TRANSFER_OF_ADMIN_AND_CONTROL.toString(),
                        ApiGen_CodeTypes_LandActTypes.CROWN_GRANT.toString(),
                      ].includes(e.target.value)
                    ) {
                      setFieldValue('landActEndDt', '');
                    }
                  }}
                />
              </SectionField>
              <SectionField label="Area" labelWidth={{ xs: 12 }}>
                <AreaContainer
                  onChange={(landArea, areaUnitTypeCode) => {
                    formikProps.setFieldValue('landActArea', landArea);
                    formikProps.setFieldValue('landActAreaUnitTypeCode', areaUnitTypeCode);
                  }}
                  isEditable={canEditTake}
                  unitCode={getIn(values, 'landActAreaUnitTypeCode')}
                  landArea={take.landActArea}
                  field="landActArea"
                />
              </SectionField>
              {/** hide the end date for land act types that result in ownership*/}
              {![
                ApiGen_CodeTypes_LandActTypes.TRANSFER_OF_ADMIN_AND_CONTROL.toString(),
                ApiGen_CodeTypes_LandActTypes.CROWN_GRANT.toString(),
              ].includes(take.landActTypeCode) && (
                <SectionField label="End date" labelWidth={{ xs: 3 }} className="mt-4">
                  <FastDatePicker
                    field="landActEndDt"
                    formikProps={formikProps}
                    disabled={!canEditTake}
                    data-testId="landActEndDt"
                  />
                </SectionField>
              )}
            </>
          )}
        </StyledBorderSection>
        <StyledBorderSection>
          <SectionField
            label="Is there a new Licence for Construction Access (TLCA/LTC)? *"
            labelWidth={{ xs: 8 }}
          >
            <RadioGroup
              radioValues={yesNoRadioGroupValues}
              flexDirection="row"
              field="isNewLicenseToConstruct"
              handleChange={getModalWarning(() => {
                setFieldValue('isNewLicenseToConstruct', 'false');
                setFieldValue('licenseToConstructArea', 0);
                setFieldValue('ltcEndDt', '');
              })}
              disabled={!canEditTake}
            />
          </SectionField>
          {take.isNewLicenseToConstruct === 'true' && (
            <>
              <SectionField label="Area" labelWidth={{ xs: 12 }}>
                <AreaContainer
                  onChange={(landArea, areaUnitTypeCode) => {
                    formikProps.setFieldValue('licenseToConstructArea', landArea);
                    formikProps.setFieldValue(
                      'licenseToConstructAreaUnitTypeCode',
                      areaUnitTypeCode,
                    );
                  }}
                  isEditable={canEditTake}
                  unitCode={getIn(values, 'licenseToConstructAreaUnitTypeCode')}
                  landArea={take.licenseToConstructArea}
                  field="licenseToConstructArea"
                />
              </SectionField>

              <SectionField label="LTC end date" labelWidth={{ xs: 3 }} className="mt-4">
                <FastDatePicker field="ltcEndDt" formikProps={formikProps} />
              </SectionField>
            </>
          )}
        </StyledBorderSection>
        <StyledBorderSection>
          <SectionField label="Is there a Lease (Payable)? *" labelWidth={{ xs: 8 }}>
            <RadioGroup
              radioValues={yesNoRadioGroupValues}
              flexDirection="row"
              field="isLeasePayable"
              handleChange={getModalWarning(() => {
                setFieldValue('isLeasePayable', 'false');
                setFieldValue('leasePayableArea', 0);
                setFieldValue('leasePayableEndDt', '');
              }, true)}
            />
          </SectionField>
          {take.isLeasePayable === 'true' && (
            <>
              <SectionField label="Area" labelWidth={{ xs: 12 }}>
                <AreaContainer
                  onChange={(landArea, areaUnitTypeCode) => {
                    formikProps.setFieldValue('leasePayableArea', landArea);
                    formikProps.setFieldValue('leasePayableAreaUnitTypeCode', areaUnitTypeCode);
                  }}
                  isEditable
                  unitCode={getIn(values, 'leasePayableAreaUnitTypeCode')}
                  landArea={take.leasePayableArea}
                  field="leasePayableArea"
                />
              </SectionField>

              <SectionField label="End date" labelWidth={{ xs: 3 }} className="mt-4">
                <FastDatePicker
                  field="leasePayableEndDt"
                  formikProps={formikProps}
                  disabled={!canEditTake}
                />
              </SectionField>
            </>
          )}
        </StyledBorderSection>
      </StyledNoTabSection>
      <StyledNoTabSection header="Surplus">
        <StyledBorderSection>
          <SectionField label="Is there a Surplus? *" labelWidth={{ xs: 8 }}>
            <RadioGroup
              field="isThereSurplus"
              radioValues={yesNoRadioGroupValues}
              flexDirection="row"
              handleChange={getModalWarning(() => {
                setFieldValue('isThereSurplus', 'false');
                setFieldValue('surplusArea', 0);
              })}
              disabled={!canEditTake}
            />
          </SectionField>
          {take.isThereSurplus === 'true' && (
            <>
              <SectionField label="Area" labelWidth={{ xs: 12 }}>
                <AreaContainer
                  onChange={(landArea, areaUnitTypeCode) => {
                    formikProps.setFieldValue('surplusArea', landArea);
                    formikProps.setFieldValue('surplusAreaUnitTypeCode', areaUnitTypeCode);
                  }}
                  isEditable={canEditTake}
                  unitCode={getIn(values, 'surplusAreaUnitTypeCode')}
                  landArea={take.surplusArea}
                  field="surplusArea"
                />
              </SectionField>
            </>
          )}
        </StyledBorderSection>
      </StyledNoTabSection>
    </Section>
  );
};

export default TakeSubForm;
