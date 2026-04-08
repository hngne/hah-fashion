# Danh sách Trang, Bố cục, Màu sắc và Prompt tương ứng (Admin & Client)

Tài liệu này không chỉ liệt kê các trang cần có trên hệ thống Quản trị (Admin CMS) và Giao diện người dùng (Client) mà còn đưa ra định hướng Thiết kế, Bố cục thông minh, và Bảng màu chủ đạo phù hợp cho một website bán Quần Áo Đa Dạng Đối Tượng (Thời trang nam, nữ, trẻ em, unisex...) với cấu trúc danh mục cha-con nhiều lớp và sản phẩm nhiều biến thể (Size, Màu).

---

## 0. Định hướng Màu sắc & Trải nghiệm Người dùng (UI/UX)

### 0.1. Bảng màu chuẩn (Unified Color Palette)

Để AI hoặc Designer tạo ra một bộ giao diện ĐỒNG NHẤT, chúng ta sẽ quy định một bảng màu (Color Palette) cố định và dán nó vào TẤT CẢ các prompt. Ở đây tôi chọn phong cách **Tối giản & Cao cấp (Premium Minimalist)** giống Zara/H&M:

- **Nền chính (Main Background):** Trắng ngà (Off-white / `#FAFAFA` hoặc `#F9F9F9`).
- **Nền phụ/Khối (Surface/Card Background):** Trắng tính (`#FFFFFF`).
- **Chữ chính (Primary Text):** Đen than (Deep Charcoal / `#111827` hoặc `#1A1A1A`).
- **Chữ phụ (Secondary Text):** Xám trung tính (Medium Gray / `#6B7280`).
- **Màu nhấn (Accent/Brand Vector):** Đen nhám (Matte Black / `#000000`) cho các nút bấm chính (Call to Action).
- **Màu viền (Borders/Dividers):** Xám nhạt (Light Gray / `#E5E7EB`).
- **Màu hệ thống CMS:** Xanh Navy đậm (`#0F172A`) cho Sidebar Admin.

### 0.2. Bố cục chung & Xử lý Danh mục Cha - Con

- **Mega Menu:** Vì danh mục của bạn sâu (VD: Nữ -> Áo -> Áo sơ mi), Header không thể dùng menu xổ xuống bình thường. Bắt buộc phải dùng **Mega Menu**.
- **Bo góc (Border Radius):** Các thiết kế nên dùng góc bo rất nhẹ (`rounded-sm` hoặc 4px) hoặc vuông vức hẳn (0px) để giữ nét sang trọng của ngành thời trang.

---

## 1. Giao diện Người dùng (Client)

### 1.0. Client Layout & Mega Menu

- **Prompt:**
  > "Design a premium minimalist e-commerce Header and Footer tailored for a multi-tier fashion brand. STRICT COLOR PALETTE: Main background: Off-white (#FAFAFA), Text: Deep Charcoal (#111827), Accents/Buttons: Matte Black (#000000), Borders: Light Gray (#E5E7EB). The Header features a 'Mega Menu' state showing how nested categories (e.g., Women > Tops) drop down beautifully in a multi-column panel. The Footer should have a dark charcoal background with off-white text, separated into clean columns for links and newsletter."

### 1.1. Trang chủ (Home Page) — Chia thành 7 Section

> **Phong cách tổng:** Hiện đại, trẻ trung, năng động. Nhiều ảnh lớn, typography đậm, vi hoạt ảnh (micro-animation), bo góc mềm (8-12px). Tông màu ấm-lạnh phối hợp.

---

#### Section 1 — Hero Banner / Carousel chính

- **Prompt:**
  > "Design a full-width hero carousel section for a youthful fashion e-commerce homepage. WIDTH: 100vw, HEIGHT: 85vh minimum. COLOR PALETTE: Background overlays use semi-transparent dark gradient (rgba(0,0,0,0.3)) over vivid lifestyle photography. Text: Pure White (#FFFFFF). CTA Button: Rounded pill shape, solid white background with dark text (#111827), hover scales up slightly. Show 3 slides with dot indicators at the bottom. Each slide has: a large lifestyle fashion photo as background, a bold headline (e.g. 'BỘ SƯU TẬP HÈ 2026'), a short subtitle, and a 'Khám phá ngay' CTA button. Include subtle left/right arrow navigation. Modern feel with soft animations (fade or slide transition)."

---

#### Section 2 — Danh mục nổi bật (Featured Categories)

- **Prompt:**
  > "Design a 'Featured Categories' grid section for a fashion homepage. Background: Off-white (#FAFAFA). Layout: 4 equal columns on desktop, 2 columns on mobile. Each category card is a tall rectangle (3:4 ratio) showing a model photo with the category name overlaid at the bottom in bold white text on a dark gradient strip. Categories: 'Nam', 'Nữ', 'Trẻ Em', 'Unisex'. Cards have rounded corners (8px), subtle shadow on hover, and a gentle scale-up (1.03) animation on hover. Section title 'DANH MỤC NỔI BẬT' centered above in uppercase, bold, charcoal (#111827), with a small decorative line underneath."

