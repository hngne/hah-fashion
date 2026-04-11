import type { AxiosError } from "axios";
import type { FieldErrorMap, PageResult } from "../types";

export function getPageItems<T>(page?: PageResult<T> | null): T[] {
  return page?.item ?? [];
}

export function getTotalItems<T>(page?: PageResult<T> | null): number {
  return page?.totalItem ?? getPageItems(page).length;
}

export function getTotalPages<T>(page?: PageResult<T> | null): number {
  if (!page) return 1;
  if (typeof page.totalPage === "number" && page.totalPage > 0) {
    return page.totalPage;
  }

  const pageSize = page.pageSize || 1;
  return Math.max(1, Math.ceil(getTotalItems(page) / pageSize));
}

export function extractErrorMessage(
  error: unknown,
  fallback = "Co loi xay ra. Vui long thu lai.",
): string {
  const axiosError = error as AxiosError<{ message?: string }>;
  return axiosError.response?.data?.message || fallback;
}

export function isNotFoundError(error: unknown): boolean {
  const axiosError = error as AxiosError;
  return axiosError.response?.status === 404;
}

export function hasFieldErrors(errors: FieldErrorMap): boolean {
  return Object.keys(errors).length > 0;
}

export function cleanFieldErrors<T extends FieldErrorMap>(errors: T): T {
  const next = Object.entries(errors).filter(([, value]) => value);
  return Object.fromEntries(next) as T;
}

