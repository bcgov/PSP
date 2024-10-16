import React from 'react';

export interface ITrayHeaderProps {
  title: React.ReactNode;
  icon: React.ReactNode;
}

const TrayHeader: React.FunctionComponent<React.PropsWithChildren<ITrayHeaderProps>> = ({
  title,
  icon,
  ...props
}) => {
  useEffect(() => {
    setShow(!!context);
  }, [context, setShow]);
  return <></>;
};

export default TrayHeader;
