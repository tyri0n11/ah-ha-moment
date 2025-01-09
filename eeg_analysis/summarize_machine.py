import pandas as pd
import numpy as np
from scipy.signal import welch
from tkinter import Tk
from tkinter.filedialog import askopenfilenames, asksaveasfilename

def calculate_frequency_bands(signal, fs=256):
    """
    Calculate frequency bands (Delta, Theta, Alpha, Beta, Gamma) from raw EEG signals.
    """
    nperseg = min(len(signal), fs)
    freqs, psd = welch(signal, fs, nperseg=nperseg)
    bands = {
        'Delta': (0.5, 4),
        'Theta': (4, 8),
        'Alpha': (8, 13),
        'Beta': (13, 30),
        'Gamma': (30, 50),
    }
    band_powers = {}
    for band, (low, high) in bands.items():
        band_idx = np.logical_and(freqs >= low, freqs <= high)
        band_powers[band] = np.mean(psd[band_idx]) if np.any(band_idx) else 0
    return band_powers

def extract_signal_segment(group, start_time, stop_time, channel):
    """
    Extract the signal for a specific timeframe and channel.
    """
    signal = group[(group['Timestamp'] >= start_time) & (group['Timestamp'] <= stop_time)][channel]
    return signal.values

def process_participant_data(raw_data):
    """
    Process raw EEG data for a single participant and generate a formatted table.
    """
    fs = 256  # Sampling frequency
    formatted_data = []

    # Convert Timestamp to datetime
    raw_data['Timestamp'] = pd.to_datetime(raw_data['Timestamp'], format="%Y.%m.%d.%H.%M.%S.%f")

    grouped = raw_data.groupby('Order')
    for order, group in grouped:
        trial_number = order

        ts_show_question = group.loc[group['Event'] == 'Reading question', 'Timestamp'].min()
        ts_click_hint = group.loc[group['Event'] == 'Reading hint', 'Timestamp'].min()
        ts_submit = group.loc[group['Event'] == 'Click Submit', 'Timestamp'].min()

        submit_row = group[group['Event'] == 'Click Submit']
        answer_correct = submit_row['Result'].iloc[0].strip() if not submit_row.empty else 'incorrect'

        hint_clicked = not pd.isna(ts_click_hint)
        no_answer = pd.isna(ts_submit)

        no_hint_correct = not hint_clicked and (answer_correct == 'correct')
        no_hint_incorrect = not hint_clicked and (answer_correct == 'incorrect' or no_answer)
        hint_correct = hint_clicked and (answer_correct == 'correct')
        hint_incorrect = hint_clicked and (answer_correct == 'incorrect')

        ts_start = ts_show_question
        ts_stop = ts_submit if not pd.isna(ts_submit) else group['Timestamp'].iloc[-1]

        if no_answer:
            ts_stop = group.loc[group['Timestamp'] > ts_show_question, 'Timestamp'].min()

        condition = (
            'no hint, correct' if no_hint_correct else
            'no hint, incorrect' if no_hint_incorrect else
            'hint, correct' if hint_correct else
            'hint, incorrect'
        )

        psd_results = {}
        channels = ['T3', 'T4', 'O1', 'O2', 'AF3', 'AF4', 'T7', 'T8']

        for channel in channels:
            if channel in group.columns:
                signal = extract_signal_segment(group, ts_start, ts_stop, channel)
                band_powers = calculate_frequency_bands(signal, fs)
                psd_results.update({f"{channel}_{band}": value for band, value in band_powers.items()})

        formatted_data.append({
            'Date': ts_start.date(),
            'Time': ts_start.time(),
            'Trial Number': trial_number,
            'Condition': condition,
            'ts_start': ts_start,
            'ts_stop': ts_stop,
            **psd_results
        })

    return pd.DataFrame(formatted_data)

def main():
    """
    Main function to process multiple participant files and save the output.
    """
    Tk().withdraw()
    print("Select raw data CSV files for participants.")
    input_files = askopenfilenames(filetypes=[("CSV Files", "*.csv")], title="Select Participant Data Files")

    if not input_files:
        print("No files selected. Exiting.")
        return

    print("Select the location to save the combined formatted CSV file.")
    output_file = asksaveasfilename(defaultextension=".csv", filetypes=[("CSV Files", "*.csv")], title="Save the Formatted Data")

    if not output_file:
        print("No output file selected. Exiting.")
        return

    all_data = pd.DataFrame()
    for file in input_files:
        try:
            raw_data = pd.read_csv(file)
            participant_data = process_participant_data(raw_data)
            all_data = pd.concat([all_data, participant_data], ignore_index=True)
        except Exception as e:
            print(f"Error processing file {file}: {e}")

    try:
        all_data.to_csv(output_file, index=False)
        print(f"Formatted data has been saved to {output_file}")
    except PermissionError:
        print("Permission denied. Try saving to a different location.")

if __name__ == "__main__":
    main()
