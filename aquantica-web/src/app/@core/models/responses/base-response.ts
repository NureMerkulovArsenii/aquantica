export interface BaseResponse<T> {
    isSuccess: boolean;
    error: string;
    data: T | undefined;
}
