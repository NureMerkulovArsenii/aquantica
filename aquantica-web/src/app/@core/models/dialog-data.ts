export interface DialogData<T, TData>{
  isEdit: boolean;
  data: T | null;
  additionalData: TData | null
}
