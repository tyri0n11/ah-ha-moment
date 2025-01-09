import numpy as np
import matplotlib.pyplot as plt
from scipy.signal import welch
from tkinter import Tk
from tkinter.filedialog import askopenfilename

# Hàm tính năng lượng phổ cho từng dải tần
def compute_band_power(psd, freqs, band):
    band_idx = np.logical_and(freqs >= band[0], freqs <= band[1])
    return np.sum(psd[band_idx])

# Định nghĩa các dải tần
bands = {
    "Delta": (0.5, 4),
    "Theta": (4, 8),
    "Alpha": (8, 12),
    "Beta": (12, 30),
    "Gamma": (30, 50),
}

# Mở hộp thoại để chọn tệp CSV
def select_file():
    Tk().withdraw()  # Ẩn cửa sổ chính Tkinter
    file_path = askopenfilename(
        title="Chọn file CSV tín hiệu EEG",
        filetypes=[("CSV files", "*.csv"), ("All files", "*.*")]
    )
    return file_path

# Chọn tệp CSV
file_path = select_file()
if not file_path:
    print("Không có tệp nào được chọn.")
    exit()

# Đọc dữ liệu từ file CSV
data = np.genfromtxt(file_path, delimiter=",", dtype=None, encoding="utf-8", names=True)
timestamps = data["Timestamp"]  # Cột thời gian dưới dạng timestamp
events = data["Event"]  # Cột sự kiện
signals = np.column_stack([data["O1"], data["O2"], data["T3"], data["T4"]])  # Các tín hiệu từ các kênh
channel_names = ["O1", "O2", "T3", "T4"]  # Tên kênh

# Chuyển đổi timestamp sang thời gian tương đối (tính từ 0)
relative_time = timestamps - timestamps[0]

# Thông số lấy mẫu
sampling_rate = 250  # Hz, thay đổi theo thiết bị của bạn
epoch_duration = 2.0  # Độ dài mỗi epoch (giây)
n_samples_epoch = int(epoch_duration * sampling_rate)

# Tiền xử lý tín hiệu: Chia thành các epoch
n_epochs = signals.shape[0] // n_samples_epoch
epochs = signals[:n_epochs * n_samples_epoch].reshape(n_epochs, n_samples_epoch, -1)

# Phân tích năng lượng phổ cho từng epoch và kênh
results = {}
for i, epoch in enumerate(epochs):
    print(f"Phân tích epoch {i+1}/{n_epochs}")
    results[f"Epoch {i+1}"] = {}
    for ch_idx, ch_name in enumerate(channel_names):
        signal = epoch[:, ch_idx]
        freqs, psd = welch(signal, fs=sampling_rate, nperseg=256)
        results[f"Epoch {i+1}"][ch_name] = {
            band: compute_band_power(psd, freqs, band_range) for band, band_range in bands.items()
        }

# Hiển thị trạng thái hoạt động từ event1 -> event2
print("\nChuyển trạng thái giữa các sự kiện:")
for i in range(len(events) - 1):
    print(f"Từ {events[i]} (tại {relative_time[i]}s) -> {events[i+1]} (tại {relative_time[i+1]}s)")

# Hiển thị kết quả năng lượng phổ
for epoch_name, channels in results.items():
    print(f"\n{epoch_name}:")
    for ch_name, band_powers in channels.items():
        print(f"  {ch_name}: {band_powers}")

# Vẽ đồ thị năng lượng phổ cho từng kênh
for ch_name in channel_names:
    plt.figure(figsize=(10, 5))
    for epoch_name, channels in results.items():
        band_powers = channels[ch_name].values()
        plt.bar(bands.keys(), band_powers, alpha=0.5, label=epoch_name)
    plt.title(f"Năng lượng phổ - {ch_name}")
    plt.xlabel("Dải tần")
    plt.ylabel("Năng lượng (mV^2)")
    plt.legend()
    plt.show()
