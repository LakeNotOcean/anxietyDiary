import { observer } from "mobx-react-lite";
import { Modal } from "semantic-ui-react";
import { useStore } from "../stores/store";

export default observer(function ModalContainer() {
  const { modalStore } = useStore();
  return (
    <Modal
      dimmer="blurring"
      open={modalStore.modal.open}
      closeIcon
      onClose={modalStore.closeModal}
      size="mini"
    >
      <Modal.Header text>{modalStore.modal.header}</Modal.Header>
      <Modal.Content>{modalStore.modal.body}</Modal.Content>
    </Modal>
  );
});
