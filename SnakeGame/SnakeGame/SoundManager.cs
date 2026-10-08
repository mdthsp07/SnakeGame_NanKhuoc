using System;
using System.Windows.Forms;
using NAudio.Wave;

namespace SnakeGame
{
    public class SoundManager
    {
        private WaveOutEvent bgOutput;
        private AudioFileReader bgReader;

        public bool IsMusicOn { get; private set; } = true;
        public bool IsSoundOn { get; private set; } = true;

        public void PlayBackground(string filePath)
        {
            try
            {
                bgReader = new AudioFileReader(filePath);
                bgOutput = new WaveOutEvent();
                bgOutput.Init(bgReader);
                bgOutput.PlaybackStopped += (s, e) =>
                {
                    if (IsMusicOn && bgReader != null)
                    {
                        bgReader.Position = 0;
                        bgOutput.Play();
                    }
                };
                bgOutput.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi nhạc nền: " + ex.Message);
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

        // Phát 1 lần, không ảnh hưởng nhạc nền
        private void PlayOneShot(string filePath)
        {
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
                    bgOutput.PlaybackStopped -= null;   // Ngắt sự kiện
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
            if (IsMusicOn) bgOutput?.Play();
            else bgOutput?.Pause();
        }

        public void ToggleSound()
        {
            IsSoundOn = !IsSoundOn;
        }
    }
}