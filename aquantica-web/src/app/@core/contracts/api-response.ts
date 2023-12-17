export interface ApiResponse<T> {
  isSuccess: boolean;
  error: string;
  data?: T;
}
