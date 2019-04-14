using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeChallengeV2.Models;

namespace CodeChallengeV2.Services
{
    public class SensorService : ISensorService
    {
        public Task<SensorPayloadDecoded> DecodePayload(SensorPayload payload)
        {
            //TODO: implement decoding
            var res = new SensorPayloadDecoded()
            {
                DevEUI = payload.DevEUI,
                Time = payload.Time
            };

            string data = payload.Data;
            int port = payload.FPort;
            var startIndex = 0;
            
            while(startIndex < data.Length) {
                switch(port) {
                    case 40:
                        var tempInternal = getData(data.Substring(startIndex, 4)) / 100.0;
                        startIndex += 4;

                        res.TempInternal = tempInternal;
                        break;
                    case 41:
                        var red = getData(data.Substring(startIndex, 4)) / 100.0;
                        startIndex += 4;

                        res.TempRed = red;
                        break;
                    case 42:
                        var blue = getData(data.Substring(startIndex, 4)) / 100.0;
                        startIndex += 4;

                        res.TempBlue = blue;
                        break;
                    case 43:
                        var temp = getData(data.Substring(startIndex, 4)) / 100.0;
                        var humidity = getData(data.Substring(startIndex+4, 2)) / 2.0;
                        startIndex += 6;

                        res.TempHumidity = temp;
                        res.Humidity = humidity;
                        break;
                    default:
                        break;
                }

                if(startIndex < data.Length) {
                    port = getData(data.Substring(startIndex, 2));
                    startIndex += 2;
                }
            }

            return Task.FromResult(res);
        }

        private short getData(string data)
        {
            char[] cData = data.ToCharArray();

            string reversed = "";
            for (int i = 0; i < cData.Length; i += 2) {
                reversed = cData[i] + "" + cData[i+1] + "" + reversed;
            }
            
            return Convert.ToInt16(reversed, 16);
        }

    }
}
