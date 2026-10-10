using NAudio.Wave;
using System;
using System.Windows.Forms;

namespace SnakeGame
{
    public class SoundManager
    {
        // ===== SINGLETON =====
        private static SoundManager _instance;
        public static SoundManager Instance
        {
            get
            {
                if (_instance == null) _instance = new SoundManager();
                return _instance;
            }
        }

        private WaveOutEvent bgOutput;
        private AudioFileReader bgReader;
        private EventHandler<StoppedEventArgs> bgPlaybackStoppedHandler;

        public bool IsMusicOn { get; private set; } = true;
        public bool IsSoundOn { get; private set; } = true;

        public SoundManager() { }

        // ===== NHẠC NỀN (LOOP) =====
        public void PlayBackground(string filePath)
        {
            try
            {
                bgReader = new AudioFileReader(filePath);
                bgOutput = new WaveOutEvent();
                bgOutput.Init(bgReader);

                bgPlaybackStoppedHandler = (s, e) =>
                {
                    if (IsMusicOn && bgReader != null)
                    {
                        bgReader.Position = 0;
                        bgOutput.Play();
                    }
                };
                bgOutput.PlaybackStopped += bgPlaybackStoppedHandler;

                bgOutput.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi nhạc nền: " + ex.Message);
            }
        }

        public void StopBackground()
        {
            try
            {
                if (bgOutput != null)
                {
                    if (bgPlaybackStoppedHandler != null)
                    {
                        bgOutput.PlaybackStopped -= bgPlaybackStoppedHandler;
                        bgPlaybackStoppedHandler = null;
                    }
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

        // ===== ÂM THANH GAME (ONE-SHOT) =====
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

      
        public void PlayClick()
        {
            if (!IsSoundOn) return;
            PlayOneShot("click.wav");
        }

        // ===== HÀM CHUNG PHÁT 1 LẦN =====
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

        // ===== BẬT/TẮT =====
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

        // ===== SETTER =====
        public void SetMusicOn(bool value)
        {
            IsMusicOn = value;
            if (IsMusicOn) bgOutput?.Play();
            else bgOutput?.Pause();
        }

        public void SetSoundOn(bool value)
        {
            IsSoundOn = value;
        }
    }
}