---

#### Section 3 — Sản phẩm mới (New Arrivals Carousel)

- **Prompt:**
  > "Design a horizontal scrollable 'New Arrivals' product carousel for a fashion store. Background: White (#FFFFFF). Section title: 'HÀNG MỚI VỀ' left-aligned, bold uppercase, with a 'Xem tất cả →' link on the right in accent color. Show 5 product cards visible at once on desktop, scrollable with arrow buttons. Each card: product image (3:4 ratio, rounded 8px), product name below in medium weight, price in bold, and a small color swatch row (3-4 tiny circles). On hover: image slightly zooms, a subtle 'Thêm vào giỏ' overlay fades in at the bottom of the image. Keep spacing generous and clean."

---

#### Section 4 — Banner quảng cáo đôi (Promotional Duo Banners)

- **Prompt:**
  > "Design a side-by-side promotional banner section for a fashion homepage. Layout: two equal-width banners in a row, each roughly 50vw × 350px. Left banner: warm-toned lifestyle photo (e.g. summer collection), overlay text 'SALE MÙA HÈ — GIẢM ĐẾN 50%' in white bold text, with a 'Mua ngay' pill button. Right banner: cool-toned photo (e.g. new season lookbook), overlay text 'BST THU ĐÔNG 2026' with 'Khám phá' button. Both banners have rounded corners (12px), subtle parallax or zoom-on-scroll effect. Gap between banners: 16-24px. On mobile, stack vertically."

---

#### Section 5 — Sản phẩm bán chạy (Best Sellers)

- **Prompt:**
  > "Design a 'Best Sellers' product grid section. Background: very light gray (#F9FAFB). Title: 'SẢN PHẨM BÁN CHẠY' centered, uppercase bold, with filter tabs below: 'Tất cả | Áo | Quần | Phụ kiện' — active tab has a bottom border accent. Product grid: 4 columns desktop, 2 mobile. Each card shows: product thumbnail (model shot, 3:4), a small 'HOT' badge (red rounded pill) on top-left corner, product name, original price with strikethrough if on sale, sale price in red, star rating (5 stars). Hover effect: soft shadow elevation. Clean, modern, youthful typography."

---

#### Section 6 — Bộ sưu tập / Lookbook (Collections Showcase)

- **Prompt:**
  > "Design a 'Collections / Lookbook' showcase section for a fashion site. Background: White. Layout: asymmetric masonry-style grid with one large image (spanning 2 rows) on the left and two smaller images stacked on the right. Each image is a styled lookbook photo. Overlay on each: collection name in elegant white typography (e.g. 'Streetwear Essentials', 'Office Chic', 'Weekend Casual'). On hover: a translucent dark overlay appears with a 'Xem bộ sưu tập →' link. Section title 'BỘ SƯU TẬP' at top, centered uppercase. Rounded corners 8px. Contemporary fashion editorial feel."

---

#### Section 7 — Cam kết & Đăng ký nhận tin (Trust Bar + Newsletter)

- **Prompt:**
  > "Design the bottom pre-footer section combining two parts. PART A — Trust/Feature Bar: a horizontal strip with 4 icons+text in a row on light background (#F5F5F5). Items: '🚚 Giao hàng miễn phí đơn từ 500K', '🔄 Đổi trả trong 30 ngày', '💳 Thanh toán an toàn', '📞 Hỗ trợ 24/7'. Icons are simple line style, text is small and charcoal. PART B — Newsletter CTA: dark charcoal background (#1A1A1A), centered white text 'ĐĂNG KÝ NHẬN ƯU ĐÃI ĐỘC QUYỀN', a short description, then an email input field with a rounded 'Đăng ký' button (white bg, dark text). Modern and inviting."

### 1.2. Danh sách Sản phẩm (Product Listing / Category Page)

- **Prompt:**
  > "Design a fashion category page listing products. STRICT COLOR PALETTE: Main background: Off-white (#FAFAFA), Card background: White (#FFFFFF), Text: Deep Charcoal (#111827), Accents: Matte Black (#000000). On the left, include a powerful sidebar filter panel with a nested category accordion tree, clickable Color Swatches (actual colored circles), and Size toggle buttons (S, M, L, XL) outlined in light gray. The product grid should feature tall images (3:4 ratio) ideal for clothing modeling shots."

### 1.3. Chi tiết Sản phẩm (Product Detail Page) - TỐI QUAN TRỌNG

- **Prompt:**
  > "Design a highly converting product detail page for apparel. STRICT COLOR PALETTE: Background: Off-white (#FAFAFA), Text: Deep Charcoal (#111827), 'Add to Cart' Button: Matte Black (#000000) with white text. Left side: a large vertical gallery of model shots. Right side: bold product title, price, and clear visual selectors for Variations: round Color Swatches and rectangular Size Option buttons. Include a 'Size Guide' text link. Below, design a 'Complete the Look' cross-selling vertical section."

