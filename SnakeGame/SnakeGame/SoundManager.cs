using System;
using System.IO;
using System.Windows.Forms;
using NAudio.Wave;

namespace SnakeGame
{
    public class SoundManager
    {
        private WaveOutEvent bgOutput;
        private AudioFileReader bgReader;

        public bool IsMusicOn { get; set; } = true;
        public bool IsSoundOn { get; set; } = true;

        public void PlayBackground(string filePath)
        {
            if (!IsMusicOn || string.IsNullOrEmpty(filePath) || !File.Exists(filePath)) return;

            try
            {
                StopBackground(); // Dọn dẹp luồng cũ nếu có

                bgReader = new AudioFileReader(filePath);
                bgOutput = new WaveOutEvent();
                bgOutput.Init(bgReader);

                bgOutput.PlaybackStopped += OnBgmPlaybackStopped;
                bgOutput.Play();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Lỗi phát nhạc nền: " + ex.Message);
            }
        }

        private void OnBgmPlaybackStopped(object sender, StoppedEventArgs e)
        {
            if (IsMusicOn && bgReader != null && bgOutput != null)
            {
                try
                {
                    bgReader.Position = 0;
                    bgOutput.Play();
                }
                catch { }
            }
        }

        public void PlayEat(string filePath)
        {
            if (!IsSoundOn) return;
            PlayOneShot(filePath);
        }

        public void PlayGameOver(string filePath)
        {
            if (!IsSoundOn) return;
            PlayOneShot(filePath);
        }

        private void PlayOneShot(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath)) return;

            try
            {
                var reader = new AudioFileReader(filePath);
                var output = new WaveOutEvent();
                output.Init(reader);
                output.PlaybackStopped += (s, e) =>
                {
                    output.Dispose();
                    reader.Dispose();
                };
                output.Play();
            }
            catch { }
        }

        public void StopBackground()
        {
            try
            {
                if (bgOutput != null)
                {
                    bgOutput.PlaybackStopped -= OnBgmPlaybackStopped;
                    bgOutput.Stop();
                    bgOutput.Dispose();
                    bgOutput = null;
                }
                if (bgReader != null)
                {
                    bgReader.Dispose();
                    bgReader = null;
                }
            }
            catch { }
        }

        public void ToggleMusic()
        {
            IsMusicOn = !IsMusicOn;
            if (IsMusicOn)
            {
                if (bgOutput != null) bgOutput.Play();
                else PlayBackground("background.wav");
            }
            else
            {
                bgOutput?.Pause();
            }
        }

        public void ToggleSound()
        {
            IsSoundOn = !IsSoundOn;
        }
    }
}