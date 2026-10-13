using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
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

        // ===== CACHE ÂM THANH VÀO RAM =====
        private Dictionary<string, byte[]> soundCache = new Dictionary<string, byte[]>();

        // Output riêng cho tiếng click (để chặn chồng chéo)
        private WaveOutEvent clickOutput;
        private WaveFileReader clickReader;
        private MemoryStream clickStream;

        public bool IsMusicOn { get; private set; } = true;
        public bool IsSoundOn { get; private set; } = true;

        public SoundManager()
        {
            // Nạp sẵn các file âm thanh ngắn vào RAM ngay khi khởi động
            PreloadSound("click.wav");
            PreloadSound("eat.wav");
            PreloadSound("gameover.wav");
        }

        // ===== NẠP FILE VÀO RAM =====
        private void PreloadSound(string fileName)
        {
            try
            {
                string path = ResolvePath(fileName);
                if (path != null)
                {
                    soundCache[fileName] = File.ReadAllBytes(path);
                }
            }
            catch { }
        }

        // Tìm file ở thư mục gốc hoặc thư mục chạy (bin/Debug)
        private string ResolvePath(string fileName)
        {
            if (File.Exists(fileName)) return fileName;

            string basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            if (File.Exists(basePath)) return basePath;

            return null;
        }

        // ===== NHẠC NỀN (LOOP) =====
        public void PlayBackground(string filePath)
        {
            try
            {
                StopBackground();

                string path = ResolvePath(filePath) ?? filePath;
                bgReader = new AudioFileReader(path);
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

        // ===== ÂM THANH GAME =====
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

        // ===== PHÁT ÂM THANH NGẮN (TỪ RAM, KHÔNG DELAY) =====
        private void PlayOneShot(string fileName)
        {
            try
            {
                byte[] data;
                if (!soundCache.TryGetValue(fileName, out data))
                {
                    // Nếu chưa cache thì load và cache luôn
                    string path = ResolvePath(fileName);
                    if (path == null) return;
                    data = File.ReadAllBytes(path);
                    soundCache[fileName] = data;
                }

                // Chặn chồng chéo cho tiếng click: dừng cái cũ trước khi phát cái mới
                if (fileName == "click.wav")
                {
                    StopClick();
                }

                var ms = new MemoryStream(data);
                var reader = new WaveFileReader(ms);

                var output = new WaveOutEvent
                {
                    DesiredLatency = 100,   // Giảm độ trễ xuống 100ms (mặc định là 300ms)
                    NumberOfBuffers = 2
                };
                output.Init(reader);

                // Nếu là click, lưu tham chiếu để có thể dừng khi bấm tiếp
                if (fileName == "click.wav")
                {
                    clickOutput = output;
                    clickReader = reader;
                    clickStream = ms;
                }

                output.PlaybackStopped += (s, e) =>
                {
                    try
                    {
                        output.Dispose();
                        reader.Dispose();
                        ms.Dispose();

                        if (fileName == "click.wav" && clickOutput == output)
                        {
                            clickOutput = null;
                            clickReader = null;
                            clickStream = null;
                        }
                    }
                    catch { }
                };

                output.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi phát âm thanh: " + ex.Message + "\nFile: " + fileName);
            }
        }

        // Dừng tiếng click đang phát (nếu có)
        private void StopClick()
        {
            try
            {
                if (clickOutput != null)
                {
                    clickOutput.Stop();
                    clickOutput.Dispose();
                    clickOutput = null;
                }
                if (clickReader != null) { clickReader.Dispose(); clickReader = null; }
                if (clickStream != null) { clickStream.Dispose(); clickStream = null; }
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