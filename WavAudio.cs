using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Steganography
{
    public class WavAudio
    {
        public byte[] data;
        private byte[] header = new byte[0x2E];
        public uint channels { get; private set; }
        public uint bitsPerSample { get; private set; }

        private readonly int start = 0x2E;
        public uint totalSamples { get; private set; }
        public uint[] samples;
        public long bytesAvailable { get; private set; }

        public WavAudio(byte[] data)
        {
            this.data = data;
            Array.Copy(data, 0, header, 0, 0x2E);
            channels = BitConverter.ToUInt16(data, 0x16);
            bitsPerSample = BitConverter.ToUInt16(data, 0x22);
            if (bitsPerSample == 8 || bitsPerSample == 16 || bitsPerSample == 24 || bitsPerSample == 32)
            {
                totalSamples = (BitConverter.ToUInt32(data, 0x2A) / channels) / (bitsPerSample / 8);
                samples = new uint[totalSamples];
                int i = 0;
                for (int n = 0; n < totalSamples; n++)
                {
                    switch (bitsPerSample)
                    {
                        case 8:
                            samples[n] = data[start + i];
                            break;
                        case 16:
                        default:
                            samples[n] = BitConverter.ToUInt16(data, start + i);
                            break;
                        case 24:
                            samples[n] = BitConverter.ToUInt32(data, start + i) & 0xFFFFFF;
                            break;
                        case 32:
                            samples[n] = BitConverter.ToUInt32(data, start + i);
                            break;
                    }
                    i += (int)(bitsPerSample / 8);
                }
                bytesAvailable = (long)Math.Floor((double)(totalSamples / 8));
            }
            else
            {
                OutputConsole.Write("This file is incompatible, bits per sample must be 8, 16, 24 or 32");
                this.data = null;
            }
        }

        public void Save()
        {
            if (bitsPerSample == 8 || bitsPerSample == 16 || bitsPerSample == 24 || bitsPerSample == 32)
            {
                int i = 0;
                for (int n = 0; n < totalSamples; n++)
                {
                    switch (bitsPerSample)
                    {
                        case 8:
                            data[start + i] = (byte)samples[n];
                            break;
                        case 16:
                        default:
                            data[start + i] = (byte)(samples[n] & 0xFF);
                            data[start + i + 1] = (byte)((samples[n] >> 8) & 0xFF);
                            break;
                        case 24:
                            data[start + i] = (byte)(samples[n] & 0xFF);
                            data[start + i + 1] = (byte)((samples[n] >> 8) & 0xFF);
                            data[start + i + 2] = (byte)((samples[n] >> 16) & 0xFF);
                            break;
                        case 32:
                            data[start + i] = (byte)(samples[n] & 0xFF);
                            data[start + i + 1] = (byte)((samples[n] >> 8) & 0xFF);
                            data[start + i + 2] = (byte)((samples[n] >> 16) & 0xFF);
                            data[start + i + 3] = (byte)((samples[n] >> 24) & 0xFF);
                            break;
                    }
                    i += (int)(bitsPerSample / 8);
                }
            }
        }
    }
}
