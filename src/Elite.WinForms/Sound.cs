// 'Elite - The Sharp Kind' - Andy Hawkins 2023.
// 'Elite - The New Kind' - C.J.Pinder 1999-2001.
// Elite (C) I.Bell & D.Braben 1984.

/*
 * Elite - The New Kind.
 *
 * Reverse engineered from the BBC disk version of Elite.
 * Additional material by C.J.Pinder.
 *
 * The original Elite code is (C) I.Bell & D.Braben 1984.
 * This version re-engineered in C by C.J.Pinder 1999-2001.
 *
 * email: <christian@newkind.co.uk>
 *
 *
 */

using System.Media;
using Commons.Music.Midi;
using Elite.Common.Enums;
using Elite.Engine;

namespace Elite.WinForms
{
    public class Sound : ISound, IDisposable
    {
        private MidiPlayer? _midiPlayer;
        private readonly IMidiOutput? _output;
        private bool _disposedValue;
        private readonly Dictionary<SoundEffect, SoundPlayer> _waves = new();
        private readonly Dictionary<Music, MidiMusic> _midis = new();

        public Sound()
        {
#pragma warning disable CS0618 // Type or member is obsolete
            IMidiAccess _access = MidiAccessManager.Default;
#pragma warning restore CS0618 // Type or member is obsolete
            _output = _access.OpenOutputAsync(_access.Outputs.Last().Id).Result;
        }

        public void Load(Music midiType, Stream midiStream) => _midis[midiType] = MidiMusic.Read(midiStream);

        public void Load(SoundEffect waveType, Stream waveStream) => _waves[waveType] = new(waveStream);

        public void PlayWave(SoundEffect waveType) => _waves[waveType].Play();

        public void PlayMidi(Music midiNo, bool repeat)
        {
            StopMidi();

            //TODO: Get repeat/loop working
            _midiPlayer = new(_midis[midiNo], _output);
            if (repeat)
            {
                //_midiPlayer.Finished += _midiPlayer_Finished;
                _midiPlayer.Play();
            }
            else
            {
                _midiPlayer.Play();
            }
        }

        public void StopMidi()
        {
            if (_midiPlayer != null)
            {
                _midiPlayer.Stop();
                _midiPlayer.Dispose();
                _midiPlayer = null;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    // dispose managed state (managed objects)
                    _midiPlayer?.Stop();
                    _midiPlayer?.Dispose();

                    foreach (KeyValuePair<SoundEffect, SoundPlayer> v in _waves)
                    {
                        v.Value.Dispose();
                    }
                }

                // free unmanaged resources (unmanaged objects) and override finalizer
                // set large fields to null
                _disposedValue = true;
            }
        }

        // override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~Sound()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
