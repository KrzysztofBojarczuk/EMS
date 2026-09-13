export interface Result<T> {
  isSuccess: boolean;
  error: string | null;
  value: T;
  isFailure: boolean;
}
