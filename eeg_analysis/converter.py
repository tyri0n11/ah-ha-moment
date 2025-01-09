import pandas as pd
import numpy as np
from scipy.signal import welch
from tkinter import Tk
from tkinter.filedialog import askopenfilenames, asksaveasfilename


def calculate_frequency_bands(signal, fs=250):
    """
    Calculate frequency bands (Delta, Theta, Alpha, Beta Low, Beta High, Gamma, Gamma+) from raw EEG signals.
    """
    nperseg = min(len(signal), fs)
    freqs, psd = welch(signal, fs, nperseg=nperseg)

    bands = {
        'Delta': (0.5, 4),
        'Theta': (4, 8),
        'Alpha': (8, 13),
        'Beta Low': (13, 20),
        'Beta High': (20, 30),
        'Gamma': (30, 50),
        'Gamma+': (50, 100),
    }

    band_powers = {}
    for band, (low, high) in bands.items():
        band_idx = np.logical_and(freqs >= low, freqs <= high)
        band_powers[band] = np.mean(psd[band_idx]) if np.any(band_idx) else 0
    return band_powers


def extract_signal_segment(group, start_time, end_time, channel):
    """
    Extract the signal for a specific timeframe and channel.
    """
    signal = group[(group['Timestamp'] >= start_time) & (group['Timestamp'] <= end_time)][channel]
    return signal.values


def process_raw_data(input_file, participant_id):
    """
    Process raw EEG data, compute PSD for each trial case, and format the output.
    """
    raw_data = pd.read_csv(input_file)

    required_columns = ['Timestamp', 'O1', 'O2', 'T3', 'T4', 'Order', 'Event', 'Result']
    if not all(col in raw_data.columns for col in required_columns):
        print(f"Error: The input file must include the columns {required_columns}.")
        return pd.DataFrame()

    raw_data['Timestamp'] = raw_data['Timestamp'].astype(str)
    raw_data['Date'] = raw_data['Timestamp'].apply(lambda x: '.'.join(x.split('.')[:3]))
    raw_data['Time'] = raw_data['Timestamp'].apply(lambda x: '.'.join(x.split('.')[3:]))


    fs = 250
    formatted_data = []
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

        if not hint_clicked:
            condition = 'nohint_correct' if answer_correct == 'correct' else 'nohint_incorrect'
        elif hint_clicked:
            condition = 'hint_correct' if answer_correct == 'correct' else 'hint_incorrect'
        else:
            condition = 'undefined'

        if condition in ['nohint_correct', 'nohint_incorrect']:
            start_time = ts_show_question
            end_time = ts_submit if not pd.isna(ts_submit) else None
        elif condition in ['hint_correct', 'hint_incorrect']:
            start_time = ts_click_hint
            end_time = ts_submit if not pd.isna(ts_submit) else None
        else:
            start_time, end_time = None, None

        if end_time is None:
            next_question_start = raw_data.loc[
                (raw_data['Order'] > order) & (raw_data['Event'] == 'Reading question'),
                'Timestamp'
            ].min()
            end_time = next_question_start if not pd.isna(next_question_start) else group['Timestamp'].max()

        if start_time is not None and end_time is not None:
            signal_af3 = extract_signal_segment(group, start_time, end_time, 'T3')
            signal_af4 = extract_signal_segment(group, start_time, end_time, 'T4')
            signal_t7 = extract_signal_segment(group, start_time, end_time, 'O1')
            signal_t8 = extract_signal_segment(group, start_time, end_time, 'O2')

            psd_results = {
                **{f"AF3_{band}": value for band, value in calculate_frequency_bands(signal_af3, fs).items()},
                **{f"AF4_{band}": value for band, value in calculate_frequency_bands(signal_af4, fs).items()},
                **{f"T7_{band}": value for band, value in calculate_frequency_bands(signal_t7, fs).items()},
                **{f"T8_{band}": value for band, value in calculate_frequency_bands(signal_t8, fs).items()},
            }
        else:
            psd_results = {}

        formatted_data.append({
            'Date': group['Date'].iloc[0],
            'Time': group['Time'].iloc[0],
            'Participant ID': participant_id,
            'Trial Number': trial_number,
            'Condition': condition,
            'ts_start': start_time,
            'ts_stop': end_time,
            **psd_results
        })

    return pd.DataFrame(formatted_data)


def process_multiple_files():
    """
    Process multiple raw EEG CSV files, adding Participant ID and outputting a single combined file.
    """
    Tk().withdraw()
    print("Please select the raw data CSV files.")
    input_files = askopenfilenames(
        filetypes=[("CSV Files", "*.csv")],
        title="Select the Raw Data CSV Files"
    )

    if not input_files:
        print("No files selected. Exiting.")
        return

    print("Please select the location to save the combined formatted CSV file.")
    output_file = asksaveasfilename(
        defaultextension=".csv",
        filetypes=[("CSV Files", "*.csv")],
        title="Save the Combined Formatted Data"
    )

    if not output_file:
        print("No output file selected. Exiting.")
        return

    combined_data = []
    for input_file in input_files:
        participant_id = input_file.split('/')[-1].split('.')[0]  # Use file name as Participant ID
        print(f"Processing file: {input_file} (Participant ID: {participant_id})")
        try:
            processed_data = process_raw_data(input_file, participant_id)
            if not processed_data.empty:
                combined_data.append(processed_data)
            else:
                print(f"No data processed for file: {input_file}")
        except Exception as e:
            print(f"Error processing file {input_file}: {e}")

    if combined_data:
        combined_df = pd.concat(combined_data, ignore_index=True)

        try:
            combined_df.to_csv(output_file, index=False)
            print(f"Combined formatted data has been saved to {output_file}")
        except PermissionError:
            print("Permission denied. Attempting to save with a different name...")
            fallback_output = output_file.replace(".csv", "_new.csv")
            combined_df.to_csv(fallback_output, index=False)
            print(f"Combined formatted data has been saved to {fallback_output}")
    else:
        print("No valid data was processed. Exiting.")


if __name__ == "__main__":
    process_multiple_files()
