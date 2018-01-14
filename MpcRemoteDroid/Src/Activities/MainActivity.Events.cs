using System;
using System.Threading.Tasks;

using Android.OS;
using Android.Util;
using Android.Widget;

using MpcRemoteDroid.Src.Enums;
using MpcRemoteDroid.Src.Scraper;

namespace MpcRemoteDroid.Src.Activities
{
    public partial class MainActivity
    {
        public const int FeedbacklIntervalExecution = 250;
        public const int VolumeSteps = 10;

        public bool IsVolumeChanging { get; set; }
        public bool IsPositionChanging { get; set; }

        public PlayerExecutionData PlayerData { get; set; }

        public bool IsRunning { get; set; } = true;

        public async void VolumeIncrease(int currentVolume) => await ExecuteCommandAsync(Command.Volume, currentVolume + VolumeSteps);

        public async void VolumeDecrease(int currentVolume) => await ExecuteCommandAsync(Command.Volume, currentVolume - VolumeSteps);

        private void VolumeStartTrackingTouch(object sender, SeekBar.StartTrackingTouchEventArgs e) => IsVolumeChanging = true;

        private async void VolumeStopTrackingTouch(object sender, SeekBar.StopTrackingTouchEventArgs e)
        {
            await ExecuteCommandAsync(Command.Volume, Volume.Progress);

            IsVolumeChanging = false;
        }

        private void PositionStartTrackingTouch(object sender, SeekBar.StartTrackingTouchEventArgs e) => IsPositionChanging = true;

        private async void PositionStopTrackingTouch(object sender, SeekBar.StopTrackingTouchEventArgs e)
        {
            await ExecuteCommandAsync(Command.Position, Position.Progress);

            IsPositionChanging = false;
        }

        public Task PlayAndPauseCommandAsync()
        {
            if (PlayerData != null)
            {
                return ExecuteCommandAsync(PlayerData.State != PlayerState.Playing ? Command.Play : Command.Pause);
            }
            return null;
        }

        private async void PlayAndPauseClick(object sender, EventArgs e)
        {
            var task = PlayAndPauseCommandAsync();
            if (task != null)
            {
                await task;
            }
        }

        public Task SkipBackCommandAsync() => ExecuteCommandAsync(Command.SkipBack);

        private async void BackClick(object sender, EventArgs e) => await SkipBackCommandAsync();

        public Task SkipForwardCommandAsync() => ExecuteCommandAsync(Command.SkipForward);

        private async void ForwardClick(object sender, EventArgs e) => await SkipForwardCommandAsync();

        private async void PreviewClick(object sender, EventArgs e)
        {
            Preview.Clickable = false;
            try
            {
                var imageBytes = await GetSnapshotAsync();
                if (imageBytes != null)
                {
                    var bmp = await Android.Graphics.BitmapFactory.DecodeByteArrayAsync(imageBytes, 0, imageBytes.Length);
                    if (Preview.Drawable is Android.Graphics.Drawables.BitmapDrawable temp)
                    {
                        temp.Bitmap.Recycle();
                    }
                    Preview.SetImageBitmap(bmp);
                }
            }
            catch (Exception ex)
            {
                Log.Debug(nameof(PreviewClick), ex.Message);
            }
            Preview.Clickable = true;
        }

        private async Task UpdateViewWithDataFromPlayer()
        {
            var handler = new Handler(MainLooper);
            while (IsRunning)
            {
                var currentData = await GetVariablesAsync();
                if (currentData != null)
                {
                    try
                    {
                        PlayerData = currentData;
                        handler.Post(() =>
                        {
                            switch (PlayerData.State)
                            {
                                case PlayerState.Paused:
                                    PlayAndPause.SetImageResource(Android.Resource.Drawable.IcMediaPlay);
                                    break;
                                case PlayerState.Stopped:
                                    PlayAndPause.SetImageResource(Android.Resource.Drawable.IcMediaPlay);
                                    break;
                                default:
                                    PlayAndPause.SetImageResource(Android.Resource.Drawable.IcMediaPause);
                                    break;
                            }
                            if (!IsVolumeChanging)
                            {
                                Volume.Progress = PlayerData.VolumeLevel;
                            }
                            if (!IsPositionChanging)
                            {
                                var ratio = (double)PlayerData.Position / PlayerData.Duration;
                                var currentPosition = (int)(ratio * 100);
                                Position.Progress = currentPosition;
                            }
                            ShowNotification(PlayerData.File, PlayerData.PositionString, Position.Progress);
                        });
                    }
                    catch (Exception ex)
                    {
                        Log.Debug(nameof(UpdateViewWithDataFromPlayer), ex.Message);
                    }
                }
                await Task.Delay(FeedbacklIntervalExecution);
            }
            HideNotification();
        }
    }
}