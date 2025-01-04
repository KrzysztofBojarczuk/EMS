import React from "react";
import { Dialog } from "primereact/dialog";
import { Button } from "primereact/button";

interface ConfirmationDialogProps {
  visible: boolean;
  header: string;
  message: string;
  onConfirm: () => void;
  onCancel: () => void;
}

const ConfirmationDialog: React.FC<ConfirmationDialogProps> = ({
  visible,
  header,
  message,
  onConfirm,
  onCancel,
}) => {
  return (
    <Dialog
      header={header}
      visible={visible}
      style={{ width: "20vw" }}
      onHide={onCancel}
      modal
    >
      <div className="flex flex-column align-items-center gap-3">
        <p>{message}</p>
        <div className="flex justify-content-center gap-2">
          <Button label="Yes" onClick={onConfirm} />
          <Button label="No" onClick={onCancel} />
        </div>
      </div>
    </Dialog>
  );
};

export default ConfirmationDialog;
