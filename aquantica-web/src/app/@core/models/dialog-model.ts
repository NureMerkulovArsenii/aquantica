export interface DialogModel {
  data: {
    title: string;
    message: string;
    okButtonText: string;
    cancelButtonText: string;
  };
  onClose: (result: boolean) => void;

}
