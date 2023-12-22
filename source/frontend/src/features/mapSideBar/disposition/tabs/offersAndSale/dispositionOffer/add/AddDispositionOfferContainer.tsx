export interface IAddDispositionOfferContainerProps {
  dispositionFileId: number;
}

const AddDispositionOfferContainer: React.FunctionComponent<
  React.PropsWithChildren<IAddDispositionOfferContainerProps>
> = ({ dispositionFileId }) => {
  return <p>Add Disposition Offer Works!</p>;
};

export default AddDispositionOfferContainer;
