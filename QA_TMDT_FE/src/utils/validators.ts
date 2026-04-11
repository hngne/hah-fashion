import type {
  CategoryItem,
  CategoryFormValues,
  CheckoutFormValues,
  FieldErrorMap,
  LoginFormValues,
  ProductFormValues,
  ProductVariantDraft,
  RegisterFormValues,
} from "../types";
import { cleanFieldErrors } from "../services/service-helpers";

const EMAIL_REGEX = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
const PHONE_REGEX = /^(0|\+84)\d{9,10}$/;
const HEX_REGEX = /^#(?:[0-9A-Fa-f]{6})$/;

function normalizeText(value?: string | null): string {
  return value?.trim() ?? "";
}

export function isValidEmail(value?: string | null): boolean {
  if (!value) return true;
  return EMAIL_REGEX.test(value.trim());
}

export function isValidPhone(value?: string | null): boolean {
  if (!value) return false;
  return PHONE_REGEX.test(value.trim());
}

export function isValidHexColor(value?: string | null): boolean {
  if (!value) return true;
  return HEX_REGEX.test(value.trim());
}

export function getCategoryDescendantIds(
  categories: CategoryItem[],
  categoryId: number,
): number[] {
  const descendants: number[] = [];
  const stack = [categoryId];

  while (stack.length) {
    const currentId = stack.pop();
    if (currentId == null) continue;

    const children = categories.filter((item) => item.maDanhMucCha === currentId);
    for (const child of children) {
      descendants.push(child.maDanhMuc);
      stack.push(child.maDanhMuc);
    }
  }

  return descendants;
}

export function validateCategoryForm(
  values: CategoryFormValues,
  categories: CategoryItem[],
  editingId?: number | null,
): FieldErrorMap {
  const errors: FieldErrorMap = {};
  const tenDanhMuc = normalizeText(values.tenDanhMuc);

  if (!tenDanhMuc) {
    errors.tenDanhMuc = "Ten danh muc khong duoc de trong.";
  }

  if (editingId && values.maDanhMucCha === editingId) {
    errors.maDanhMucCha = "Khong the chon chinh danh muc hien tai lam danh muc cha.";
  }

  if (editingId && values.maDanhMucCha) {
    const descendants = getCategoryDescendantIds(categories, editingId);
    if (descendants.includes(values.maDanhMucCha)) {
      errors.maDanhMucCha = "Khong the chon danh muc con lam danh muc cha.";
    }
  }

  return cleanFieldErrors(errors);
}

export function getVariantKey(variant: Pick<ProductVariantDraft, "maKichThuoc" | "maMauSac">): string {
  return `${variant.maKichThuoc}-${variant.maMauSac}`;
}

export function findDuplicateVariantKeys(variants: ProductVariantDraft[]): Set<string> {
  const counts = new Map<string, number>();
  for (const variant of variants) {
    const key = getVariantKey(variant);
    counts.set(key, (counts.get(key) || 0) + 1);
  }

  return new Set(
    Array.from(counts.entries())
      .filter(([, count]) => count > 1)
      .map(([key]) => key),
  );
}

export function validateProductForm(
  values: ProductFormValues,
  options: {
    requireInitialVariants: boolean;
    existingVariantKeys?: string[];
  },
): FieldErrorMap {
  const errors: FieldErrorMap = {};
  const maSp = normalizeText(values.maSp);
  const tenSp = normalizeText(values.tenSp);

  if (!maSp) errors.maSp = "Ma san pham khong duoc de trong.";
  if (!tenSp) errors.tenSp = "Ten san pham khong duoc de trong.";
  if (!values.maDanhMuc) errors.maDanhMuc = "Vui long chon danh muc.";
  if (values.giaGoc < 0) errors.giaGoc = "Gia goc khong duoc nho hon 0.";

  if (options.requireInitialVariants && values.newVariants.length === 0) {
    errors.newVariants = "San pham moi phai co it nhat 1 bien the.";
  }

  const duplicateKeys = findDuplicateVariantKeys(values.newVariants);
  const existingKeys = new Set(options.existingVariantKeys ?? []);

  values.newVariants.forEach((variant, index) => {
    const key = getVariantKey(variant);
    if (!variant.maKichThuoc) {
      errors[`newVariants.${index}.maKichThuoc`] = "Chon size.";
    }
    if (!variant.maMauSac) {
      errors[`newVariants.${index}.maMauSac`] = "Chon mau.";
    }
    if (variant.soLuongTon < 0) {
      errors[`newVariants.${index}.soLuongTon`] = "Ton kho khong duoc am.";
    }
    if (variant.giaBan < 0) {
      errors[`newVariants.${index}.giaBan`] = "Gia ban khong duoc am.";
    }
    if (duplicateKeys.has(key)) {
      errors[`newVariants.${index}.duplicate`] = "Bien the size/mau bi trung.";
    }
    if (existingKeys.has(key)) {
      errors[`newVariants.${index}.duplicate`] = "Bien the da ton tai.";
    }
  });

  values.existingVariants.forEach((variant, index) => {
    if (variant.soLuongTon < 0) {
      errors[`existingVariants.${index}.soLuongTon`] = "Ton kho khong duoc am.";
    }
    if ((variant.giaBan ?? 0) < 0) {
      errors[`existingVariants.${index}.giaBan`] = "Gia ban khong duoc am.";
    }
  });

  return cleanFieldErrors(errors);
}

export function validateLoginForm(values: LoginFormValues): FieldErrorMap {
  const errors: FieldErrorMap = {};

  if (!normalizeText(values.tenDangNhap)) {
    errors.tenDangNhap = "Ten dang nhap khong duoc de trong.";
  }

  if (!normalizeText(values.matKhau)) {
    errors.matKhau = "Mat khau khong duoc de trong.";
  }

  return cleanFieldErrors(errors);
}

export function validateRegisterForm(values: RegisterFormValues): FieldErrorMap {
  const errors = validateLoginForm(values);

  if (normalizeText(values.matKhau).length < 6) {
    errors.matKhau = "Mat khau phai co it nhat 6 ky tu.";
  }

  if (values.email && !isValidEmail(values.email)) {
    errors.email = "Email khong hop le.";
  }

  if (values.soDienThoai && !isValidPhone(values.soDienThoai)) {
    errors.soDienThoai = "So dien thoai phai la dinh dang Viet Nam hop le.";
  }

  return cleanFieldErrors(errors);
}

export function validateCheckoutForm(values: CheckoutFormValues): FieldErrorMap {
  const errors: FieldErrorMap = {};

  if (!normalizeText(values.tenNguoiNhan)) {
    errors.tenNguoiNhan = "Vui long nhap ten nguoi nhan.";
  }

  if (!isValidPhone(values.soDienThoai)) {
    errors.soDienThoai = "So dien thoai khong hop le.";
  }

  if (!normalizeText(values.diaChiGiaoHang)) {
    errors.diaChiGiaoHang = "Vui long nhap dia chi giao hang.";
  }

  return cleanFieldErrors(errors);
}

