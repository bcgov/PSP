import GenericModal from '@/components/common/GenericModal';
import { useConfiguration } from '@/hooks/useConfiguration';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';

export interface IRoleMismatchModalProps {
  display: boolean;
  setDisplay: (display: boolean) => void;
}

export const RoleMismatchModal: React.FunctionComponent<IRoleMismatchModalProps> = props => {
  const keycloak = useKeycloakWrapper();
  const configuration = useConfiguration();

  return (
    <GenericModal
      display={props.display}
      setDisplay={props.setDisplay}
      title="Role claims mismatch"
      message={
        <>
          <p>
            There seems to be a mismatch of claims for this user's role. The claims in the
            authentication service do not match the claims in PIMS.
          </p>
          <p>
            Please contact the System Administrator and ask them to update the user profile with the
            appropriate role to correct this. User can continue to use PIMS, but may observe
            reduced/inconsistent functionality, or log out.
          </p>
        </>
      }
      handleOk={() => {
        props.setDisplay(false);
      }}
      okButtonText="Continue"
      handleCancel={() => {
        keycloak.obj.logout({ redirectUri: `${configuration.baseUrl}/logout` });
      }}
      cancelButtonText="Logout"
    />
  );
};
