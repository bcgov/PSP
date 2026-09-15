import React from 'react';

import PersonAdditionalDetailsSections from './PersonAdditionalDetailsSections';
import PersonContactDetailsSection from './PersonContactDetailsSection';
import PersonContactInfoSection from './PersonContactInfoSection';

interface IPersonSubFormProps {
  isContactMethodInvalid?: boolean;
}

const PersonSubForm: React.FunctionComponent<React.PropsWithChildren<IPersonSubFormProps>> = ({
  isContactMethodInvalid,
}) => {
  return (
    <>
      <PersonContactDetailsSection />
      <PersonContactInfoSection isContactMethodInvalid={isContactMethodInvalid} />
      <PersonAdditionalDetailsSections />
    </>
  );
};

export default PersonSubForm;
