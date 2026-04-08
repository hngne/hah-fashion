import { ref } from "vue";

export interface ToastMessage {
  id: number;
  type: "success" | "error" | "info" | "warning";
  message: string;
}

const toasts = ref<ToastMessage[]>([]);
let nextId = 0;

export function useToast() {
  const addToast = (
    type: ToastMessage["type"],
    message: string,
    duration = 3500,
  ) => {
    const id = nextId++;
    toasts.value.push({ id, type, message });
    setTimeout(() => {
      toasts.value = toasts.value.filter((t) => t.id !== id);
    }, duration);
  };

  const success = (msg: string) => addToast("success", msg);
  const error = (msg: string) => addToast("error", msg);
  const info = (msg: string) => addToast("info", msg);
  const warning = (msg: string) => addToast("warning", msg);

  return { toasts, success, error, info, warning };
}
