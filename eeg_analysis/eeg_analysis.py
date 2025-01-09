import pandas as pd
import numpy as np
from scipy.signal import butter, filtfilt
import matplotlib.pyplot as plt
from tkinter import Tk
from tkinter.filedialog import askopenfilename

def bandpass_filter(signal, lowcut, highcut, fs, order=4):
    """
    Applies a bandpass filter to the given signal.
    """
    nyquist = 0.5 * fs
    low = lowcut / nyquist
    high = highcut / nyquist
    b, a = butter(order, [low, high], btype='band')
    return filtfilt(b, a, signal)

def remove_dc_offset(signal, fs):
    """
    Removes DC offset by subtracting the mean of the first 200ms of the signal.
    """
    samples_200ms = int(0.2 * fs)
    dc_offset = np.mean(signal[:samples_200ms])
    return signal - dc_offset

def process_and_visualize_eeg():
    """
    Processes EEG data and visualizes epochs centered around events.
    """
    # File Selection
    Tk().withdraw()
    print("Please select your EEG CSV file.")
    file_path = askopenfilename(
        filetypes=[("CSV Files", "*.csv")],
        title="Select EEG CSV File"
    )
    if not file_path:
        print("No file selected. Exiting.")
        return

    # Load and Prepare Data
    print("Loading data...")
    raw_data = pd.read_csv(file_path)

    # Convert Timestamp column to datetime
    raw_data['Timestamp'] = pd.to_datetime(raw_data['Timestamp'], format='%Y.%m.%d.%H.%M.%S.%f')

    print("Data preview:")
    print(raw_data.head())

    # Initialize Analysis Parameters
    fs = 128  # Sampling frequency in Hz
    epoch_duration = 2  # Epoch duration in seconds (1s before and after)
    epoch_samples = int(epoch_duration * fs)  # Number of samples in an epoch

    # Process Each Question
    unique_orders = raw_data['Order'].unique()
    for order in unique_orders:
        question_data = raw_data[raw_data['Order'] == order]
        submit_event = question_data[question_data['Event'] == 'Click Submit']
        hint_event = question_data[question_data['Event'] == 'Reading hint']

        if submit_event.empty:
            print(f"No 'Click Submit' event found for question {order}. Skipping.")
            continue

        submit_time = submit_event.iloc[0]['Timestamp']
        hint_time = hint_event.iloc[0]['Timestamp'] if not hint_event.empty else None

        epoch_start = submit_time - pd.Timedelta(seconds=epoch_duration / 2)
        epoch_end = submit_time + pd.Timedelta(seconds=epoch_duration / 2)

        # Extract EEG epochs for O1, O2, T3, T4
        epoch_data = question_data[
            (question_data['Timestamp'] >= epoch_start) & (question_data['Timestamp'] <= epoch_end)
        ]
        time_axis = np.linspace(-epoch_duration / 2, epoch_duration / 2, len(epoch_data))

        # Process channels
        channels = ['O1', 'O2', 'T3', 'T4']
        processed_signals = {}
        for channel in channels:
            if channel in epoch_data.columns:
                raw_signal = epoch_data[channel].values
                signal_no_dc = remove_dc_offset(raw_signal, fs)
                filtered_signal = bandpass_filter(signal_no_dc, lowcut=1, highcut=10, fs=fs)
                processed_signals[channel] = filtered_signal

        # Visualization
        plt.figure(figsize=(12, 6))
        for channel, signal in processed_signals.items():
            plt.plot(time_axis, signal, label=channel)

        result = submit_event.iloc[0]['Result']
        hint_text = f"Hint Time: {hint_time}" if hint_time else "No Hint Clicked"
        plt.title(f"Question {order} EEG Epochs (Answer: {result})\n{hint_text}")
        plt.xlabel("Time (s)")
        plt.ylabel("Amplitude")
        plt.legend()
        plt.grid()
        plt.tight_layout()

        # Save the plot
        plt.savefig(f"Question_{order}_EEG_Epochs.png")
        plt.close()
        print(f"Saved plot for question {order}.")

    print("Processing and visualization complete.")

if __name__ == "__main__":
    process_and_visualize_eeg()