### 1.4. Giỏ hàng & Thanh toán (Cart & Checkout)

- **Prompt:**
  > "Design a clean, distraction-free checkout flow layout for fashion items. STRICT COLOR PALETTE: Background: Off-white (#FAFAFA), Forms/Cards: White (#FFFFFF), Text: Deep Charcoal (#111827), Primary Buttons: Matte Black (#000000), Borders/Inputs: Light Gray (#E5E7EB). Divide into a two-column layout: left side for Shipping/Payment multi-step forms with squared inputs, and right side for an 'Order Summary' box displaying thumbnails, selected Size/Color attributes, and the final price."

### 1.5. Trang Cá nhân / Đăng nhập

- **Prompt:**
  > "Design a minimalist user User Dashboard and Authentication modal (Login/Register) for a fashion brand. STRICT COLOR PALETTE: Background: Off-white (#FAFAFA), Cards: White (#FFFFFF), Text: Deep Charcoal (#111827), Buttons: Matte Black (#000000). The dashboard must show user profile details and a 'Recent Orders' table where each purchased item clearly displays its Size and Color attributes next to a small thumbnail, encouraging repurchasing."

---

## 2. Giao diện Quản trị (Admin CMS) - UI/UX Chuyên Biệt

Khác với Client, Admin cần giao diện làm việc Data-heavy, ít gây mỏi mắt.

### 2.0. Admin Layout (Thành phần dùng chung)

- **Prompt:**
  > "Design the layout shell for an e-commerce admin dashboard. STRICT COLOR PALETTE: Main Canvas Background: Light Gray (#F3F4F6), Sidebar: Dark Navy (#0F172A), Topbar/Cards: White (#FFFFFF), Primary Buttons: Deep Blue (#1D4ED8), Text: Charcoal (#1F2937). Ensure the sidebar has clear icons for Fashion CRM items (Products, Categories, Orders) and the center canvas is left empty for dynamic rendering."

### 2.6. Đăng nhập Admin (Admin Login)

- **Prompt:**
  > "Design a clinical, highly secure Admin Login page for a fashion e-commerce CMS. STRICT COLOR PALETTE: Background: Light Gray (#F3F4F6), Login Card: White (#FFFFFF), Text: Charcoal (#1F2937), Primary Button: Deep Blue (#1D4ED8). Center a sleek login card vertically and horizontally. Include fields for Username and Password with crisp borders, a subtle brand logo at the top, and a full-width 'Sign In' button."

### 2.1. Bảng điều khiển chính (Dashboard)

- **Prompt:**
  > "Design the content area of an admin dashboard for a fashion store. STRICT COLOR PALETTE: Background: Light Gray (#F3F4F6), Cards: White (#FFFFFF), Text: Charcoal (#1F2937). Feature summary metric cards at the top (Total Revenue, New Orders, Active Users). Below that, generate a visually striking data visualization section containing a large curved Area Chart for 'Revenue Over Time' and a colorful Doughnut Chart for 'Sales by Clothing Category'. Finally, include a 'Recent Orders' data table at the bottom."

### 2.2. Quản lý Sản phẩm VÀ BIẾN THỂ (Cực Rối)

- **Prompt:**
  > "Design an advanced 'Create Product' admin interface for apparel. STRICT COLOR PALETTE: Canvas: Light Gray (#F3F4F6), Form Blocks: White (#FFFFFF), Text: Charcoal (#1F2937), Action Buttons: Deep Blue (#1D4ED8). Emphasize a complex 'Variations Management' tab where admins combine Color and Size attributes, generating a dynamic sub-table below. This table must allow setting unique prices, SKUs, and uploading specific model images per variation (e.g., Red-Size M)."

### 2.3. Quản lý Đơn hàng (Order Management)

- **Prompt:**
  > "Design an 'Order Management' table interface. STRICT COLOR PALETTE: Background: Light Gray (#F3F4F6), Table/Cards: White (#FFFFFF), Text: Charcoal (#1F2937). Include columns for order ID, customer, date, total amount, and visually distinct color-coded status badges (e.g., Green for Delivered, Yellow for Processing). Include a detailed 'View Order' modal showing purchased clothing variations."

### 2.4. Quản lý Người dùng (User Management)

- **Prompt:**
  > "Design a 'User Management' data table with user avatars, names, email, roles (Admin/Customer), and action buttons for editing permissions. STRICT COLOR PALETTE: Background: Light Gray (#F3F4F6), Form Blocks: White (#FFFFFF), Text: Charcoal (#1F2937)."

### 2.5. Quản lý Danh mục (Category Management)

- **Prompt:**
  > "Design an admin interface for 'Category Tree Management' displaying deep nesting typical in fashion (e.g. Men > Tops > Polo). STRICT COLOR PALETTE: Background: Light Gray (#F3F4F6), Tree Items: White (#FFFFFF), Borders: Gray (#E5E7EB), Text: Charcoal (#1F2937). Use a drag-and-drop indicator UI representing a modern data tree with action menus on hover."
