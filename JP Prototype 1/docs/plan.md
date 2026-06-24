# Kế Hoạch Lập Trình Game Mô Phỏng Lái Xe (Vật Lý & Cơ Chế)

**Mục tiêu:** Xây dựng core loop mô phỏng xe ô tô với cơ chế tăng/giảm tốc, đánh lái theo vận tốc, và hộp số tự động 4 cấp.
**Phương pháp:** Lập trình theo từng giai đoạn, đảm bảo code xong bước nào có thể Play và Test ngay bước đó (Testable).

---

## Giai đoạn 1: Nền tảng vật lý & Di chuyển cơ bản
**Mục tiêu:** Xe hiện hình, đứng vững trên mặt đất, nhận input và có thể đẩy tới trước bằng lực vật lý.

* **Task 1.1: Setup Scene và Rigidbody**
    * Tạo mặt phẳng (Ground) có Collider.
    * Tạo một khối hộp (Cube) đại diện cho thân xe, gắn `Rigidbody`. Trọng lượng (Mass) set tạm ở mức 1000 - 1500kg.
    * Tạo script `CarController` gắn vào xe.
    * **Cách test:** Bấm Play, khối hộp rơi xuống đất và không bị xuyên qua sàn hay lăn lóc vô lý.

* **Task 1.2: Xử lý Input & Đọc tốc độ**
    * Trong `Update()`, lấy input dọc (W/S) gán vào biến `gasInput`.
    * Trong `FixedUpdate()`, tính tốc độ km/h: `currentSpeed = rigidbody.velocity.magnitude * 3.6f`.
    * Hiển thị `currentSpeed` ra Console (`Debug.Log`).
    * **Cách test:** Play game, chưa cần xe chạy, log phải nhảy liên tục ở mức ~0.

* **Task 1.3: Áp dụng lực đẩy (Acceleration)**
    * Viết hàm thêm lực đẩy tới: `rigidbody.AddForce(transform.forward * gasInput * baseForce)`.
    * Tạm thời dùng một biến `baseForce` cố định để test.
    * **Cách test:** Bấm W, xe bị đẩy thẳng về phía trước. Log báo tốc độ tăng dần. Nhả W, xe trượt đi theo quán tính.

---

## Giai đoạn 2: Hệ thống đánh lái (Steering)
**Mục tiêu:** Xe có thể rẽ trái/phải, góc đánh lái và tốc độ vô lăng hoạt động đúng thời gian 1.5s và tỉ lệ nghịch với vận tốc.

* **Task 2.1: Tính toán giới hạn góc lái theo tốc độ**
    * Viết hàm tính `MaxSteerAngle` dựa trên `currentSpeed` (từ 45 độ xuống 10 độ khi tốc độ đạt 180km/h).
    * **Cách test:** Viết thêm dòng log: `MaxSteerAngle`. Bấm W cho xe chạy, quan sát log xem góc tối đa có giảm dần khi xe chạy nhanh hơn không.

* **Task 2.2: Tịnh tiến góc lái trong 1.5s**
    * Lấy input ngang (A/D) gán vào biến `steerInput`.
    * Sử dụng hàm nội suy (VD: `Mathf.MoveTowards`) để nội suy biến `currentSteerAngle` về mục tiêu (`steerInput * MaxSteerAngle`) với tốc độ xoay tính toán (`MaxSteerAngle / 1.5f`).
    * **Cách test:** Không bấm ga. Bấm giữ A hoặc D, log `currentSteerAngle` phải mất đúng 1.5s để đạt 45 hoặc -45. Nhả phím, mất 1.5s để về 0.

* **Task 2.3: Áp dụng lực xoay (Turn Vehicle)**
    * Áp dụng xoay xe (VD: `rigidbody.AddRelativeTorque` hoặc `transform.Rotate`) dựa trên `currentSteerAngle` nhân với `currentSpeed`.
    * **Cách test:** Bấm W để xe chạy, kết hợp A/D để bẻ lái. Xe phải rẽ được. Chạy càng nhanh, xe rẽ vòng cung càng rộng (do góc lái bị giới hạn ở 10 độ).

---

## Giai đoạn 3: Trái tim động cơ (Hộp số & Vòng tua)
**Mục tiêu:** Khớp vận tốc của xe với bảng thiết kế RPM và hộp số 4 cấp.

* **Task 3.1: Khai báo cấu trúc Hộp số**
    * Khai báo mảng giới hạn tốc độ của 4 số: `[50, 90, 140, 180]`.
    * Khai báo biến `currentGear` (mặc định = 1), `currentRPM`.

* **Task 3.2: Logic tính Vòng tua (RPM)**
    * Viết công thức: `currentRPM = (8000 / maxSpeeds[currentGear - 1]) * currentSpeed`.
    * **Cách test:** Bấm ga cho xe chạy. Theo dõi log `currentRPM`. Vòng tua phải tăng đồng biến với tốc độ.

* **Task 3.3: Logic tự động sang số**
    * Thêm logic điều kiện: 
        * Nếu `currentRPM > 7500` -> `currentGear++`. 
        * Nếu `currentRPM < 3000` -> `currentGear--`.
    * **Cách test:** Cho xe chạy. Khi tốc độ chạm ~46km/h, xe phải tự nhảy `currentGear` lên 2, và RPM phải rớt xuống ngay lập tức (về ~4088).

* **Task 3.4: Đồng bộ lực đẩy theo cấp số**
    * Thay thế `baseForce` ở Giai đoạn 1 bằng lực đẩy tính theo mảng thời gian `accelTimes: [3s, 4s, 8s, 15s]`.
    * **Cách test:** Chạy từ 0 đến 50km/h, dùng đồng hồ bấm giây để xem có xấp xỉ 3 giây không.

---

## Giai đoạn 4: Lực cản & Phanh (Hoàn thiện Feeling)
**Mục tiêu:** Xử lý hiện tượng phanh động cơ khi nhả ga và phanh khẩn cấp.

* **Task 4.1: Ma sát lăn và Cản không khí